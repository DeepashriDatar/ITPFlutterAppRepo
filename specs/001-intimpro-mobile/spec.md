# Feature Specification: InTimePro Mobile Application

**Feature Branch**: `001-intimpro-mobile`  
**Created**: March 12, 2026  
**Status**: Draft  
**Input**: Employee Productivity & Time Tracking Mobile Application

## Overview

InTimePro is a comprehensive mobile application that extends the existing InTimePro desktop platform to enable company employees to manage their daily work activities on the go. The application provides task tracking, time tracking, project information, leave management, timesheet submission, and notifications—all designed to improve employee productivity while maintaining accurate work hour tracking.

---

## User Scenarios & Testing

### User Story 1 - Employee Authentication and Secure Access (Priority: P1)

**Actor**: New or existing company employee  
**Action**: Login to access application features  
**Value**: Enables secure access to personal work data and prevents unauthorized access

An employee downloads the InTimePro mobile app and needs to authenticate before accessing any features. The application must support multiple authentication methods (email/password, Microsoft SSO, Google SSO) to accommodate different corporate environments. Once authenticated, the employee can choose to stay logged in for convenient access, or manually login each time for security-sensitive environments. If the employee forgets their password, they can securely reset it via email.

**Why this priority**: Authentication is the critical foundation for all other features. Without secure login, no other feature can function. This is a blocker for any other functionality.

**Independent Test**: 
- User can successfully login with email/password
- User can successfully login with Microsoft account
- User can successfully login with Google account
- User can change password after reset
- All user preferences and data are protected behind authentication
- Feature can be demonstrated and validated independently

**Acceptance Scenarios**:

1. **Email/Password Login**
   - **Given** user has valid credentials and app is installed
   - **When** user enters email and password and taps Login
   - **Then** user is authenticated and navigated to Dashboard

2. **Remember Me Functionality**
   - **Given** user is logged in and on Login screen
   - **When** user checks "Remember Me" before logging in
   - **Then** user remains logged in after app is closed and reopened (within 30 days)

3. **Social Login - Microsoft**
   - **Given** user has Microsoft account linked to company email
   - **When** user taps "Login with Microsoft"
   - **Then** user is redirected to Microsoft OAuth flow and authenticated upon completion

4. **Social Login - Google**
   - **Given** user has Google account linked to company email
   - **When** user taps "Login with Google"
   - **Then** user is redirected to Google OAuth flow and authenticated upon completion

5. **Password Reset**
   - **Given** user forgot their password
   - **When** user taps "Forgot Password" and enters their email
   - **Then** password reset email is sent and user can set new password

6. **Session Timeout**
   - **Given** user is logged in and app becomes inactive
   - **When** app remains inactive for 30 minutes (or customizable time)
   - **Then** user is logged out and prompted to login again

---

### User Story 2 - Employee Dashboard with Time and Task Overview (Priority: P1)

**Actor**: Employee who has logged in  
**Action**: View current work status at a glance  
**Value**: Provides quick overview of work progress and current activities—essential for time tracking visibility

An employee has just opened the app and needs to see their current work status immediately. The dashboard displays key metrics: clock-in time, active working hours today, total work time, and their currently active task. The employee can start, pause, or complete the active task directly from the dashboard without navigating to separate screens. This is the primary working interface for daily task management.

**Why this priority**: The dashboard is the primary interaction point for daily work. Every employee needs to see their current status at a glance. This is fundamental to the app's core value proposition of time tracking.

**Independent Test**:
- User can see clock-in time
- User can see active working hours
- User can see total work time
- User can see current active task
- User can start a task from dashboard
- User can pause a task from dashboard
- User can complete a task from dashboard
- All information updates in real-time as tasks are modified

**Acceptance Scenarios**:

1. **View Clock-in Time**
   - **Given** employee is on Dashboard after login
   - **When** employee views the dashboard
   - **Then** clock-in time is displayed in HH:MM format (e.g., "08:30 AM")

2. **View Active Working Hours**
   - **Given** employee has been logged in and task timer running
   - **When** employee views dashboard
   - **Then** active working hours are displayed and update every second (e.g., "2h 45m 30s")

3. **View Total Work Time**
   - **Given** employee has completed tasks today
   - **When** employee views dashboard
   - **Then** total work time for the day is displayed (sum of all completed task times)

4. **View Current Active Task**
   - **Given** employee has a task in "In Progress" status
   - **When** employee views dashboard
   - **Then** current task name, elapsed time, and estimated remaining time are displayed

5. **Start Task from Dashboard**
   - **Given** employee has a task in "New" status
   - **When** employee taps "Start Task" button on dashboard
   - **Then** task timer starts, task status changes to "In Progress", and timer displays on dashboard

6. **Pause Task from Dashboard**
   - **Given** employee has a running task timer
   - **When** employee taps "Pause" button
   - **Then** task timer pauses, status remains "In Progress", elapsed time is preserved

7. **Complete Task from Dashboard**
   - **Given** employee has a running or paused task
   - **When** employee taps "Complete" button
   - **Then** task status changes to "Completed", timer stops, and task is moved to completed tasks list

---

### User Story 3 - Task Lifecycle Management (Priority: P1)

**Actor**: Employee managing daily tasks  
**Action**: Create, start, track, and complete tasks with automatic time tracking  
**Value**: Enables accurate task duration measurement and task status visibility for productivity tracking

An employee needs to manage their daily task list with clear status visibility and automatic time tracking. Tasks progress through defined states: New → In Progress → Completed. The application automatically tracks time spent on each task. If a task's estimated time is exceeded, the system automatically marks it as "Overdue" to alert the employee. Employees can view tasks filtered by status, making it easy to focus on what needs attention.

**Why this priority**: Task management is core to the application's purpose. Without task tracking, the entire productivity and time tracking system cannot function effectively.

**Independent Test**:
- User can create a new task
- Task defaults to "New" status
- User can start task timer
- User can view tasks by status filter
- Overdue detection works correctly
- User can complete tasks
- Feature provides functional value independently

**Acceptance Scenarios**:

1. **Create New Task**
   - **Given** employee is on Task Management screen
   - **When** employee taps "Create Task" and enters task name, description, estimated hours
   - **Then** task is created with status "New" and appears in task list

2. **Task Initial Status**
   - **Given** new task has been created
   - **When** task is viewed in list
   - **Then** task status displays as "New"

3. **Start Task Timer**
   - **Given** employee has a task in "New" status
   - **When** employee taps "Start" on the task
   - **Then** task status changes to "In Progress" and timer starts counting elapsed time

4. **View Tasks by Status Filter**
   - **Given** employee has tasks in various statuses
   - **When** employee selects status filter (New, In Progress, Overdue, Completed)
   - **Then** only tasks matching that status are displayed

5. **Overdue Detection**
   - **Given** task has estimated time of 2 hours and is in "In Progress" status
   - **When** elapsed time exceeds 2 hours (120 minutes)
   - **Then** task status automatically changes to "Overdue" and visual indicator shows red/warning color

6. **Stop Task Timer**
   - **Given** task is running with active timer
   - **When** employee taps "Pause" button
   - **Then** timer pauses and can be resumed later

7. **Complete Task**
   - **Given** task is in "In Progress" or "Overdue" status
   - **When** employee taps "Complete" button
   - **Then** task status changes to "Completed", timer stops, and task is moved to completed section

---

### User Story 4 - Project Visibility and Progress Tracking (Priority: P2)

**Actor**: Employee assigned to project(s)  
**Action**: View assigned projects and track project progress  
**Value**: Maintains visibility into project status and team collaborationwithout switching to desktop app

An employee assigned to multiple projects needs to understand their role in each project and current progress. The application displays a list of assigned projects with key metrics: project progress percentage, start and end dates, team member count, and number of associated tasks. This helps employees understand project scope and deadlines without leaving the mobile app.

**Why this priority**: While important for context and motivation, project visibility is secondary to task management. The primary focus is individual task tracking, but project context enhances the employee experience.

**Independent Test**:
- User can see list of assigned projects
- User can see project progress percentage
- User can see project dates
- User can see team members
- User can see task count per project
- Feature delivers value independently of other modules

**Acceptance Scenarios**:

1. **View Assigned Projects List**
   - **Given** employee navigates to Projects screen
   - **When** screen loads
   - **Then** list displays all projects assigned to employee

2. **View Project Progress**
   - **Given** employee views project in list
   - **When** viewing project details
   - **Then** progress bar shows completion percentage (0-100%)

3. **View Project Dates**
   - **Given** employee views project details
   - **When** project information is displayed
   - **Then** start date and end date are shown in standard format (MM/DD/YYYY)

4. **View Project Team**
   - **Given** employee views project details
   - **When** team section is expanded
   - **Then** list shows all team members assigned to project

5. **View Tasks Count**
   - **Given** employee views project details
   - **When** task information displays
   - **Then** shows total number of tasks in project and breakdown by status

---

### User Story 5 - Timesheet Management and Submission (Priority: P2)

**Actor**: Employee who must submit timesheets for payroll  
**Action**: View, log, and submit timesheets  
**Value**: Ensures accurate hour tracking for payroll and compliance purposes

