# Contract: Timesheets

## GET /api/v1/timesheets/calendar
- Auth: Bearer
- Query params:
  - `month` required format `YYYY-MM`
- Success 200:
```json
{
  "success": true,
  "data": {
    "month": "2026-03",
    "days": [
      {
        "date": "2026-03-14",
        "totalLoggedHours": 7.5,
        "submittedHours": 7.5,
        "status": "Submitted"
      }
    ]
  },
  "message": null,
  "errors": null
}
```

## PUT /api/v1/timesheets/daily/{date}
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "entries": [
    {
      "taskId": "uuid",
      "hours": 4.0
    },
    {
      "taskId": "uuid",
      "hours": 3.5
    }
  ]
}
```
- Success 200: persisted draft entries and totals
- Errors:
  - 400 malformed date or validation
  - 422 daily total > 24 or business rule violation

## POST /api/v1/timesheets/daily/{date}/submit
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Success 200:
```json
{
  "success": true,
  "data": {
    "date": "2026-03-14",
    "status": "Submitted",
    "submittedAtUtc": "2026-03-14T18:02:00Z",
    "totalLoggedHours": 7.5
  },
  "message": "Timesheet submitted",
  "errors": null
}
```
- Errors:
  - 409 already submitted
  - 422 missing/zero required entries

## GET /api/v1/timesheets/submitted
- Auth: Bearer
- Query params: `from`, `to`, `page`, `pageSize`
- Success 200: paged submitted timesheets with `submittedHours` and `totalLoggedHours`

## Integrity and Compliance
- Daily submit must be ACID transaction.
- Submit and update actions require audit logs.
- Failed submission events must trigger admin alerts.
