# Tasks: InTimePro Mobile Application

**Input**: Design documents from /specs/002-intimpro-mobile/
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Included because spec and quickstart define explicit testing expectations and independent verification criteria.

## Format: [ID] [P?] [Story] Description

- [P]: Task can run in parallel (different files, no dependency on unfinished tasks)
- [Story]: User story label (US1..US8)
- Requirement traceability is embedded in each task using FR/VE/MC/NFR/SC IDs

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Initialize Flutter app workspace and API integration baseline.

- [X] T001 Create Flutter application scaffold and module folders in InTimeProMobile/pubspec.yaml and InTimeProMobile/lib/ for feature-first clean architecture (NFR-004)
- [X] T002 Add core Flutter dependencies and code generation config in InTimeProMobile/pubspec.yaml for BLoC, Dio, Drift, notifications, and OAuth providers (NFR-001, NFR-004)
- [X] T003 [P] Add environment templates and runtime config loading in InTimeProMobile/.env.example and InTimeProMobile/lib/core/config/env_config.dart for API base URL and provider keys (FR-LOGIN-4, FR-LOGIN-5, MC-008)
- [X] T004 [P] Configure mobile linting/analysis/testing defaults in InTimeProMobile/analysis_options.yaml and InTimeProMobile/test/ to enforce maintainability standards (NFR-004)
- [X] T005 Define API versioning and route group constants in InTimeProAPI/Program.cs and InTimeProMobile/lib/core/network/api_paths.dart aligned to /api/v1 contracts (NFR-004)

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared security, compliance, observability, storage, and cross-module primitives.

**CRITICAL**: All user story work depends on this phase.

