import 'package:equatable/equatable.dart';

class DashboardSummaryModel extends Equatable {
  const DashboardSummaryModel({
    required this.clockInTime,
    required this.activeWorkingHours,
    required this.totalWorkTime,
    required this.activeTaskName,
    required this.activeTaskState,
    required this.activeTaskElapsedSeconds,
  });

  final String clockInTime;
  final String activeWorkingHours;
  final String totalWorkTime;
  final String activeTaskName;
  final String activeTaskState;
  final int activeTaskElapsedSeconds;

  factory DashboardSummaryModel.fromJson(Map<String, dynamic> json) {
    return DashboardSummaryModel(
      clockInTime: json['clockInTime'] as String? ?? '--:--',
      activeWorkingHours: json['activeWorkingHours'] as String? ?? '0h 00m',
      totalWorkTime: json['totalWorkTime'] as String? ?? '0h 00m',
      activeTaskName: json['activeTaskName'] as String? ?? 'No active task',
      activeTaskState: json['activeTaskState'] as String? ?? 'None',
      activeTaskElapsedSeconds: json['activeTaskElapsedSeconds'] as int? ?? 0,
    );
  }

  @override
  List<Object?> get props => <Object?>[
        clockInTime,
        activeWorkingHours,
        totalWorkTime,
        activeTaskName,
        activeTaskState,
        activeTaskElapsedSeconds,
      ];
}
