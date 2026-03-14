part of 'auth_bloc.dart';

abstract class AuthEvent extends Equatable {
  const AuthEvent();

  @override
  List<Object?> get props => <Object?>[];
}

class AuthStarted extends AuthEvent {
  const AuthStarted();
}

class AuthLoginSubmitted extends AuthEvent {
  const AuthLoginSubmitted({
    required this.email,
    required this.password,
    required this.rememberMe,
  });

  final String email;
  final String password;
  final bool rememberMe;

  @override
  List<Object?> get props => <Object?>[email, password, rememberMe];
}

class AuthGoogleLoginRequested extends AuthEvent {
  const AuthGoogleLoginRequested({required this.rememberMe});

  final bool rememberMe;

  @override
  List<Object?> get props => <Object?>[rememberMe];
}

class AuthMicrosoftLoginRequested extends AuthEvent {
  const AuthMicrosoftLoginRequested({required this.rememberMe});

  final bool rememberMe;

  @override
  List<Object?> get props => <Object?>[rememberMe];
}

class AuthForgotPasswordRequested extends AuthEvent {
  const AuthForgotPasswordRequested({required this.email});

  final String email;

  @override
  List<Object?> get props => <Object?>[email];
}

class AuthLogoutRequested extends AuthEvent {
  const AuthLogoutRequested();
}
