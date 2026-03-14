import 'package:intimepro_mobile/features/auth/domain/models/auth_session.dart';

abstract class AuthRepository {
  Future<AuthSession?> restoreSession();

  Future<AuthSession> loginWithEmail({
    required String email,
    required String password,
    required bool rememberMe,
  });

  Future<AuthSession> loginWithGoogle({required bool rememberMe});

  Future<AuthSession> loginWithMicrosoft({required bool rememberMe});

  Future<void> forgotPassword({required String email});

  Future<AuthSession> refreshSession();

  Future<T> withAutoRefresh<T>(Future<T> Function(String accessToken) action);

  Future<void> logout();
}
