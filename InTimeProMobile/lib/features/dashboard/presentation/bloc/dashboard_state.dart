part of 'dashboard_bloc.dart';

enum DashboardStatus { initial, loading, ready, failure }

class DashboardState extends Equatable {
  const DashboardState({
    this.status = DashboardStatus.initial,
    this.summary,
    this.message,
  });

  final DashboardStatus status;
  final DashboardSummaryModel? summary;
  final String? message;

  DashboardState copyWith({
    DashboardStatus? status,
    DashboardSummaryModel? summary,
    String? message,
    bool clearMessage = false,
  }) {
    return DashboardState(
      status: status ?? this.status,
      summary: summary ?? this.summary,
      message: clearMessage ? null : (message ?? this.message),
    );
  }

  @override
  List<Object?> get props => <Object?>[status, summary, message];
}