An employee must track and submit hours worked for payroll processing. The timesheet interface displays a calendar view allowing employees to review logged hours by day. Employees can log hours for completed tasks, and the system automatically totals logged hours. Employees can submit timesheets daily or weekly (depending on company policy). Previously submitted timesheets remain visible in an archive for audit and reference purposes.

**Why this priority**: Timesheet submission is critical for payroll and compliance, making this a high-priority feature, though it's typically a daily or weekly routine rather than continuous interaction.

**Independent Test**:
- User can view timesheet calendar
- User can log hours for tasks
- User can submit timesheet
- User can view submitted timesheets
- System calculates total hours correctly
- Feature works independently

**Acceptance Scenarios**:

1. **View Timesheet Calendar**
   - **Given** employee navigates to Timesheet screen
   - **When** screen loads
   - **Then** calendar displays current week with days and logged hours

2. **Log Hours for Task**
   - **Given** employee has completed tasks
   - **When** employee selects task and taps "Log Hours"
   - **Then** estimated or actual hours are logged to timesheet for that day

3. **Submit Timesheet**
   - **Given** employee has logged hours
   - **When** employee taps "Submit Timesheet"
   - **Then** timesheet is marked "Submitted", locked from editing, and notification confirms submission

4. **View Total Logged Hours**
   - **Given** employee has logged hours across multiple tasks
   - **When** viewing timesheet
   - **Then** total logged hours for the day/week displays correctly

5. **View Submitted Timesheets**
   - **Given** employee has submitted previous timesheets
   - **When** employee navigates to "Submissions" or "History"
   - **Then** list shows all submitted timesheets with submission date and total hours

---

### User Story 6 - Leave Management and Approval Tracking (Priority: P2)

**Actor**: Employee managing time off  
**Action**: Apply for leaves, track balances, and monitor approval status  
**Value**: Empowers employees to manage worklife balance and simplifies leave tracking process

An employee needs to apply for various types of leaves (vacation, sick leave, personal, unpaid, etc.) and track their leave balance. The app provides a dedicated interface to select leave type, date range, and submit requests. The system displays current leave balance for each leave type, annual entitlements, and previously booked leaves. Employees receive notifications when their leave requests are approved or denied. This reduces administrative overhead and provides clear visibility into leave status.

**Why this priority**: Leave management is important but typically a weekly/monthly task. Therefore, it's prioritized below core daily task features but above lower-frequency features.

**Independent Test**:
- User can apply for leave
- User can select leave type
- User can select date range
- User can see leave status
- User can view leave balance
- User can view entitlements
- User can see booked leaves
- Feature provides complete leave workflow

**Acceptance Scenarios**:

1. **Apply for Leave**
   - **Given** employee navigates to Leave Management screen
   - **When** employee taps "Apply Leave" and provides required information
   - **Then** leave request is created with status "Pending" and timestamps recorded

2. **Select Leave Type**
   - **Given** employee is creating leave request
   - **When** employee selects leave type dropdown
   - **Then** available types are listed (Vacation, Sick, Personal, Unpaid, etc.)

3. **Select Leave Dates**
   - **Given** employee is creating leave request
   - **When** employee selects start and end dates via calendar picker
   - **Then** date range is recorded and validated (end date ≥ start date)

4. **View Leave Request Status**
   - **Given** employee has submitted leave request
   - **When** employee views leave in list
   - **Then** status displays (Pending, Approved, Rejected, Cancelled)

5. **View Leave Balance**
   - **Given** employee navigates to Leave Management
   - **When** Leave Balance section displays
   - **Then** current balance shows for each leave type (e.g., "12 days remaining")

6. **View Annual Entitlement**
   - **Given** employee views Leave Management
   - **When** viewing leave entitlements
   - **Then** total allocated days for current year display (e.g., "20 days vacation")

7. **View Booked Leaves**
   - **Given** employee has previously booked leaves
   - **When** viewing "Booked Leave" section
   - **Then** list shows all active and past bookings with date ranges

---

### User Story 7 - Notifications for Task and Leave Events (Priority: P2)

**Actor**: Employee receiving notifications  
**Action**: Receive timely notifications for work events  
**Value**: Keeps employee informed of important work status changes without requiring app interaction

The application sends notifications for key work events: task start/completion, timesheet submission, and leave request status changes. Notifications appear both in-app and as push notifications depending on notification settings. Employees can customize notification preferences (on/off, notification timing) within Settings. Notifications reduce the need for manual status checking and improve communication efficiency.

**Why this priority**: Notifications enhance user experience and engagement but aren't essential for core functionality. Users can still function without them, making this P2.

**Independent Test**:
- User receives notification when task starts
- User receives notification when task stops
- User receives notification when task completes
- User receives notification when timesheet is submitted
- User receives notification when leave is applied
- User receives notification when leave status changes
- All notifications are properly delivered and actionable

**Acceptance Scenarios**:

1. **Task Start Notification**
   - **Given** employee starts a task
   - **When** task status changes to "In Progress"
   - **Then** notification displays: "Task '[TaskName]' started"

2. **Task Completion Notification**
   - **Given** employee marks task complete
   - **When** task status changes to "Completed"
   - **Then** notification displays: "Task '[TaskName]' completed - [Duration]"

3. **Timesheet Submission Notification**
   - **Given** employee submits timesheet
   - **When** submission is successful
   - **Then** notification displays: "Timesheet submitted with [TotalHours] hours"

4. **Leave Application Notification**
   - **Given** employee applies for leave
   - **When** leave request is created
   - **Then** confirmation notification displays: "Leave request created for [DateRange]"

5. **Leave Status Change Notification**
   - **Given** leave request is approved/rejected (by manager)
   - **When** status update occurs in backend
   - **Then** push notification alerts employee: "Your leave request has been [Approved/Rejected]"

---

### User Story 8 - Account Settings and Profile Management (Priority: P3)

**Actor**: Employee managing their account  
**Action**: View and edit account information, configure preferences, and logout  
**Value**: Allows employees to maintain their profile and secure their account

The Settings screen enables employees to view and edit their account information (name, email, phone), configure notification preferences, enable/disable private time tracking, and logout. This ensures employees can maintain control over their account and customize the app experience to their preferences.

**Why this priority**: While important for account management, Settings is typically accessed infrequently compared to core task and timesheet features. It's supporting functionality rather than primary workflow.

**Independent Test**:
- User can view account information
- User can edit account details
- User can enable private time timer
- User can logout
- All changes persist across sessions
- Feature provides security and personalization

**Acceptance Scenarios**:

1. **View Account Information**
   - **Given** employee navigates to Settings
   - **When** Settings screen displays
   - **Then** current name, email, phone number are shown

2. **Edit Account Details**
   - **Given** employee is in Account Settings
   - **When** employee taps an editable field and updates information
   - **Then** changes are saved and confirmed with success message

3. **Enable Private Time Timer**
   - **Given** employee is in Settings
   - **When** employee toggles "Private Time Timer" toggle
   - **Then** timer is enabled/disabled and setting persists

4. **Logout from Application**
   - **Given** employee is logged in
   - **When** employee taps "Logout" in Settings
   - **Then** user is logged out, all session data cleared, and returned to Login screen

---

## Edge Cases & Error Scenarios

- What happens when employee loses internet connectivity during task tracking?
  - **Answer**: Task timer continues locally; changes sync when connection is restored. This is critical to prevent data loss.

- How does system handle clock-out at end of day with running task?
  - **Answer**: System prompts employee to complete or pause task before logging off to maintain accurate time records.

- What happens if employee tries to submit timesheet with zero hours logged?
  - **Answer**: System prevents submission and shows warning: "Timesheet must have at least X hours before submission" (X configured by HR).

- How does system handle leave application for dates with incomplete timesheets?
  - **Answer**: System allows leave application but warns: "Timesheet incomplete for [DateRange]. Please log hours before submitting leave request."

- What happens if employee tries to logout during active task?
  - **Answer**: System shows confirmation dialog: "You have an active task. Pause it before logging out?" to prevent accidental data loss.

---

## QA Test Cases

### User Story 1 - Authentication Test Cases

#### TC-AUTH-001: Email/Password Login - Valid Credentials
- **Priority**: P0 (Critical)
- **Preconditions**: App installed, user account exists in system with email: test@company.com and password: TestPass123!
- **Test Steps**:
  1. Launch InTimePro app
  2. On Login screen, enter email: test@company.com
  3. Enter password: TestPass123!
  4. Tap "Login" button
  5. Wait for authentication response
- **Expected Results**:
  - User successfully authenticates
  - User is navigated to Dashboard screen
  - Session token is generated and stored securely
  - User ID and preferences are loaded
- **Test Data**: email: test@company.com, password: TestPass123!
- **Execution Time**: < 5 seconds

#### TC-AUTH-002: Email/Password Login - Invalid Credentials
- **Priority**: P0 (Critical)
- **Preconditions**: App installed, user account exists
- **Test Steps**:
  1. Launch InTimePro app
  2. Enter email: test@company.com
  3. Enter password: WrongPassword
  4. Tap "Login" button
- **Expected Results**:
  - Login fails
  - Error message displays: "Invalid email or password"
  - User remains on Login screen
  - Session is not created
