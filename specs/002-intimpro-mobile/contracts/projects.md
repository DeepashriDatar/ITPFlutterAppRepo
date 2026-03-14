# Contract: Projects

## GET /api/v1/projects/assigned
- Auth: Bearer
- Query params:
  - `page` default 1
  - `pageSize` default 20, max 100
- Success 200:
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "name": "Client Onboarding Revamp",
        "progressPercent": 64.5,
        "startDate": "2026-02-01",
        "endDate": "2026-05-30",
        "taskCount": 18
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

## GET /api/v1/projects/{projectId}
- Auth: Bearer
- Success 200:
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "name": "Client Onboarding Revamp",
    "progressPercent": 64.5,
    "startDate": "2026-02-01",
    "endDate": "2026-05-30",
    "taskCount": 18,
    "teamMembers": [
      {
        "id": "uuid",
        "name": "Jane Doe",
        "role": "Employee"
      }
    ]
  },
  "message": null,
  "errors": null
}
```
- Errors: 404 project not assigned/found

## Validation and Safety
- `progressPercent` range is 0..100.
- `endDate` must be on/after `startDate`.
- Project list endpoint must be paged and index-backed on project and employee assignment keys.
