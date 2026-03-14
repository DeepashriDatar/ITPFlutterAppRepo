# Contract: Notifications

## Push Registration

### POST /api/v1/notifications/device-tokens
- Auth: Bearer
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "platform": "android",
  "deviceToken": "fcm-token",
  "appVersion": "1.0.0"
}
```
- Success 200: token registered/upserted

## In-App Notification Feed

### GET /api/v1/notifications
- Auth: Bearer
- Query params: `page`, `pageSize`, `unreadOnly`
- Success 200:
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "eventType": "TimesheetSubmitted",
        "title": "Timesheet submitted",
        "message": "Your timesheet for 2026-03-14 was submitted.",
        "relatedEntityType": "TimesheetEntry",
        "relatedEntityId": "uuid",
        "deliveredAtUtc": "2026-03-14T18:02:01Z",
        "readAtUtc": null
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

### PATCH /api/v1/notifications/{notificationId}/read
- Auth: Bearer
- Headers: `X-Correlation-Id`
- Success 200: read timestamp applied

## Event Coverage
- Must emit events for:
  - task started/stopped/completed
  - timesheet submitted
  - leave applied/status changed

## Operational Controls
- Duplicate suppression required for same event/entity window.
- Failed push dispatch attempts must be logged at WARN/ERROR and retried by backend policy.