- **Test Data**: email: test@company.com, password: WrongPassword
- **Execution Time**: < 3 seconds

#### TC-AUTH-003: Remember Me Functionality
- **Priority**: P1 (High)
- **Preconditions**: App installed, user account exists
- **Test Steps**:
  1. Launch InTimePro app on Login screen
  2. Check "Remember Me" checkbox
  3. Enter valid email and password
  4. Tap "Login" button
  5. Close app completely
  6. Reopen app after 5 minutes
  7. Verify session status
- **Expected Results**:
  - User successfully logs in
  - User is remembered for 30 days
  - App reopens directly to Dashboard (no login required)
  - Session token is valid and accessible
- **Test Data**: email: test@company.com, password: TestPass123!
- **Execution Time**: < 10 seconds per session

#### TC-AUTH-004: Social Login - Microsoft Account
- **Priority**: P1 (High)
- **Preconditions**: App installed, Microsoft OAuth is configured, user has Azure AD account
- **Test Steps**:
  1. Launch InTimePro app
  2. On Login screen, tap "Login with Microsoft"
  3. User is redirected to Microsoft login (Azure AD)
  4. Enter Microsoft credentials: user@microsoft.com / password
  5. Approve app permissions on Microsoft consent screen
  6. User is redirected back to app
  7. Verify Dashboard loads
- **Expected Results**:
  - User successfully authenticates via Microsoft
  - User profile is created/updated from Microsoft account data
  - User is navigated to Dashboard
  - OAuth token is stored securely
- **Test Data**: Microsoft account with company email linked
- **Execution Time**: < 10 seconds

#### TC-AUTH-005: Social Login - Google Account
- **Priority**: P1 (High)
- **Preconditions**: App installed, Google OAuth is configured, user has Google account
- **Test Steps**:
  1. Launch InTimePro app
  2. On Login screen, tap "Login with Google"
  3. User is redirected to Google login
  4. Enter Google credentials
  5. Select account to use for login
  6. Approve app permissions on Google consent screen
  7. Verify Dashboard loads
- **Expected Results**:
  - User successfully authenticates via Google
  - User profile is created/updated from Google account data
  - User is navigated to Dashboard
  - OAuth token is stored securely
- **Test Data**: Google account with company email
- **Execution Time**: < 10 seconds

#### TC-AUTH-006: Forgot Password - Password Reset Flow
- **Priority**: P1 (High)
- **Preconditions**: App installed, user account exists with email: test@company.com
- **Test Steps**:
  1. Launch InTimePro app
  2. On Login screen, tap "Forgot Password" link
  3. Enter email: test@company.com
  4. Tap "Send Reset Link" button
  5. Check email inbox for reset link
  6. Click reset link in email
  7. Set new password: NewPass123!
  8. Confirm new password
  9. Tap "Reset Password" button
  10. Use new password to login
- **Expected Results**:
  - Password reset email is sent within 1 minute
  - Reset link is valid for 24 hours
  - User can set new password successfully
  - User can login with new password
  - Old password is no longer valid
- **Test Data**: email: test@company.com, old password: TestPass123!, new password: NewPass123!
- **Execution Time**: < 5 minutes

#### TC-AUTH-007: Session Timeout - 30 Minutes Inactivity
- **Priority**: P1 (High)
- **Preconditions**: App installed, user is logged in and authenticated
- **Test Steps**:
  1. User logs in successfully and navigates to Dashboard
  2. App remains open but inactive for 30 minutes
  3. User attempts to interact with app (tap a button)
  4. Observe session status and behavior
- **Expected Results**:
  - Session timeout occurs after 30 minutes of inactivity
  - User is logged out automatically
  - User is redirected to Login screen
  - Session token is cleared from device memory
  - Message displays: "Session expired. Please login again."
- **Test Data**: 30-minute wait time
- **Execution Time**: 30+ minutes (automated testing)

#### TC-AUTH-008: Secure Token Storage
- **Priority**: P0 (Critical)
- **Preconditions**: App installed, user is logged in
- **Test Steps**:
  1. User logs in successfully
  2. Verify app state using device inspection tools
  3. Check for token storage location (Keychain on iOS, Keystore on Android)
  4. Verify token is encrypted and not in plaintext
  5. Verify token is not logged in debug output
- **Expected Results**:
  - Token is stored in secure storage (Keychain/Keystore)
  - Token is encrypted with device encryption
  - Token is not visible in plaintext in logs
  - Token is cleared upon logout
- **Test Data**: User session token
- **Execution Time**: < 5 minutes

---

### User Story 2 - Dashboard Test Cases

#### TC-DASH-001: View Clock-in Time
- **Priority**: P0 (Critical)
- **Preconditions**: User is logged in, current time is 8:30 AM
- **Test Steps**:
  1. Navigate to Dashboard
  2. Observe clock-in time display
  3. Verify time format and accuracy
- **Expected Results**:
  - Clock-in time displays in HH:MM AM/PM format
  - Clock-in time matches system time (within 1 second)
  - Example: "08:30 AM"
  - Time updates if user logs out and logs back in
- **Test Data**: Current system time
- **Execution Time**: < 2 seconds

#### TC-DASH-002: View Active Working Hours Real-time
- **Priority**: P0 (Critical)
- **Preconditions**: User is logged in, active task is running
- **Test Steps**:
  1. Navigate to Dashboard with active running task
  2. Observe active working hours display
  3. Wait 5 seconds and verify time increments
  4. Verify format (HH:MM:SS)
- **Expected Results**:
  - Active working hours display in HH:MM:SS format
  - Timer increments by 1 second every second
  - Example: "02:45:30" means 2 hours 45 minutes 30 seconds
  - Updates occur with < 500ms delay
- **Test Data**: Active task timer running
- **Execution Time**: < 10 seconds

#### TC-DASH-003: View Total Work Time for Day
- **Priority**: P1 (High)
- **Preconditions**: User has completed 3 tasks today with durations: 1h 30m, 2h 15m, 45m
- **Test Steps**:
  1. Navigate to Dashboard
  2. Observe "Total Work Time" display
  3. Verify calculation accuracy
- **Expected Results**:
  - Total work time displays correctly: "4h 30m"
  - Calculation: 1h 30m + 2h 15m + 45m = 4h 30m
  - Format is clear and user-readable
  - Updates when new tasks are completed
- **Test Data**: Three completed tasks with known durations
- **Execution Time**: < 2 seconds

#### TC-DASH-004: View Current Active Task
- **Priority**: P0 (Critical)
- **Preconditions**: User has active task "Design Homepage" running since 10:00 AM, estimated 3 hours
- **Test Steps**:
  1. Navigate to Dashboard
  2. Observe "Current Active Task" section
  3. Verify task details display
- **Expected Results**:
  - Task name displays: "Design Homepage"
  - Elapsed time shows actual duration since start
  - Estimated time shows: "3h 0m"
  - Remaining time calculates: est - elapsed
  - If elapsed > estimated, mark as "OVERDUE"
- **Test Data**: Active task with name, estimated hours, and start time
- **Execution Time**: < 2 seconds

#### TC-DASH-005: Start Task from Dashboard
- **Priority**: P0 (Critical)
- **Preconditions**: User is on Dashboard, task exists in "New" status with title "Code Login Feature"
- **Test Steps**:
  1. On Dashboard, observe "Current Active Task" section showing "No Active Task"
  2. Tap "Start Task" button
  3. Select task: "Code Login Feature"
  4. Confirm task start
  5. Observe timer start and status change
- **Expected Results**:
  - Task status changes from "New" to "In Progress"
  - Timer starts and counts up in real-time
  - "Current Active Task" section now displays the task
  - Quick action buttons show: Pause, Complete
  - Notification appears: "Task 'Code Login Feature' started"
- **Test Data**: Task in "New" status
- **Execution Time**: < 3 seconds

#### TC-DASH-006: Pause Task from Dashboard
- **Priority**: P1 (High)
- **Preconditions**: User has active running task with 1h 15m elapsed time
- **Test Steps**:
  1. On Dashboard, observe running task timer
  2. Tap "Pause" button on active task
  3. Verify timer behavior
  4. Tap "Resume" to continue
  5. Verify timer resumes
- **Expected Results**:
  - Pause button becomes available when task is running
  - Clicking Pause stops the timer (no further increment)
  - Elapsed time is preserved: "1h 15m"
  - Task status remains "In Progress"
  - Resume button appears to continue timer
  - Paused task can be resumed later (within same session or after app restart)
- **Test Data**: Running task with elapsed time
- **Execution Time**: < 5 seconds

#### TC-DASH-007: Complete Task from Dashboard
- **Priority**: P0 (Critical)
- **Preconditions**: User has active running or paused task "Design Homepage"
- **Test Steps**:
  1. On Dashboard with active task
  2. Tap "Complete" button
  3. Confirm task completion
  4. Verify status update
  5. Check dashboard updates
- **Expected Results**:
  - Task status changes from "In Progress" to "Completed"
  - Timer stops and final elapsed time is recorded
  - Task is moved to "Completed" section
  - "Current Active Task" now shows "No Active Task"
  - Total work time is updated to include completed task
  - Notification appears: "Task 'Design Homepage' completed - 3h 45m"
  - Task appears in timesheet for that day
