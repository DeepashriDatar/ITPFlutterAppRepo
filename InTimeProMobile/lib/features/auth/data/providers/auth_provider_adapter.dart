class SocialAuthToken {
  const SocialAuthToken({
    required this.email,
    required this.idToken,
  });

  final String email;
  final String idToken;
}

abstract class AuthProviderAdapter {
  Future<SocialAuthToken> signIn();

  Future<void> signOut();
}
