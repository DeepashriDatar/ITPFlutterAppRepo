class AppFailure {
  const AppFailure({required this.code, required this.message, this.details});

  final String code;
  final String message;
  final String? details;
}
