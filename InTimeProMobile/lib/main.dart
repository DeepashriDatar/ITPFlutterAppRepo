import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intimepro_mobile/core/auth/session_manager.dart';
import 'package:intimepro_mobile/core/config/env_config.dart';
import 'package:intimepro_mobile/core/network/api_client.dart';
import 'package:intimepro_mobile/core/storage/secure_token_store.dart';
import 'package:intimepro_mobile/features/auth/data/providers/google_auth_provider.dart';
import 'package:intimepro_mobile/features/auth/data/providers/microsoft_auth_provider.dart';
import 'package:intimepro_mobile/features/auth/data/remote/auth_api.dart';
import 'package:intimepro_mobile/features/auth/data/repositories/auth_repository_impl.dart';
import 'package:intimepro_mobile/features/auth/domain/repositories/auth_repository.dart';
import 'package:intimepro_mobile/features/auth/presentation/bloc/auth_bloc.dart';
import 'package:intimepro_mobile/features/auth/presentation/pages/login_page.dart';
import 'package:intimepro_mobile/features/dashboard/data/remote/dashboard_api.dart';
import 'package:intimepro_mobile/features/dashboard/data/repositories/dashboard_repository_impl.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await EnvConfig.load();

  final apiClient = ApiClient();
  final SecureTokenStore tokenStore = SecureTokenStore();
  final AuthRepository authRepository = AuthRepositoryImpl(
    authApi: AuthApi(apiClient.dio),
    tokenStore: tokenStore,
    sessionManager: SessionManager(tokenStore),
    googleAuthProvider: GoogleAuthProvider(),
    microsoftAuthProvider: MicrosoftAuthProvider(),
  );
  final DashboardRepositoryImpl dashboardRepository = DashboardRepositoryImpl(
    dashboardApi: DashboardApi(apiClient.dio),
    authRepository: authRepository,
  );

  runApp(
    InTimeProApp(
      authRepository: authRepository,
      dashboardRepository: dashboardRepository,
    ),
  );
}

class InTimeProApp extends StatelessWidget {
  const InTimeProApp({
    required this.authRepository,
    required this.dashboardRepository,
    super.key,
  });

  final AuthRepository authRepository;
  final DashboardRepositoryImpl dashboardRepository;

  @override
  Widget build(BuildContext context) {
    return MultiRepositoryProvider(
      providers: <RepositoryProvider<dynamic>>[
        RepositoryProvider<AuthRepository>.value(value: authRepository),
        RepositoryProvider<DashboardRepositoryImpl>.value(value: dashboardRepository),
      ],
      child: BlocProvider<AuthBloc>(
        create: (_) => AuthBloc(authRepository: authRepository),
        child: MaterialApp(
          title: 'InTimePro',
          theme: ThemeData(colorSchemeSeed: Colors.indigo, useMaterial3: true),
          home: const LoginPage(),
        ),
      ),
    );
  }
}
