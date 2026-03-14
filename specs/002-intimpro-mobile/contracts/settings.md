# Contract: Settings and Profile

## GET /api/v1/settings/profile
- Auth: Bearer
- Success 200:
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "email": "employee@company.com",
    "name": "Employee",
    "department": "Engineering",
    "phone": "+1-000-000-0000",
    "avatarUrl": null,
    "privateTimerEnabled": false,
    "role": "Employee"
  },
  "message": null,
  "errors": null
}
```

## PATCH /api/v1/settings/profile
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "name": "Employee Updated",
  "phone": "+1-000-000-0001",
  "avatarUrl": "https://cdn.example.com/avatar.png"
}
```
- Success 200: updated profile
- Errors: 400 invalid format, 422 policy violations

## PATCH /api/v1/settings/preferences
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "privateTimerEnabled": true
}
```
- Success 200: updated preference

## POST /api/v1/auth/logout
- Reused for settings logout action.

## Validation and Security
- Profile updates must validate phone/URL formats.
- Role is server-controlled and not mutable from this contract.
- Profile updates must be audit logged.
