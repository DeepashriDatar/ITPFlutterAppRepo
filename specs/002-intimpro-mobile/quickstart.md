# Quickstart: Implementing Feature 002

## 1. Prerequisites
- Flutter latest stable (Dart 3.x)
- Android Studio/Xcode toolchains
- .NET SDK 8.0+
- SQL Server (LocalDB for local dev is acceptable)
- Firebase project (for push notifications)
- Google and Microsoft OAuth app credentials

## 2. Backend Setup (existing InTimeProAPI)
1. Configure `InTimeProAPI/appsettings.Development.json` (or environment secrets):
   - `ConnectionStrings:DefaultConnection` with explicit instance/database/auth mode.
   - Enforce `Encrypt=True;TrustServerCertificate=False;`.
   - Keep pooling enabled.
2. Configure JWT and social provider settings:
   - `JwtSettings` (Issuer, Audience, SecretKey, expiry)
   - `GoogleAuth` and `MicrosoftAuth` credentials.
3. Run backend:
   - `dotnet restore`
   - `dotnet ef database update`
   - `dotnet run --project InTimeProAPI`
4. Verify auth endpoints in Swagger: `/swagger`.

## 3. Mobile App Bootstrap
1. Create Flutter app root `InTimeProMobile/`.
2. Add dependencies:
   - `flutter_bloc`, `equatable`, `dio`, `json_annotation`, `retrofit`, `drift`, `sqlite3_flutter_libs`, `flutter_secure_storage`, `firebase_messaging`, `flutter_local_notifications`, `google_sign_in`, `msal_flutter`, `connectivity_plus`.
3. Configure environments:
   - `API_BASE_URL`
   - OAuth client IDs
   - Push notification keys
4. Implement clean architecture modules per plan structure.

## 4. Core Implementation Sequence
1. Auth module:
   - Email/password + Google + Microsoft login.
   - Remember Me + refresh token rotation + logout.
2. Dashboard and task lifecycle:
   - Active hours/task summary.
   - Start/pause/complete and overdue transitions.
3. Projects view:
   - Assigned projects and detail metadata.
4. Timesheets:
   - Calendar entries, daily submit, totals.
5. Leaves:
   - Apply, status tracking, entitlement view.
6. Notifications:
   - Push handling + local reminders + in-app inbox.
7. Settings/profile:
   - Profile edit, private timer toggle, logout.

## 5. Offline and Sync Rules
- Cache read models locally for dashboard/tasks/projects/timesheets/leaves.
- Queue offline mutations in `SyncOperation` with idempotency key.
- Replay on reconnect with exponential backoff and conflict handling.
- Never block UI on sync completion; show pending/synced indicators.

## 6. Test and Validation Checklist
- Unit tests:
  - Domain use cases and validation rules.
  - BLoC state transitions.
- Integration tests:
  - Auth flows (all providers), task lifecycle, timesheet submit, leave apply.
  - Offline enqueue and reconnect replay.
- Contract tests:
  - Request/response validation for all endpoints in `contracts/`.
- NFR checks:
  - P95 action latency under 2 seconds.
  - Reliability target >=99.5% in pilot telemetry.

## 7. Definition of Ready for /speckit.tasks
- Contracts approved by API and mobile owners.
- Data model fields and state transitions mapped to entities/use cases.
- Constitution gates still PASS with no unresolved clarifications.
- Requirement IDs mapped from spec to planned implementation modules.
