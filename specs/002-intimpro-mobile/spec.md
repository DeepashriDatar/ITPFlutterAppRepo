# Feature Specification: InTimePro Mobile Application

**Feature Branch**: `002-intimpro-mobile`  
**Created**: 2026-03-14  
**Status**: Ready for Implementation  
**Input**: User-provided requirement specification for InTimePro mobile app

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Secure Employee Access (Priority: P1)

As an employee, I want to securely sign in and manage my account access, so that only authorized users can use my work data.

**Story Points**: 8

**Why this priority**: Authentication gates every other function and is required before any business workflow can run.

**Independent Test**: A test user can login using email/password or SSO, persist session with Remember Me, and recover access via password reset.

**Acceptance Scenarios**:

1. **Given** a registered employee account, **When** the user enters valid email and password, **Then** the user is authenticated and directed to dashboard.
2. **Given** a logged-in employee who selected Remember Me, **When** the app is reopened within the configured session window, **Then** the employee stays authenticated.
3. **Given** an employee who forgot password, **When** they submit a valid reset request, **Then** reset instructions are sent and a new password can be set.
4. **Given** an employee using Microsoft or Google corporate identity, **When** they complete SSO consent, **Then** access is granted to their account.
5. **Given** repeated failed login attempts from the same account, **When** failure threshold is crossed, **Then** account lockout and security alert rules are applied.
6. **Given** valid credentials and healthy network conditions (RTT <= 200ms), **When** login is submitted, **Then** authentication result is returned within 2 seconds for at least 95% of attempts.

---

### User Story 2 - Daily Work Dashboard (Priority: P1)

As an employee, I want a real-time dashboard of my work hours and active task, so that I can quickly understand and control my day.

**Story Points**: 8

**Why this priority**: Dashboard visibility is central to daily productivity and time tracking.

**Independent Test**: User can view clock-in time, active hours, total hours, active task, and control task state from one screen.

**Acceptance Scenarios**:

1. **Given** a logged-in employee, **When** dashboard loads, **Then** clock-in time, active working hours, and total work time are shown.
2. **Given** an active task, **When** dashboard refreshes, **Then** active task details and elapsed time are shown.
3. **Given** a task in progress, **When** the user starts, pauses, or completes from dashboard controls, **Then** task state and timer update correctly.
4. **Given** an expired or invalid session, **When** dashboard is requested, **Then** access is denied and the user is redirected to authenticate.
5. **Given** normal operating profile (RTT <= 200ms, payload <= 250KB), **When** dashboard opens, **Then** dashboard data renders within 2 seconds for at least 95% of requests.

---

### User Story 3 - Task Lifecycle Tracking (Priority: P1)

As an employee, I want to create and manage tasks with timer-based states, so that my effort and status are accurately tracked.

**Story Points**: 8

**Why this priority**: Task lifecycle and time tracking are core business value of the application.

**Independent Test**: User can create tasks, start/stop timer, auto-mark overdue, complete tasks, and filter by status.

**Acceptance Scenarios**:

1. **Given** task creation input is valid, **When** user creates a task, **Then** task is stored in New state.
2. **Given** a New task, **When** user starts timer, **Then** state becomes In Progress and elapsed time starts.
3. **Given** elapsed time exceeds estimate, **When** threshold is crossed, **Then** state changes to Overdue.
4. **Given** an active task, **When** user stops timer and completes task, **Then** state is set to Completed and duration is preserved.
5. **Given** a user attempts to update another employee's task, **When** transition is requested, **Then** operation is rejected by authorization rules.
6. **Given** a valid task transition request, **When** start/pause/complete is submitted, **Then** state confirmation is returned within 2 seconds for at least 95% of requests.

---

### User Story 4 - Project Visibility (Priority: P2)

As an employee, I want to view assigned project status and team context, so that I can align my tasks with project goals.

**Story Points**: 5

**Why this priority**: Project context improves planning and execution but is not as blocking as auth/tasks/dashboard.

**Independent Test**: User can see assigned projects with progress, date range, team members, and task count.

**Acceptance Scenarios**:

1. **Given** assigned projects exist, **When** user opens projects module, **Then** project list is displayed.
2. **Given** a project record, **When** user views details, **Then** progress percentage, start/end dates, team members, and task count are visible.
3. **Given** a project not assigned to the employee, **When** access is attempted, **Then** project details are not returned.
4. **Given** paged assigned projects data, **When** projects module loads, **Then** first page renders within 2 seconds for at least 95% of requests.

