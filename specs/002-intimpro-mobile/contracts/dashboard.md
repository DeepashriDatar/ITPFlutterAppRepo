# Contract: Dashboard

## GET /api/v1/dashboard/summary
- Auth: Bearer (`Employee`, `Admin`, `Auditor` read)
- Query params:
  - `date` (optional, ISO date, default today)
- Success 200:
```json
{
  "success": true,
  "data": {
    "clockInTimeUtc": "2026-03-14T08:55:00Z",
    "activeWorkingMinutes": 215,
    "totalWorkMinutes": 390,
    "activeTask": {
      "id": "uuid",
      "title": "Prepare client report",
      "status": "InProgress",
      "elapsedMinutes": 75,
      "estimateMinutes": 120,
      "projectId": "uuid"
    }
  },
  "message": null,
  "errors": null
}
```
- Errors:
  - 401 unauthorized
  - 404 employee dashboard context not found

## PATCH /api/v1/dashboard/active-task/state
- Auth: Bearer (`Employee` self only)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "taskId": "uuid",
  "action": "start"
}
```
- `action` enum: `start|pause|complete`
- Success 200: returns updated active task payload
- Errors:
  - 400 invalid transition/action
  - 409 conflicting active task state
  - 422 business rule violation

## Notes
- Dashboard values must be computable within 2 seconds P95 under normal operating conditions.
- Mutating state transitions must be ACID on backend and audit logged.
