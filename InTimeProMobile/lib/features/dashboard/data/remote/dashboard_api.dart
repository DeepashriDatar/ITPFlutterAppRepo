import 'package:dio/dio.dart';
import 'package:intimepro_mobile/core/network/api_paths.dart';
import 'package:intimepro_mobile/features/auth/data/models/auth_models.dart';
import 'package:intimepro_mobile/features/dashboard/data/models/dashboard_summary_model.dart';

class DashboardApi {
  DashboardApi(this._dio);

  final Dio _dio;

  Future<ApiEnvelope<DashboardSummaryModel>> getSummary(String accessToken) async {
    final Response<dynamic> response = await _dio.get<dynamic>(
      '${ApiPaths.dashboard}/summary',
      options: Options(headers: <String, dynamic>{'Authorization': 'Bearer $accessToken'}),
    );

    final Map<String, dynamic> body = response.data as Map<String, dynamic>;
    return ApiEnvelope<DashboardSummaryModel>.fromJson(body, DashboardSummaryModel.fromJson);
  }

  Future<void> updateActiveTaskState({
    required String accessToken,
    required String action,
  }) async {
    await _dio.post<dynamic>(
      '${ApiPaths.dashboard}/active-task/actions/$action',
      options: Options(headers: <String, dynamic>{'Authorization': 'Bearer $accessToken'}),
    );
  }
}
