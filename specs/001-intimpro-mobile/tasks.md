# Tasks: InTimePro Mobile Application

**Input**: Design documents from `/specs/001-intimpro-mobile/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are OPTIONAL - not requested in feature specification, so excluded.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Flutter mobile app**: `lib/` at repository root with clean architecture layers
- Domain: `lib/domain/`
- Data: `lib/data/`
- Presentation: `lib/presentation/`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [ ] T001 Create Flutter project structure per plan.md in lib/core/, lib/data/, lib/domain/, lib/presentation/
- [ ] T002 Configure pubspec.yaml with dependencies (dio, flutter_bloc, sqflite, firebase_auth, flutter_secure_storage) in pubspec.yaml
- [ ] T003 Set up Firebase project configuration in lib/core/config/firebase_config.dart
- [ ] T004 Implement core utilities (network client with Dio, error handling, constants) in lib/core/network/, lib/core/error/, lib/core/constants/
- [ ] T005 Set up local database schema and migrations in lib/data/datasources/local/database_helper.dart

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core domain and data layer foundations required by all user stories

- [ ] T006 Implement domain entities (Employee, Task, Project, TimeEntry, Timesheet) in lib/domain/entities/
- [ ] T007 Define repository interfaces in lib/domain/repositories/
- [ ] T008 Implement data models with JSON converters in lib/data/models/
- [ ] T009 Set up BLoC architecture foundation in lib/presentation/blocs/base/

## Phase 3: User Story 1 - Employee Authentication and Secure Access

**Story Goal**: Enable secure access to personal work data and prevent unauthorized access

**Independent Test Criteria**:
- User can successfully login with email/password
- User can successfully login with Microsoft account
- User can successfully login with Google account
- User can change password after reset
- All user preferences and data are protected behind authentication
- Session timeout works correctly

**Implementation Tasks**:
- [ ] T010 Implement auth repository with local and remote data sources in lib/data/repositories/auth_repository_impl.dart
- [ ] T011 Create auth BLoC for login/logout state management in lib/presentation/blocs/auth/auth_bloc.dart
- [ ] T012 Build login screen UI with form validation in lib/presentation/pages/auth/login_page.dart
- [ ] T013 Implement social login (Google, Microsoft) integration in lib/data/services/social_auth_service.dart
- [ ] T014 Add password reset functionality in lib/presentation/pages/auth/forgot_password_page.dart
- [ ] T015 Implement session management and auto-logout in lib/core/utils/session_manager.dart
- [ ] T016 Add secure token storage and refresh logic in lib/data/datasources/local/secure_storage.dart

## Phase 4: User Story 2 - Employee Dashboard with Time and Task Overview

**Story Goal**: Provide quick overview of work progress and current activities

**Independent Test Criteria**:
- User can see clock-in time
- User can see active working hours
- User can see total work time
- User can see current active task
- User can start a task from dashboard
- User can pause a task from dashboard
- User can complete a task from dashboard
- All information updates in real-time

**Implementation Tasks**:
- [ ] T017 Implement dashboard repository for aggregating data in lib/data/repositories/dashboard_repository_impl.dart
- [ ] T018 Create dashboard BLoC for real-time updates in lib/presentation/blocs/dashboard/dashboard_bloc.dart
- [ ] T019 Build dashboard screen UI with metrics display in lib/presentation/pages/dashboard/dashboard_page.dart
- [ ] T020 Implement active task timer display in lib/presentation/widgets/dashboard/active_task_timer.dart
- [ ] T021 Add clock-in time and total work time calculations in lib/domain/usecases/calculate_work_time.dart

## Phase 5: User Story 3 - Task Lifecycle Management

**Story Goal**: Enable accurate task duration measurement and task status visibility

**Independent Test Criteria**:
- User can create a new task
- Task defaults to "New" status
- User can start task timer
- User can view tasks by status filter
- Overdue detection works correctly
- User can complete tasks
- Task time tracking is accurate

**Implementation Tasks**:
- [ ] T022 Implement task repository with CRUD operations in lib/data/repositories/task_repository_impl.dart
- [ ] T023 Create task BLoC for task state management in lib/presentation/blocs/task/task_bloc.dart
- [ ] T024 Build task list screen with status filtering in lib/presentation/pages/task/task_list_page.dart
- [ ] T025 Implement task creation and editing forms in lib/presentation/pages/task/task_form_page.dart
- [ ] T026 Add task timer start/pause/complete functionality in lib/domain/usecases/task_timer_usecase.dart
- [ ] T027 Implement overdue detection and status updates in lib/domain/usecases/overdue_detection_usecase.dart

## Phase 6: User Story 4 - Project Visibility and Progress Tracking

**Story Goal**: Maintain visibility into project status and team collaboration

**Independent Test Criteria**:
- User can see list of assigned projects
- User can see project progress percentage
- User can see project dates
- User can see team members
- User can see task count per project

**Implementation Tasks**:
- [ ] T028 Implement project repository in lib/data/repositories/project_repository_impl.dart
- [ ] T029 Create project BLoC in lib/presentation/blocs/project/project_bloc.dart
- [ ] T030 Build project list screen in lib/presentation/pages/project/project_list_page.dart
- [ ] T031 Add project progress visualization in lib/presentation/widgets/project/progress_indicator.dart

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Final enhancements and system-wide improvements

- [ ] T032 Implement offline sync mechanism in lib/data/services/sync_service.dart
- [ ] T033 Add global error handling and user feedback in lib/core/error/error_handler.dart
- [ ] T034 Polish UI/UX with consistent theming in lib/presentation/theme/app_theme.dart
- [ ] T035 Implement push notifications for task reminders in lib/data/services/notification_service.dart
- [ ] T036 Add app settings and preferences in lib/presentation/pages/settings/settings_page.dart
- [ ] T037 Performance optimization and final testing in various files

## Dependencies

**Story Completion Order**:
1. US1 (Authentication) - Foundation for all other features
2. US2, US3, US4 - Can be developed in parallel after US1 completion

**Parallel Execution Examples**:
- **US1**: T010 (repository) and T011 (BLoC) can run in parallel
- **US2**: T017 (repository) and T019 (UI) can run in parallel  
- **US3**: T022 (repository) and T024 (UI) can run in parallel
- **US4**: T028 (repository) and T030 (UI) can run in parallel

## Implementation Strategy

**MVP Scope**: User Story 1 (Authentication) - Provides core value of secure access
**Incremental Delivery**: Add dashboard, then task management, then projects
**Parallel Opportunities**: Within each story, data layer and UI can be developed simultaneously