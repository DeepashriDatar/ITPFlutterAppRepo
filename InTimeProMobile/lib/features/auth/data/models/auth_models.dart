import 'package:equatable/equatable.dart';
import 'package:intimepro_mobile/features/auth/domain/models/auth_session.dart';

class ApiEnvelope<T> extends Equatable {
  const ApiEnvelope({
    required this.success,
    required this.data,
    this.message,
    this.errors,
  });

  final bool success;
  final T? data;
  final String? message;
  final List<String>? errors;

  factory ApiEnvelope.fromJson(
    Map<String, dynamic> json,
    T Function(Map<String, dynamic>) dataParser,
  ) {
    final Object? rawData = json['data'];
    return ApiEnvelope<T>(
      success: json['success'] as bool? ?? false,
      data: rawData is Map<String, dynamic> ? dataParser(rawData) : null,
      message: json['message'] as String?,
      errors: (json['errors'] as List<dynamic>?)?.map((dynamic e) => e.toString()).toList(),
    );
  }

  @override
  List<Object?> get props => <Object?>[success, data, message, errors];
}

class LoginRequestModel extends Equatable {
  const LoginRequestModel({
    required this.email,
    required this.provider,
    required this.rememberMe,
    this.password,
    this.socialToken,
  });

  final String email;
  final String provider;
  final bool rememberMe;
  final String? password;
  final String? socialToken;

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      'email': email,
      'password': password,
      'provider': provider,
      'socialToken': socialToken,
      'rememberMe': rememberMe,
    };
  }

  @override
  List<Object?> get props => <Object?>[email, password, provider, socialToken, rememberMe];
}

class RefreshTokenRequestModel extends Equatable {
  const RefreshTokenRequestModel({required this.refreshToken});

  final String refreshToken;

  Map<String, dynamic> toJson() => <String, dynamic>{'refreshToken': refreshToken};

  @override
  List<Object?> get props => <Object?>[refreshToken];
}

class ForgotPasswordRequestModel extends Equatable {
  const ForgotPasswordRequestModel({required this.email});

  final String email;

  Map<String, dynamic> toJson() => <String, dynamic>{'email': email};

  @override
  List<Object?> get props => <Object?>[email];
}

class TokenModel extends Equatable {
  const TokenModel({
    required this.accessToken,
    required this.refreshToken,
    required this.expiresIn,
  });

  final String accessToken;
  final String refreshToken;
  final int expiresIn;

  factory TokenModel.fromJson(Map<String, dynamic> json) {
    return TokenModel(
      accessToken: json['accessToken'] as String? ?? '',
      refreshToken: json['refreshToken'] as String? ?? '',
      expiresIn: json['expiresIn'] as int? ?? 0,
    );
  }

  @override
  List<Object?> get props => <Object?>[accessToken, refreshToken, expiresIn];
}

class AuthUserModel extends Equatable {
  const AuthUserModel({
    required this.id,
    required this.email,
    required this.name,
    required this.role,
    this.department,
    this.phone,
    this.avatarUrl,
  });

  final String id;
  final String email;
  final String name;
  final String role;
  final String? department;
  final String? phone;
  final String? avatarUrl;

  factory AuthUserModel.fromJson(Map<String, dynamic> json) {
    return AuthUserModel(
      id: json['id']?.toString() ?? '',
      email: json['email'] as String? ?? '',
      name: json['name'] as String? ?? '',
      role: json['role'] as String? ?? 'Employee',
      department: json['department'] as String?,
      phone: json['phone'] as String?,
      avatarUrl: json['avatarUrl'] as String?,
    );
  }

  AuthUser toDomain() {
    return AuthUser(
      id: id,
      email: email,
      name: name,
      role: role,
      department: department,
      phone: phone,
      avatarUrl: avatarUrl,
    );
  }

  @override
  List<Object?> get props => <Object?>[id, email, name, role, department, phone, avatarUrl];
}

class LoginDataModel extends Equatable {
  const LoginDataModel({required this.user, required this.tokens});

  final AuthUserModel user;
  final TokenModel tokens;

  factory LoginDataModel.fromJson(Map<String, dynamic> json) {
    return LoginDataModel(
      user: AuthUserModel.fromJson(json['user'] as Map<String, dynamic>? ?? <String, dynamic>{}),
      tokens: TokenModel.fromJson(json['tokens'] as Map<String, dynamic>? ?? <String, dynamic>{}),
    );
  }

  AuthSession toDomain() {
    final DateTime expiresAtUtc =
        DateTime.now().toUtc().add(Duration(seconds: tokens.expiresIn));
    return AuthSession(
      user: user.toDomain(),
      accessToken: tokens.accessToken,
      refreshToken: tokens.refreshToken,
      expiresAtUtc: expiresAtUtc,
    );
  }

  @override
  List<Object?> get props => <Object?>[user, tokens];
}

class RefreshDataModel extends Equatable {
  const RefreshDataModel({required this.accessToken, required this.expiresIn});

  final String accessToken;
  final int expiresIn;

  factory RefreshDataModel.fromJson(Map<String, dynamic> json) {
    return RefreshDataModel(
      accessToken: json['accessToken'] as String? ?? '',
      expiresIn: json['expiresIn'] as int? ?? 0,
    );
  }

  @override
  List<Object?> get props => <Object?>[accessToken, expiresIn];
}