---

### User Story 5 - Timesheet Logging and Submission (Priority: P1)

As an employee, I want to log task hours and submit daily timesheets, so that my work hours are accurately reported.

**Story Points**: 8

**Why this priority**: Timesheet operations are required for attendance, payroll, and compliance.

**Independent Test**: User can log hours on calendar, submit daily timesheet, and review submitted entries with totals.

**Acceptance Scenarios**:

1. **Given** a working day, **When** user opens timesheet calendar, **Then** date-wise entries are accessible.
2. **Given** task effort data, **When** user logs hours and submits timesheet, **Then** system records submitted state and totals.
3. **Given** previously submitted days, **When** user views submitted timesheets, **Then** submitted and total logged hours are shown.
4. **Given** missing mandatory entries or invalid hours, **When** submission is attempted, **Then** submission is blocked with actionable validation feedback.
5. **Given** an authenticated employee with valid entries, **When** timesheet is submitted, **Then** confirmation is visible within 2 seconds for at least 95% of submissions.

---

### User Story 6 - Leave Planning and Tracking (Priority: P2)

As an employee, I want to apply for leave and track balance/status, so that I can plan absences and understand entitlement usage.

**Story Points**: 5

**Why this priority**: Leave workflows are important business operations but less frequent than daily task/time actions.

**Independent Test**: User can apply for leave with type and dates and can view status, balance, entitlement, and booked leave.

**Acceptance Scenarios**:

1. **Given** available leave policy, **When** user submits leave request with type and dates, **Then** request is created.
2. **Given** existing leave requests, **When** user checks leave module, **Then** status and booked leave are visible.
3. **Given** leave balances and annual entitlement data, **When** user views summary, **Then** current balance and entitlement are shown.
4. **Given** leave request data for other employees, **When** a standard employee requests it, **Then** only their own leave records are returned.
5. **Given** valid leave data in normal conditions, **When** leave summary is opened, **Then** summary loads within 2 seconds for at least 95% of requests.

---

### User Story 7 - Event Notifications (Priority: P2)

As an employee, I want notifications on key task, timesheet, and leave events, so that I do not miss important updates.

**Story Points**: 5

**Why this priority**: Notifications improve responsiveness and reduce missed operational actions.

**Independent Test**: Notifications are generated and visible for task lifecycle, timesheet submission, and leave workflow updates.

**Acceptance Scenarios**:

1. **Given** task lifecycle events occur, **When** start/stop/complete actions happen, **Then** corresponding notifications are delivered.
2. **Given** timesheet is submitted, **When** submission succeeds, **Then** submission notification is delivered.
3. **Given** leave request is created or status changes, **When** workflow updates occur, **Then** leave notifications are delivered.
4. **Given** notification feed access, **When** employee opens inbox, **Then** only notifications for that employee are visible.
5. **Given** a task/timesheet/leave event is committed, **When** notification pipeline processes it, **Then** notification is delivered within 10 seconds for at least 95% of events.

---

### User Story 8 - Settings and Profile Management (Priority: P3)

As an employee, I want to manage account details and session preferences, so that I can keep profile data current and control usage options.

**Story Points**: 3

**Why this priority**: Settings are supporting capabilities with lower frequency usage than operational workflows.

**Independent Test**: User can view/edit account info, toggle private time timer, and logout.

**Acceptance Scenarios**:

1. **Given** user opens settings, **When** account section loads, **Then** account information is displayed.
2. **Given** editable profile fields, **When** user updates details and saves, **Then** updated information is persisted.
3. **Given** private time preference and active session, **When** user toggles private timer or logs out, **Then** preference and session state are correctly updated.
4. **Given** a sensitive profile update, **When** save is attempted with expired session, **Then** re-authentication is required before update is committed.
5. **Given** valid profile updates, **When** settings are saved, **Then** save confirmation appears within 2 seconds for at least 95% of updates.

### Edge Cases

- Invalid credentials, expired SSO token, or account lockout on repeated failed login attempts.
- Network loss during timer transitions, timesheet submission, or leave submission.
- Duplicate task start actions or conflicting state transitions from multiple sessions.
- Attempting timesheet submission with missing/zero required hour entries.
- Leave request overlaps, invalid date ranges, or insufficient leave balance.
- Notification delivery delay or duplicate notifications for same event.

## Requirements *(mandatory)*

### Functional Requirements