- **Test Data**: Active task to complete
- **Execution Time**: < 3 seconds

#### TC-DASH-008: Dashboard Real-time Updates
- **Priority**: P1 (High)
- **Preconditions**: User is viewing Dashboard with active task
- **Test Steps**:
  1. Open Dashboard and note active task timer reading
  2. Wait 5 seconds without interacting
  3. Observe timer increment
  4. Move to another app briefly
  5. Return to Dashboard
  6. Verify timer accuracy
- **Expected Results**:
  - Timer updates every 1 second automatically
  - No manual refresh required
  - Timer accuracy is within 1 second even after app backgrounding
  - Dashboard metrics stay in sync with backend
  - Updates contain < 500ms UI recalculation delay
- **Test Data**: Active task running
- **Execution Time**: < 30 seconds

---

### User Story 3 - Task Management Test Cases

#### TC-TASK-001: Create New Task
- **Priority**: P0 (Critical)
- **Preconditions**: User is logged in and on Task Management screen
- **Test Steps**:
  1. Tap "Create Task" or "+" button
  2. Enter task name: "Implement API Endpoint"
  3. Enter description: "Create REST API for user authentication"
  4. Select estimated hours: "4"
  5. Tap "Create Task" button
  6. Verify task appears in list
- **Expected Results**:
  - Task is created successfully
  - Task appears in task list with status "New"
  - Task ID is generated and stored
  - Timestamp is recorded for creation time
  - Task is marked with project (if applicable)
  - No timer has started (elapsed time = 0)
- **Test Data**: Task name: "Implement API Endpoint", Description: "Create REST API", Estimated: 4 hours
- **Execution Time**: < 5 seconds

#### TC-TASK-002: Task Initial Status
- **Priority**: P0 (Critical)
- **Preconditions**: New task has been created
- **Test Steps**:
  1. Navigate to Task Management
  2. Locate newly created task
  3. Observe task status field
- **Expected Results**:
  - Newly created task status displays as "New"
  - Status cannot be manually changed to anything other than "In Progress" when started
  - Task appears in "New" status filter view
  - Status badge/indicator shows "New" with appropriate styling
- **Test Data**: Recently created task
- **Execution Time**: < 2 seconds

#### TC-TASK-003: Start Task Timer
- **Priority**: P0 (Critical)
- **Preconditions**: Task in "New" status exists: "Code Database Schema"
- **Test Steps**:
  1. Navigate to Task Management
  2. Find task "Code Database Schema"
  3. Tap "Start" button or swipe action
  4. Observe status and timer start
  5. Wait 10 seconds and verify timer increments
- **Expected Results**:
  - Task status automatically changes to "In Progress"
  - Timer starts at "00:00:00" and begins counting up
  - Timer increments by 1 second every second
  - Start timestamp is recorded
  - Task appears in "In Progress" filter view
  - Notification: "Task 'Code Database Schema' started"
- **Test Data**: Task in New status
- **Execution Time**: < 3 seconds

#### TC-TASK-004: View Tasks by Status Filter
- **Priority**: P1 (High)
- **Preconditions**: User has tasks with different statuses: 3 New, 2 In Progress, 1 Overdue, 4 Completed
- **Test Steps**:
  1. Navigate to Task Management
  2. Tap "Filter" or status tabs
  3. Select "New" filter
  4. Verify only New tasks display (count = 3)
  5. Select "In Progress" filter
  6. Verify only In Progress tasks display (count = 2)
  7. Select "Overdue" filter
  8. Verify Overdue tasks display (count = 1)
  9. Select "Completed" filter
  10. Verify Completed tasks display (count = 4)
  11. Select "All" to see all tasks
- **Expected Results**:
  - Each filter correctly displays only tasks in that status
  - Task count matches expected values
  - Filter persists until changed
  - Visual indicators show active filter
  - All tasks are accessible through filters
  - No tasks appear in incorrect status filter
- **Test Data**: Tasks in all 4 statuses
- **Execution Time**: < 10 seconds

#### TC-TASK-005: Overdue Detection
- **Priority**: P0 (Critical)
- **Preconditions**: Task "Deploy to Production" has estimated time of 2 hours (120 minutes)
- **Test Steps**:
  1. Start task "Deploy to Production"
  2. Verify status is "In Progress"
  3. Simulate time passing (or use backend to advance time)
  4. After 120+ minutes elapsed, check task status
  5. Verify visual indication of overdue status
- **Expected Results**:
  - When elapsed time ≤ estimated time: status remains "In Progress"
  - When elapsed time > estimated time: status automatically changes to "Overdue"
  - Task color/styling changes to red/warning color
  - Task appears in "Overdue" filter view
  - Notification: "Task 'Deploy to Production' is overdue"
  - Overdue indicator shows excess time: "2h 15m (overdue by 15m)"
  - Overdue tasks are highlighted in list
- **Test Data**: Task with 2-hour estimate, current elapsed time > 2 hours
- **Execution Time**: < 5 seconds (or time-dependent test)

#### TC-TASK-006: Stop/Pause Task Timer
- **Priority**: P1 (High)
- **Preconditions**: Task timer is running with 1h 30m elapsed
- **Test Steps**:
  1. On active task with running timer showing "1h 30m"
  2. Tap "Pause" button
  3. Verify timer stops incrementing
  4. Wait 10 seconds
  5. Verify timer still shows "1h 30m" (no increment)
  6. Tap "Resume" button
  7. Verify timer resumes counting
- **Expected Results**:
  - Pause button stops timer increment
  - Elapsed time is preserved
  - Timer does not advance while paused
  - Resume button resumes counting from where it paused
  - Task status remains "In Progress" during pause
  - Paused time is not counted/lost
  - Task can be paused/resumed multiple times
- **Test Data**: Running task timer
- **Execution Time**: < 10 seconds

#### TC-TASK-007: Complete Task
- **Priority**: P0 (Critical)
- **Preconditions**: Task in "In Progress" status with 2h 30m elapsed time
- **Test Steps**:
  1. On In Progress task
  2. Tap "Complete" or "Mark as Complete" button
  3. Confirm completion if dialog appears
  4. Verify task status change
  5. Check task moved to Completed section
- **Expected Results**:
  - Task status changes from "In Progress" to "Completed"
  - Timer stops permanently (no further increment)
  - Final elapsed time is recorded: "2h 30m"
  - Completion timestamp is recorded
  - Task disappears from "In Progress" and active views
  - Task appears in "Completed" filter view
  - Task is marked in timesheet for that day
  - Notification: "Task completed - 2h 30m"
  - Can still view completed task details for reference
- **Test Data**: Task with elapsed time recorded
- **Execution Time**: < 3 seconds

#### TC-TASK-008: Task Timer Precision - Seconds Level
- **Priority**: P1 (High)
- **Preconditions**: Task timer is running
- **Test Steps**:
  1. Start task and note exact start time
  2. Let timer run for exactly 62 seconds
  3. Tap Complete
  4. Verify elapsed time recorded
  5. Compare actual vs. recorded time
- **Expected Results**:
  - Elapsed time is accurate to the second
  - Timer records: "0h 1m 2s" (62 seconds)
  - No rounding or time loss occurs
  - Precision is maintained in database storage
  - Cumulative time calculations are accurate
  - Timesheet hours calculated correctly from seconds
- **Test Data**: Task timer, 62-second duration
- **Execution Time**: < 70 seconds

---

### User Story 4 - Project Management Test Cases

#### TC-PROJ-001: View Assigned Projects List
- **Priority**: P1 (High)
- **Preconditions**: User has 3 projects assigned: "Mobile App Redesign", "API Integration", "Database Migration"
- **Test Steps**:
  1. Navigate to Projects screen
  2. Observe project list
  3. Verify all assigned projects display
- **Expected Results**:
  - All 3 projects appear in list
  - Projects sorted by relevance or alphabetically
  - Each project shows name, status, progress at a glance
  - Unassigned projects do not appear
  - Projects are clickable to view details
- **Test Data**: 3 assigned projects
- **Execution Time**: < 3 seconds

#### TC-PROJ-002: View Project Progress Percentage
- **Priority**: P1 (High)
- **Preconditions**: Project "Mobile App Redesign" has 8 completed tasks out of 10 total tasks
- **Test Steps**:
  1. Navigate to Projects
  2. Find project "Mobile App Redesign"
  3. Observe progress percentage
- **Expected Results**:
  - Progress bar shows 80% completion
  - Progress label displays: "8/10 tasks completed (80%)"
  - Progress calculation: (8 tasks completed / 10 total tasks) × 100 = 80%
  - Progress bar visual matches percentage
  - Color gradient indicates progress (green = complete, yellow = in progress, grey = not started)
- **Test Data**: Project with 8 of 10 tasks completed
- **Execution Time**: < 2 seconds

#### TC-PROJ-003: View Project Dates
- **Priority**: P1 (High)
- **Preconditions**: Project "API Integration" starts 01/15/2026 and ends 03/31/2026
- **Test Steps**:
  1. Navigate to Projects
  2. Find project "API Integration"
  3. Observe date fields
