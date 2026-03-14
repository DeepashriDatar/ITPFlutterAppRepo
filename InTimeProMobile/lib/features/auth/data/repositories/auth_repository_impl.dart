import 'package:dio/dio.dart';
import 'package:intimepro_mobile/core/auth/session_manager.dart';
import 'package:intimepro_mobile/core/storage/secure_token_store.dart';
import 'package:intimepro_mobile/features/auth/data/models/auth_models.dart';
import 'package:intimepro_mobile/features/auth/data/providers/google_auth_provider.dart';
import 'package:intimepro_mobile/features/auth/data/providers/microsoft_auth_provider.dart';
import 'package:intimepro_mobile/features/auth/data/remote/auth_api.dart';
import 'package:intimepro_mobile/features/auth/domain/models/auth_session.dart';
import 'package:intimepro_mobile/features/auth/domain/repositories/auth_repository.dart';

class AuthRepositoryImpl implements AuthRepository {
  AuthRepositoryImpl({
    required AuthApi authApi,
    required SecureTokenStore tokenStore,
    required SessionManager sessionManager,
    required GoogleAuthProvider googleAuthProvider,
    required MicrosoftAuthProvider microsoftAuthProvider,
  })  : _authApi = authApi,
        _tokenStore = tokenStore,
        _sessionManager = sessionManager,
        _googleAuthProvider = googleAuthProvider,
        _microsoftAuthProvider = microsoftAuthProvider;

  final AuthApi _authApi;
  final SecureTokenStore _tokenStore;
  final SessionManager _sessionManager;
  final GoogleAuthProvider _googleAuthProvider;
  final MicrosoftAuthProvider _microsoftAuthProvider;

  AuthSession? _currentSession;

  @override
  Future<AuthSession?> restoreSession() async {
    if (_currentSession != null && !_currentSession!.isExpired) {
      return _currentSession;
    }

    final bool hasActiveSession = await _sessionManager.hasActiveSession();
    if (hasActiveSession) {
      final String? accessToken = await _tokenStore.readAccessToken();
      if (accessToken != null && accessToken.isNotEmpty) {
        try {
          final ApiEnvelope<AuthUserModel> meEnvelope = await _authApi.me(accessToken);
          final AuthUserModel? user = meEnvelope.data;
          final String? refreshToken = await _tokenStore.readRefreshToken();
          final DateTime? expiresAtUtc = await _tokenStore.readExpiryUtc();

          if (user != null && refreshToken != null && expiresAtUtc != null) {
            _currentSession = AuthSession(
              user: user.toDomain(),
              accessToken: accessToken,
              refreshToken: refreshToken,
              expiresAtUtc: expiresAtUtc,
            );
            return _currentSession;
          }
        } on DioException {
          // If the access token is invalid, continue to refresh flow below.
        }
      }
    }

    final String? refreshToken = await _tokenStore.readRefreshToken();
    if (refreshToken == null || refreshToken.isEmpty) {
      return null;
    }

    try {
      return await refreshSession();
    } on DioException {
      await _sessionManager.endSession();
      _currentSession = null;
      return null;
    }
  }

  @override
  Future<AuthSession> loginWithEmail({
    required String email,
    required String password,
    required bool rememberMe,
  }) {
    return _login(
      LoginRequestModel(
        email: email,
        password: password,
        provider: 'email',
        rememberMe: rememberMe,
      ),
    );
  }

  @override
  Future<AuthSession> loginWithGoogle({required bool rememberMe}) async {
    final socialToken = await _googleAuthProvider.signIn();
    return _login(
      LoginRequestModel(
        email: socialToken.email,
        provider: 'google',
        socialToken: socialToken.idToken,
        rememberMe: rememberMe,
      ),
    );
  }

  @override
  Future<AuthSession> loginWithMicrosoft({required bool rememberMe}) async {
    final socialToken = await _microsoftAuthProvider.signIn();
    return _login(
      LoginRequestModel(
        email: socialToken.email,
        provider: 'microsoft',
        socialToken: socialToken.idToken,
        rememberMe: rememberMe,
      ),
    );
  }

  @override
  Future<void> forgotPassword({required String email}) {
    return _authApi.forgotPassword(ForgotPasswordRequestModel(email: email));
  }

  @override
  Future<AuthSession> refreshSession() async {
    final String? refreshToken = await _tokenStore.readRefreshToken();
    if (refreshToken == null || refreshToken.isEmpty) {
      throw StateError('No refresh token available');
    }

    final ApiEnvelope<RefreshDataModel> refreshEnvelope = await _authApi
        .refresh(RefreshTokenRequestModel(refreshToken: refreshToken));
    final RefreshDataModel? refreshData = refreshEnvelope.data;
    if (!refreshEnvelope.success || refreshData == null) {
      throw StateError(refreshEnvelope.message ?? 'Refresh token request failed');
    }

    final String accessToken = refreshData.accessToken;
    final DateTime expiresAtUtc = DateTime.now().toUtc().add(
          Duration(seconds: refreshData.expiresIn),
        );

    final ApiEnvelope<AuthUserModel> meEnvelope = await _authApi.me(accessToken);
    final AuthUserModel? user = meEnvelope.data;
    if (user == null) {
      throw StateError(meEnvelope.message ?? 'Could not load profile after refresh');
    }

    await _tokenStore.saveTokens(
      accessToken: accessToken,
      refreshToken: refreshToken,
      expiresAtUtc: expiresAtUtc,
    );

    _currentSession = AuthSession(
      user: user.toDomain(),
      accessToken: accessToken,
      refreshToken: refreshToken,
      expiresAtUtc: expiresAtUtc,
    );

    return _currentSession!;
  }

  @override
  Future<T> withAutoRefresh<T>(Future<T> Function(String accessToken) action) async {
    AuthSession? session = _currentSession ?? await restoreSession();
    if (session == null) {
      throw StateError('No active session');
    }

    if (session.isExpired) {
      session = await refreshSession();
    }

    try {
      return await action(session.accessToken);
    } on DioException catch (error) {
      if (error.response?.statusCode != 401) {
        rethrow;
      }

      final AuthSession refreshed = await refreshSession();
      return action(refreshed.accessToken);
    }
  }

  @override
  Future<void> logout() async {
    final String? accessToken = _currentSession?.accessToken ?? await _tokenStore.readAccessToken();
    if (accessToken != null && accessToken.isNotEmpty) {
      try {
        await _authApi.logout(accessToken);
      } on DioException {
        // Server logout best effort; local cleanup still required.
      }
    }

    await _googleAuthProvider.signOut();
    await _microsoftAuthProvider.signOut();
    await _sessionManager.endSession();
    _currentSession = null;
  }

  Future<AuthSession> _login(LoginRequestModel request) async {
    final ApiEnvelope<LoginDataModel> envelope = await _authApi.login(request);
    final LoginDataModel? data = envelope.data;
    if (!envelope.success || data == null) {
      throw StateError(envelope.message ?? envelope.errors?.join(', ') ?? 'Login failed');
    }

    final AuthSession session = data.toDomain();
    await _tokenStore.saveTokens(
      accessToken: session.accessToken,
      refreshToken: session.refreshToken,
      expiresAtUtc: session.expiresAtUtc,
    );

    _currentSession = session;
    return session;
  }
}