#### Authentication
- **FR-LOGIN-1**: User MUST be able to login using email and password.
- **FR-LOGIN-2**: User MUST be able to choose Remember Me to stay logged in.
- **FR-LOGIN-3**: User MUST be able to reset password using Forgot Password.
- **FR-LOGIN-4**: User MUST be able to login using Microsoft account.
- **FR-LOGIN-5**: User MUST be able to login using Google account.

#### Home / Dashboard
- **FR-HOME-1**: User MUST see their clock-in time.
- **FR-HOME-2**: User MUST see active working hours.
- **FR-HOME-3**: User MUST see total work time.
- **FR-HOME-4**: User MUST see the currently active task.
- **FR-HOME-5**: User MUST be able to start, pause, or complete task from dashboard.

#### Task Management
- **FR-TASK-1**: User MUST be able to create a new task.
- **FR-TASK-2**: Task MUST initially be in New state.
- **FR-TASK-3**: User MUST be able to start task timer.
- **FR-TASK-4**: When task starts, status MUST become In Progress.
- **FR-TASK-5**: If estimated time is exceeded, task MUST become Overdue.
- **FR-TASK-6**: User MUST be able to stop timer.
- **FR-TASK-7**: User MUST be able to mark task as Completed.
- **FR-TASK-8**: User MUST be able to view tasks by status (New, In Progress, Overdue, Completed).

#### Project Management
- **FR-PROJ-1**: User MUST see list of assigned projects.
- **FR-PROJ-2**: User MUST see project progress percentage.
- **FR-PROJ-3**: User MUST see project start and end dates.
- **FR-PROJ-4**: User MUST see team members involved in project.
- **FR-PROJ-5**: User MUST see number of tasks in project.

#### Timesheet Management
- **FR-TIME-1**: User MUST be able to view timesheet calendar.
- **FR-TIME-2**: User MUST be able to log hours for tasks.
- **FR-TIME-3**: User MUST be able to submit timesheet daily.
- **FR-TIME-4**: User MUST be able to view submitted timesheets.
- **FR-TIME-5**: System MUST show total logged hours.
- **FR-TIME-6**: System MUST show submitted hours.

#### Leave Management
- **FR-LEAVE-1**: User MUST be able to apply for leave.
- **FR-LEAVE-2**: User MUST be able to select leave type.
- **FR-LEAVE-3**: User MUST be able to select leave dates.
- **FR-LEAVE-4**: User MUST be able to see leave status.
- **FR-LEAVE-5**: User MUST be able to view leave balance.
- **FR-LEAVE-6**: User MUST be able to view annual leave entitlement.
- **FR-LEAVE-7**: User MUST be able to view booked leave.

#### Notifications
- **FR-NOT-1**: User MUST receive notification when task starts.
- **FR-NOT-2**: User MUST receive notification when task stops.
- **FR-NOT-3**: User MUST receive notification when task completes.
- **FR-NOT-4**: User MUST receive notification when timesheet is submitted.
- **FR-NOT-5**: User MUST receive notification when leave is applied.
- **FR-NOT-6**: User MUST receive notification when leave status changes.

#### Settings / Profile
- **FR-SET-1**: User MUST be able to view account information.
- **FR-SET-2**: User MUST be able to edit account details.
- **FR-SET-3**: User MUST be able to enable private time timer.
- **FR-SET-4**: User MUST be able to logout from application.

### Scope and Constraints *(mandatory)*

- **In Scope**: Employee-facing mobile modules for authentication, dashboard, task/project, timesheet, leave, notifications, and settings.
- **Out of Scope**: Admin console workflows, payroll processing engine, and desktop-only reporting customization.
- **Assumptions**: Existing InTimePro backend services and employee identity source are available; project/team/task master data is maintained outside mobile app.
- **Dependencies**: Identity provider integrations (Microsoft/Google), notification gateway, timesheet/leave backend APIs, project/task services.

### Validation and Error Handling Requirements *(mandatory)*

- **VE-001**: System MUST validate all login, task, timesheet, leave, and profile inputs for type, format, date range, and business constraints.
- **VE-002**: System MUST show user-facing actionable error messages for failed operations without exposing sensitive technical details.
- **VE-003**: System MUST capture operational errors and failed transitions with sufficient context for diagnosis.
- **VE-004**: System MUST support safe retry/recovery behavior for transient failures during task timer changes and submission flows.

### Mandatory Compliance and Platform Constraints *(mandatory)*

