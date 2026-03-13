# Timesheets API Contract

## GET /api/v1/timesheets
Get user's timesheets.

**Query Parameters**:
- status: Draft, Submitted, Approved, Rejected

**Response** (200):
```json
{
  "success": true,
  "data": {
    "timesheets": [
      {
        "id": "uuid",
        "weekStartDate": "2026-03-10",
        "totalHours": 40.0,
        "status": "Submitted",
        "submittedAt": "2026-03-14T17:00:00Z"
      }
    ]
  }
}
```

## POST /api/v1/timesheets/{id}/submit
Submit timesheet for approval.

**Request**: Empty

**Response** (200):
```json
{
  "success": true,
  "data": {
    "timesheet": { /* updated timesheet */ }
  }
}
```