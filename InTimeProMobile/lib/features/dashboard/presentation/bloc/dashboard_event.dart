part of 'dashboard_bloc.dart';

enum DashboardTaskAction { start, pause, complete }

sealed class DashboardEvent extends Equatable {
  const DashboardEvent();

  @override
  List<Object?> get props => <Object?>[];
}

class DashboardLoaded extends DashboardEvent {
  const DashboardLoaded();
}

class DashboardTaskActionRequested extends DashboardEvent {
  const DashboardTaskActionRequested(this.action);

  final DashboardTaskAction action;

  @override
  List<Object?> get props => <Object?>[action];
}
