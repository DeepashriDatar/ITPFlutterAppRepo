import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intimepro_mobile/features/auth/domain/models/auth_session.dart';
import 'package:intimepro_mobile/features/auth/domain/repositories/auth_repository.dart';

part 'auth_event.dart';
part 'auth_state.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc({required AuthRepository authRepository})
      : _authRepository = authRepository,
        super(const AuthState()) {
    on<AuthStarted>(_onStarted);
    on<AuthLoginSubmitted>(_onEmailLoginSubmitted);
    on<AuthGoogleLoginRequested>(_onGoogleLoginRequested);
    on<AuthMicrosoftLoginRequested>(_onMicrosoftLoginRequested);
    on<AuthForgotPasswordRequested>(_onForgotPasswordRequested);
    on<AuthLogoutRequested>(_onLogoutRequested);
  }

  final AuthRepository _authRepository;

  Future<void> _onStarted(AuthStarted event, Emitter<AuthState> emit) async {
    emit(state.copyWith(status: AuthStatus.loading, clearMessage: true));

    try {
      final AuthSession? session = await _authRepository.restoreSession();
      if (session == null) {
        emit(state.copyWith(status: AuthStatus.unauthenticated, clearMessage: true));
        return;
      }

      emit(
        state.copyWith(
          status: AuthStatus.authenticated,
          session: session,
          clearMessage: true,
        ),
      );
    } catch (error) {
      emit(
        state.copyWith(
          status: AuthStatus.failure,
          message: 'Unable to restore your session. Please sign in again.',
        ),
      );
    }
  }

  Future<void> _onEmailLoginSubmitted(
    AuthLoginSubmitted event,
    Emitter<AuthState> emit,
  ) async {
    emit(state.copyWith(status: AuthStatus.loading, clearMessage: true));

    try {
      final AuthSession session = await _authRepository.loginWithEmail(
        email: event.email.trim(),
        password: event.password,
        rememberMe: event.rememberMe,
      );
      emit(state.copyWith(status: AuthStatus.authenticated, session: session, clearMessage: true));
    } catch (error) {
      emit(
        state.copyWith(
          status: AuthStatus.failure,
          message: error.toString().replaceFirst('Bad state: ', ''),
        ),
      );
    }
  }

  Future<void> _onGoogleLoginRequested(
    AuthGoogleLoginRequested event,
    Emitter<AuthState> emit,
  ) async {
    emit(state.copyWith(status: AuthStatus.loading, clearMessage: true));

    try {
      final AuthSession session =
          await _authRepository.loginWithGoogle(rememberMe: event.rememberMe);
      emit(state.copyWith(status: AuthStatus.authenticated, session: session, clearMessage: true));
    } catch (error) {
      emit(
        state.copyWith(
          status: AuthStatus.failure,
          message: error.toString().replaceFirst('Bad state: ', ''),
        ),
      );
    }
  }

  Future<void> _onMicrosoftLoginRequested(
    AuthMicrosoftLoginRequested event,
    Emitter<AuthState> emit,
  ) async {
    emit(state.copyWith(status: AuthStatus.loading, clearMessage: true));

    try {
      final AuthSession session =
          await _authRepository.loginWithMicrosoft(rememberMe: event.rememberMe);
      emit(state.copyWith(status: AuthStatus.authenticated, session: session, clearMessage: true));
    } catch (error) {
      emit(
        state.copyWith(
          status: AuthStatus.failure,
          message: error.toString().replaceFirst('Bad state: ', ''),
        ),
      );
    }
  }

  Future<void> _onForgotPasswordRequested(
    AuthForgotPasswordRequested event,
    Emitter<AuthState> emit,
  ) async {
    emit(state.copyWith(status: AuthStatus.loading, clearMessage: true));

    try {
      await _authRepository.forgotPassword(email: event.email.trim());
      emit(
        state.copyWith(
          status: AuthStatus.forgotPasswordSent,
          message: 'If this email is registered, a reset link has been sent.',
        ),
      );
    } catch (error) {
      emit(
        state.copyWith(
          status: AuthStatus.failure,
          message: 'Could not request a password reset right now.',
        ),
      );
    }
  }

  Future<void> _onLogoutRequested(AuthLogoutRequested event, Emitter<AuthState> emit) async {
    emit(state.copyWith(status: AuthStatus.loading, clearMessage: true));
    await _authRepository.logout();
    emit(const AuthState(status: AuthStatus.unauthenticated));
  }
}
