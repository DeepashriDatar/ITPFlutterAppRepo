import 'package:intimepro_mobile/features/auth/data/providers/auth_provider_adapter.dart';

typedef MicrosoftTokenAcquirer = Future<SocialAuthToken> Function();

class MicrosoftAuthProvider implements AuthProviderAdapter {
  MicrosoftAuthProvider({MicrosoftTokenAcquirer? tokenAcquirer}) : _tokenAcquirer = tokenAcquirer;

  final MicrosoftTokenAcquirer? _tokenAcquirer;

  @override
  Future<SocialAuthToken> signIn() async {
    if (_tokenAcquirer == null) {
      throw UnsupportedError(
        'Microsoft token acquisition is not configured. Provide a tokenAcquirer backed by your MSAL integration.',
      );
    }

    return _tokenAcquirer();
  }

  @override
  Future<void> signOut() async {
    // MSAL sign-out should be handled by the app-specific integration passed to tokenAcquirer.
  }
}
