import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:intimepro_mobile/features/auth/domain/models/auth_session.dart';
import 'package:intimepro_mobile/features/auth/domain/repositories/auth_repository.dart';
import 'package:intimepro_mobile/features/auth/presentation/bloc/auth_bloc.dart';
import 'package:intimepro_mobile/features/auth/presentation/pages/login_page.dart';

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  testWidgets('email login authenticates and shows authenticated shell', (WidgetTester tester) async {
    final FakeAuthRepository repository = FakeAuthRepository();

    await tester.pumpWidget(
      BlocProvider<AuthBloc>(
        create: (_) => AuthBloc(authRepository: repository),
        child: const MaterialApp(home: LoginPage()),
      ),
    );

    await tester.pumpAndSettle();

    await tester.enterText(find.byType(TextField).at(0), 'employee@company.com');
    await tester.enterText(find.byType(TextField).at(1), 'P@ssw0rd!');
    await tester.tap(find.text('Sign In'));
    await tester.pumpAndSettle();

    expect(find.textContaining('Authenticated as'), findsOneWidget);
  });
}

class FakeAuthRepository implements AuthRepository {
  @override
  Future<void> forgotPassword({required String email}) async {}

  @override
  Future<AuthSession> loginWithEmail({
    required String email,
    required String password,
    required bool rememberMe,
  }) async {
    return _session(accessToken: 'email-token');
  }

  @override
  Future<AuthSession> loginWithGoogle({required bool rememberMe}) async {
    return _session(accessToken: 'google-token');
  }

  @override
  Future<AuthSession> loginWithMicrosoft({required bool rememberMe}) async {
    return _session(accessToken: 'microsoft-token');
  }

  @override
  Future<void> logout() async {}

  @override
  Future<AuthSession> refreshSession() async => _session(accessToken: 'refreshed-token');

  @override
  Future<AuthSession?> restoreSession() async => null;

  @override
  Future<T> withAutoRefresh<T>(Future<T> Function(String accessToken) action) {
    return action('token');
  }

  AuthSession _session({required String accessToken}) {
    return AuthSession(
      user: const AuthUser(
        id: '1',
        email: 'employee@company.com',
        name: 'Employee',
        role: 'Employee',
      ),
      accessToken: accessToken,
      refreshToken: 'refresh-token',
      expiresAtUtc: DateTime.now().toUtc().add(const Duration(hours: 1)),
    );
  }
}
