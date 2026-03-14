# Contract: Authentication

## Endpoints

### POST /api/v1/auth/login
- Auth: Public
- Request:
```json
{
  "email": "employee@company.com",
  "password": "P@ssw0rd!",
  "provider": "email",
  "socialToken": null,
  "rememberMe": true
}
```
- Validation:
  - `provider` in `email|google|microsoft`
  - `email` required and valid for all providers
  - `password` required when provider=`email`
  - `socialToken` required when provider=`google|microsoft`
- Success 200:
```json
{
  "success": true,
  "data": {
    "user": {
      "id": "uuid",
      "email": "employee@company.com",
      "name": "Employee",
      "department": "Engineering",
      "role": "Employee",
      "phone": null,
      "avatarUrl": null
    },
    "tokens": {
      "accessToken": "jwt",
      "refreshToken": "opaque-token",
      "expiresIn": 3600
    }
  },
  "message": null,
  "errors": null
}
```
- Errors:
  - 400 validation failed
  - 401 invalid credentials/invalid provider token
  - 423 account locked

### POST /api/v1/auth/refresh
- Auth: Public
- Request:
```json
{ "refreshToken": "opaque-token" }
```
- Success 200: new access token and expiry
- Errors: 401 invalid/expired/revoked refresh token

### POST /api/v1/auth/logout
- Auth: Bearer
- Behavior: revokes all refresh tokens for current employee.
- Success 200: logout confirmation

### POST /api/v1/auth/forgot-password
- Auth: Public
- Request:
```json
{ "email": "employee@company.com" }
```
- Success 200 always (anti-enumeration)

### GET /api/v1/auth/me
- Auth: Bearer
- Success 200: current employee profile
- Errors: 401 invalid token, 404 user not found

## Security Requirements
- Password policy: min 8 chars, upper/lower/number/special, configurable expiry and reset windows.
- SSO providers: Google and Microsoft token validation must occur server-side.
- JWT claims must include employee ID and role.
- RBAC roles supported: `Admin`, `Employee`, `Auditor`.
