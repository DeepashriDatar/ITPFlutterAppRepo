import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intimepro_mobile/features/auth/presentation/bloc/auth_bloc.dart';
import 'package:intimepro_mobile/features/auth/presentation/pages/forgot_password_page.dart';
import 'package:intimepro_mobile/features/dashboard/data/repositories/dashboard_repository_impl.dart';
import 'package:intimepro_mobile/features/dashboard/presentation/bloc/dashboard_bloc.dart';
import 'package:intimepro_mobile/features/dashboard/presentation/pages/dashboard_page.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  bool _rememberMe = true;

  @override
  void initState() {
    super.initState();
    context.read<AuthBloc>().add(const AuthStarted());
  }

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('InTimePro Login')),
      body: BlocConsumer<AuthBloc, AuthState>(
        listenWhen: (AuthState previous, AuthState current) =>
            previous.status != current.status || previous.message != current.message,
        listener: (BuildContext context, AuthState state) {
          if (state.status == AuthStatus.authenticated) {
            final String userName = state.session?.user.name ?? 'Employee';
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(content: Text('Welcome back, $userName')),
            );
          }

          if (state.status == AuthStatus.failure && state.message != null) {
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(content: Text(state.message!)),
            );
          }
        },
        builder: (BuildContext context, AuthState state) {
          if (state.status == AuthStatus.authenticated) {
            return BlocProvider<DashboardBloc>(
              create: (BuildContext context) => DashboardBloc(
                dashboardRepository: context.read<DashboardRepositoryImpl>(),
              ),
              child: const DashboardPage(),
            );
          }

          final bool busy = state.status == AuthStatus.loading;

          return AbsorbPointer(
            absorbing: busy,
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  TextField(
                    controller: _emailController,
                    keyboardType: TextInputType.emailAddress,
                    decoration: const InputDecoration(
                      labelText: 'Email',
                      border: OutlineInputBorder(),
                    ),
                  ),
                  const SizedBox(height: 12),
                  TextField(
                    controller: _passwordController,
                    obscureText: true,
                    decoration: const InputDecoration(
                      labelText: 'Password',
                      border: OutlineInputBorder(),
                    ),
                  ),
                  const SizedBox(height: 8),
                  CheckboxListTile(
                    value: _rememberMe,
                    onChanged: (bool? value) {
                      setState(() {
                        _rememberMe = value ?? true;
                      });
                    },
                    contentPadding: EdgeInsets.zero,
                    title: const Text('Remember me'),
                  ),
                  const SizedBox(height: 8),
                  ElevatedButton(
                    onPressed: () {
                      context.read<AuthBloc>().add(
                            AuthLoginSubmitted(
                              email: _emailController.text,
                              password: _passwordController.text,
                              rememberMe: _rememberMe,
                            ),
                          );
                    },
                    child: const Text('Sign In'),
                  ),
                  const SizedBox(height: 8),
                  OutlinedButton(
                    onPressed: () {
                      context
                          .read<AuthBloc>()
                          .add(AuthGoogleLoginRequested(rememberMe: _rememberMe));
                    },
                    child: const Text('Continue with Google'),
                  ),
                  const SizedBox(height: 8),
                  OutlinedButton(
                    onPressed: () {
                      context
                          .read<AuthBloc>()
                          .add(AuthMicrosoftLoginRequested(rememberMe: _rememberMe));
                    },
                    child: const Text('Continue with Microsoft'),
                  ),
                  const SizedBox(height: 8),
                  TextButton(
                    onPressed: () {
                      Navigator.of(context).push(
                        MaterialPageRoute<void>(
                          builder: (_) => const ForgotPasswordPage(),
                        ),
                      );
                    },
                    child: const Text('Forgot password?'),
                  ),
                  if (busy)
                    const Padding(
                      padding: EdgeInsets.only(top: 16),
                      child: Center(child: CircularProgressIndicator()),
                    ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}
