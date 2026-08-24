---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E03
title: "Credential Lifecycle and Secret Containment"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 3
  user_stories: 6
  subtasks: 18
  traceability_owned: 9
  traceability_items: 9
story_estimate_days: 16.5
subtask_estimate_hours: 116
---

<a id="id-e03"></a>
# E03 — Credential Lifecycle and Secret Containment

This is the self-contained coding-agent backlog for E03. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

## Coding Agent Rules

1. Keep every ID immutable and preserve Feature → Epic, Story → Feature, and Subtask → Story parentage.
2. Create parent items before child items and treat dependency links as blockers.
3. Preserve priorities, estimates, labels, source findings, statuses, owner roles, and deliverables.
4. Complete every acceptance criterion and subtask before closing a Story.
5. Follow cross-Epic dependency links to the sibling Markdown files in this package.
6. Keep every included traceability row covered or replace it with demonstrably equivalent coverage.
7. Apply the Delivery Guide and relevant Roadmap gates before declaring this Epic complete.

## 1. Portfolio Slice

| Epic | Priority | Features | Stories | Subtasks | Story Days | Subtask Hours | Phase | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E03 | P0 | 3 | 6 | 18 | 16.5 | 116 | M0 — Hardened Core | Not Started |

## 2. Epic Definition

**Objective:** Keep Finnhub credentials out of requests and logs, support managed secret stores and rotation, and eliminate redirect-based token disclosure.

**Business value:** Reduces the likelihood and blast radius of API-key compromise while enabling production deployment without restart-driven rotations.

**Exit criteria:**

