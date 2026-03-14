# Implementation Plan: InTimePro Mobile Application

**Branch**: `002-intimpro-mobile` | **Date**: 2026-03-14 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-intimpro-mobile/spec.md`

## Summary

Deliver an employee-facing Flutter mobile app (clean architecture + BLoC) that consumes existing InTimeProAPI (.NET 8) endpoints for authentication, dashboard, tasks, projects, timesheets, leave, notifications, and settings. The plan emphasizes secure multi-provider auth (email/password, Microsoft, Google), offline-safe local persistence with deterministic sync, and event notifications while preserving spec scope boundaries.

## Technical Context

**Language/Version**: Dart 3.x with Flutter latest stable channel  
**Primary Dependencies**: flutter_bloc, equatable, dio, retrofit (or dio adapter), json_serializable, drift (SQLite ORM), flutter_secure_storage, firebase_messaging, flutter_local_notifications, google_sign_in, msal_flutter  
**Storage**: Drift (SQLite) for offline cache and pending mutation queue; flutter_secure_storage for access/refresh tokens; backend system-of-record remains SQL Server via InTimeProAPI  
**Testing**: flutter_test, bloc_test, mocktail, integration_test, golden tests for key dashboard/task states, API contract tests against local InTimeProAPI  
**Target Platform**: Android 8.0+ and iOS 14+  
**Project Type**: Mobile app consuming existing ASP.NET Core Web API  
**Performance Goals**: P95 <2s for dashboard load, task state transitions, timesheet submit confirmation under normal network conditions (aligned to NFR-001)  
**Constraints**: Offline-safe UX for task/timesheet/leave flows; no direct client-to-database access; JWT + refresh token security; TLS 1.2+ only; strict requirement traceability to FR/VE/MC/NFR IDs  
**Scale/Scope**: One employee mobile app across 8 user stories, expected pilot cohort 100-1000 employees, modules limited to spec 002 in-scope capabilities

**Operating Profile**: Validation baseline uses RTT <= 200ms, payload <= 250KB for interactive calls, and pilot concurrency up to 1,000 active employees.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Pre-Phase 0 Gate Assessment

| Principle/Gate | Status | Evidence / Decision |
|----------------|--------|---------------------|
| 1. Requirement Clarity and Scope Control | PASS | Spec 002 defines explicit in-scope/out-of-scope boundaries, assumptions/dependencies, and acceptance scenarios per user story. |
| 2. User Story Format | PASS | All stories follow As/I want/So that structure with priority, story points, and independent test definition. |
| 3. Acceptance Criteria Standards | PASS | Each story contains 3+ Given/When/Then criteria and includes functional, security, and performance-focused acceptance coverage. |
| 4. Validation and Error Handling | PASS | VE-001 through VE-004 define validation, user-safe errors, operational logging, and retry/recovery expectations. |
| 5. Non-Functional Requirements | PASS | NFR-001..007 are measurable and include performance, reliability, security/compliance, maintainability, usability, scalability, and accessibility. |
| 6. Traceability and Consistency | PASS | Requirement IDs (FR/VE/MC/NFR/SC) are explicit and mapped in this plan artifacts set. |
| 7. Database Connectivity | PASS | SQL Server baseline is explicit: environment-defined instance/database, SQL-auth service account, `Encrypt=True`, `TrustServerCertificate=False`, max pool size 100, approved secret-store usage, and CI/CD pre-release connection validation gate. |
| 8. Authentication and Access Control | PASS | Contracts define `Admin`, `Employee`, `Auditor` claims expectations, least-privilege API checks, and SSO controls for Microsoft/Google plus password reset policy. |
| 9. Data Integrity and Compliance | PASS | ACID boundaries and audit-log requirements are defined for task/timesheet/leave mutating operations in design artifacts. |
| 10. Schema and Version Control | PASS | Mobile depends on API migrations only; plan requires forward+rollback EF migration scripts and version-locked DB schema per release. |
| 11. Performance and Scalability | PASS | Index/query-boundary and timeout expectations are captured, and reviewed stored procedures are required for critical task/timesheet/leave operations. |
| 12. Security and Network Boundaries | PASS | No direct DB access from mobile; HTTPS/TLS required; sensitive local data stored in secure storage/encrypted SQLite fields where applicable. |
| 13. Monitoring and Notifications | PASS | Logging levels, health checks, and alert triggers for failed logins/overdue tasks/failed timesheets are explicitly required. |

### Post-Phase 1 Re-Check

| Principle/Gate | Status | Design Confirmation |
|----------------|--------|---------------------|
| Principles 1-13 | PASS | `research.md`, `data-model.md`, `quickstart.md`, and `contracts/*` include explicit controls and implementation-ready decisions with no unresolved clarifications. |

## Project Structure

### Documentation (this feature)

```text
specs/002-intimpro-mobile/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── README.md
│   ├── auth.md
│   ├── dashboard.md
│   ├── tasks.md
│   ├── projects.md
│   ├── timesheets.md
│   ├── leaves.md
│   ├── notifications.md
│   └── settings.md
└── tasks.md                # Created later by /speckit.tasks
```

### Source Code (repository root)

```text
InTimeProAPI/
├── Controllers/
├── Data/
├── DTOs/
├── Models/
└── Services/

InTimeProAPI.Tests/
├── Contracts/
├── Integration/
└── Unit/

InTimeProMobile/            # New Flutter app root to implement from this plan
├── lib/
│   ├── app/
│   │   ├── routes/
│   │   └── di/
│   ├── core/
│   │   ├── network/
│   │   ├── storage/
│   │   ├── error/
│   │   └── notifications/
│   ├── features/
│   │   ├── auth/
│   │   ├── dashboard/
│   │   ├── tasks/
│   │   ├── projects/
│   │   ├── timesheets/
│   │   ├── leaves/
│   │   ├── notifications/
│   │   └── settings/
│   └── shared/
├── test/
├── integration_test/
└── pubspec.yaml
```

**Structure Decision**: Use a mobile-plus-existing-API structure. Existing server implementation stays in `InTimeProAPI/`; Flutter app will be added in `InTimeProMobile/` with clean architecture by feature. This preserves exact scope of spec 002 while enabling direct traceability from contracts to BLoC/use case implementation tasks.

## Complexity Tracking

No constitution violations require exception handling at planning time.