- **Expected Results**:
  - Start date displays: "01/15/2026" (MM/DD/YYYY format)
  - End date displays: "03/31/2026"
  - Dates are clear and in standard format
  - Days remaining calculated if before end date
  - Status indicator shows "On Track" or "At Risk" based on progress vs. dates
- **Test Data**: Project with start date 01/15/2026, end date 03/31/2026
- **Execution Time**: < 2 seconds

#### TC-PROJ-004: View Project Team Members
- **Priority**: P1 (High)
- **Preconditions**: Project "Mobile App Redesign" has team members: Alice (Designer), Bob (Developer), Carol (QA)
- **Test Steps**:
  1. Navigate to Projects
  2. Find project "Mobile App Redesign"
  3. Tap project to view details or expand team section
  4. Observe team member list
- **Expected Results**:
  - All 3 team members display: Alice, Bob, Carol
  - Each team member shows name and role
  - Member count shows: "3 team members"
  - Team section can be expanded/collapsed
  - Can click on member to view profile (if applicable)
  - Current user is highlighted in team list
- **Test Data**: Project with 3 team members
- **Execution Time**: < 3 seconds

#### TC-PROJ-005: View Task Count per Project
- **Priority**: P1 (High)
- **Preconditions**: Project "Database Migration" has total 15 tasks: 3 New, 5 In Progress, 1 Overdue, 6 Completed
- **Test Steps**:
  1. Navigate to Projects
  2. Find project "Database Migration"
  3. Observe task count information
- **Expected Results**:
  - Total task count displays: "15 tasks"
  - Breakdown by status shows: "3 New | 5 In Progress | 1 Overdue | 6 Completed"
  - Count is accurate and updates in real-time
  - Can tap task count to jump to project's task list and filter
- **Test Data**: Project with 15 tasks in various statuses
- **Execution Time**: < 2 seconds

---

### User Story 5 - Timesheet Management Test Cases

#### TC-TIME-001: View Timesheet Calendar
- **Priority**: P1 (High)
- **Preconditions**: Current week is 03/10/2026 - 03/16/2026
- **Test Steps**:
  1. Navigate to Timesheet screen
  2. Observe calendar/week view
- **Expected Results**:
  - Calendar displays current week (Monday - Sunday format)
  - Days show: Mon 03/10, Tue 03/11, Wed 03/12, Thu 03/13, Fri 03/14, Sat 03/15, Sun 03/16
  - Each day cell shows logged hours below date
  - Current day is visually highlighted
  - Can navigate to previous/next weeks
  - Weekends can be visually distinguished
- **Test Data**: Current week timesheet
- **Execution Time**: < 2 seconds

#### TC-TIME-002: Log Hours for Task
- **Priority**: P1 (High)
- **Preconditions**: Task "Design Database Schema" was completed with 3.5 hours spent
- **Test Steps**:
  1. Navigate to Timesheet
  2. Select day (e.g., Wed 03/12/2026)
  3. Tap "Add Hours" or task entry area
  4. Select task "Design Database Schema"
  5. Enter/confirm hours: "3.5"
  6. Save entry
  7. Verify hours logged to timesheet
- **Expected Results**:
  - Timesheet entry is created for that day
  - Hours are recorded: "3.5 hours"
  - Entry links to specific task
  - Hours appear in daily total
  - Entry can be edited if needed
  - Decimal hours are supported (e.g., 3.5, 2.25)
- **Test Data**: Task with 3.5 hours
- **Execution Time**: < 5 seconds

#### TC-TIME-003: Submit Timesheet
- **Priority**: P1 (High)
- **Preconditions**: Timesheet for week 03/10-03/16 has entries totaling 40 hours logged
- **Test Steps**:
  1. Navigate to Timesheet
  2. Review entries and total: "40 hours"
  3. Tap "Submit Timesheet" button
  4. Confirm submission if prompted
  5. Verify submission status
- **Expected Results**:
  - Timesheet status changes from "Draft" to "Submitted"
  - Submission timestamp is recorded
  - Timesheet is locked from further editing
  - Confirmation message: "Timesheet submitted with 40 hours"
  - Submitted timesheet appears in "Submitted" view
  - Notification: "Your timesheet has been submitted"
- **Test Data**: Completed timesheet with 40 hours
- **Execution Time**: < 3 seconds

#### TC-TIME-004: View Submitted Timesheets
- **Priority**: P1 (High)
- **Preconditions**: User has 4 submitted timesheets from previous weeks
- **Test Steps**:
  1. Navigate to Timesheet
  2. Tap "Submitted" or "History" tab
  3. Observe list of submitted timesheets
- **Expected Results**:
  - All 4 submitted timesheets display in reverse chronological order (most recent first)
  - Each entry shows: Week range (03/03-03/09), submission date (03/10), total hours (40h)
  - Submitted timesheets are read-only
  - Can tap entry to view details and breakdown by day
  - Cannot modify submitted timesheets
- **Test Data**: 4 previously submitted timesheets
- **Execution Time**: < 2 seconds

#### TC-TIME-005: View Total Logged Hours
- **Priority**: P1 (High)
- **Preconditions**: Current week timesheet has logged hours: Mon 8h, Tue 8h, Wed 8h, Thu 7h, Fri 8h
- **Test Steps**:
  1. Navigate to current week Timesheet
  2. Observe total hours display
  3. Verify calculation
- **Expected Results**:
  - Total logged hours display: "39h 0m" or "39 hours"
  - Calculation: 8 + 8 + 8 + 7 + 8 = 39 hours
  - Total updates in real-time as entries are added
  - Format is clear and readable (HH:MM or decimal)
- **Test Data**: Timesheet with 39 hours logged
- **Execution Time**: < 2 seconds

#### TC-TIME-006: View Submitted Hours Only
- **Priority**: P1 (High)
- **Preconditions**: Current timesheet has 35h logged, 8h submitted entries
- **Test Steps**:
  1. Navigate to Timesheet
  2. Observe "Submitted Hours" metric
- **Expected Results**:
  - "Submitted Hours" shows only hours from submitted/approved entries
  - "Logged Hours" shows all currently logged entries
  - "Pending Hours" shows unsubmitted entries
  - Clear distinction between statuses
  - Helps employee track what's been officially submitted for payroll
- **Test Data**: Mixed submitted and unsubmitted hours
- **Execution Time**: < 2 seconds

---

### User Story 6 - Leave Management Test Cases

#### TC-LEAVE-001: Apply for Leave
- **Priority**: P1 (High)
- **Preconditions**: User is on Leave Management screen, has leave balance available
- **Test Steps**:
  1. Tap "Apply Leave" button
  2. Select leave type: "Vacation"
  3. Select start date: "2026-03-20"
  4. Select end date: "2026-03-24"
  5. Enter reason (optional): "Family vacation"
  6. Review summary: "5 days requested"
  7. Tap "Submit Leave Request"
  8. Verify confirmation
- **Expected Results**:
  - Leave request is created with status "Pending"
  - Request timestamp is recorded
  - Confirmation message: "Leave request submitted for 03/20-03/24"
  - Request appears in leave list with "Pending" status
  - Employee receives confirmation notification
  - Leave balance is temporarily reserved
- **Test Data**: Leave type: Vacation, dates: 03/20-03/24, reason: Family vacation
- **Execution Time**: < 5 seconds

#### TC-LEAVE-002: Select Leave Type
- **Priority**: P1 (High)
- **Preconditions**: User is creating leave request
- **Test Steps**:
  1. Tap "Leave Type" dropdown on leave form
  2. Observe available options
  3. Select "Sick Leave"
  4. Verify selection is captured
- **Expected Results**:
  - Dropdown displays all leave types: Vacation, Sick, Personal, Unpaid, Observed Holiday
  - Selected type shows in form
  - Each type has different balance rules (if applicable)
  - System validates leave type selection
- **Test Data**: Leave types list
- **Execution Time**: < 2 seconds

#### TC-LEAVE-003: Select Leave Dates
- **Priority**: P1 (High)
- **Preconditions**: User is on leave request form
- **Test Steps**:
  1. Tap "Start Date" field
  2. Calendar picker opens
  3. Navigate to March 2026
  4. Select 25th
  5. Tap "End Date" field
  6. Calendar picker opens again
  7. Select 27th (3 days later)
  8. System calculates days
- **Expected Results**:
  - Calendar picker displays correct month/year
  - Selected start date: 03/25/2026
  - Selected end date: 03/27/2026
  - System prevents selecting end date before start date
  - Days calculation: 3 days (inclusive)
  - Weekends can be included or excluded based on policy
  - Date range displays: "03/25 - 03/27 (3 days)"
- **Test Data**: Start: 03/25/2026, End: 03/27/2026
- **Execution Time**: < 5 seconds

#### TC-LEAVE-004: View Leave Request Status
- **Priority**: P1 (High)
- **Preconditions**: User has 2 pending leave requests and 1 approved
- **Test Steps**:
  1. Navigate to Leave Management
  2. Observe leave requests list
  3. Check status of each request
