# Quickstart Guide

## Prerequisites
- Flutter 3.19+ installed
- Dart SDK
- Android Studio (for Android) or Xcode (for iOS)
- Git

## Setup
1. Clone the repository
2. Run `flutter pub get` to install dependencies
3. Configure Firebase project for authentication
4. Set up local database migrations
5. Run `flutter run` to start the app

## Development
- Use `flutter test` for unit tests
- Use `flutter drive` for integration tests
- Follow clean architecture: presentation → domain → data
- Use BLoC pattern for state management
- Store sensitive data in flutter_secure_storage

## API Configuration
- Base URL: Configure in environment variables
- Authentication: JWT tokens with refresh
- Offline sync: Automatic background sync when online

## Key Features
- Secure authentication with multiple providers
- Offline-capable task and time tracking
- Real-time dashboard with work metrics
- Project visibility and progress tracking
- Timesheet submission and approval workflow