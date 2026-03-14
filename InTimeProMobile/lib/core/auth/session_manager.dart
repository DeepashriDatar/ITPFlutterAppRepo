import 'package:intimepro_mobile/core/storage/secure_token_store.dart';

class SessionManager {
  SessionManager(this._tokenStore);

  final SecureTokenStore _tokenStore;

  Future<bool> hasActiveSession() async {
    final DateTime? expiry = await _tokenStore.readExpiryUtc();
    final String? access = await _tokenStore.readAccessToken();
    if (expiry == null || access == null || access.isEmpty) {
      return false;
    }

    return DateTime.now().toUtc().isBefore(expiry);
  }

  Future<void> endSession() => _tokenStore.clear();
}