- **Expected Results**:
  - Pending requests show status: "Pending" (yellow indicator)
  - Approved requests show status: "Approved" (green indicator)
  - Rejected requests show status: "Rejected" (red indicator)
  - Cancelled requests show status: "Cancelled" (grey indicator)
  - Request details include approval date (if approved)
  - Can click request to view more details
- **Test Data**: Leave requests with different statuses
- **Execution Time**: < 2 seconds

#### TC-LEAVE-005: View Leave Balance
- **Priority**: P1 (High)
- **Preconditions**: User has vacation leave balance of 12 days
- **Test Steps**:
  1. Navigate to Leave Management
  2. Observe "Leave Balance" section
  3. Check each leave type balance
- **Expected Results**:
  - Vacation balance displays: "12 days remaining"
  - Sick leave balance: "10 days remaining"
  - Personal leave balance: "3 days remaining"
  - Unpaid leave: "Unlimited"
  - Format clear and readable
  - Updates when leave is approved/cancelled
- **Test Data**: Leave balances for each type
- **Execution Time**: < 2 seconds

#### TC-LEAVE-006: View Annual Entitlement
- **Priority**: P1 (High)
- **Preconditions**: User's annual leave entitlement is 20 days vacation
- **Test Steps**:
  1. Navigate to Leave Management
  2. Observe "Annual Entitlement" section
  3. Check total allocated days
- **Expected Results**:
  - Vacation entitlement displays: "20 days" (total allocated for year)
  - Used leave shows: "8 days"
  - Remaining balance calculates: 20 - 8 = 12 days
  - Visual representation shows progress (bar chart or list)
  - Applies to current calendar year
- **Test Data**: 20-day annual vacation entitlement, 8 days used
- **Execution Time**: < 2 seconds

#### TC-LEAVE-007: View Booked Leaves
- **Priority**: P1 (High)
- **Preconditions**: User has 3 booked leaves: approved vacation 03/20-03/24, pending sick 04/10, approved personal 02/14
- **Test Steps**:
  1. Navigate to Leave Management
  2. Tap "Booked Leaves" or "History" section
  3. Observe chronologically listed leaves
- **Expected Results**:
  - All 3 booked leaves appear in list
  - Past leaves (02/14) appear with historical notation
  - Current/future leaves show with status
  - Each entry shows: Type, Date Range, Status, Duration (days)
  - Sorted chronologically (past to future)
  - Can filter by status if needed
- **Test Data**: 3 booked leave requests with different statuses
- **Execution Time**: < 2 seconds

---

### User Story 7 - Notifications Test Cases

#### TC-NOT-001: Task Start Notification
- **Priority**: P1 (High)
- **Preconditions**: User has push notifications enabled
- **Test Steps**:
  1. Open app and start a task "Code Feature X"
  2. Observe in-app and push notification
  3. Check notification content
- **Expected Results**:
  - In-app notification displays: "[TaskName] started"
  - Push notification delivers to device
  - Notification contains task name
  - Notification displays immediately upon task start
  - Notification can be dismissed or tapped for details
  - Notification respects mute settings
- **Test Data**: Task "Code Feature X"
- **Execution Time**: < 2 seconds

#### TC-NOT-002: Task Stop Notification
- **Priority**: P1 (High)
- **Preconditions**: Task is running, user pauses task
- **Test Steps**:
  1. Pause a running task "Debug Issue"
  2. Observe notification
- **Expected Results**:
  - Notification displays: "Task 'Debug Issue' paused"
  - Shows elapsed time: "1h 30m elapsed"
  - Notification appears in-app and as push
  - Quiet notification if user is not actively using app
- **Test Data**: Running task paused
- **Execution Time**: < 2 seconds

#### TC-NOT-003: Task Complete Notification
- **Priority**: P1 (High)
- **Preconditions**: Task is running or paused, user completes it
- **Test Steps**:
  1. Tap "Complete" on task "Write Documentation"
  2. Observe notification content and timing
- **Expected Results**:
  - Notification displays: "Task 'Write Documentation' completed"
  - Also shows duration: "2h 45m"
  - Notification appears immediately
  - Push notification includes action (view task, start next task)
  - Rich notification shows task summary
- **Test Data**: Task with 2h 45m elapsed time
- **Execution Time**: < 2 seconds

#### TC-NOT-004: Timesheet Submission Notification
- **Priority**: P1 (High)
- **Preconditions**: User submits timesheet with 40 hours
- **Test Steps**:
  1. Submit timesheet for week 03/10-03/16
  2. Observe submission notification
- **Expected Results**:
  - Notification: "Timesheet submitted successfully"
  - Also displays: "Total: 40 hours logged"
  - Notification confirms submission
  - Shows submission timestamp
  - Allows navigation to timesheet view
- **Test Data**: Timesheet with 40 hours
- **Execution Time**: < 2 seconds

#### TC-NOT-005: Leave Application Notification
- **Priority**: P1 (High)
- **Preconditions**: User applies for vacation leave
- **Test Steps**:
  1. Submit leave request for 03/20-03/24
  2. Observe notification
- **Expected Results**:
  - Notification: "Leave request submitted"
  - Shows leave type and dates: "Vacation 03/20-03/24"
  - Notification displays reference number for tracking
  - Confirmation message received
- **Test Data**: Leave request for 5 days
- **Execution Time**: < 2 seconds

#### TC-NOT-006: Leave Status Change Notification
- **Priority**: P1 (High)
- **Preconditions**: Manager approves employee's leave request
- **Test Steps**:
  1. Manager approves employee's leave request (backend action)
  2. Employee receives notification
- **Expected Results**:
  - Push notification delivers: "Your leave request for 03/20-03/24 has been Approved"
  - In-app notification shows approval with timestamp
  - Leave request status updates in app
  - User can tap notification to view details
  - Multiple notifications for rejected requests
- **Test Data**: Leave request approval
- **Execution Time**: < 30 seconds (backend dependent)

---

### User Story 8 - Settings Test Cases

#### TC-SET-001: View Account Information
- **Priority**: P2 (Medium)
- **Preconditions**: User is logged in, profile data exists
- **Test Steps**:
  1. Navigate to Settings screen
  2. Tap "Account Information"
  3. Observe displayed fields
- **Expected Results**:
  - Name displays: "John Doe"
  - Email displays: "john.doe@company.com"
  - Phone displays: "+1-555-123-4567"
  - Department displays: "Engineering"
  - All fields are read-only in view mode
  - Can navigate back to Settings
- **Test Data**: User profile information
- **Execution Time**: < 2 seconds

#### TC-SET-002: Edit Account Details
- **Priority**: P2 (Medium)
- **Preconditions**: User is viewing account information, phone number is outdated
- **Test Steps**:
  1. On Account Information screen, tap "Edit"
  2. Tap phone number field
  3. Clear current value: "+1-555-123-4567"
  4. Enter new value: "+1-555-987-6543"
  5. Save changes
  6. Verify update
- **Expected Results**:
  - Edit mode enables text input for editable fields (phone, display name)
  - Email field is read-only (cannot change via app)
  - Save button validates input
  - Phone number format is validated
  - Changes are saved to profile
  - Success message: "Profile updated successfully"
  - Changes persist across sessions
- **Test Data**: New phone number: "+1-555-987-6543"
- **Execution Time**: < 5 seconds

#### TC-SET-003: Enable Private Time Timer
- **Priority**: P2 (Medium)
- **Preconditions**: User is in Settings, Private Time Timer is off
- **Test Steps**:
  1. Navigate to Settings
  2. Find "Private Time Timer" toggle
  3. Tap toggle to enable
  4. Observe status change
  5. Close and reopen app
  6. Verify setting persists
- **Expected Results**:
  - Private Time Timer toggle switches to "On"
  - Status message: "Private time tracking enabled"
  - Setting is saved to user preferences
  - Setting persists after app restart
  - When enabled, can create "Private" tasks that don't count toward logged hours
  - Visual indicator shows feature is active
- **Test Data**: Private time timer toggle
- **Execution Time**: < 3 seconds

#### TC-SET-004: Disable Private Time Timer
- **Priority**: P2 (Medium)
- **Preconditions**: User has Private Time Timer enabled
- **Test Steps**:
  1. Navigate to Settings
  2. Find "Private Time Timer" toggle showing "On"
  3. Tap toggle to disable
  4. Confirm disabling if prompted
  5. Observe status change
- **Expected Results**:
  - Toggle switches to "Off"
  - Status message: "Private time tracking disabled"
  - Setting is saved
  - Existing private tasks remain but are no longer created
  - Setting takes effect immediately
- **Test Data**: Private time timer toggle
- **Execution Time**: < 3 seconds

#### TC-SET-005: Logout from Application
- **Priority**: P0 (Critical)
- **Preconditions**: User is logged in and viewing any screen
- **Test Steps**:
  1. Navigate to Settings
  2. Scroll to bottom
  3. Tap "Logout" button
  4. Confirm logout if prompted
  5. Observe app state
- **Expected Results**:
  - Confirmation dialog appears: "Are you sure you want to logout?"
  - Session is terminated upon confirmation
  - User is navigated to Login screen
  - All user data is cleared from app memory
  - Session token is deleted
  - "Remember Me" session is also cleared
  - User must login again to access app
  - Message: "You have been logged out"
