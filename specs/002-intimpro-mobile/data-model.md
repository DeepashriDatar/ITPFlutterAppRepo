# Data Model: InTimePro Mobile (Feature 002)

## Modeling Notes
- Backend remains source of truth for business records.
- Mobile maintains denormalized local read models and a sync queue for offline-safe behavior.
- All entity IDs are UUID strings unless otherwise noted.

## Entities

### Employee
- Purpose: Authenticated employee profile and app preferences.
- Fields:
  - id (UUID, required)
  - email (string, required, email format)
  - name (string, required, 1..100)
  - department (string, optional)
  - role (enum: Admin|Employee|Auditor, required)
  - phone (string, optional)
  - avatarUrl (string, optional)
  - privateTimerEnabled (bool, default false)
  - lastSyncedAt (datetime, optional)
- Validation:
  - Role must be one of Admin/Employee/Auditor.
  - Email must pass RFC-compliant format check.

### SessionToken
- Purpose: Manage authentication lifecycle.
- Fields:
  - accessToken (string, required, short-lived)
  - refreshToken (string, required, secure storage only)
  - expiresAtUtc (datetime, required)
  - provider (enum: email|google|microsoft, required)
  - rememberMe (bool, required)
- Validation:
  - Refresh token cannot be persisted outside secure storage.
  - Access token considered invalid once expired or refresh fails.

### Task
- Purpose: Track employee work items with timer lifecycle.
- Fields:
  - id (UUID, required)
  - projectId (UUID, required)
  - title (string, required, 1..200)
  - description (string, optional)
  - status (enum: New|InProgress|Overdue|Completed, required)
  - estimateMinutes (int, required, >0)
  - elapsedMinutes (int, required, >=0)
  - startedAtUtc (datetime, optional)
  - completedAtUtc (datetime, optional)
  - updatedAtUtc (datetime, required)
  - syncState (enum: Synced|PendingCreate|PendingUpdate|Conflict, required)
- Validation:
  - Completed requires elapsedMinutes > 0 and completedAtUtc present.
  - Overdue is derived when elapsedMinutes > estimateMinutes and status not Completed.
- State transitions:
  - New -> InProgress (start)
  - InProgress -> Overdue (elapsed > estimate)
  - InProgress|Overdue -> Completed (complete)
  - Invalid: Completed -> InProgress (reopen not in scope)

### Project
- Purpose: Show assigned project context.
- Fields:
  - id (UUID, required)
  - name (string, required)
  - progressPercent (decimal 0..100, required)
  - startDate (date, required)
  - endDate (date, required)
  - teamMembers (array<EmployeeSummary>, required)
  - taskCount (int, required, >=0)
- Validation:
  - endDate must be >= startDate.

### TimesheetEntry
- Purpose: Record daily task-hour logs and submission state.
- Fields:
  - id (UUID, required)
  - date (date, required)
  - taskId (UUID, required)
  - hours (decimal, required, >0 and <=24)
  - status (enum: Draft|Submitted|Rejected|Approved, required)
  - submittedAtUtc (datetime, optional)
  - totalDayHours (decimal, computed)
  - syncState (enum: Synced|PendingCreate|PendingUpdate|Conflict, required)
- Validation:
  - Daily total cannot exceed 24 hours.
  - Submission requires at least one non-zero entry for date.
- State transitions:
  - Draft -> Submitted
  - Submitted -> Rejected|Approved

### LeaveRequest
- Purpose: Handle leave applications and entitlement visibility.
- Fields:
  - id (UUID, required)
  - leaveType (enum: Annual|Sick|Casual|Unpaid, required)
  - startDate (date, required)
  - endDate (date, required)
  - reason (string, optional, encrypted at rest on backend)
  - status (enum: Pending|Approved|Rejected|Cancelled, required)
  - requestedDays (decimal, required)
  - availableBalanceDays (decimal, required)
  - annualEntitlementDays (decimal, required)
  - bookedDays (decimal, required)
  - createdAtUtc (datetime, required)
- Validation:
  - Date range must not overlap approved leave periods.
  - requestedDays must be <= availableBalanceDays unless leaveType is Unpaid.
- State transitions:
  - Pending -> Approved|Rejected|Cancelled

### NotificationEvent
- Purpose: Represent delivered or pending task/timesheet/leave notifications.
- Fields:
  - id (UUID, required)
  - eventType (enum: TaskStarted|TaskStopped|TaskCompleted|TimesheetSubmitted|LeaveApplied|LeaveStatusChanged, required)
  - title (string, required)
  - message (string, required)
  - relatedEntityType (enum: Task|TimesheetEntry|LeaveRequest, required)
  - relatedEntityId (UUID, required)
  - deliveredAtUtc (datetime, optional)
  - readAtUtc (datetime, optional)
  - source (enum: Push|Local, required)
- Validation:
  - Duplicate suppression key: eventType + relatedEntityId + minute bucket.

### SyncOperation
- Purpose: Queue offline mutations for guaranteed retry.
- Fields:
  - id (UUID, required)
  - entityType (string, required)
  - entityId (UUID, required)
  - operation (enum: Create|Update|Submit|StateTransition, required)
  - payloadJson (string, required)
  - idempotencyKey (string, required, unique)
  - attemptCount (int, required, default 0)
  - nextAttemptAtUtc (datetime, required)
  - lastError (string, optional)
  - status (enum: Pending|Processing|Succeeded|DeadLetter, required)
- Validation:
  - Move to DeadLetter after max retry threshold.
  - Idempotency key must be stable across retries.

## Relationships
- Employee 1..* Task
- Employee 1..* TimesheetEntry
- Employee 1..* LeaveRequest
- Project 1..* Task
- Task 1..* TimesheetEntry
- Task/TimesheetEntry/LeaveRequest 1..* NotificationEvent
- Any mutable entity 1..* SyncOperation (until successful replay)

## Transaction and Audit Boundaries
- Task state transitions, timesheet submission, and leave request creation/update must be ACID transactions on backend.
- Each create/update/delete operation must produce an audit record containing actor, timestamp, action, and correlation ID.
- Mobile must pass a correlation ID header for all mutating requests.
