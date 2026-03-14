# InTimePro Problem Statement Development Guidelines

## Purpose

This guideline translates the InTimePro problem statement and database reality into actionable Speckit development rules for UI, backend, and data integration.

## Inputs Reviewed

- Problem statement requirements in [spec.md](spec.md)
- Feature plan and task decomposition in [plan.md](plan.md) and [tasks.md](tasks.md)
- Existing backend entry point [InTimeProAPI/Program.cs](../../InTimeProAPI/Program.cs)
- Existing mobile app entry point [InTimeProMobile/lib/main.dart](../../InTimeProMobile/lib/main.dart)
- Database backup [itpdevbackup.bacpac](../../itpdevbackup.bacpac)

## BACPAC Analysis Summary

- Archive contains schema and data payload files (`model.xml`, `DacMetadata.xml`, `Data/*.BCP`).
- Estimated schema object counts from model metadata:
  - Tables: 205
  - Views: 2
  - Stored procedures: 9
- Data payload includes 124 unique tables.
- Domain-relevant table names observed include:
  - `dbo.AbpUsers`, `dbo.AbpRoles`, `dbo.AbpUserRoles`
  - `dbo.Project`, `dbo.TaskMasters`, `dbo.TaskState`, `dbo.TaskPriority`
  - `dbo.MyTimeSheet`, `dbo.TimesheetApprovals`, `dbo.Leave`, `dbo.LeaveByUser`
  - `dbo.MobileNotification`, `dbo.UserDevice`

## Speckit Process Rules

1. Always run commands in this order for each feature:
   - `/speckit.constitution`
   - `/speckit.specify`
   - `/speckit.clarify` (if ambiguity exists)
   - `/speckit.plan`
   - `/speckit.tasks`
   - `/speckit.analyze`
   - `/speckit.implement`
2. Never start coding before `/speckit.tasks` is generated and `/speckit.analyze` has no CRITICAL issues.
3. Keep all feature artifacts under `specs/<feature-id>/` only.
4. Maintain traceability in every implementation task using requirement IDs (`FR-*`, `VE-*`, `MC-*`, `NFR-*`, `SC-*`).

## Backend Development Rules

1. API boundary:
   - Mobile app must access backend only through HTTPS API.
   - No direct client-to-database access.
2. Data and security:
   - Enforce `Encrypt=True` and `TrustServerCertificate=False` for SQL connections.
   - Implement RBAC with `Admin`, `Employee`, `Auditor` roles.
   - Record audit entries for create/update/delete operations.
3. Reliability:
   - All task/timesheet/leave write operations must be transactional.
   - Health checks and structured logs (`INFO`, `WARN`, `ERROR`) are mandatory.
4. API governance:
   - Version endpoints under `/api/v1`.
   - Keep response contracts aligned with `specs/002-intimpro-mobile/contracts/`.

## Mobile UI Development Rules

1. Architecture:
   - Use feature-first clean architecture in `InTimeProMobile/lib/features/`.
   - Keep app shell concerns in `InTimeProMobile/lib/app/` and shared infra in `InTimeProMobile/lib/core/`.
2. State and UX:
   - Use BLoC state management for user-facing modules.
   - Every screen must expose loading, success, and failure states.
3. Offline behavior:
   - Queue write operations and replay on reconnect.
   - Show pending sync state to users; do not silently drop mutations.
4. Notification behavior:
   - Support event notifications for task, timesheet, and leave actions as specified in `FR-NOT-*`.

## BACPAC-to-API Mapping Rules

1. Treat BACPAC schema as reference baseline, not direct runtime dependency for mobile.
2. Define explicit entity mapping documents before implementation of each module:
   - Auth mapping: ABP identity tables to auth DTOs
   - Task mapping: `TaskMasters`, `TaskState`, `TaskPriority` to task API models
   - Timesheet mapping: `MyTimeSheet`, approvals tables to timesheet API models
   - Leave mapping: leave tables to leave request DTOs
3. Never expose raw table names/columns to mobile; use stable API DTO contracts.

## Definition of Done Rules

A user story is done only when all are true:

1. Functional acceptance scenarios pass.
2. Validation and error behavior (`VE-*`) is implemented and tested.
3. Compliance controls (`MC-*`) touched by the story are implemented.
4. At least one automated test exists for happy path and one for failure/edge path.
5. Traceability matrix is updated with code and test references.

## Build and Verification Commands

Backend:

- `dotnet restore ITPFlutterAppRepo.sln`
- `dotnet build ITPFlutterAppRepo.sln`

Mobile (requires Flutter SDK installed):

- `flutter pub get`
- `flutter analyze`
- `flutter test`

## Immediate Next Steps

1. Install Flutter SDK on developer machine and add to PATH.
2. Complete backend compile and warning cleanup.
3. Execute `/speckit.analyze` again after any major task generation changes.
4. Begin implementation in priority order: US1, US2, US3, US5, then remaining stories.
