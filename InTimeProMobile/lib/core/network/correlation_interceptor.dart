import 'package:dio/dio.dart';
import 'package:uuid/uuid.dart';

class CorrelationInterceptor extends Interceptor {
  CorrelationInterceptor({Uuid? uuid}) : _uuid = uuid ?? const Uuid();

  final Uuid _uuid;

  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    final String method = options.method.toUpperCase();
    const Set<String> mutatingMethods = <String>{'POST', 'PUT', 'PATCH', 'DELETE'};

    if (mutatingMethods.contains(method) && !options.headers.containsKey('X-Correlation-Id')) {
      options.headers['X-Correlation-Id'] = _uuid.v4();
    }

    handler.next(options);
  }
}
