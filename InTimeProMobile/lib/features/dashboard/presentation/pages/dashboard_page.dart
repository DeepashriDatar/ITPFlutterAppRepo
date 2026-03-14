import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intimepro_mobile/features/dashboard/presentation/bloc/dashboard_bloc.dart';

class DashboardPage extends StatefulWidget {
  const DashboardPage({super.key});

  @override
  State<DashboardPage> createState() => _DashboardPageState();
}

class _DashboardPageState extends State<DashboardPage> {
  @override
  void initState() {
    super.initState();
    context.read<DashboardBloc>().add(const DashboardLoaded());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Dashboard')),
      body: BlocBuilder<DashboardBloc, DashboardState>(
        builder: (BuildContext context, DashboardState state) {
          if (state.status == DashboardStatus.loading) {
            return const Center(child: CircularProgressIndicator());
          }

          if (state.status == DashboardStatus.failure) {
            return Center(child: Text(state.message ?? 'Could not load dashboard'));
          }

          final summary = state.summary;
          if (summary == null) {
            return const Center(child: Text('No dashboard data available'));
          }

          return Padding(
            padding: const EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                Text('Clock-in Time: ${summary.clockInTime}'),
                Text('Active Hours: ${summary.activeWorkingHours}'),
                Text('Total Work Time: ${summary.totalWorkTime}'),
                const SizedBox(height: 16),
                Text('Active Task: ${summary.activeTaskName}'),
                Text('State: ${summary.activeTaskState}'),
                Text('Elapsed: ${summary.activeTaskElapsedSeconds}s'),
                const SizedBox(height: 16),
                Wrap(
                  spacing: 8,
                  children: <Widget>[
                    ElevatedButton(
                      onPressed: () => context
                          .read<DashboardBloc>()
                          .add(const DashboardTaskActionRequested(DashboardTaskAction.start)),
                      child: const Text('Start'),
                    ),
                    ElevatedButton(
                      onPressed: () => context
                          .read<DashboardBloc>()
                          .add(const DashboardTaskActionRequested(DashboardTaskAction.pause)),
                      child: const Text('Pause'),
                    ),
                    ElevatedButton(
                      onPressed: () => context
                          .read<DashboardBloc>()
                          .add(const DashboardTaskActionRequested(DashboardTaskAction.complete)),
                      child: const Text('Complete'),
                    ),
                  ],
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}
