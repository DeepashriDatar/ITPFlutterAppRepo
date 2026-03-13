# Research Findings

## HTTP Client Choice
**Decision**: Use Dio as the primary HTTP client  
**Rationale**: Dio provides advanced features like interceptors for authentication, request/response logging, and error handling, which are essential for a clean architecture with centralized API management. It also supports cancel tokens and progress tracking.  
**Alternatives considered**: http package (built-in, simpler but lacks advanced features), chopper (code generation but adds complexity)

## State Management
**Decision**: Use BLoC pattern with Cubits for state management  
**Rationale**: BLoC fits well with clean architecture by separating business logic from UI. Cubits are simpler than full BLoCs for straightforward state changes, reducing boilerplate while maintaining testability.  
**Alternatives considered**: Provider (simple but can lead to tight coupling), Riverpod (modern but steeper learning curve), GetX (convenient but less structured)

## Local Storage
**Decision**: Use Sqflite for structured data storage  
**Rationale**: Tasks, projects, and time entries have relational structure that benefits from SQL queries for filtering and aggregation. Sqflite provides robust offline data persistence with migration support.  
**Alternatives considered**: Hive (key-value, simpler but less flexible for complex queries), SharedPreferences (too basic for structured data)

## Authentication Method
**Decision**: Use Firebase Authentication with custom JWT tokens  
**Rationale**: Firebase provides secure, scalable auth with multiple providers (email/password, Google, Microsoft). Custom JWT allows backend control over permissions while leveraging Firebase's security.  
**Alternatives considered**: Pure custom JWT (more control but requires full auth server), OAuth2 only (complex setup)

## Clean Architecture Implementation
**Decision**: Three-layer architecture: Presentation (UI + BLoC), Domain (entities + use cases), Data (repositories + API/remote sources)  
**Rationale**: Clear separation of concerns with dependency inversion. Domain layer is platform-independent, making it testable and reusable.  
**Alternatives considered**: Feature-based architecture (can lead to duplication), MVVM (less strict boundaries)

## Offline Sync Patterns
**Decision**: Repository pattern with local-first approach and background sync  
**Rationale**: Local data source handles reads/writes immediately, remote sync happens in background. Conflict resolution via last-write-wins for simplicity.  
**Alternatives considered**: Remote-first (poor UX with network issues), full sync on app start (battery/performance impact)

## Secure Token Storage
**Decision**: Use flutter_secure_storage for tokens  
**Rationale**: Platform-specific secure storage (Keychain on iOS, KeyStore on Android) protects sensitive data from unauthorized access.  
**Alternatives considered**: SharedPreferences (not secure), local file storage (easily accessible)