import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:dio/dio.dart';
import 'package:intimepro_mobile/features/auth/domain/models/auth_session.dart';
import 'package:intimepro_mobile/features/auth/domain/repositories/auth_repository.dart';
import 'package:intimepro_mobile/features/dashboard/data/remote/dashboard_api.dart';
import 'package:intimepro_mobile/features/dashboard/data/models/dashboard_summary_model.dart';
import 'package:intimepro_mobile/features/dashboard/data/repositories/dashboard_repository_impl.dart';
import 'package:intimepro_mobile/features/dashboard/presentation/bloc/dashboard_bloc.dart';
import 'package:intimepro_mobile/features/dashboard/presentation/pages/dashboard_page.dart';

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  testWidgets('dashboard displays summary metrics', (WidgetTester tester) async {
    final fakeRepository = _FakeDashboardRepository();

    await tester.pumpWidget(
      MaterialApp(
        home: BlocProvider<DashboardBloc>(
          create: (_) => DashboardBloc(dashboardRepository: fakeRepository),
          child: const DashboardPage(),
        ),
      ),
    );

    await tester.pumpAndSettle();

    expect(find.textContaining('Clock-in Time:'), findsOneWidget);
    expect(find.textContaining('Active Hours:'), findsOneWidget);
    expect(find.textContaining('Total Work Time:'), findsOneWidget);
  });
}

class _FakeDashboardRepository extends DashboardRepositoryImpl {
  _FakeDashboardRepository()
      : super(
          dashboardApi: _NoopDashboardApi(),
          authRepository: _NoopAuthRepository(),
        );

  @override
  Future<DashboardSummaryModel> getSummary() async {
    return const DashboardSummaryModel(
      clockInTime: '08:30',
      activeWorkingHours: '2h 12m',
      totalWorkTime: '2h 12m',
      activeTaskName: 'Task A',
      activeTaskState: 'In Progress',
      activeTaskElapsedSeconds: 7920,
    );
  }

  @override
  Future<void> startTask() async {}

  @override
  Future<void> pauseTask() async {}

  @override
  Future<void> completeTask() async {}
}

class _NoopDashboardApi extends DashboardApi {
  _NoopDashboardApi() : super(Dio());
}

class _NoopAuthRepository implements AuthRepository {
  @override
  Future<void> forgotPassword({required String email}) async {}

  @override
  Future<AuthSession> loginWithEmail({required String email, required String password, required bool rememberMe}) {
    throw UnimplementedError();
  }

  @override
  Future<AuthSession> loginWithGoogle({required bool rememberMe}) {
    throw UnimplementedError();
  }

  @override
  Future<AuthSession> loginWithMicrosoft({required bool rememberMe}) {
    throw UnimplementedError();
  }

  @override
  Future<void> logout() async {}

  @override
  Future<AuthSession> refreshSession() {
    throw UnimplementedError();
  }

  @override
  Future<AuthSession?> restoreSession() async => null;

  @override
  Future<T> withAutoRefresh<T>(Future<T> Function(String accessToken) action) {
    return action('token');
  }
}
