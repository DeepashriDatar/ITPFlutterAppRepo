# Alerting Runbook

Requirement IDs: MC-009, NFR-002.

## Purpose
Define routing and response expectations for critical operational alerts emitted by InTimeProAPI.

## Alert Matrix

| Alert Type | Trigger Source | Primary Owner | Acknowledge SLA | Escalation Timer | Escalation Path |
|------------|----------------|---------------|-----------------|------------------|-----------------|
| Failed login burst | AuthService / AlertingService | Security On-Call | 15 minutes | 30 minutes | Security Lead -> Engineering Manager |
| Overdue task detected | Task workflows / AlertingService | Operations On-Call | 30 minutes | 60 minutes | Product Operations Lead -> Engineering Manager |
| Timesheet submission failed | TimesheetService / AlertingService | Operations On-Call | 15 minutes | 30 minutes | Payroll Ops Lead -> Engineering Manager |

## Operational Steps
1. Confirm alert payload includes correlation ID, actor/employee identifiers (if available), and timestamp.
2. Validate impact scope from logs at INFO/WARN/ERROR levels and current /health status.
3. For login alerts, inspect lockout spikes and suspicious IP concentration before remediation.
4. For task/timesheet alerts, verify database health and retry backlog before manual intervention.
5. Record incident timeline and link post-incident notes to release evidence artifacts.

## Escalation Rules
- If acknowledge SLA is missed, escalate immediately through the configured path.
- If repeated alerts for same employee/entity occur 3+ times in 1 hour, open an incident ticket.
- If /health reports unhealthy during alert triage, treat as Severity-1 until stabilized.

## Evidence
- Attach alert screenshots or log excerpts with correlation IDs.
- Reference rollout/release gate artifacts where applicable.
- Record closure timestamp and responder in incident tracker.
