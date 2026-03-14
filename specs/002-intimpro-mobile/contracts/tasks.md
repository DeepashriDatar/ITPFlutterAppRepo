# Contract: Tasks

## GET /api/v1/tasks
- Auth: Bearer
- Query params:
  - `status` optional: `New|InProgress|Overdue|Completed`
  - `page` default 1
  - `pageSize` default 20, max 100
  - `projectId` optional UUID
- Success 200:
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "projectId": "uuid",
        "title": "Task title",
        "description": "Optional",
        "status": "New",
        "estimateMinutes": 90,
        "elapsedMinutes": 0,
        "startedAtUtc": null,
        "completedAtUtc": null,
        "updatedAtUtc": "2026-03-14T09:10:00Z"
      }
    ],
    "page": 1,
    "pageSize": 20,
    "total": 1
  },
  "message": null,
  "errors": null
}
```

## POST /api/v1/tasks
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "projectId": "uuid",
  "title": "Prepare status deck",
  "description": "For Monday review",
  "estimateMinutes": 120
}
```
- Success 201: created task with status=`New`
- Errors: 400 validation, 404 project not found, 422 domain constraints

## PATCH /api/v1/tasks/{taskId}/state
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "action": "start",
  "occurredAtUtc": "2026-03-14T10:00:00Z"
}
```
- `action` enum: `start|pause|complete`
- Success 200: updated task
- Errors:
  - 400 invalid action
  - 409 invalid transition or conflict
  - 404 task not found

## Validation and Integrity
- `estimateMinutes` must be > 0.
- Server auto-derives `Overdue` if elapsed exceeds estimate.
- State transitions and elapsed time updates must be atomic and audit logged.
- Queries must always be bounded by pagination (no unbounded list endpoint).
