class ApiPaths {
  ApiPaths._();

  static const String apiVersion = 'v1';
  static const String basePrefix = '/api/$apiVersion';

  static const String auth = '$basePrefix/auth';
  static const String dashboard = '$basePrefix/dashboard';
  static const String tasks = '$basePrefix/tasks';
  static const String projects = '$basePrefix/projects';
  static const String timesheets = '$basePrefix/timesheets';
  static const String leaves = '$basePrefix/leaves';
  static const String notifications = '$basePrefix/notifications';
  static const String settings = '$basePrefix/settings';
}