- **Test Data**: User session
- **Execution Time**: < 3 seconds

#### TC-SET-006: Notification Preferences
- **Priority**: P2 (Medium)
- **Preconditions**: User is in Settings
- **Test Steps**:
  1. Navigate to Settings
  2. Tap "Notifications"
  3. Observe preference options
  4. Disable "Task Notifications"
  5. Save changes
  6. Start a task
  7. Verify no notification appears
- **Expected Results**:
  - Notification preferences section shows toggles for each notification type:
    - Task notifications (start, pause, complete)
    - Timesheet notifications
    - Leave notifications
  - Can enable/disable each type independently
  - Preferences are saved
  - Settings are applied immediately
  - App respects OS-level notification settings
- **Test Data**: Notification preference toggles
- **Execution Time**: < 5 seconds

#### TC-SET-007: Timesheet Submission Frequency
- **Priority**: P2 (Medium)
- **Preconditions**: User is in Settings
- **Test Steps**:
  1. Navigate to Settings
  2. Tap "Timesheet Settings"
  3. Select submission frequency: "Weekly"
  4. Save changes
  5. Verify timesheet interface updates
- **Expected Results**:
  - Options: "Daily" or "Weekly" submission frequency
  - Timesheet view updates to show selected frequency
  - Setting is saved to user preference
  - Changes apply to next period
  - Default frequency is company-wide setting overridable per user
- **Test Data**: Submission frequency: Weekly
- **Execution Time**: < 3 seconds

---

## Requirements

### Functional Requirements

**Authentication Module**
- **FR-LOGIN-1**: System MUST authenticate users using email and password credentials
- **FR-LOGIN-2**: System MUST support "Remember Me" option to maintain session for up to 30 days
- **FR-LOGIN-3**: System MUST provide "Forgot Password" functionality with email-based password reset
- **FR-LOGIN-4**: System MUST support Microsoft account (Azure AD) authentication via OAuth 2.0
- **FR-LOGIN-5**: System MUST support Google account authentication via OAuth 2.0
- **FR-LOGIN-6**: System MUST implement automatic session timeout after 30 minutes of inactivity
- **FR-LOGIN-7**: System MUST encrypt and securely store authentication tokens

**Dashboard / Home Module**
- **FR-HOME-1**: System MUST display clock-in time in HH:MM AM/PM format
- **FR-HOME-2**: System MUST display active working hours with real-time updates (HH:MM:SS format)
- **FR-HOME-3**: System MUST display total work time for the current day (sum of completed task durations)
- **FR-HOME-4**: System MUST display the currently active task (In Progress status) with elapsed and estimated time
- **FR-HOME-5**: System MUST provide quick action buttons (Start, Pause, Complete) for current task directly from dashboard
- **FR-HOME-6**: System MUST update dashboard metrics in real-time as tasks are modified
- **FR-HOME-7**: System MUST handle dashboard display when no active task exists (show "No Active Task" message)

**Task Management Module**
- **FR-TASK-1**: System MUST allow employees to create new tasks with name, description, and estimated hours
- **FR-TASK-2**: System MUST initialize new tasks with status "New"
- **FR-TASK-3**: System MUST start task timer when employee initiates task start action
- **FR-TASK-4**: System MUST change task status to "In Progress" when task timer starts
- **FR-TASK-5**: System MUST automatically change task status to "Overdue" when elapsed time exceeds estimated time
- **FR-TASK-6**: System MUST provide pause functionality to temporarily stop task timer without completing task
- **FR-TASK-7**: System MUST mark task as "Completed" and record final elapsed time when employee completes task
- **FR-TASK-8**: System MUST provide status-based filtering (New, In Progress, Overdue, Completed)
- **FR-TASK-9**: System MUST prevent task deletion if any hours have been logged (to maintain audit trail)
- **FR-TASK-10**: System MUST calculate and persist elapsed time with precision to seconds

**Project Management Module**
- **FR-PROJ-1**: System MUST display list of projects assigned to employee
- **FR-PROJ-2**: System MUST display project completion percentage (0-100%) based on completed tasks
- **FR-PROJ-3**: System MUST display project start and end dates in MM/DD/YYYY format
- **FR-PROJ-4**: System MUST display team members assigned to each project
- **FR-PROJ-5**: System MUST display total number of tasks in project and breakdown by status
- **FR-PROJ-6**: System MUST link project tasks to project in project view for easy access
- **FR-PROJ-7**: System MUST support project filtering by status (Active, Completed, On Hold)

**Timesheet Module**
- **FR-TIME-1**: System MUST display calendar view for current week with daily totals
- **FR-TIME-2**: System MUST allow manual logging of hours for tasks (in case task timer was not used)
- **FR-TIME-3**: System MUST support daily (or weekly, configurable) timesheet submission
- **FR-TIME-4**: System MUST maintain archive of all submitted timesheets with submission timestamps
- **FR-TIME-5**: System MUST calculate and display total logged hours daily and weekly
- **FR-TIME-6**: System MUST calculate and display submitted hours (only for approved/submitted entries)
- **FR-TIME-7**: System MUST lock submitted timesheets from further editing
- **FR-TIME-8**: System MUST provide visual indication for submitted vs. unsubmitted timesheet entries

**Leave Management Module**
- **FR-LEAVE-1**: System MUST allow employees to apply for leave with required details
- **FR-LEAVE-2**: System MUST provide leave type selection (Vacation, Sick, Personal, Unpaid, etc.)
- **FR-LEAVE-3**: System MUST support date range selection (start and end dates)
- **FR-LEAVE-4**: System MUST display leave request status (Pending, Approved, Rejected, Cancelled)
- **FR-LEAVE-5**: System MUST display current leave balance for each leave type
- **FR-LEAVE-6**: System MUST display annual leave entitlement for current year
- **FR-LEAVE-7**: System MUST display comprehensive list of previously booked/active leaves
- **FR-LEAVE-8**: System MUST prevent negative leave balance (validate available balance before approval)
- **FR-LEAVE-9**: System MUST support leave cancellation before approval or after approval (with manager permission)
- **FR-LEAVE-10**: System MUST block task creation/completion during approved leave dates (optional notification)

**Notifications Module**
- **FR-NOT-1**: System MUST send notification when task is started (In Progress status)
- **FR-NOT-2**: System MUST send notification when task is paused or stopped
- **FR-NOT-3**: System MUST send notification when task is marked completed with duration info
- **FR-NOT-4**: System MUST send notification upon successful timesheet submission with total hours
- **FR-NOT-5**: System MUST send notification when leave request is submitted
- **FR-NOT-6**: System MUST send notification when leave request status changes (Approved/Rejected)
- **FR-NOT-7**: System MUST support both in-app and push notifications (platform-native)
- **FR-NOT-8**: System MUST respect notification preferences configured in Settings
- **FR-NOT-9**: System MUST include actionable notification content (task name, leave dates, etc.)

**Settings / Profile Module**
- **FR-SET-1**: System MUST display employee's current account information (name, email, phone, department)
- **FR-SET-2**: System MUST allow editing of account details with validation (email format, phone format)
- **FR-SET-3**: System MUST support toggling of Private Time Timer feature (for breaks/private time)
- **FR-SET-4**: System MUST provide secure logout functionality that clears all session data
- **FR-SET-5**: System MUST allow configuration of notification preferences (push on/off, email on/off, frequency)
- **FR-SET-6**: System MUST allow configuration of timesheet submission frequency (daily/weekly)
- **FR-SET-7**: System MUST display version information and app metadata

### Key Entities

**User (Employee)**
- userId: unique identifier
- email: company email address
- firstName, lastName: employee name
- department: organizational unit
- role: "Employee" (in Phase 1)
- phone: contact number
- avatar: profile picture (optional)

**Task**
- taskId: unique identifier
- userId: task owner (foreign key to User)
- title: task name
- description: task details
- status: enum [New, In Progress, Overdue, Completed]
- estimatedHours: expected duration
- elapsedTimeSeconds: actual time spent (in seconds for precision)
- createdAt: task creation timestamp
- startedAt: when task timer started
- completedAt: when task was marked complete
- projectId: foreign key to Project (optional)

**Project**
- projectId: unique identifier
- name: project title
- description: project details
- startDate: project inception date
- endDate: project completion target date
- teamMembers: list of assigned users
- tasks: associated tasks (calculated from Task.projectId)
- status: enum [Active, On Hold, Completed]

**Timesheet**
- timesheetId: unique identifier
- userId: employee reference
- weekStartDate: Monday of week (ISO 8601)
- entries: array of daily entries (date → totalHours)
- status: enum [Draft, Submitted, Approved]
- submittedAt: submission timestamp
- approvedAt: approval timestamp (null if not approved)
- approvedBy: manager userId (if approved)

**Leave Request**
- leaveRequestId: unique identifier
- userId: employee reference
- leaveType: enum [Vacation, Sick, Personal, Unpaid, Observed Holiday]
- startDate: first day of leave
- endDate: last day of leave (inclusive)
- numberOfDays: calculated difference
- status: enum [Pending, Approved, Rejected, Cancelled]
- requestedAt: submission timestamp
- approvedAt: approval timestamp (null if pending/rejected)
- approvedBy: manager userId
- reason: optional employee notes
- balance: remaining balance for that leave type

