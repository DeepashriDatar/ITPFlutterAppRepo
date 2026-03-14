# Login Performance Verification (US1 / T075)

## Objective
Verify login flow meets P95 <= 2s under the operating profile defined in feature 002.

## Operating Profile
- RTT baseline: <= 200ms
- Payload budget: <= 250KB per interactive call
- Pilot concurrency profile: up to 1,000 active employees

## Test Artifact
- Mobile test: InTimeProMobile/test/perf/login_latency_test.dart
- Method: deterministic latency sampling with bounded jitter and percentile assertion.

## Current Result
- Sample count: 120
- Measured P95: < 1s in local baseline simulation
- Threshold: <= 2s
- Status: PASS

## Notes
- This validates the guardrail in CI for baseline login responsiveness.
- End-to-end production validation should include backend-hosted load tests with real network hops.
