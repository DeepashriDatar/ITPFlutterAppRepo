# Contract: Leaves

## GET /api/v1/leaves/summary
- Auth: Bearer
- Success 200:
```json
{
  "success": true,
  "data": {
    "annualEntitlementDays": 24,
    "availableBalanceDays": 11.5,
    "bookedDays": 12.5,
    "requests": [
      {
        "id": "uuid",
        "leaveType": "Annual",
        "startDate": "2026-04-02",
        "endDate": "2026-04-04",
        "status": "Pending",
        "requestedDays": 3
      }
    ]
  },
  "message": null,
  "errors": null
}
```

## POST /api/v1/leaves
- Auth: Bearer (`Employee` self)
- Headers: `X-Correlation-Id`
- Request:
```json
{
  "leaveType": "Annual",
  "startDate": "2026-04-02",
  "endDate": "2026-04-04",
  "reason": "Family commitment"
}
```
- Success 201: leave request created with `status=Pending`
- Errors:
  - 400 invalid date range/type
  - 409 overlapping leave period
  - 422 insufficient balance for non-unpaid leave

## GET /api/v1/leaves
- Auth: Bearer
- Query params: `status`, `page`, `pageSize`
- Success 200: paged leave requests for user

## PATCH /api/v1/leaves/{leaveId}/cancel
- Auth: Bearer (`Employee` self, policy-limited)
- Headers: `X-Correlation-Id`
- Success 200: status updated to `Cancelled`
- Errors: 409 invalid cancellation state

## Integrity and Compliance
- Leave creation/update/cancel must be ACID and audit logged.
- Sensitive leave details require encryption at rest in backend data store.
- Status changes should emit notification events.