- [ ] Credentials are supplied only by an injectable credential provider and never by an MCP argument.
- [ ] Production deployments can use managed cloud identity and secret stores; local development retains safe environment/user-secret options.
- [ ] Credential changes take effect without process restart and without mixing tenant or key-version cache/quota state.
- [ ] No Finnhub token is forwarded across redirects or to a non-approved host.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F03.01](#id-f03-01) | P0 | Credential Provider Abstraction | 3 | 9 | Not Started |
| [F03.02](#id-f03-02) | P0 | Zero-Downtime Credential and Client Rotation | 2 | 6 | Not Started |
| [F03.03](#id-f03-03) | P0 | Redirect-Safe Upstream Fetching | 1 | 1.5 | Not Started |

<a id="id-f03-01"></a>
### F03.01 — Credential Provider Abstraction

- **Parent Epic:** [E03](#id-e03)
- **Priority:** P0
- **Status:** Not Started

**Description:** Centralize credential resolution across local and managed-cloud sources with explicit source precedence.

**Expected outcome:** Application code consumes credentials through one testable boundary and production keys need not live in files or environment variables.

**Stories:**

- [US03.01.01](#id-us03-01-01) — Introduce an injectable Finnhub credential provider (P0, 3d)
- [US03.01.02](#id-us03-01-02) — Support managed cloud secret stores (P1, 5d)
- [US03.01.03](#id-us03-01-03) — Harden local .env loading (P1, 1d)

<a id="id-f03-02"></a>
### F03.02 — Zero-Downtime Credential and Client Rotation

- **Parent Epic:** [E03](#id-e03)
- **Priority:** P0
- **Status:** Not Started

**Description:** Remove singleton-captured key/options state and support versioned rotation and configured timeouts.

**Expected outcome:** Operators rotate keys and timeout policy without restarting or leaking state between key versions.

**Stories:**

- [US03.02.01](#id-us03-02-01) — Hot-rotate API keys and key-scoped clients (P0, 4d)
- [US03.02.02](#id-us03-02-02) — Honor configured outbound timeout and cancellation (P0, 2d)

<a id="id-f03-03"></a>
### F03.03 — Redirect-Safe Upstream Fetching

- **Parent Epic:** [E03](#id-e03)
- **Priority:** P0
- **Status:** Not Started

**Description:** Prevent the X-Finnhub-Token header from following redirects to static or unexpected hosts.

**Expected outcome:** Redirect behavior cannot disclose Finnhub credentials.

**Stories:**

- [US03.03.01](#id-us03-03-01) — Fetch redirected exchange data without forwarding credentials (P0, 1.5d)

## 4. User Stories and Subtasks

<a id="id-us03-01-01"></a>
### US03.01.01 — Introduce an injectable Finnhub credential provider

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F03.01](#id-f03-01) | P0 | 3 | 19 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** application developer

**User story:** As an application developer, I want one credential abstraction so that HTTP clients, quota tracking, and tenant policy resolve keys consistently without exposing them to tools.

**Acceptance criteria:**

- [ ] Finnhub clients request a credential value and opaque version/fingerprint from an interface scoped to the current tenant.
- [ ] No tool schema or MCP argument permits callers to supply or override a Finnhub API key.
- [ ] Credential source precedence and missing-key behavior are deterministic and covered by unit tests.
- [ ] ToString, diagnostics, exceptions, configuration dumps, and metrics never contain the credential value.
- [ ] Local environment variables and .NET user-secrets remain supported through provider adapters.

**Dependencies:** —

**Labels:** `security` `secrets` `architecture` `credentials`

**Source findings:**

- API keys are supported through environment variables, user-secrets, and .env but lack a standard credential-provider boundary.

**Subtasks:**

<a id="id-st03-01-01-01"></a>
- [ ] **ST03.01.01.01 — Define credential value/version provider interface**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: software architect
  - Deliverable/evidence: Credential provider contract
  - Status: Not Started
<a id="id-st03-01-01-02"></a>
- [ ] **ST03.01.01.02 — Refactor Finnhub clients to request-scoped credential resolution**
  - Type: implementation
  - Estimate: 9 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Provider-backed Finnhub clients
  - Status: Not Started
<a id="id-st03-01-01-03"></a>
- [ ] **ST03.01.01.03 — Add key-redaction and source-precedence tests**
  - Type: security-testing
  - Estimate: 6 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Credential containment tests
  - Status: Not Started

<a id="id-us03-01-02"></a>
### US03.01.02 — Support managed cloud secret stores

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F03.01](#id-f03-01) | P1 | 5 | 35 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** cloud platform engineer

**User story:** As a cloud platform engineer, I want managed-identity secret-store adapters so that production keys do not need to be copied into application files or static environment configuration.

**Acceptance criteria:**

- [ ] Documented adapters support Azure Key Vault with managed identity, AWS Secrets Manager with IAM role credentials, and GCP Secret Manager with workload identity.
- [ ] Cloud adapters are optional packages/configuration paths and do not add a mandatory cloud dependency to local or self-hosted builds.
- [ ] Secret retrieval failures map to ConfigurationError or ServiceUnavailable without revealing provider payloads.
- [ ] Least-privilege policy examples grant read access only to the configured secret/version.
- [ ] Integration tests use emulators/fakes and never require repository-stored cloud credentials.

**Dependencies:** [US03.01.01](#id-us03-01-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01)

**Labels:** `security` `azure` `aws` `gcp` `secrets`

**Source findings:**

- Cloud KMS/secret-manager integrations are a safer production alternative to long-lived environment or file-based secrets.

**Subtasks:**

<a id="id-st03-01-02-01"></a>
- [ ] **ST03.01.02.01 — Implement optional Azure, AWS, and GCP provider adapters**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: cloud engineer
  - Deliverable/evidence: Managed secret-store adapters
  - Status: Not Started
<a id="id-st03-01-02-02"></a>
- [ ] **ST03.01.02.02 — Create least-privilege deployment examples**
  - Type: documentation
  - Estimate: 7 hours
  - Suggested owner role: cloud security engineer
  - Deliverable/evidence: Cloud identity configuration guides
  - Status: Not Started
<a id="id-st03-01-02-03"></a>
- [ ] **ST03.01.02.03 — Test adapters with fakes and failure mapping**
  - Type: testing
  - Estimate: 8 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Cloud secret adapter tests
  - Status: Not Started

<a id="id-us03-01-03"></a>
### US03.01.03 — Harden local .env loading

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F03.01](#id-f03-01) | P1 | 1 | 7 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** local developer

**User story:** As a local developer, I want predictable .env behavior so that the server never walks parent directories and loads an unintended credential file.

**Acceptance criteria:**

- [ ] Legacy .env loading uses only an explicitly configured exact path and never searches parent directories.
- [ ] The server warns that .env is development-only and recommends user-secrets or environment injection.
- [ ] Missing .env files do not alter source precedence or produce secret-bearing logs.
- [ ] Repository ignore rules and startup diagnostics prevent accidental key commits without printing the value.

**Dependencies:** [US03.01.01](#id-us03-01-01)

**Labels:** `security` `dotenv` `developer-experience`

**Source findings:**

- Current .env loading does not require an exact path and can discover an unintended parent file.

**Subtasks:**

<a id="id-st03-01-03-01"></a>
- [ ] **ST03.01.03.01 — Change .env loader to configured exact path**
  - Type: implementation
  - Estimate: 2 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Exact-path dotenv loading
  - Status: Not Started
<a id="id-st03-01-03-02"></a>
- [ ] **ST03.01.03.02 — Add warnings, ignore checks, and documentation**
  - Type: documentation
  - Estimate: 3 hours
  - Suggested owner role: developer experience engineer
  - Deliverable/evidence: Safe local-secret guidance
  - Status: Not Started
<a id="id-st03-01-03-03"></a>
- [ ] **ST03.01.03.03 — Test parent-directory and missing-file behavior**
  - Type: testing
  - Estimate: 2 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Dotenv path tests
  - Status: Not Started

<a id="id-us03-02-01"></a>
### US03.02.01 — Hot-rotate API keys and key-scoped clients

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F03.02](#id-f03-02) | P0 | 4 | 25 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** service operator

**User story:** As a service operator, I want key rotation without a process restart so that compromised or expiring credentials can be replaced with no downtime.

**Acceptance criteria:**

- [ ] Typed clients and singleton services do not capture a credential value or immutable options snapshot for process lifetime.
- [ ] A credential version change causes new requests to use a fresh client/request header while in-flight requests complete on the prior version.
- [ ] An optional dual-version overlap supports validation and rollback without sending both keys on one request.
- [ ] Cache and quota state that depends on entitlement is keyed by the opaque credential version/fingerprint.
- [ ] Rotation tests prove a new key is used without restart and the previous key is absent from subsequent requests and logs.

**Dependencies:** [US03.01.01](#id-us03-01-01)

**Labels:** `security` `rotation` `http-client` `availability`

**Source findings:**

- Singleton services and typed clients capture options/credentials, making practical key rotation require restart.

**Subtasks:**

<a id="id-st03-02-01-01"></a>
- [ ] **ST03.02.01.01 — Remove singleton credential/options capture**
  - Type: refactoring
  - Estimate: 8 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Rotation-aware DI lifetimes
  - Status: Not Started
<a id="id-st03-02-01-02"></a>
- [ ] **ST03.02.01.02 — Implement versioned client refresh and overlap policy**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: platform engineer
  - Deliverable/evidence: Zero-downtime rotation mechanism
  - Status: Not Started
<a id="id-st03-02-01-03"></a>
- [ ] **ST03.02.01.03 — Add in-flight and post-rotation integration tests**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Credential rotation suite
  - Status: Not Started

<a id="id-us03-02-02"></a>
### US03.02.02 — Honor configured outbound timeout and cancellation

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F03.02](#id-f03-02) | P0 | 2 | 17 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** service operator

**User story:** As a service operator, I want runtime timeout configuration applied to all Finnhub calls so that latency policy is not silently overridden by a hard-coded value.

**Acceptance criteria:**

- [ ] The configured TimeoutSeconds value controls the outbound Finnhub client or resilience timeout rather than a hard-coded 30 seconds.
- [ ] Configuration validation rejects non-positive and unreasonably large timeout values.
- [ ] Request cancellation propagates through tools, cache factories, quota waits, retries, and HTTP calls.
- [ ] Timeout and caller cancellation map to distinct Timeout and Cancelled errors.
- [ ] Tests cover runtime option changes where supported, timeout expiration, and client cancellation.

**Dependencies:** [US03.02.01](#id-us03-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01)

**Labels:** `configuration` `timeout` `cancellation` `resilience`

**Source findings:**

- Finnhub TimeoutSeconds is configured as 10 seconds but a 30-second timeout is hard-coded and used instead.

**Subtasks:**

<a id="id-st03-02-02-01"></a>
- [ ] **ST03.02.02.01 — Wire validated timeout options into resilience pipeline**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Configured outbound timeout
  - Status: Not Started
<a id="id-st03-02-02-02"></a>
- [ ] **ST03.02.02.02 — Propagate cancellation through cache/quota/retry layers**
  - Type: refactoring
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: End-to-end cancellation flow
  - Status: Not Started
<a id="id-st03-02-02-03"></a>
- [ ] **ST03.02.02.03 — Test timeout, cancellation, and option validation**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Timeout/cancellation test suite
  - Status: Not Started

<a id="id-us03-03-01"></a>
### US03.03.01 — Fetch redirected exchange data without forwarding credentials

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F03.03](#id-f03-03) | P0 | 1.5 | 13 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** security engineer

**User story:** As a security engineer, I want redirect handling to strip the Finnhub token before a second host is contacted so that static-content redirects cannot leak the key.

**Acceptance criteria:**

- [ ] Automatic redirects are disabled for the exchange-symbol client that sends X-Finnhub-Token.
- [ ] Only the expected redirect status and one exact configured HTTPS destination host/path policy are accepted.
- [ ] The follow-up request is created as a fresh message and contains no Finnhub token, bearer token, cookie, or tenant header.
- [ ] Unexpected schemes, hosts, redirect chains, downgrade redirects, or missing Location are rejected with InvalidResponse.
- [ ] A regression server captures both hops and proves the credential appears only on the approved Finnhub origin request.

**Dependencies:** [US03.01.01](#id-us03-01-01)

**Labels:** `security` `redirect` `api-key` `ssrf`

**Source findings:**

- The exchange client follows a 302 to static2.finnhub.io while using the non-Authorization X-Finnhub-Token header; .NET does not guarantee that custom header is cleared on redirect.

**Subtasks:**

<a id="id-st03-03-01-01"></a>
- [ ] **ST03.03.01.01 — Disable automatic redirects for credentialed exchange client**
  - Type: security
  - Estimate: 3 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Non-redirecting primary client
  - Status: Not Started
<a id="id-st03-03-01-02"></a>
- [ ] **ST03.03.01.02 — Implement one-hop HTTPS destination validation and clean request**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Redirect-safe fetch flow
  - Status: Not Started
<a id="id-st03-03-01-03"></a>
- [ ] **ST03.03.01.03 — Add two-server credential leakage regression test**
  - Type: security-testing
  - Estimate: 5 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Redirect header-containment test
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST03.01.01.01](#id-st03-01-01-01) | [US03.01.01](#id-us03-01-01) | P0 | Define credential value/version provider interface | design | 4 | software architect | Credential provider contract | Not Started |
| [ST03.01.01.02](#id-st03-01-01-02) | [US03.01.01](#id-us03-01-01) | P0 | Refactor Finnhub clients to request-scoped credential resolution | implementation | 9 | backend engineer | Provider-backed Finnhub clients | Not Started |
| [ST03.01.01.03](#id-st03-01-01-03) | [US03.01.01](#id-us03-01-01) | P0 | Add key-redaction and source-precedence tests | security-testing | 6 | security engineer | Credential containment tests | Not Started |
| [ST03.01.02.01](#id-st03-01-02-01) | [US03.01.02](#id-us03-01-02) | P1 | Implement optional Azure, AWS, and GCP provider adapters | implementation | 20 | cloud engineer | Managed secret-store adapters | Not Started |
| [ST03.01.02.02](#id-st03-01-02-02) | [US03.01.02](#id-us03-01-02) | P1 | Create least-privilege deployment examples | documentation | 7 | cloud security engineer | Cloud identity configuration guides | Not Started |
| [ST03.01.02.03](#id-st03-01-02-03) | [US03.01.02](#id-us03-01-02) | P1 | Test adapters with fakes and failure mapping | testing | 8 | test engineer | Cloud secret adapter tests | Not Started |
| [ST03.01.03.01](#id-st03-01-03-01) | [US03.01.03](#id-us03-01-03) | P1 | Change .env loader to configured exact path | implementation | 2 | backend engineer | Exact-path dotenv loading | Not Started |
| [ST03.01.03.02](#id-st03-01-03-02) | [US03.01.03](#id-us03-01-03) | P1 | Add warnings, ignore checks, and documentation | documentation | 3 | developer experience engineer | Safe local-secret guidance | Not Started |
| [ST03.01.03.03](#id-st03-01-03-03) | [US03.01.03](#id-us03-01-03) | P1 | Test parent-directory and missing-file behavior | testing | 2 | test engineer | Dotenv path tests | Not Started |
| [ST03.02.01.01](#id-st03-02-01-01) | [US03.02.01](#id-us03-02-01) | P0 | Remove singleton credential/options capture | refactoring | 8 | backend engineer | Rotation-aware DI lifetimes | Not Started |
| [ST03.02.01.02](#id-st03-02-01-02) | [US03.02.01](#id-us03-02-01) | P0 | Implement versioned client refresh and overlap policy | implementation | 10 | platform engineer | Zero-downtime rotation mechanism | Not Started |
| [ST03.02.01.03](#id-st03-02-01-03) | [US03.02.01](#id-us03-02-01) | P0 | Add in-flight and post-rotation integration tests | testing | 7 | test engineer | Credential rotation suite | Not Started |
| [ST03.02.02.01](#id-st03-02-02-01) | [US03.02.02](#id-us03-02-02) | P0 | Wire validated timeout options into resilience pipeline | implementation | 5 | backend engineer | Configured outbound timeout | Not Started |
| [ST03.02.02.02](#id-st03-02-02-02) | [US03.02.02](#id-us03-02-02) | P0 | Propagate cancellation through cache/quota/retry layers | refactoring | 7 | backend engineer | End-to-end cancellation flow | Not Started |
| [ST03.02.02.03](#id-st03-02-02-03) | [US03.02.02](#id-us03-02-02) | P0 | Test timeout, cancellation, and option validation | testing | 5 | test engineer | Timeout/cancellation test suite | Not Started |
| [ST03.03.01.01](#id-st03-03-01-01) | [US03.03.01](#id-us03-03-01) | P0 | Disable automatic redirects for credentialed exchange client | security | 3 | security engineer | Non-redirecting primary client | Not Started |
| [ST03.03.01.02](#id-st03-03-01-02) | [US03.03.01](#id-us03-03-01) | P0 | Implement one-hop HTTPS destination validation and clean request | implementation | 5 | backend engineer | Redirect-safe fetch flow | Not Started |
| [ST03.03.01.03](#id-st03-03-01-03) | [US03.03.01](#id-us03-03-01) | P0 | Add two-server credential leakage regression test | security-testing | 5 | security engineer | Redirect header-containment test | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E03 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| B-01 | B. Security Improvements | Harden API-key management with cloud secret managers/KMS, workload identity, exact-path .env behavior, rotation, and log redaction. | [US03.01.01](#id-us03-01-01), [US03.01.02](#id-us03-01-02), [US03.01.03](#id-us03-01-03), [US03.02.01](#id-us03-02-01) | [E03](#id-e03) | P0 | Covered | Explicit review question B1. |
| R-03 | Repository finding | Prevent X-Finnhub-Token forwarding to static2.finnhub.io on automatic redirects. | [US03.03.01](#id-us03-03-01) | [E03](#id-e03) | P0 | Covered | Code-level P0 security finding. |
| R-20 | Repository finding | Honor configured timeouts and support runtime credential rotation without singleton-captured stale options. | [US03.02.01](#id-us03-02-01), [US03.02.02](#id-us03-02-02) | [E03](#id-e03) | P0 | Covered | Code-level configuration finding. |
| RF-086 | Code-review detail | Environment variables, user-secrets, and .env need a standard tenant-aware credential provider and must never be tool arguments. | [US03.01.01](#id-us03-01-01) | [E03](#id-e03) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-087 | Code-review detail | Production deployments should support Azure Key Vault, AWS Secrets Manager, and GCP Secret Manager through workload identity. | [US03.01.02](#id-us03-01-02) | [E03](#id-e03) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-088 | Code-review detail | Legacy .env loading should use an exact configured path rather than searching parent paths. | [US03.01.03](#id-us03-01-03) | [E03](#id-e03) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-089 | Code-review detail | Singleton services/typed clients capture keys and options, preventing practical hot rotation. | [US03.02.01](#id-us03-02-01) | [E03](#id-e03) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-090 | Code-review detail | Configured TimeoutSeconds is ignored in favor of a hard-coded 30 seconds. | [US03.02.02](#id-us03-02-02) | [E03](#id-e03) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-091 | Code-review detail | X-Finnhub-Token can be retained across an automatic redirect to static2.finnhub.io. | [US03.03.01](#id-us03-03-01) | [E03](#id-e03) | P0 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 1. M0A — Critical Stabilization

- **Priority:** P0
- **Indicative window:** Weeks 1–3
- **Primary epics:** E01, E03, E04
- **Goal:** Patch the highest-risk protocol, secret-containment, and failure-signaling defects.
- **Entry criteria:** Current CI green; deployment mode and emergency scope agreed.
- **Exit criteria:** Real clients reach the SDK endpoint; simulated routes are gone; redirect leakage is blocked; domain failures set protocol errors; regression tests cover both transports.
- **Delivery note:** This is the immediate hotfix milestone, not the full P0 backlog.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E03 | Epic | — | P0 | Credential Lifecycle and Secret Containment | See [E03](#id-e03) | See [E03](#id-e03) | — | finnhub-mcp; epic | — | Not Started |
| F03.01 | Feature | [E03](#id-e03) | P0 | Credential Provider Abstraction | See [F03.01](#id-f03-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e03 | — | Not Started |
| F03.02 | Feature | [E03](#id-e03) | P0 | Zero-Downtime Credential and Client Rotation | See [F03.02](#id-f03-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e03 | — | Not Started |
| F03.03 | Feature | [E03](#id-e03) | P0 | Redirect-Safe Upstream Fetching | See [F03.03](#id-f03-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e03 | — | Not Started |
| US03.01.01 | Story | [F03.01](#id-f03-01) | P0 | Introduce an injectable Finnhub credential provider | See [US03.01.01](#id-us03-01-01) | See [US03.01.01](#id-us03-01-01) | 3d | security; secrets; architecture; credentials | — | Not Started |
| US03.01.02 | Story | [F03.01](#id-f03-01) | P1 | Support managed cloud secret stores | See [US03.01.02](#id-us03-01-02) | See [US03.01.02](#id-us03-01-02) | 5d | security; azure; aws; gcp; secrets | [US03.01.01](#id-us03-01-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01) | Not Started |
| US03.01.03 | Story | [F03.01](#id-f03-01) | P1 | Harden local .env loading | See [US03.01.03](#id-us03-01-03) | See [US03.01.03](#id-us03-01-03) | 1d | security; dotenv; developer-experience | [US03.01.01](#id-us03-01-01) | Not Started |
| US03.02.01 | Story | [F03.02](#id-f03-02) | P0 | Hot-rotate API keys and key-scoped clients | See [US03.02.01](#id-us03-02-01) | See [US03.02.01](#id-us03-02-01) | 4d | security; rotation; http-client; availability | [US03.01.01](#id-us03-01-01) | Not Started |
| US03.02.02 | Story | [F03.02](#id-f03-02) | P0 | Honor configured outbound timeout and cancellation | See [US03.02.02](#id-us03-02-02) | See [US03.02.02](#id-us03-02-02) | 2d | configuration; timeout; cancellation; resilience | [US03.02.01](#id-us03-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01) | Not Started |
| US03.03.01 | Story | [F03.03](#id-f03-03) | P0 | Fetch redirected exchange data without forwarding credentials | See [US03.03.01](#id-us03-03-01) | See [US03.03.01](#id-us03-03-01) | 1.5d | security; redirect; api-key; ssrf | [US03.01.01](#id-us03-01-01) | Not Started |
| ST03.01.01.01 | Sub-task | [US03.01.01](#id-us03-01-01) | P0 | Define credential value/version provider interface | See [ST03.01.01.01](#id-st03-01-01-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST03.01.01.02 | Sub-task | [US03.01.01](#id-us03-01-01) | P0 | Refactor Finnhub clients to request-scoped credential resolution | See [ST03.01.01.02](#id-st03-01-01-02) | Not applicable; see detail or parent section | 9h | finnhub-mcp; implementation | — | Not Started |
| ST03.01.01.03 | Sub-task | [US03.01.01](#id-us03-01-01) | P0 | Add key-redaction and source-precedence tests | See [ST03.01.01.03](#id-st03-01-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; security-testing | — | Not Started |
| ST03.01.02.01 | Sub-task | [US03.01.02](#id-us03-01-02) | P1 | Implement optional Azure, AWS, and GCP provider adapters | See [ST03.01.02.01](#id-st03-01-02-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST03.01.02.02 | Sub-task | [US03.01.02](#id-us03-01-02) | P1 | Create least-privilege deployment examples | See [ST03.01.02.02](#id-st03-01-02-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; documentation | — | Not Started |
| ST03.01.02.03 | Sub-task | [US03.01.02](#id-us03-01-02) | P1 | Test adapters with fakes and failure mapping | See [ST03.01.02.03](#id-st03-01-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; testing | — | Not Started |
| ST03.01.03.01 | Sub-task | [US03.01.03](#id-us03-01-03) | P1 | Change .env loader to configured exact path | See [ST03.01.03.01](#id-st03-01-03-01) | Not applicable; see detail or parent section | 2h | finnhub-mcp; implementation | — | Not Started |
| ST03.01.03.02 | Sub-task | [US03.01.03](#id-us03-01-03) | P1 | Add warnings, ignore checks, and documentation | See [ST03.01.03.02](#id-st03-01-03-02) | Not applicable; see detail or parent section | 3h | finnhub-mcp; documentation | — | Not Started |
| ST03.01.03.03 | Sub-task | [US03.01.03](#id-us03-01-03) | P1 | Test parent-directory and missing-file behavior | See [ST03.01.03.03](#id-st03-01-03-03) | Not applicable; see detail or parent section | 2h | finnhub-mcp; testing | — | Not Started |
| ST03.02.01.01 | Sub-task | [US03.02.01](#id-us03-02-01) | P0 | Remove singleton credential/options capture | See [ST03.02.01.01](#id-st03-02-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; refactoring | — | Not Started |
| ST03.02.01.02 | Sub-task | [US03.02.01](#id-us03-02-01) | P0 | Implement versioned client refresh and overlap policy | See [ST03.02.01.02](#id-st03-02-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST03.02.01.03 | Sub-task | [US03.02.01](#id-us03-02-01) | P0 | Add in-flight and post-rotation integration tests | See [ST03.02.01.03](#id-st03-02-01-03) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |
| ST03.02.02.01 | Sub-task | [US03.02.02](#id-us03-02-02) | P0 | Wire validated timeout options into resilience pipeline | See [ST03.02.02.01](#id-st03-02-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST03.02.02.02 | Sub-task | [US03.02.02](#id-us03-02-02) | P0 | Propagate cancellation through cache/quota/retry layers | See [ST03.02.02.02](#id-st03-02-02-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; refactoring | — | Not Started |
| ST03.02.02.03 | Sub-task | [US03.02.02](#id-us03-02-02) | P0 | Test timeout, cancellation, and option validation | See [ST03.02.02.03](#id-st03-02-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST03.03.01.01 | Sub-task | [US03.03.01](#id-us03-03-01) | P0 | Disable automatic redirects for credentialed exchange client | See [ST03.03.01.01](#id-st03-03-01-01) | Not applicable; see detail or parent section | 3h | finnhub-mcp; security | — | Not Started |
| ST03.03.01.02 | Sub-task | [US03.03.01](#id-us03-03-01) | P0 | Implement one-hop HTTPS destination validation and clean request | See [ST03.03.01.02](#id-st03-03-01-02) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST03.03.01.03 | Sub-task | [US03.03.01](#id-us03-03-01) | P0 | Add two-server credential leakage regression test | See [ST03.03.01.03](#id-st03-03-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; security-testing | — | Not Started |

## 9. Delivery Guide

Definitions, governance rules, and quality gates for turning the backlog into releases.

| Area | Rule | Definition / Gate |
| --- | --- | --- |
| Hierarchy | Epic | A measurable product or platform outcome with exit criteria. |
| Hierarchy | Feature | A coherent capability slice that can be released or feature-flagged. |
| Hierarchy | User Story | A user- or operator-valued behavior with testable acceptance criteria. |
| Hierarchy | Subtask | An implementation action that produces code, tests, docs, design, or operational evidence. |
| Priority | P0 | Required for availability, security, protocol correctness, or trustworthy core financial output. |
| Priority | P1 | Material improvement to usefulness, developer productivity, or operating quality. |
| Priority | P2 | Longer-term optimization, ecosystem scale, localization, or optional breadth. |
| Estimation | Story days | Ideal engineering working days, including implementation and unit tests; excludes queue time and external approvals. |
| Estimation | Subtask hours | Implementation-level estimate used for sprint planning; re-estimate after technical design. |
| Definition of Ready | Scope | Persona, desired outcome, acceptance criteria, dependencies, quota/licensing impact, and rollout mode are understood. |
| Definition of Ready | Security | Trust boundary, sensitive inputs, tenant/cache partitioning, and logging requirements are identified. |
| Definition of Done | Quality | Code reviewed; unit, integration, and protocol tests pass; failure paths and cancellation are covered. |
| Definition of Done | Contract | Tool/resource/prompt schema, examples, error semantics, token/output bounds, and compatibility notes are updated. |
| Definition of Done | Operations | Metrics, structured logs, runbook, feature flag/rollback, cache/quota behavior, and alert thresholds are defined where relevant. |
| Definition of Done | Security | No secrets or raw sensitive payloads are logged; abuse limits and authorization tests pass for hosted surfaces. |
| Release gate | P0 | No open P0 defects; real client E2E green for supported transports; security regression suite green; docs match runtime routes. |
| Release gate | Financial accuracy | Metric definitions, units, periods, provenance, edge cases, and reference fixtures are independently reviewed. |
| Release gate | Context quality | Golden intent corpus meets agreed tool@1, Recall@3, invalid-argument, quota, latency, and token thresholds. |
| Operating rule | Hosted demo | Never expose a shared Finnhub key to browsers. Use an authenticated bounded BFF, strict allowlists, and per-user quotas. |
| Operating rule | Streaming | Use standard MCP Streamable HTTP. WebSocket is an upstream Finnhub ingestion mechanism or a separately secured UI channel. |
| Operating rule | Feedback | Collect opt-in, privacy-safe ranking feedback for offline evaluation; do not self-train the live BM25 ranker online. |

## 10. Epic Completion Checklist

- [ ] Create E03 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 3 Features under E03.
- [ ] Create all 6 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 18 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 9 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 28 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

