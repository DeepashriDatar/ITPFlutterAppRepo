import 'package:google_sign_in/google_sign_in.dart';
import 'package:intimepro_mobile/core/config/env_config.dart';
import 'package:intimepro_mobile/features/auth/data/providers/auth_provider_adapter.dart';

class GoogleAuthProvider implements AuthProviderAdapter {
  GoogleAuthProvider({GoogleSignIn? googleSignIn})
      : _googleSignIn = googleSignIn ?? GoogleSignIn.instance;

  final GoogleSignIn _googleSignIn;
  bool _initialized = false;

  @override
  Future<SocialAuthToken> signIn() async {
    if (!_initialized) {
      await _googleSignIn.initialize(
        clientId: EnvConfig.googleClientId.isEmpty ? null : EnvConfig.googleClientId,
      );
      _initialized = true;
    }

    final GoogleSignInAccount account = await _googleSignIn.authenticate(
      scopeHint: const <String>['email'],
    );

    final GoogleSignInAuthentication auth = account.authentication;
    final String? idToken = auth.idToken;
    if (idToken == null || idToken.isEmpty) {
      throw StateError('Google sign-in did not return an ID token');
    }

    return SocialAuthToken(email: account.email, idToken: idToken);
  }

  @override
  Future<void> signOut() async {
    await _googleSignIn.signOut();
  }
}
