import 'package:equatable/equatable.dart';

class AuthUser extends Equatable {
  const AuthUser({
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

  @override
  List<Object?> get props => <Object?>[id, email, name, role, department, phone, avatarUrl];
}

class AuthSession extends Equatable {
  const AuthSession({
    required this.user,
    required this.accessToken,
    required this.refreshToken,
    required this.expiresAtUtc,
  });

  final AuthUser user;
  final String accessToken;
  final String refreshToken;
  final DateTime expiresAtUtc;

  bool get isExpired => DateTime.now().toUtc().isAfter(expiresAtUtc);

  @override
  List<Object?> get props => <Object?>[user, accessToken, refreshToken, expiresAtUtc];
}
