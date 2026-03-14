import 'package:flutter_dotenv/flutter_dotenv.dart';

class EnvConfig {
  EnvConfig._();

  static bool _initialized = false;

  static Future<void> load() async {
    if (_initialized) {
      return;
    }

    await dotenv.load(fileName: '.env');
    _initialized = true;
  }

  static String get apiBaseUrl {
    return dotenv.env['API_BASE_URL'] ?? 'https://localhost:7058';
  }

  static String get googleClientId {
    return dotenv.env['GOOGLE_CLIENT_ID'] ?? '';
  }

  static String get microsoftClientId {
    return dotenv.env['MICROSOFT_CLIENT_ID'] ?? '';
  }

  static String get microsoftTenantId {
    return dotenv.env['MICROSOFT_TENANT_ID'] ?? '';
  }
}
