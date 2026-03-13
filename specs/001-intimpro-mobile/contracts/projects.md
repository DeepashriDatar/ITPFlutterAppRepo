# Projects API Contract

## GET /api/v1/projects
Get user's assigned projects.

**Response** (200):
```json
{
  "success": true,
  "data": {
    "projects": [
      {
        "id": "uuid",
        "name": "Project Alpha",
        "description": "Mobile app development",
        "progressPercentage": 75.0,
        "startDate": "2026-01-01",
        "endDate": "2026-06-01",
        "status": "Active",
        "teamMemberCount": 5,
        "taskCount": 20
      }
    ]
  }
}
```