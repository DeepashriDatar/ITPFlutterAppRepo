import 'dart:async';

import 'package:connectivity_plus/connectivity_plus.dart';
import 'package:dio/dio.dart';
import 'package:intimepro_mobile/core/config/env_config.dart';

class ApiClient {
  ApiClient({Dio? dio})
      : _dio = dio ??
            Dio(
              BaseOptions(
                baseUrl: EnvConfig.apiBaseUrl,
                connectTimeout: const Duration(seconds: 15),
                receiveTimeout: const Duration(seconds: 20),
                sendTimeout: const Duration(seconds: 15),
                queryParameters: <String, dynamic>{'pageSize': 20},
              ),
            ) {
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (RequestOptions options, RequestInterceptorHandler handler) async {
          final List<ConnectivityResult> connectivity = await Connectivity().checkConnectivity();
          if (connectivity.contains(ConnectivityResult.none)) {
            return handler.reject(
              DioException(
                requestOptions: options,
                type: DioExceptionType.connectionError,
                message: 'No network connection available',
              ),
            );
          }

          if (!options.uri.scheme.toLowerCase().startsWith('https')) {
            return handler.reject(
              DioException(
                requestOptions: options,
                type: DioExceptionType.badCertificate,
                message: 'Only HTTPS endpoints are allowed',
              ),
            );
          }

          return handler.next(options);
        },
      ),
    );

    _dio.interceptors.add(
      RetryOnTransientFailureInterceptor(_dio),
    );
  }

  final Dio _dio;

  Dio get dio => _dio;
}

class RetryOnTransientFailureInterceptor extends Interceptor {
  RetryOnTransientFailureInterceptor(this._dio);

  final Dio _dio;

  @override
  Future<void> onError(DioException err, ErrorInterceptorHandler handler) async {
    final RequestOptions requestOptions = err.requestOptions;
    final int retries = (requestOptions.extra['retries'] as int?) ?? 0;
    final bool shouldRetry = retries < 2 &&
        (err.type == DioExceptionType.connectionError ||
            err.type == DioExceptionType.connectionTimeout ||
            err.response?.statusCode == 408 ||
            err.response?.statusCode == 429 ||
            (err.response?.statusCode ?? 0) >= 500);

    if (!shouldRetry) {
      return handler.next(err);
    }

    requestOptions.extra['retries'] = retries + 1;
    await Future<void>.delayed(Duration(milliseconds: 250 * (retries + 1)));

    try {
      final Response<dynamic> response = await _dio.fetch<dynamic>(requestOptions);
      return handler.resolve(response);
    } on DioException catch (retryError) {
      return handler.next(retryError);
    }
  }
}
