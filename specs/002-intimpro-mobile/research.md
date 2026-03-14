# Phase 0 Research: InTimePro Mobile (Feature 002)

## Decision 1: Mobile Architecture
- Decision: Use Flutter latest stable with clean architecture split by feature (presentation/domain/data) and BLoC for state management.
- Rationale: Matches required technical direction, keeps business rules testable and framework-light, and supports parallel development across modules.
- Alternatives considered: Riverpod with feature-first architecture (rejected due to explicit BLoC requirement), MVVM with Provider (rejected due to weaker event-driven traceability for timer/task workflows).

## Decision 2: API Integration Strategy
- Decision: Consume existing ASP.NET Core .NET 8 backend at `InTimeProAPI` via versioned REST contracts under `/api/v1/*`, beginning with existing auth endpoints and defining required contracts for remaining modules.
- Rationale: Backend already exists and includes JWT auth + SQL Server persistence; contract-first planning allows mobile and API workstreams to align without scope expansion.
- Alternatives considered: GraphQL aggregation layer (rejected as out of scope and unnecessary), direct SQL access from mobile (rejected by security/network constraints).

## Decision 3: Offline-Safe UX and Local Persistence
- Decision: Use Drift (SQLite) for cached read models and an outbound mutation queue with idempotency keys for task/timesheet/leave writes.
- Rationale: Provides robust typed local schema, transactional writes, and deterministic replay after reconnect to satisfy VE-004 and NFR reliability needs.
- Alternatives considered: SharedPreferences only (rejected: insufficient for relational/time-series data), Hive-only (rejected: weaker SQL-style querying and migration governance for this domain).

## Decision 4: Authentication and Session Security
- Decision: Support three login providers through backend-authenticated flows: email/password, Google, and Microsoft. Store access token in memory and refresh token in secure storage; enforce refresh rotation and explicit logout revocation.
- Rationale: Meets FR-LOGIN-1..5 and aligns with existing `AuthController` endpoints (`login`, `refresh`, `logout`, `forgot-password`, `me`).
- Alternatives considered: Client-only social auth bypassing backend (rejected: inconsistent identity/claims and audit trail), long-lived access tokens without refresh (rejected: security risk).

## Decision 5: Notification Strategy
- Decision: Use Firebase Cloud Messaging for remote push and flutter_local_notifications for in-app/local reminders and foreground display.
- Rationale: Covers FR-NOT-* for server-originated events (leave status, timesheet result) and local reminders (overdue task/timesheet prompts) in one cohesive pipeline.
- Alternatives considered: Local notifications only (rejected: cannot deliver server-side workflow updates), platform-specific native code only (rejected: unnecessary complexity for cross-platform app).

## Decision 6: Validation, Error Handling, and Observability
- Decision: Implement layered validation (UI + domain + API), map backend validation errors to user-safe messages, and capture structured logs/events per module.
- Rationale: Satisfies VE-001..004 and constitution requirements for operational diagnosis without leaking sensitive details.
- Alternatives considered: Backend-only validation (rejected: poor UX and avoidable round-trips), generic catch-all error UI (rejected: weak actionability).

## Decision 7: Compliance and SQL/Schema Controls
- Decision: Treat SQL Server and migration controls as backend responsibilities but enforce them through contract and release gates for this feature: explicit instance/db/auth mode per environment, `Encrypt=True`, `TrustServerCertificate=False`, pooling enabled, forward and rollback migrations required.
- Rationale: Spec includes mandatory controls (MC-001/006/007/008/009); mobile feature depends on backend compliance and must not bypass these constraints.
- Alternatives considered: Defer DB/security controls to later release (rejected: violates constitution and mandatory compliance requirements).

## Resolved Clarifications
- All technical context placeholders are resolved with concrete selections.
- External interface assumptions are documented in `contracts/*` for implementation and `/speckit.tasks` generation.
