# API Contracts

## Contract Format
- **Protocol**: HTTPS REST API
- **Authentication**: JWT Bearer tokens in Authorization header
- **Content-Type**: application/json
- **Response Format**: JSON with standard structure
- **Error Handling**: HTTP status codes + error details in response
- **Pagination**: offset/limit for list endpoints
- **Versioning**: /api/v1/ prefix

## Standard Response Format
```json
{
  "success": true,
  "data": { ... },
  "message": "Optional message",
  "errors": null
}
```

## Error Response Format
```json
{
  "success": false,
  "data": null,
  "message": "Error description",
  "errors": {
    "field": ["Error details"]
  }
}
```