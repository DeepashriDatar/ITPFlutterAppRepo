# Tasks API Contract

## GET /api/v1/tasks
Get user's tasks with optional filtering.

**Query Parameters**:
- status: New, InProgress, Completed, Overdue
- projectId: filter by project
- offset: pagination offset (default 0)
- limit: pagination limit (default 20)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "tasks": [
      {
        "id": "uuid",
        "name": "Implement login screen",
        "description": "Create Flutter login UI",
        "status": "InProgress",
        "estimatedHours": 4.0,
        "actualHours": 2.5,
        "projectId": "project-uuid",
        "startTime": "2026-03-13T09:00:00Z",
        "createdAt": "2026-03-12T10:00:00Z"
      }
    ],
    "total": 25,
    "offset": 0,
    "limit": 20
  }
}
```

## POST /api/v1/tasks
Create new task.

**Request**:
```json
{
  "name": "New task name",
  "description": "Task description",
  "estimatedHours": 2.0,
  "projectId": "project-uuid"
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "task": { /* full task object */ }
  }
}
```

## PUT /api/v1/tasks/{id}
Update task.

**Request**:
```json
{
  "name": "Updated name",
  "status": "Completed"
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "task": { /* updated task object */ }
  }
}
```

## PUT /api/v1/tasks/{id}/start
Start task timer.

**Request**: Empty

**Response** (200):
```json
{
  "success": true,
  "data": {
    "task": { /* task with startTime set */ }
  }
}
```

## PUT /api/v1/tasks/{id}/pause
Pause task timer.

**Request**: Empty

**Response** (200):
```json
{
  "success": true,
  "data": {
    "task": { /* task with endTime set */ }
  }
}
```