import 'package:intimepro_mobile/features/auth/domain/repositories/auth_repository.dart';
import 'package:intimepro_mobile/features/dashboard/data/models/dashboard_summary_model.dart';
import 'package:intimepro_mobile/features/dashboard/data/remote/dashboard_api.dart';

class DashboardRepositoryImpl {
  DashboardRepositoryImpl({
    required DashboardApi dashboardApi,
    required AuthRepository authRepository,
  })  : _dashboardApi = dashboardApi,
        _authRepository = authRepository;

  final DashboardApi _dashboardApi;
  final AuthRepository _authRepository;

  Future<DashboardSummaryModel> getSummary() {
    return _authRepository.withAutoRefresh((String accessToken) async {
      final envelope = await _dashboardApi.getSummary(accessToken);
      final summary = envelope.data;
      if (!envelope.success || summary == null) {
        throw StateError(envelope.message ?? 'Failed to load dashboard summary');
      }
      return summary;
    });
  }

  Future<void> startTask() => _setState('start');
  Future<void> pauseTask() => _setState('pause');
  Future<void> completeTask() => _setState('complete');

  Future<void> _setState(String action) {
    return _authRepository.withAutoRefresh((String accessToken) async {
      await _dashboardApi.updateActiveTaskState(accessToken: accessToken, action: action);
    });
  }
}
