# Authentication API Contract

## POST /api/v1/auth/login
Login with email/password or social provider.

**Request**:
```json
{
  "email": "user@example.com",
  "password": "password123",
  "provider": "email" // or "google", "microsoft"
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "user": {
      "id": "uuid",
      "email": "user@example.com",
      "name": "John Doe",
      "department": "Engineering"
    },
    "tokens": {
      "accessToken": "jwt_access",
      "refreshToken": "jwt_refresh",
      "expiresIn": 3600
    }
  }
}
```

## POST /api/v1/auth/refresh
Refresh access token.

**Request**:
```json
{
  "refreshToken": "jwt_refresh"
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "accessToken": "new_jwt_access",
    "expiresIn": 3600
  }
}
```

## POST /api/v1/auth/logout
Logout and invalidate tokens.

**Request**: Empty body

**Response** (200):
```json
{
  "success": true,
  "message": "Logged out successfully"
}
```