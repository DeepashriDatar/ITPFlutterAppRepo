# Migration Rollback Playbook

This playbook supports MC-006 rollback control for release operations.

Requirement IDs: MC-006, NFR-004.

## Forward Apply
1. Review pending migration files and SQL script output.
2. Run: dotnet ef database update
3. Verify health endpoint and smoke API calls.

## Rollback Strategy
1. Identify target migration from history:
   - dotnet ef migrations list
2. Backup database before rollback.
3. Roll back to previous migration:
   - dotnet ef database update <TargetMigration>
4. Validate schema and critical auth endpoints.

## Safety Rules
- Never run rollback without backup confirmation.
- Always execute rollback drill in staging before production.
- Capture evidence in specs/002-intimpro-mobile/checklists/rollback-evidence.md.

## Evidence Checklist
1. Record target migration and operator identity.
2. Save generated artifact: InTimeProAPI/artifacts/rollback-evidence.json.
3. Attach post-rollback `/health` output and key endpoint smoke-test result.
4. Link evidence to feature requirement IDs MC-006 and NFR-004.
