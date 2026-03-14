import 'package:dio/dio.dart';
import 'package:intimepro_mobile/core/error/app_failure.dart';

class ErrorPresenter {
  AppFailure map(Object error) {
    if (error is DioException) {
      final int? status = error.response?.statusCode;
      if (status == 401) {
        return const AppFailure(code: 'unauthorized', message: 'Your session has expired. Please sign in again.');
      }
      if (status == 403) {
        return const AppFailure(code: 'forbidden', message: 'You do not have access to this action.');
      }
      if (status == 422) {
        return const AppFailure(code: 'validation', message: 'Please check the provided values and try again.');
      }
      if ((status ?? 0) >= 500) {
        return const AppFailure(code: 'server', message: 'Service is temporarily unavailable. Please retry shortly.');
      }

      return AppFailure(code: 'network', message: 'Unable to complete request.', details: error.message);
    }

    return AppFailure(code: 'unknown', message: 'Unexpected error occurred.', details: error.toString());
  }
}