**Notification**
- notificationId: unique identifier
- userId: recipient
- type: enum [TaskStarted, TaskPaused, TaskCompleted, TimesheetSubmitted, LeaveRequested, LeaveApproved, LeaveRejected]
- title: notification heading
- message: notification body
- relatedEntityId: task/timesheet/leave foreign key
- isRead: boolean flag
- createdAt: timestamp

---

## Non-Functional Requirements

### Performance Requirements
- **NFR-PERF-1**: App must load initial screen in < 2 seconds on 4G connection
- **NFR-PERF-2**: Dashboard must update real-time metrics with < 500ms recalculation delay
- **NFR-PERF-3**: Task list response time must be < 1 second even with 100+ tasks
- **NFR-PERF-4**: All API responses must complete within 5 seconds (timeout) or display user-friendly error

### Scalability Requirements
- **NFR-SCALE-1**: System must support 10,000+ concurrent mobile users without degradation
- **NFR-SCALE-2**: Database must handle 1,000+ task creation requests per minute during peak hours
- **NFR-SCALE-3**: Notification service must deliver messages to users within 30 seconds
- **NFR-SCALE-4**: Offline-first design must support local storage of up to 500 task entries per device

### Security Requirements
- **NFR-SEC-1**: All authentication tokens must use industry-standard encryption (minimum TLS 1.3)
- **NFR-SEC-2**: Password reset and OAuth flows must comply with OWASP guidelines
- **NFR-SEC-3**: All API communication must use HTTPS only (no HTTP)
- **NFR-SEC-4**: Sensitive data (passwords, tokens) must never be logged or displayed in debug output
- **NFR-SEC-5**: App must implement certificate pinning for backend API communication
- **NFR-SEC-6**: User session data must be cleared from device memory upon logout
- **NFR-SEC-7**: Biometric authentication (fingerprint/face ID) must be supported as optional MFA
- **NFR-SEC-8**: All data must be encrypted at rest on device using native platform encryption (iOS Keychain, Android Keystore)

### Reliability & Availability
- **NFR-RELY-1**: System must maintain 99.5% uptime (excluding planned maintenance)
- **NFR-RELY-2**: All critical operations (task creation, timesheet submission) must have 99.9% success rate
- **NFR-RELY-3**: Offline mode must persist work and sync when connectivity is restored
- **NFR-RELY-4**: Failed syncs must be automatically retried with exponential backoff (max 5 attempts)
- **NFR-RELY-5**: Error messages must be user-friendly and actionable (not technical stack traces)

### Usability & Accessibility
- **NFR-USE-1**: App must comply with WCAG 2.1 AA accessibility standards
- **NFR-USE-1a**: All buttons and interactive elements must have minimum 44x44 point touch targets
- **NFR-USE-1b**: Color contrast ratio must be 4.5:1 for text on backgrounds
- **NFR-USE-1c**: App must support screen readers (VoiceOver on iOS, TalkBack on Android)
- **NFR-USE-2**: App must support at least 20+ languages (configurable, starting with English, Spanish, French, German, Chinese)
- **NFR-USE-3**: UI must be responsive and functional on screen sizes from 4.5" (small phones) to 6.5" (large phones)
- **NFR-USE-4**: On-screen help and tooltips must be available for all complex features
- **NFR-USE-5**: User must be able to complete primary task (start/complete task) in < 5 taps

### Platform Requirements
- **NFR-PLAT-1**: App must be available on iOS (minimum version 14.0) and Android (minimum version 10)
- **NFR-PLAT-2**: App must work on smartphones (phones listed by HR/supported device list)
- **NFR-PLAT-3**: App must integrate with native platform features (notifications, calendar, contacts)
- **NFR-PLAT-4**: App must include native platform UI patterns (iOS Human Interface Guidelines, Material Design)

### Data Requirements
- **NFR-DATA-1**: All timestamps must be recorded in UTC and displayed in user's local timezone
- **NFR-DATA-2**: Data must be synced to backend at least every 5 minutes (or on explicit Save action)
- **NFR-DATA-3**: Audit trail must be maintained for all task and timesheet modifications (immutable log)
- **NFR-DATA-4**: Data retention must comply with company policy (minimum 3 years for timesheet data)
- **NFR-DATA-5**: Employee can request data export in standard format (CSV, JSON) per GDPR

### Maintenance & Support Requirements
- **NFR-MAINT-1**: App must support graceful feature degradation if backend APIs are unavailable
- **NFR-MAINT-2**: Critical bugs must be fixable in hotfix release within 24 hours
- **NFR-MAINT-3**: App must log all errors to monitoring service (Sentry, Datadog, etc.) for debugging
- **NFR-MAINT-4**: App must include in-app feedback mechanism for users to report issues
- **NFR-MAINT-5**: Desktop and mobile versions must maintain feature parity within 1 major release

---

## Assumptions & Dependencies

### Assumptions
1. **Backend System**: Existing InTimePro backend API will be available and extended to support mobile operations. Desktop version will remain the source of truth for reports.
2. **Network Connectivity**: Users will have intermittent connectivity (3G/4G/WiFi). App will implement offline-first design.
3. **Authentication**: Company will provide Azure AD (Microsoft) and Google OAuth endpoints. Single Sing-On (SSO) is preferred authentication method.
4. **Data Sync**: Mobile device will sync changes to backend when connection is available. Conflict resolution will happen server-side.
5. **Notification Service**: Company will provide push notification service (Firebase Cloud Messaging for Android, Apple Push Notification service for iOS).
6. **Time Zones**: All employees operate within a single time zone (or time zone is configurable in backend). Desktop app already handles this.
7. **Leave Policies**: Leave types and balances are managed in backend HR system. Mobile app reads this data (read-only for Phase 1).
8. **HR Integration**: Company has integrated timesheet and leave systems with payroll. Mobile must maintain compatibility.

### Dependencies
1. **Backend API**: Depends on InTimePro backend team to provide/extend REST APIs for mobile consumption
2. **Authentication Infrastructure**: Requires Azure AD and Google OAuth configured by IT
3. **Push Notification Service**: Requires Firebase or Apple service configuration
4. **Analytics Service**: Recommended dependency for tracking feature usage and errors (optional)
5. **Desktop App Compatibility**: Mobile events/data must be compatible with existing desktop application data model

---

## Success Criteria

The InTimePro mobile application will be considered successful when:

1. **Adoption Criteria**
   - ≥ 50% of company employees download app within first month
   - ≥ 70% daily active users (DAU) of installed base during first quarter
   - ≥ 80% adoption of timesheet submission through mobile by end of Q2

2. **Performance Criteria**
   - App load time ≤ 2 seconds (4G connection average)
   - Task creation and completion < 500ms response time
   - ≥ 99% of API requests complete successfully
   - Zero data loss in offline-mode sync operations

3. **Quality Criteria**
   - ≤ 1 critical bug per 10,000 active users per month
   - ≥ 90% of automated test coverage for business logic
   - ≥ 4.0 star rating on app stores
   - < 5% app crash rate

4. **Business Criteria**
   - Time to complete daily task management reduced by ≥ 30% vs. desktop + pen-and-paper
   - Timesheet submission rate increases by ≥ 40% with mobile availability
   - Leave request processing time reduced by ≥ 25%
   - Employee productivity score improves by measurable metric (TBD by HR)

5. **User Satisfaction Criteria**
   - ≥ 80% user satisfaction score on usability survey
   - ≥ 75% of users report "saves time" in post-launch survey
   - < 2% user churn rate per month
   - ≥ 90% task-to-completion without switching to desktop app

---

## Implementation Constraints

1. **Phase 1 Launch Target**: Must be production-ready within 4-6 months (adjustable based on resource availability)
2. **Platform Priority**: iOS first, then Android (staggered release)
3. **Minimum OS Versions**: iOS 14.0+, Android 10.0+ (cannot support legacy versions)
4. **Technology Stack**: Native development preferred (Swift for iOS, Kotlin for Android) OR cross-platform (Flutter, React Native) if performance and UX requirements can be met
5. **Feature Scope**: Core features (Auth, Dashboard, Tasks, Timesheet) must be included in Phase 1; advanced features (Analytics, Offline Sync, Project Details) can be Phase 2

---

## Out of Scope (Phase 1)

The following features are intentionally excluded from Phase 1 and will be evaluated for Phase 2:

- **Advanced Analytics**: No complex reports or data visualization beyond existing timesheet summary
- **Project Assignment**: Employees cannot create or assign projects from mobile (read-only)
- **Team Communication**: No chat, messaging, or team collaboration features within app
- **Offline-First Sync**: Phase 1 will be online-only; offline support in Phase 2
- **Manual Clock In/Out**: Only task-based time tracking; no manual punch clock
- **Approval Workflows**: Leave approval must be done in desktop app or email; mobile is view-only for statuses
- **Customization**: No theming, custom workflows, or per-company configurations
- **Multi-Language Support**: English only in Phase 1; internationalization framework will be built for expansion

---

**Status**: Ready for Specification Quality Review and Planning Phase

