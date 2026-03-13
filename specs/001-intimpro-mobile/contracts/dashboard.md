# Dashboard API Contract

## GET /api/v1/dashboard
Get user's dashboard data.

**Response** (200):
```json
{
  "success": true,
  "data": {
    "clockInTime": "2026-03-13T09:00:00Z",
    "activeWorkingHours": "02:45:30", // HH:MM:SS
    "totalWorkTime": "08:30:00", // today's total
    "currentTask": {
      "id": "uuid",
      "name": "Current task",
      "elapsedTime": "01:30:00",
      "estimatedRemaining": "02:30:00"
    },
    "todayTasks": [
      {
        "id": "uuid",
        "name": "Task name",
        "status": "Completed",
        "actualHours": 2.0
      }
    ]
  }
}
```