# Data Model

## Entities

### Employee
**Fields**:
- id: String (UUID)
- email: String (unique, required)
- name: String (required)
- department: String
- role: String
- createdAt: DateTime
- updatedAt: DateTime

**Validation**:
- Email must be valid format
- Name cannot be empty

**Relationships**:
- Has many Tasks
- Has many TimeEntries
- Belongs to many Projects (through assignments)

### Task
**Fields**:
- id: String (UUID)
- name: String (required)
- description: String
- status: Enum (New, InProgress, Completed, Overdue)
- estimatedHours: double
- actualHours: double
- projectId: String (foreign key)
- employeeId: String (foreign key)
- startTime: DateTime
- endTime: DateTime
- createdAt: DateTime
- updatedAt: DateTime

**Validation**:
- Name cannot be empty
- Status transitions: New → InProgress → Completed
- Actual hours calculated from time entries
- Overdue if actual > estimated and status = InProgress

**Relationships**:
- Belongs to Employee
- Belongs to Project
- Has many TimeEntries

### Project
**Fields**:
- id: String (UUID)
- name: String (required)
- description: String
- progressPercentage: double (0-100)
- startDate: Date
- endDate: Date
- status: Enum (Active, Completed, OnHold)
- createdAt: DateTime
- updatedAt: DateTime

**Validation**:
- Name cannot be empty
- End date after start date
- Progress calculated from task completion

**Relationships**:
- Has many Tasks
- Has many Employees (assigned)

### TimeEntry
**Fields**:
- id: String (UUID)
- taskId: String (foreign key)
- employeeId: String (foreign key)
- startTime: DateTime (required)
- endTime: DateTime
- durationMinutes: int (calculated)
- isActive: bool
- createdAt: DateTime

**Validation**:
- End time after start time
- Only one active entry per employee at a time

**Relationships**:
- Belongs to Task
- Belongs to Employee

### Timesheet
**Fields**:
- id: String (UUID)
- employeeId: String (foreign key)
- weekStartDate: Date (required)
- totalHours: double (calculated)
- status: Enum (Draft, Submitted, Approved, Rejected)
- submittedAt: DateTime
- approvedAt: DateTime
- createdAt: DateTime

**Validation**:
- Unique per employee per week
- Total hours from time entries

**Relationships**:
- Belongs to Employee
- Has many TimeEntries (through tasks)

## State Transitions

### Task Status Flow
```
New → InProgress (start timer)
InProgress → Completed (manual complete)
InProgress → Overdue (auto if exceeded estimated time)
InProgress → InProgress (pause/resume)
```

### Timesheet Status Flow
```
Draft → Submitted (employee submit)
Submitted → Approved (manager approve)
Submitted → Rejected (manager reject)
Approved → Draft (if changes needed)
```