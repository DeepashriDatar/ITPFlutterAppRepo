import 'package:dio/dio.dart';
import 'package:intimepro_mobile/core/network/api_paths.dart';
import 'package:intimepro_mobile/features/auth/data/models/auth_models.dart';

class AuthApi {
  AuthApi(this._dio);

  final Dio _dio;

  Future<ApiEnvelope<LoginDataModel>> login(LoginRequestModel request) async {
    final Response<dynamic> response = await _dio.post<dynamic>(
      '${ApiPaths.auth}/login',
      data: request.toJson(),
    );

    final Map<String, dynamic> body = response.data as Map<String, dynamic>;
    return ApiEnvelope<LoginDataModel>.fromJson(body, LoginDataModel.fromJson);
  }

  Future<ApiEnvelope<RefreshDataModel>> refresh(RefreshTokenRequestModel request) async {
    final Response<dynamic> response = await _dio.post<dynamic>(
      '${ApiPaths.auth}/refresh',
      data: request.toJson(),
    );

    final Map<String, dynamic> body = response.data as Map<String, dynamic>;
    return ApiEnvelope<RefreshDataModel>.fromJson(body, RefreshDataModel.fromJson);
  }

  Future<ApiEnvelope<AuthUserModel>> me(String accessToken) async {
    final Response<dynamic> response = await _dio.get<dynamic>(
      '${ApiPaths.auth}/me',
      options: Options(headers: <String, dynamic>{'Authorization': 'Bearer $accessToken'}),
    );

    final Map<String, dynamic> body = response.data as Map<String, dynamic>;
    return ApiEnvelope<AuthUserModel>.fromJson(body, AuthUserModel.fromJson);
  }

  Future<void> forgotPassword(ForgotPasswordRequestModel request) async {
    await _dio.post<dynamic>(
      '${ApiPaths.auth}/forgot-password',
      data: request.toJson(),
    );
  }

  Future<void> logout(String accessToken) async {
    await _dio.post<dynamic>(
      '${ApiPaths.auth}/logout',
      options: Options(headers: <String, dynamic>{'Authorization': 'Bearer $accessToken'}),
    );
  }
}