- **MC-001 (Database Connectivity)**: Production and staging MUST use SQL Server with environment-defined instance and database names, SQL-auth service account for app runtime, `Encrypt=True`, `TrustServerCertificate=False`, and max pool size of 100.
- **MC-002 (Access Control)**: API authorization MUST enforce `Admin`, `Employee`, and `Auditor` roles with least privilege; employees access only their own task/timesheet/leave/profile records unless explicit admin/auditor privilege exists.
- **MC-003 (Authentication)**: Microsoft and Google SSO MUST be supported for `FR-LOGIN-4` and `FR-LOGIN-5`; password policy MUST enforce minimum length 12, mixed character classes, password expiry every 90 days, no reuse of last 5 passwords, reset token expiry at 15 minutes, and lockout after 5 failed attempts in 15 minutes.
- **MC-004 (Data Integrity and Auditability)**: Task/timesheet/leave write operations MUST execute transactionally and emit audit entries for create/update/delete including actor ID, timestamp, and correlation ID.
- **MC-005 (Compliance Retention)**: Employee operational records MUST be retained for 7 years; deletion/anonymization requests MUST be fulfilled within 30 days in accordance with applicable regulation.
- **MC-006 (Schema and Migration)**: Each backend schema change MUST have versioned forward and rollback scripts; production schema is version-locked per release and untracked manual schema edits are prohibited.
- **MC-007 (Performance and Safety Limits)**: Backend MUST index employee_id, task_id, project_id, timesheet_id, and leave_request_id; unbounded list queries are disallowed; reviewed stored procedures are required for critical task/timesheet/leave operations; query timeout is capped at 5 seconds for interactive endpoints.
- **MC-008 (Security and Network Boundaries)**: Sensitive fields (credentials, tokens, leave reasons) MUST be encrypted at rest, all traffic MUST use TLS 1.2+, and mobile clients MUST access data only through API endpoints (no direct database connectivity).
- **MC-009 (Monitoring and Alerts)**: Logging MUST include `INFO`, `WARN`, and `ERROR`; health checks MUST run continuously; admin alerts MUST trigger for failed login bursts, overdue task threshold breaches, and failed timesheet submissions; alert acknowledgement ownership, acknowledgement SLA, escalation timers, and escalation paths MUST be documented and reviewed each release.

## Non-Functional Requirements *(mandatory)*

- **NFR-001 Performance**: Primary employee actions (open dashboard, start/pause/complete task, submit timesheet) MUST complete within 2 seconds for at least 95% of requests under operating profile: RTT <= 200ms, payload <= 250KB, pilot concurrency up to 1,000 active users.
- **NFR-002 Reliability**: Monthly successful completion rate for core flows (login, task update, timesheet submission, leave submission) MUST be at least 99.5%.
- **NFR-003 Security/Compliance**: Access and data handling MUST satisfy OWASP ASVS Level 2 controls, organization IAM policy, and GDPR/local privacy requirements with auditable verification evidence.
- **NFR-004 Maintainability**: Every release MUST provide traceability from requirement IDs to implementation tasks and verification outcomes.
- **NFR-005 Usability**: At least 90% of employees in pilot usage should complete daily time/task updates without support intervention.
- **NFR-006 Scalability**: System MUST support 1,000 concurrently active employee sessions in pilot rollout without violating NFR-001 latency targets.
- **NFR-007 Accessibility**: Employee-facing mobile interfaces MUST conform to WCAG 2.1 AA criteria for text contrast, semantic labels, and scalable text support.

For each NFR, validation method will be defined in planning and task phases through test evidence, operational logs, and release review checkpoints.

### Key Entities *(include if feature involves data)*

- **Employee**: Authenticated user with identity, profile details, role, and preferences.
- **Task**: Work item with status lifecycle, estimate, elapsed time, and project linkage.
- **Project**: Assigned project context including progress, schedule dates, team members, and related tasks.
- **TimesheetEntry**: Date-based record of task hours and submission status.
- **LeaveRequest**: Leave type/date request with balance impact and approval status.
- **NotificationEvent**: Event message generated from task, timesheet, and leave workflows.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 95% of employees can complete login and reach dashboard in under 60 seconds.
- **SC-002**: 95% of task state changes (start, pause, complete) are reflected to the user within 2 seconds.
- **SC-003**: At least 90% of pilot users submit timesheets daily without manual support.
- **SC-004**: Leave request creation and status visibility reduce leave-related follow-up queries by at least 40% in pilot period.
