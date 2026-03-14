# Contracts Overview: Feature 002

## Purpose
These contracts define mobile-to-backend interfaces needed to implement spec 002. They are implementation-ready inputs for `/speckit.tasks` and cover auth, dashboard, tasks, projects, timesheets, leave, notifications, and settings.

## Global Contract Rules
- Base URL: `/api/v1`
- Transport: HTTPS only (TLS 1.2+)
- Auth: `Authorization: Bearer <access-token>` for protected endpoints
- Correlation: `X-Correlation-Id` required on mutating operations
- Content type: `application/json`
- Pagination: `page`, `pageSize` with server-side max bound
- Standard success payload:
  - `success` (bool), `data` (object/array), `message` (string|null), `errors` (null)
- Standard error payload:
  - `success=false`, `message`, `errors` (field-level array or null)

## Role Access Baseline
- Employee: own records and daily operations (all feature 002 screens)
- Auditor: read-only visibility where policy allows
- Admin: operational override and approval workflows (backend-side only where applicable)

## Files
- `auth.md`
- `dashboard.md`
- `tasks.md`
- `projects.md`
- `timesheets.md`
- `leaves.md`
- `notifications.md`
- `settings.md`

## Versioning
- Contract version: v1 for this feature cycle
- Any breaking API change must increment API version or provide backward-compatible fallback
