import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intimepro_mobile/features/dashboard/data/models/dashboard_summary_model.dart';
import 'package:intimepro_mobile/features/dashboard/data/repositories/dashboard_repository_impl.dart';

part 'dashboard_event.dart';
part 'dashboard_state.dart';

class DashboardBloc extends Bloc<DashboardEvent, DashboardState> {
  DashboardBloc({required DashboardRepositoryImpl dashboardRepository})
      : _dashboardRepository = dashboardRepository,
        super(const DashboardState()) {
    on<DashboardLoaded>(_onLoaded);
    on<DashboardTaskActionRequested>(_onTaskActionRequested);
  }

  final DashboardRepositoryImpl _dashboardRepository;

  Future<void> _onLoaded(DashboardLoaded event, Emitter<DashboardState> emit) async {
    emit(state.copyWith(status: DashboardStatus.loading, clearMessage: true));
    try {
      final summary = await _dashboardRepository.getSummary();
      emit(state.copyWith(status: DashboardStatus.ready, summary: summary, clearMessage: true));
    } catch (error) {
      emit(state.copyWith(status: DashboardStatus.failure, message: error.toString()));
    }
  }

  Future<void> _onTaskActionRequested(
    DashboardTaskActionRequested event,
    Emitter<DashboardState> emit,
  ) async {
    emit(state.copyWith(status: DashboardStatus.loading, clearMessage: true));
    try {
      switch (event.action) {
        case DashboardTaskAction.start:
          await _dashboardRepository.startTask();
        case DashboardTaskAction.pause:
          await _dashboardRepository.pauseTask();
        case DashboardTaskAction.complete:
          await _dashboardRepository.completeTask();
      }

      final summary = await _dashboardRepository.getSummary();
      emit(state.copyWith(status: DashboardStatus.ready, summary: summary, clearMessage: true));
    } catch (error) {
      emit(state.copyWith(status: DashboardStatus.failure, message: error.toString()));
    }
  }
}