- [X] T006 Create mobile secure token storage and session abstractions in InTimeProMobile/lib/core/storage/secure_token_store.dart and InTimeProMobile/lib/core/auth/session_manager.dart (FR-LOGIN-2, MC-008)
- [X] T007 [P] Implement API client with TLS enforcement, retry policy, request timeout, and bounded pagination defaults in InTimeProMobile/lib/core/network/api_client.dart (VE-004, MC-007, MC-008, NFR-001)
- [X] T008 [P] Implement correlation ID interceptor for all mutating requests in InTimeProMobile/lib/core/network/correlation_interceptor.dart and InTimeProAPI/Middleware/CorrelationIdMiddleware.cs (VE-003, MC-004)
- [X] T009 [P] Define shared failure model and user-safe error mapper in InTimeProMobile/lib/core/error/app_failure.dart and InTimeProMobile/lib/core/error/error_presenter.dart (VE-002, VE-003)
- [X] T010 Implement offline queue schema and repository for pending mutations in InTimeProMobile/lib/core/storage/drift/app_database.dart and InTimeProMobile/lib/core/sync/sync_operation_repository.dart (VE-004, NFR-002)
- [X] T011 [P] Add API-side RBAC policy definitions for Admin, Employee, Auditor in InTimeProAPI/Program.cs and InTimeProAPI/Services/AuthorizationPolicyService.cs (MC-002, NFR-003)
- [X] T012 [P] Enforce role claims propagation in token creation/validation in InTimeProAPI/Services/TokenService.cs and InTimeProAPI/DTOs/AuthDTOs.cs (MC-002, MC-003)
- [X] T013 [P] Add centralized audit logging service and persistence model in InTimeProAPI/Services/AuditLogService.cs and InTimeProAPI/Models/AuditLogEntry.cs for create/update/delete operations (MC-004, VE-003)
- [X] T014 Add EF Core migration rollback playbook and executable rollback scripts in InTimeProAPI/Migrations/README.md and InTimeProAPI/scripts/db/rollback.ps1, including explicit FR/MC requirement-ID references in script headers and evidence notes (MC-006, NFR-004)
- [X] T015 [P] Configure SQL Server secure connection requirements and pooling validation in InTimeProAPI/appsettings.json and InTimeProAPI/Program.cs (MC-001, MC-008)
- [X] T016 [P] Add API health checks and structured log levels INFO/WARN/ERROR in InTimeProAPI/Program.cs and InTimeProAPI/Middleware/ExceptionMiddleware.cs (MC-009, VE-003)
- [X] T017 [P] Implement alert routing service for failed logins, overdue tasks, and failed timesheet submissions in InTimeProAPI/Services/AlertingService.cs and document acknowledgement owner, SLA, escalation timers, and escalation paths in InTimeProAPI/docs/alerting-runbook.md (MC-009, NFR-002)
- [X] T018 [P] Add backend data retention and deletion policy configuration for employee data in InTimeProAPI/appsettings.json and InTimeProAPI/Services/DataRetentionService.cs (MC-005, NFR-003)
- [X] T071 [P] Implement approved secret-store integration for API credentials and OAuth secrets in InTimeProAPI/Program.cs and .github/workflows/secrets-validation.yml (MC-001, MC-003, MC-008)
- [X] T072 [P] Add CI/CD pre-release SQL connection validation gate with pass/fail artifact output in .github/workflows/release-gates.yml and InTimeProAPI/scripts/db/validate-connection.ps1 (MC-001, MC-008, MC-009)
- [X] T073 Implement reviewed stored procedures for critical task/timesheet/leave operations in InTimeProAPI/Data/Procedures/ and wire calls via InTimeProAPI/Services/*Service.cs (MC-007, NFR-001, NFR-006)

**Checkpoint**: Foundation complete. User story phases can proceed in priority order or in parallel by team capacity.

---

## Phase 3: User Story 1 - Secure Employee Access (Priority: P1) MVP

**Goal**: Deliver secure login, social sign-in, remember-me, refresh, and password reset flows.

**Independent Test**: Employee can authenticate via email/password or SSO, persist session using remember-me, recover access, and reopen app with valid session.

- [X] T019 [P] [US1] Implement auth request/response DTOs and Retrofit service in InTimeProMobile/lib/features/auth/data/remote/auth_api.dart and InTimeProMobile/lib/features/auth/data/models/auth_models.dart (FR-LOGIN-1, FR-LOGIN-4, FR-LOGIN-5, NFR-004)
- [X] T020 [P] [US1] Implement email/google/microsoft provider adapters in InTimeProMobile/lib/features/auth/data/providers/google_auth_provider.dart and InTimeProMobile/lib/features/auth/data/providers/microsoft_auth_provider.dart (FR-LOGIN-4, FR-LOGIN-5, MC-003)
- [X] T021 [US1] Implement auth repository and token refresh handling in InTimeProMobile/lib/features/auth/data/repositories/auth_repository_impl.dart (FR-LOGIN-1, FR-LOGIN-2, VE-004)
- [X] T022 [US1] Implement AuthBloc and login/session events in InTimeProMobile/lib/features/auth/presentation/bloc/auth_bloc.dart (FR-LOGIN-1, FR-LOGIN-2, SC-001)
- [X] T023 [US1] Build login, forgot-password, and session restore screens in InTimeProMobile/lib/features/auth/presentation/pages/login_page.dart and InTimeProMobile/lib/features/auth/presentation/pages/forgot_password_page.dart (FR-LOGIN-2, FR-LOGIN-3, VE-002)
- [X] T024 [US1] Extend InTimeProAPI/Controllers/AuthController.cs and InTimeProAPI/Services/AuthService.cs to fully support provider validation, lockout behavior, and reset controls (FR-LOGIN-3, FR-LOGIN-4, FR-LOGIN-5, VE-001, MC-003)
- [X] T025 [US1] Add auth flow integration tests in InTimeProMobile/integration_test/auth_flow_test.dart and backend contract tests in InTimeProAPI.Tests/Contracts/AuthContractTests.cs (SC-001, NFR-002, NFR-004)
- [X] T075 [US1] Add login performance verification test and report for P95 <= 2s under operating profile in InTimeProMobile/test/perf/login_latency_test.dart and InTimeProAPI/docs/perf-login.md (NFR-001, SC-001)

---

## Phase 4: User Story 2 - Daily Work Dashboard (Priority: P1)

**Goal**: Show real-time work summary and support active task controls from one screen.

**Independent Test**: Dashboard shows clock-in, active/total hours, active task, and task action controls with expected state updates.

- [X] T026 [P] [US2] Implement dashboard summary API client and DTO mapping in InTimeProMobile/lib/features/dashboard/data/remote/dashboard_api.dart and InTimeProMobile/lib/features/dashboard/data/models/dashboard_summary_model.dart (FR-HOME-1, FR-HOME-2, FR-HOME-3, FR-HOME-4)
- [X] T027 [P] [US2] Implement dashboard repository with cached summary fallback and retry behavior in InTimeProMobile/lib/features/dashboard/data/repositories/dashboard_repository_impl.dart (VE-004, NFR-001, NFR-002)
- [X] T028 [US2] Implement DashboardBloc with refresh and active-task-action events in InTimeProMobile/lib/features/dashboard/presentation/bloc/dashboard_bloc.dart (FR-HOME-5, SC-002)
- [X] T029 [US2] Build dashboard screen widgets for time metrics and active task controls in InTimeProMobile/lib/features/dashboard/presentation/pages/dashboard_page.dart (FR-HOME-1, FR-HOME-5, NFR-005, NFR-007)
- [X] T030 [US2] Implement dashboard summary and active-task-state API endpoints in InTimeProAPI/Controllers/DashboardController.cs and InTimeProAPI/Services/DashboardService.cs with audit logs (FR-HOME-5, MC-004, MC-007)
- [X] T031 [US2] Add dashboard integration and performance checks in InTimeProMobile/integration_test/dashboard_summary_test.dart and InTimeProMobile/test/perf/dashboard_latency_test.dart (NFR-001, SC-002)

---

## Phase 5: User Story 3 - Task Lifecycle Tracking (Priority: P1)

**Goal**: Create and manage tasks through New, In Progress, Overdue, and Completed states with timer tracking.

**Independent Test**: Employee can create task, start/pause/complete timer transitions, auto-overdue works, and status filters return correct lists.

- [ ] T032 [P] [US3] Implement task API client and model serializers in InTimeProMobile/lib/features/tasks/data/remote/tasks_api.dart and InTimeProMobile/lib/features/tasks/data/models/task_model.dart (FR-TASK-1, FR-TASK-8)
- [ ] T033 [P] [US3] Implement local task cache and sync adapters in InTimeProMobile/lib/features/tasks/data/local/task_dao.dart and InTimeProMobile/lib/features/tasks/data/sync/task_sync_adapter.dart (VE-004, NFR-002)
- [ ] T034 [US3] Implement task repository and use cases for create/start/pause/complete transitions in InTimeProMobile/lib/features/tasks/domain/usecases/ and InTimeProMobile/lib/features/tasks/data/repositories/task_repository_impl.dart (FR-TASK-3, FR-TASK-4, FR-TASK-6, FR-TASK-7)
- [ ] T035 [US3] Implement TaskBloc and status-filter UI state in InTimeProMobile/lib/features/tasks/presentation/bloc/task_bloc.dart (FR-TASK-8, SC-002)
- [ ] T036 [US3] Build task list and task detail screens with transition guards in InTimeProMobile/lib/features/tasks/presentation/pages/tasks_page.dart and InTimeProMobile/lib/features/tasks/presentation/pages/task_detail_page.dart (VE-001, VE-002, NFR-005, NFR-007)
- [ ] T037 [US3] Implement task list/create/state endpoints and atomic transition logic in InTimeProAPI/Controllers/TasksController.cs and InTimeProAPI/Services/TaskService.cs (FR-TASK-2, FR-TASK-5, MC-004, MC-007)
- [ ] T038 [US3] Add unit/integration tests for state transitions and overdue derivation in InTimeProMobile/test/features/tasks/task_bloc_test.dart and InTimeProAPI.Tests/Integration/TaskLifecycleTests.cs (FR-TASK-5, VE-004, NFR-002)

---

## Phase 6: User Story 5 - Timesheet Logging and Submission (Priority: P1)

**Goal**: Enable daily calendar-based logging, submission, and review of submitted totals.

**Independent Test**: Employee can log daily task hours, submit once validation passes, and review submitted totals history.

- [ ] T039 [P] [US5] Implement timesheet API client and DTO mapping in InTimeProMobile/lib/features/timesheets/data/remote/timesheets_api.dart and InTimeProMobile/lib/features/timesheets/data/models/timesheet_models.dart (FR-TIME-1, FR-TIME-4, FR-TIME-5, FR-TIME-6)
- [ ] T040 [P] [US5] Implement local timesheet cache and queued submit mutation handling in InTimeProMobile/lib/features/timesheets/data/local/timesheet_dao.dart and InTimeProMobile/lib/features/timesheets/data/sync/timesheet_sync_adapter.dart (VE-004, NFR-002)
- [ ] T041 [US5] Implement timesheet domain use cases and validation for non-zero daily entries in InTimeProMobile/lib/features/timesheets/domain/usecases/submit_timesheet.dart (FR-TIME-2, FR-TIME-3, VE-001)
- [ ] T042 [US5] Build calendar logging and submission UI in InTimeProMobile/lib/features/timesheets/presentation/pages/timesheet_calendar_page.dart and InTimeProMobile/lib/features/timesheets/presentation/bloc/timesheet_bloc.dart (FR-TIME-1, FR-TIME-3, NFR-005, NFR-007)
- [ ] T043 [US5] Implement timesheet calendar/update/submit endpoints with ACID transaction and audit hooks in InTimeProAPI/Controllers/TimesheetsController.cs and InTimeProAPI/Services/TimesheetService.cs (MC-004, FR-TIME-3)
- [ ] T044 [US5] Wire failed submission alert events and error logging in InTimeProAPI/Services/TimesheetService.cs and InTimeProAPI/Services/AlertingService.cs (MC-009, VE-003)
- [ ] T045 [US5] Add timesheet integration and reliability tests in InTimeProMobile/integration_test/timesheet_submission_test.dart and InTimeProAPI.Tests/Integration/TimesheetSubmissionTests.cs (SC-003, NFR-002)

---

## Phase 7: User Story 4 - Project Visibility (Priority: P2)

**Goal**: Display assigned projects and detailed progress/team/task context.

**Independent Test**: Employee can open project list and detail view with progress, dates, team members, and task count.

- [ ] T046 [P] [US4] Implement projects API client and models in InTimeProMobile/lib/features/projects/data/remote/projects_api.dart and InTimeProMobile/lib/features/projects/data/models/project_model.dart (FR-PROJ-1, FR-PROJ-2, FR-PROJ-3, FR-PROJ-4, FR-PROJ-5)
- [ ] T047 [P] [US4] Implement project repository with paged retrieval and cache strategy in InTimeProMobile/lib/features/projects/data/repositories/project_repository_impl.dart (MC-007, NFR-001)
- [ ] T048 [US4] Build projects list/detail UI and bloc flows in InTimeProMobile/lib/features/projects/presentation/pages/projects_page.dart and InTimeProMobile/lib/features/projects/presentation/pages/project_detail_page.dart (FR-PROJ-1, FR-PROJ-5, NFR-005, NFR-007)
- [ ] T049 [US4] Implement assigned-project and project-detail endpoints in InTimeProAPI/Controllers/ProjectsController.cs and InTimeProAPI/Services/ProjectService.cs with index-aware queries (FR-PROJ-1, MC-007)
- [ ] T050 [US4] Add project visibility integration tests in InTimeProMobile/integration_test/projects_visibility_test.dart and InTimeProAPI.Tests/Contracts/ProjectsContractTests.cs (NFR-004, SC-002)

---

## Phase 8: User Story 6 - Leave Planning and Tracking (Priority: P2)

**Goal**: Let employees apply for leave and track balances, entitlements, and status.

**Independent Test**: Employee can submit leave request with valid date/type, track status, and view balance/entitlement/booked values.

- [ ] T051 [P] [US6] Implement leaves API client and models in InTimeProMobile/lib/features/leaves/data/remote/leaves_api.dart and InTimeProMobile/lib/features/leaves/data/models/leave_models.dart (FR-LEAVE-1, FR-LEAVE-4, FR-LEAVE-5, FR-LEAVE-6, FR-LEAVE-7)
- [ ] T052 [P] [US6] Implement leave date-range/overlap validation and repository logic in InTimeProMobile/lib/features/leaves/domain/validators/leave_validator.dart and InTimeProMobile/lib/features/leaves/data/repositories/leave_repository_impl.dart (FR-LEAVE-2, FR-LEAVE-3, VE-001)
- [ ] T053 [US6] Build leave summary and apply workflow UI in InTimeProMobile/lib/features/leaves/presentation/pages/leave_summary_page.dart and InTimeProMobile/lib/features/leaves/presentation/pages/leave_apply_page.dart (FR-LEAVE-1, FR-LEAVE-7, NFR-005, NFR-007)
- [ ] T054 [US6] Implement leave summary/create/list/cancel endpoints with audit and encryption safeguards in InTimeProAPI/Controllers/LeavesController.cs and InTimeProAPI/Services/LeaveService.cs (MC-004, MC-008, FR-LEAVE-1)
- [ ] T055 [US6] Add leave workflow tests including overlap and insufficient balance cases in InTimeProMobile/integration_test/leave_apply_test.dart and InTimeProAPI.Tests/Integration/LeaveWorkflowTests.cs (VE-001, NFR-002, SC-004)

---

## Phase 9: User Story 7 - Event Notifications (Priority: P2)

**Goal**: Deliver push and in-app notifications for task, timesheet, and leave events.

**Independent Test**: Triggered lifecycle events appear in notification feed and read-state updates are persisted.

- [ ] T056 [P] [US7] Implement notification API client, feed model, and read-state update calls in InTimeProMobile/lib/features/notifications/data/remote/notifications_api.dart and InTimeProMobile/lib/features/notifications/data/models/notification_model.dart (FR-NOT-1, FR-NOT-6)
- [ ] T057 [P] [US7] Implement FCM registration and local notification bridge in InTimeProMobile/lib/core/notifications/push_notification_service.dart and InTimeProMobile/lib/core/notifications/local_notification_service.dart (FR-NOT-1, FR-NOT-4, MC-009)
- [ ] T058 [US7] Build notifications inbox and unread filter UI in InTimeProMobile/lib/features/notifications/presentation/pages/notifications_page.dart and InTimeProMobile/lib/features/notifications/presentation/bloc/notifications_bloc.dart (FR-NOT-2, FR-NOT-3, NFR-005, NFR-007)
- [ ] T059 [US7] Implement notification token registration/feed/read endpoints and duplicate suppression in InTimeProAPI/Controllers/NotificationsController.cs and InTimeProAPI/Services/NotificationService.cs (FR-NOT-5, FR-NOT-6, MC-009)
- [ ] T060 [US7] Add notification event coverage tests across task/timesheet/leave triggers in InTimeProMobile/integration_test/notification_events_test.dart and InTimeProAPI.Tests/Integration/NotificationDispatchTests.cs (NFR-002, NFR-004)

---

## Phase 10: User Story 8 - Settings and Profile Management (Priority: P3)

**Goal**: Provide profile view/edit, private timer preference, and logout controls.

**Independent Test**: Employee can view and update profile, toggle private timer, and securely logout.

- [ ] T061 [P] [US8] Implement settings/profile API client and models in InTimeProMobile/lib/features/settings/data/remote/settings_api.dart and InTimeProMobile/lib/features/settings/data/models/profile_model.dart (FR-SET-1, FR-SET-2, FR-SET-3)
- [ ] T062 [P] [US8] Implement profile validation and settings repository in InTimeProMobile/lib/features/settings/domain/validators/profile_validator.dart and InTimeProMobile/lib/features/settings/data/repositories/settings_repository_impl.dart (VE-001, VE-002)
- [ ] T063 [US8] Build profile/settings screen and logout action flow in InTimeProMobile/lib/features/settings/presentation/pages/settings_page.dart and InTimeProMobile/lib/features/settings/presentation/bloc/settings_bloc.dart (FR-SET-1, FR-SET-4, NFR-005, NFR-007)
- [ ] T064 [US8] Implement profile/preferences endpoints and audit logging in InTimeProAPI/Controllers/SettingsController.cs and InTimeProAPI/Services/SettingsService.cs (FR-SET-2, FR-SET-3, MC-004)
- [ ] T065 [US8] Add settings integration tests for update and logout behavior in InTimeProMobile/integration_test/settings_profile_test.dart and InTimeProAPI.Tests/Contracts/SettingsContractTests.cs (NFR-004, NFR-005)

---

## Phase 11: Polish and Cross-Cutting Concerns

**Purpose**: Final hardening, evidence collection, and release readiness.

- [ ] T066 [P] Build end-to-end requirement traceability matrix mapping FR/VE/MC/NFR/SC to tasks and verification artifacts in specs/002-intimpro-mobile/traceability-matrix.md (NFR-004)
- [ ] T067 [P] Add release readiness checklist for compliance gates, rollback drills, and observability verification in specs/002-intimpro-mobile/checklists/release-readiness.md (MC-006, MC-009, NFR-003)
- [ ] T068 Perform performance and scalability tuning for dashboard/task/timesheet critical paths and document P95 plus 1,000-session evidence in InTimeProMobile/test/perf/perf_report.md and InTimeProAPI/docs/perf-notes.md (NFR-001, NFR-006, SC-002)
- [ ] T069 Conduct reliability runbook validation for offline replay and transient failure recovery in InTimeProMobile/docs/offline-recovery.md and InTimeProAPI/docs/reliability-runbook.md (VE-004, NFR-002)
- [ ] T070 Execute quickstart scenario walkthrough and capture final test evidence in specs/002-intimpro-mobile/implementation-evidence.md (SC-001, SC-002, SC-003, SC-004)
- [ ] T074 Execute automated rollback drill against staging baseline and capture evidence in InTimeProAPI/scripts/db/rollback-drill.ps1 and specs/002-intimpro-mobile/checklists/rollback-evidence.md (MC-006, NFR-002)
- [ ] T076 [P] Produce ASVS L2, IAM policy, and GDPR compliance verification matrix with linked test/control evidence in specs/002-intimpro-mobile/checklists/security-compliance-evidence.md (NFR-003, MC-002, MC-004, MC-005, MC-008)

---

## Dependencies and Execution Order

### Phase Dependencies

- Phase 1 must complete before Phase 2.
- Phase 2 blocks all user story phases.
- User story phases proceed in business priority order: US1, US2, US3, US5, US4, US6, US7, US8.
- Phase 11 depends on completion of all selected user stories.

### User Story Dependencies

- US1 depends only on Phase 2 and unlocks authenticated access for all other stories.
- US2 depends on US1 session/auth context and Phase 2 shared networking/error infrastructure.
- US3 depends on US1 auth and Phase 2 sync/error/correlation infrastructure.
- US5 depends on US1 auth plus US3 task references for hour logging.
- US4 depends on US1 auth and Phase 2 pagination/network constraints.
- US6 depends on US1 auth and Phase 2 validation/error/audit infrastructure.
- US7 depends on US3, US5, and US6 event producers plus Phase 2 monitoring/alerts.
- US8 depends on US1 auth/session baseline and Phase 2 RBAC/audit controls.

### Within-Story Ordering Rule

- API contracts/client models before repositories.
- Repositories/domain logic before BLoC/UI.
- API endpoint implementation before integration/performance tests.
- Story is complete only after independent test criteria are satisfied.

---

## Parallel Execution Examples

### US1

- T019 and T020 can run in parallel, then T021.
- T023 and T024 can run in parallel after T022.

### US2

- T026 and T027 can run in parallel, then T028.
- T029 and T030 can run in parallel before T031.

### US3

- T032 and T033 can run in parallel, then T034.
- T035 and T037 can run in parallel before T038.

### US5

- T039 and T040 can run in parallel, then T041.
- T042 and T043 can run in parallel before T045.

### US4

- T046 and T047 can run in parallel, then T048.
- T049 can run in parallel with T048 once contracts are stable.

### US6

- T051 and T052 can run in parallel, then T053.
- T054 can run in parallel with T053 once API contract fixtures are ready.

### US7

- T056 and T057 can run in parallel, then T058.
- T059 can run in parallel with T058 before T060.

### US8

- T061 and T062 can run in parallel, then T063.
- T064 can run in parallel with T063 before T065.

---

## Implementation Strategy

### MVP Scope

- MVP is US1 only after completing Phase 1 and Phase 2.
- Validate SC-001 plus auth-related FR/VE/MC/NFR evidence before expanding scope.

### Incremental Delivery

1. Complete Setup and Foundational phases.
2. Deliver US1 and validate independently.
3. Deliver remaining P1 stories (US2, US3, US5) with independent validation after each.
4. Deliver P2 stories (US4, US6, US7), then P3 story (US8).
5. Run Phase 11 hardening and release evidence capture.

### Suggested Team Parallelization

- Stream A: Flutter feature modules and BLoCs.
- Stream B: InTimeProAPI controllers/services and compliance controls.
- Stream C: Test automation, performance evidence, and traceability artifacts.
