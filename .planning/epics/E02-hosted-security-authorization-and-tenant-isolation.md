---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E02
title: "Hosted Security, Authorization, and Tenant Isolation"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 10
  subtasks: 31
  traceability_owned: 15
  traceability_items: 16
story_estimate_days: 33.5
subtask_estimate_hours: 218
---

<a id="id-e02"></a>
# E02 — Hosted Security, Authorization, and Tenant Isolation

This is the self-contained coding-agent backlog for E02. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E02 | P0 | 5 | 10 | 31 | 33.5 | 218 | M0 — Hardened Core | Not Started |

> [!WARNING]
> This Epic participates in the pre-existing circular blocker [US02.04.01](#id-us02-04-01) → [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02) → [US02.04.01](#id-us02-04-01). The backlog owner must decide which edge is a true blocker, reclassify one edge as coordination, or introduce a shared foundation Story before automated sequencing.

## 2. Epic Definition

**Objective:** Define explicit local and hosted trust boundaries, enforce authenticated and bounded access, and isolate tenant state and sensitive operations.

**Business value:** Makes remote hosting defensible by preventing cross-origin abuse, anonymous quota consumption, cross-tenant leakage, and unaudited sensitive access.

**Exit criteria:**

- [ ] STDIO, loopback HTTP, and remote HTTP have documented and enforced security profiles.
- [ ] Remote HTTP requires TLS-backed OAuth/OIDC authentication and least-privilege authorization.
- [ ] Host, Origin, CORS, body-size, concurrency, and request-rate controls have automated negative tests.
- [ ] Caches, quota state, credentials, and audit records are tenant-aware.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F02.01](#id-f02-01) | P0 | Deployment Trust Profiles | 1 | 2 | Not Started |
| [F02.02](#id-f02-02) | P0 | Authenticated and Authorized Remote Access | 2 | 9 | Not Started |
| [F02.03](#id-f02-03) | P0 | HTTP Perimeter Controls | 2 | 5 | Not Started |
| [F02.04](#id-f02-04) | P0 | Tenant Isolation and Auditable Sensitive Access | 3 | 12 | Not Started |
| [F02.05](#id-f02-05) | P1 | Input and Untrusted-Content Safety | 2 | 5.5 | Not Started |

<a id="id-f02-01"></a>
### F02.01 — Deployment Trust Profiles

- **Parent Epic:** [E02](#id-e02)
- **Priority:** P0
- **Status:** Not Started

**Description:** Make STDIO, loopback HTTP, and remote HTTP explicit modes with safe defaults and startup validation.

**Expected outcome:** Operators cannot accidentally expose a local-trust configuration as an internet service.

**Stories:**

- [US02.01.01](#id-us02-01-01) — Enforce explicit deployment security profiles (P0, 2d)

<a id="id-f02-02"></a>
### F02.02 — Authenticated and Authorized Remote Access

- **Parent Epic:** [E02](#id-e02)
- **Priority:** P0
- **Status:** Not Started

**Description:** Add OAuth/OIDC authentication, scoped tool authorization, and entitlement-aware data controls for hosted use.

**Expected outcome:** Every hosted request has a verified subject, tenant, and permitted operation.

**Stories:**

- [US02.02.01](#id-us02-02-01) — Authenticate remote MCP requests with OAuth/OIDC (P0, 5d)
- [US02.02.02](#id-us02-02-02) — Authorize tools, views, and premium entitlements (P0, 4d)

<a id="id-f02-03"></a>
### F02.03 — HTTP Perimeter Controls

- **Parent Epic:** [E02](#id-e02)
- **Priority:** P0
- **Status:** Not Started

**Description:** Enforce Host, Origin, CORS, request-size, concurrency, and inbound rate policies before MCP processing.

**Expected outcome:** Cross-site, host-header, oversized-payload, and resource-exhaustion attacks are rejected predictably.

**Stories:**

- [US02.03.01](#id-us02-03-01) — Validate Host and Origin and enforce an environment-specific CORS allowlist (P0, 2d)
- [US02.03.02](#id-us02-03-02) — Bound inbound body size, concurrency, and request rate (P0, 3d)

<a id="id-f02-04"></a>
### F02.04 — Tenant Isolation and Auditable Sensitive Access

- **Parent Epic:** [E02](#id-e02)
- **Priority:** P0
- **Status:** Not Started

**Description:** Partition operational state, constrain sensitive views, and record privacy-preserving access events.

**Expected outcome:** One tenant cannot consume or observe another tenant's credentials, cache, quota, or access history.

**Stories:**

- [US02.04.01](#id-us02-04-01) — Partition credentials, cache, quota, and sessions by tenant (P0, 5d)
- [US02.04.02](#id-us02-04-02) — Apply policy controls to sensitive insider and raw data (P1, 3d)
- [US02.04.03](#id-us02-04-03) — Record privacy-preserving security audit events (P0, 4d)

<a id="id-f02-05"></a>
### F02.05 — Input and Untrusted-Content Safety

- **Parent Epic:** [E02](#id-e02)
- **Priority:** P1
- **Status:** Not Started

**Description:** Replace brittle regex-only validation with bounded canonicalization and mark upstream prose as untrusted data.

**Expected outcome:** Legitimate Unicode inputs work while control characters, unsafe URLs, oversized values, and prompt-like upstream content remain contained.

**Stories:**

- [US02.05.01](#id-us02-05-01) — Canonicalize and bound tool inputs (P1, 3d)
- [US02.05.02](#id-us02-05-02) — Validate outbound links and label provider text as untrusted (P1, 2.5d)

## 4. User Stories and Subtasks

<a id="id-us02-01-01"></a>
### US02.01.01 — Enforce explicit deployment security profiles

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.01](#id-f02-01) | P0 | 2 | 16 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** service operator

**User story:** As a service operator, I want explicit STDIO, loopback HTTP, and remote HTTP profiles so that insecure local defaults cannot be deployed remotely by accident.

**Acceptance criteria:**

- [ ] STDIO mode trusts only the spawning process and does not open a network listener.
- [ ] Loopback HTTP binds only to an explicit loopback address and enforces an exact local Host and Origin policy.
- [ ] Remote HTTP refuses startup unless authentication, TLS-forwarding validation, allowed hosts, allowed origins, and tenant resolution are configured.
- [ ] Profile selection and effective security settings are logged without secrets at startup.
- [ ] Configuration tests prove that unsafe combinations fail closed.

**Dependencies:** [US01.01.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-01)

**Labels:** `security` `deployment` `configuration` `fail-closed`

**Source findings:**

- The project currently has no explicit security distinction between local STDIO, loopback HTTP, and internet-hosted HTTP.

**Subtasks:**

<a id="id-st02-01-01-01"></a>
- [ ] **ST02.01.01.01 — Define profile configuration schema and threat assumptions**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: security architect
  - Deliverable/evidence: Deployment profile specification
  - Status: Not Started
<a id="id-st02-01-01-02"></a>
- [ ] **ST02.01.01.02 — Implement profile-specific bindings and startup validation**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Fail-closed profile loader
  - Status: Not Started
<a id="id-st02-01-01-03"></a>
- [ ] **ST02.01.01.03 — Test unsafe configuration combinations**
  - Type: testing
  - Estimate: 4 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Security-profile configuration tests
  - Status: Not Started

<a id="id-us02-02-01"></a>
### US02.02.01 — Authenticate remote MCP requests with OAuth/OIDC

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.02](#id-f02-02) | P0 | 5 | 35 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** hosted-service administrator

**User story:** As a hosted-service administrator, I want standards-based bearer authentication so that anonymous callers cannot spend quota or read data through my deployment.

**Acceptance criteria:**

- [ ] Remote HTTP validates issuer, audience, signature, expiry, and required claims from a configured OAuth/OIDC authority.
- [ ] Unauthenticated and invalid-token requests are rejected before a session or upstream request is created.
- [ ] The authenticated principal resolves to a stable tenant and subject identifier.
- [ ] Authentication discovery and challenge metadata follow the supported MCP authorization profile.
- [ ] Integration tests cover valid, expired, wrong-audience, wrong-issuer, and missing tokens.

**Dependencies:** [US02.01.01](#id-us02-01-01)

**Labels:** `security` `oauth` `oidc` `authentication`

**Source findings:**

- HTTP transport currently has no inbound request-level authentication.

**Subtasks:**

<a id="id-st02-02-01-01"></a>
- [ ] **ST02.02.01.01 — Select MCP-compatible OAuth/OIDC flow and claims model**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: identity architect
  - Deliverable/evidence: Authentication design and threat model
  - Status: Not Started
<a id="id-st02-02-01-02"></a>
- [ ] **ST02.02.01.02 — Implement bearer validation and tenant resolution**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: identity engineer
  - Deliverable/evidence: Authenticated remote request pipeline
  - Status: Not Started
<a id="id-st02-02-01-03"></a>
- [ ] **ST02.02.01.03 — Publish discovery/challenge metadata**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Authorization discovery surface
  - Status: Not Started
<a id="id-st02-02-01-04"></a>
- [ ] **ST02.02.01.04 — Add token-negative integration matrix**
  - Type: testing
  - Estimate: 8 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: OAuth/OIDC integration tests
  - Status: Not Started

<a id="id-us02-02-02"></a>
### US02.02.02 — Authorize tools, views, and premium entitlements

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.02](#id-f02-02) | P0 | 4 | 25 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** security administrator

**User story:** As a security administrator, I want scoped authorization so that identities can access only approved tools and data detail levels.

**Acceptance criteria:**

- [ ] Authorization policies can constrain tool invocation, resource access, full/raw views, and premium endpoints by scope or role.
- [ ] A denied operation returns PermissionDenied without invoking Finnhub or revealing whether inaccessible data exists.
- [ ] Tool discovery and capabilities resources either hide unauthorized operations or mark them unavailable consistently.
- [ ] Authorization decisions are tenant-aware and cannot be overridden by tool arguments.
- [ ] Policy tests cover least-privilege, entitlement downgrade, and cross-tenant denial.

**Dependencies:** [US02.02.01](#id-us02-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01)

**Labels:** `security` `authorization` `scopes` `entitlements`

**Source findings:**

- There is no request-level authorization or premium/sensitive-data access policy.

**Subtasks:**

<a id="id-st02-02-02-01"></a>
- [ ] **ST02.02.02.01 — Define scope, role, view, and entitlement matrix**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: security architect
  - Deliverable/evidence: Authorization policy matrix
  - Status: Not Started
<a id="id-st02-02-02-02"></a>
- [ ] **ST02.02.02.02 — Enforce policies in discovery, tools, resources, and views**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: End-to-end authorization handlers
  - Status: Not Started
<a id="id-st02-02-02-03"></a>
- [ ] **ST02.02.02.03 — Test denial without upstream side effects**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Authorization side-effect tests
  - Status: Not Started

<a id="id-us02-03-01"></a>
### US02.03.01 — Validate Host and Origin and enforce an environment-specific CORS allowlist

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.03](#id-f02-03) | P0 | 2 | 15 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** security engineer

**User story:** As a security engineer, I want strict host and origin validation so that browsers and DNS-rebinding attacks cannot drive the MCP server from untrusted sites.

**Acceptance criteria:**

- [ ] AllowedHosts is not wildcard in production and requests with an unapproved Host are rejected.
- [ ] Every HTTP MCP request validates Origin according to the MCP transport security requirements; missing-Origin behavior is explicitly documented for non-browser clients.
- [ ] Production CORS uses an exact configured allowlist, permitted methods, and permitted headers with no wildcard fallback.
- [ ] Development AllowAnyOrigin is restricted to an explicit local profile and is never applied by custom endpoints.
- [ ] Integration tests cover approved, unapproved, null, malformed, and DNS-rebinding-style Host/Origin values.

**Dependencies:** [US02.01.01](#id-us02-01-01), [US01.01.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-02)

**Labels:** `security` `cors` `origin` `host`

**Source findings:**

- AllowedHosts is '*'.
- Development permits any origin and custom MCP-like routes emit wildcard CORS in every environment.
- MCP Streamable HTTP requires Origin validation.

**Subtasks:**

<a id="id-st02-03-01-01"></a>
- [ ] **ST02.03.01.01 — Implement Host and Origin validation middleware**
  - Type: security
  - Estimate: 6 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Host/Origin guard
  - Status: Not Started
<a id="id-st02-03-01-02"></a>
- [ ] **ST02.03.01.02 — Replace wildcard CORS with exact environment policies**
  - Type: implementation
  - Estimate: 4 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Environment-scoped CORS policies
  - Status: Not Started
<a id="id-st02-03-01-03"></a>
- [ ] **ST02.03.01.03 — Add hostile Host/Origin integration cases**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Perimeter origin test matrix
  - Status: Not Started

<a id="id-us02-03-02"></a>
### US02.03.02 — Bound inbound body size, concurrency, and request rate

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.03](#id-f02-03) | P0 | 3 | 21 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** site reliability engineer

**User story:** As an SRE, I want cheap perimeter limits so that oversized JSON-RPC payloads or concurrent sessions cannot exhaust memory, threads, or Finnhub quota.

**Acceptance criteria:**

- [ ] A documented maximum request-body size is enforced before buffering or JSON deserialization.
- [ ] Per-subject and per-tenant request-rate limits apply before tool execution, with a separate conservative anonymous limit where applicable.
- [ ] Global and per-tenant concurrent tool-call/session limits use bounded queues and deadlines rather than unbounded waiting.
- [ ] Rejected requests return RateLimited or a correct HTTP status with Retry-After and a trace id.
- [ ] Load tests prove memory and active upstream calls remain within configured bounds.

**Dependencies:** [US02.02.01](#id-us02-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01)

**Labels:** `security` `dos` `rate-limit` `concurrency`

**Source findings:**

- The HTTP surface has no request-size, inbound rate, or concurrency limits; regex validation does not mitigate resource exhaustion.

**Subtasks:**

<a id="id-st02-03-02-01"></a>
- [ ] **ST02.03.02.01 — Choose body, session, concurrency, and queue defaults**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Capacity-limit specification
  - Status: Not Started
<a id="id-st02-03-02-02"></a>
- [ ] **ST02.03.02.02 — Implement early size/rate/concurrency limiters**
  - Type: implementation
  - Estimate: 9 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Bounded request pipeline
  - Status: Not Started
<a id="id-st02-03-02-03"></a>
- [ ] **ST02.03.02.03 — Run overload and rejection-behavior tests**
  - Type: performance-testing
  - Estimate: 8 hours
  - Suggested owner role: performance engineer
  - Deliverable/evidence: Load test report and thresholds
  - Status: Not Started

<a id="id-us02-04-01"></a>
### US02.04.01 — Partition credentials, cache, quota, and sessions by tenant

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.04](#id-f02-04) | P0 | 5 | 29 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** multi-tenant platform owner

**User story:** As a multi-tenant platform owner, I want all mutable state scoped to a verified tenant and credential fingerprint so that one customer cannot affect or observe another.

**Acceptance criteria:**

- [ ] Every cache key includes tenant and credential-version/fingerprint where provider entitlements or results can differ.
- [ ] Rate and quota ledgers are partitioned by tenant and Finnhub credential while also respecting global provider limits.
- [ ] MCP sessions cannot be resumed or deleted by a different tenant or subject.
- [ ] Tenant resolution is immutable for the request and never sourced from an unverified argument or header.
- [ ] Isolation tests run identical symbols across two tenants and prove no cache, quota, credential, or session crossover.

**Dependencies:** [US02.02.01](#id-us02-02-01), [US03.01.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-01-01), [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02)

**Labels:** `security` `multi-tenancy` `cache` `quota`

**Source findings:**

- Hybrid cache and rate tracking are currently global/shared rather than tenant-partitioned.

**Subtasks:**

<a id="id-st02-04-01-01"></a>
- [ ] **ST02.04.01.01 — Create immutable tenant execution context**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Tenant context service
  - Status: Not Started
<a id="id-st02-04-01-02"></a>
- [ ] **ST02.04.01.02 — Apply tenant and credential partition keys across state stores**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: platform engineer
  - Deliverable/evidence: Partitioned cache/quota/session state
  - Status: Not Started
<a id="id-st02-04-01-03"></a>
- [ ] **ST02.04.01.03 — Build two-tenant isolation tests**
  - Type: security-testing
  - Estimate: 8 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Cross-tenant isolation suite
  - Status: Not Started

<a id="id-us02-04-02"></a>
### US02.04.02 — Apply policy controls to sensitive insider and raw data

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.04](#id-f02-04) | P1 | 3 | 18 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** data governance owner

**User story:** As a data governance owner, I want sensitive or licensed views controlled by policy so that public facts remain useful without exposing disallowed detail.

**Acceptance criteria:**

- [ ] Public insider names are not masked by default solely because they are names, but raw/full transaction detail requires an explicit entitlement policy.
- [ ] Policy can suppress, aggregate, or deny fields based on tenant, purpose, and licensing rules.
- [ ] The response states when fields were omitted by policy without leaking their values.
- [ ] User-created search or watch history is treated as private tenant data and never appears in shared cache or logs.
- [ ] Governance tests verify summary access, denied raw access, and field-level redaction.

**Dependencies:** [US02.02.02](#id-us02-02-02), [US02.04.01](#id-us02-04-01)

**Labels:** `security` `privacy` `insider-data` `licensing`

**Source findings:**

- Insider transactions are public data and need not be blanket-masked, but sensitive/full detail and user query history need policy and isolation.

**Subtasks:**

<a id="id-st02-04-02-01"></a>
- [ ] **ST02.04.02.01 — Document insider/raw-data classification and licensing rules**
  - Type: governance
  - Estimate: 5 hours
  - Suggested owner role: data governance owner
  - Deliverable/evidence: Data access policy
  - Status: Not Started
<a id="id-st02-04-02-02"></a>
- [ ] **ST02.04.02.02 — Implement field-level suppress/aggregate/deny projections**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Policy-aware response projection
  - Status: Not Started
<a id="id-st02-04-02-03"></a>
- [ ] **ST02.04.02.03 — Test raw/full entitlement and history privacy**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Sensitive-view policy tests
  - Status: Not Started

<a id="id-us02-04-03"></a>
### US02.04.03 — Record privacy-preserving security audit events

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.04](#id-f02-04) | P0 | 4 | 23 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** compliance analyst

**User story:** As a compliance analyst, I want searchable audit records for hosted tool and sensitive-data access so that investigations can reconstruct who accessed what without storing secrets or raw prompts.

**Acceptance criteria:**

- [ ] Each hosted call records subject, tenant, client, trace id, tool/version, outcome, cache status, upstream call count/cost, duration, and authorization decision.
- [ ] Arguments are allowlisted and normalized or irreversibly hashed; API keys, bearer tokens, raw JSON-RPC bodies, and full news/filing text are never recorded.
- [ ] Sensitive/full view access and authorization denials are distinguishable audit event types.
- [ ] Audit retention, access control, integrity, and export policy are configurable and documented.
- [ ] Automated log scans verify representative secrets and raw request bodies are absent.

**Dependencies:** [US02.02.01](#id-us02-02-01), [US02.02.02](#id-us02-02-02)

**Labels:** `security` `audit` `compliance` `logging`

**Source findings:**

- No audit logging currently tracks sensitive data access, and raw-body logging would be inappropriate for an audit trail.

**Subtasks:**

<a id="id-st02-04-03-01"></a>
- [ ] **ST02.04.03.01 — Define audit event schema, retention, and access policy**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: compliance engineer
  - Deliverable/evidence: Audit logging specification
  - Status: Not Started
<a id="id-st02-04-03-02"></a>
- [ ] **ST02.04.03.02 — Emit structured audit events at authorization and tool boundaries**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Audit event pipeline
  - Status: Not Started
<a id="id-st02-04-03-03"></a>
- [ ] **ST02.04.03.03 — Add secret/raw-content log scanning tests**
  - Type: security-testing
  - Estimate: 7 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Log privacy regression suite
  - Status: Not Started

<a id="id-us02-05-01"></a>
### US02.05.01 — Canonicalize and bound tool inputs

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.05](#id-f02-05) | P1 | 3 | 20 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** API consumer

**User story:** As an API consumer, I want input validation that accepts legitimate financial identifiers and international names while rejecting controls and pathological sizes consistently.

**Acceptance criteria:**

- [ ] Natural-language queries normalize Unicode and whitespace, allow common punctuation such as ampersands and apostrophes, and reject control characters including CR/LF.
- [ ] Ticker, exchange, date, identifier, view, field, and pagination inputs use explicit allowlists and maximum lengths appropriate to their domain.
- [ ] Validation occurs before cache lookup, logging, quota reservation, or upstream calls.
- [ ] Error messages truncate and encode rejected input and never reflect control characters verbatim.
- [ ] Property/fuzz tests cover Unicode, very long strings, control characters, encoded delimiters, and representative symbols such as BRK.A.

**Dependencies:** —

**Labels:** `security` `validation` `unicode` `fuzzing`

**Source findings:**

- Current regexes reduce simple injection risk but reject legitimate ampersands, apostrophes, and Unicode, while \s can admit CR/LF.
- URI escaping is already used, so the primary remaining risks are resource exhaustion, log injection, and semantic validation rather than SQL/shell injection.

**Subtasks:**

<a id="id-st02-05-01-01"></a>
- [ ] **ST02.05.01.01 — Define canonical validators and maximum sizes by input type**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Input contract matrix
  - Status: Not Started
<a id="id-st02-05-01-02"></a>
- [ ] **ST02.05.01.02 — Replace regex-only natural-query validation**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Unicode-aware canonical validation library
  - Status: Not Started
<a id="id-st02-05-01-03"></a>
- [ ] **ST02.05.01.03 — Add fuzz/property tests and safe error rendering**
  - Type: security-testing
  - Estimate: 8 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Input fuzz suite
  - Status: Not Started

<a id="id-us02-05-02"></a>
### US02.05.02 — Validate outbound links and label provider text as untrusted

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F02.05](#id-f02-05) | P1 | 2.5 | 16 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** AI agent integrator

**User story:** As an AI agent integrator, I want upstream URLs and prose safely constrained so that financial news or filing content cannot be mistaken for trusted instructions.

**Acceptance criteria:**

- [ ] Returned external URLs must parse as HTTPS and match configured provider/publisher rules or be omitted with a warning.
- [ ] News, filing, and company-description text is explicitly marked as external untrusted content in schemas and prompts.
- [ ] No upstream text is interpolated into system-level tool instructions or executable templates.
- [ ] Response-size and per-field length caps apply before provider prose is returned to the client.
- [ ] Tests cover javascript/data URLs, malformed hosts, prompt-injection-like article text, and oversized fields.

**Dependencies:** [US02.05.01](#id-us02-05-01)

**Labels:** `security` `untrusted-content` `urls` `prompt-injection`

**Source findings:**

- News and future filing content must be treated as untrusted provider data, and outbound URLs require validation.

**Subtasks:**

<a id="id-st02-05-02-01"></a>
- [ ] **ST02.05.02.01 — Implement provider URL allow/deny validation**
  - Type: security
  - Estimate: 5 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Outbound URL validator
  - Status: Not Started
<a id="id-st02-05-02-02"></a>
- [ ] **ST02.05.02.02 — Annotate and bound untrusted provider text**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Untrusted-content response contract
  - Status: Not Started
<a id="id-st02-05-02-03"></a>
- [ ] **ST02.05.02.03 — Test malicious links and instruction-like content**
  - Type: security-testing
  - Estimate: 5 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Content safety test fixtures
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST02.01.01.01](#id-st02-01-01-01) | [US02.01.01](#id-us02-01-01) | P0 | Define profile configuration schema and threat assumptions | design | 5 | security architect | Deployment profile specification | Not Started |
| [ST02.01.01.02](#id-st02-01-01-02) | [US02.01.01](#id-us02-01-01) | P0 | Implement profile-specific bindings and startup validation | implementation | 7 | backend engineer | Fail-closed profile loader | Not Started |
| [ST02.01.01.03](#id-st02-01-01-03) | [US02.01.01](#id-us02-01-01) | P0 | Test unsafe configuration combinations | testing | 4 | security engineer | Security-profile configuration tests | Not Started |
| [ST02.02.01.01](#id-st02-02-01-01) | [US02.02.01](#id-us02-02-01) | P0 | Select MCP-compatible OAuth/OIDC flow and claims model | design | 8 | identity architect | Authentication design and threat model | Not Started |
| [ST02.02.01.02](#id-st02-02-01-02) | [US02.02.01](#id-us02-02-01) | P0 | Implement bearer validation and tenant resolution | implementation | 14 | identity engineer | Authenticated remote request pipeline | Not Started |
| [ST02.02.01.03](#id-st02-02-01-03) | [US02.02.01](#id-us02-02-01) | P0 | Publish discovery/challenge metadata | implementation | 5 | MCP engineer | Authorization discovery surface | Not Started |
| [ST02.02.01.04](#id-st02-02-01-04) | [US02.02.01](#id-us02-02-01) | P0 | Add token-negative integration matrix | testing | 8 | security engineer | OAuth/OIDC integration tests | Not Started |
| [ST02.02.02.01](#id-st02-02-02-01) | [US02.02.02](#id-us02-02-02) | P0 | Define scope, role, view, and entitlement matrix | design | 6 | security architect | Authorization policy matrix | Not Started |
| [ST02.02.02.02](#id-st02-02-02-02) | [US02.02.02](#id-us02-02-02) | P0 | Enforce policies in discovery, tools, resources, and views | implementation | 12 | backend engineer | End-to-end authorization handlers | Not Started |
| [ST02.02.02.03](#id-st02-02-02-03) | [US02.02.02](#id-us02-02-02) | P0 | Test denial without upstream side effects | testing | 7 | security engineer | Authorization side-effect tests | Not Started |
| [ST02.03.01.01](#id-st02-03-01-01) | [US02.03.01](#id-us02-03-01) | P0 | Implement Host and Origin validation middleware | security | 6 | security engineer | Host/Origin guard | Not Started |
| [ST02.03.01.02](#id-st02-03-01-02) | [US02.03.01](#id-us02-03-01) | P0 | Replace wildcard CORS with exact environment policies | implementation | 4 | backend engineer | Environment-scoped CORS policies | Not Started |
| [ST02.03.01.03](#id-st02-03-01-03) | [US02.03.01](#id-us02-03-01) | P0 | Add hostile Host/Origin integration cases | testing | 5 | security engineer | Perimeter origin test matrix | Not Started |
| [ST02.03.02.01](#id-st02-03-02-01) | [US02.03.02](#id-us02-03-02) | P0 | Choose body, session, concurrency, and queue defaults | design | 4 | SRE | Capacity-limit specification | Not Started |
| [ST02.03.02.02](#id-st02-03-02-02) | [US02.03.02](#id-us02-03-02) | P0 | Implement early size/rate/concurrency limiters | implementation | 9 | backend engineer | Bounded request pipeline | Not Started |
| [ST02.03.02.03](#id-st02-03-02-03) | [US02.03.02](#id-us02-03-02) | P0 | Run overload and rejection-behavior tests | performance-testing | 8 | performance engineer | Load test report and thresholds | Not Started |
| [ST02.04.01.01](#id-st02-04-01-01) | [US02.04.01](#id-us02-04-01) | P0 | Create immutable tenant execution context | implementation | 7 | backend engineer | Tenant context service | Not Started |
| [ST02.04.01.02](#id-st02-04-01-02) | [US02.04.01](#id-us02-04-01) | P0 | Apply tenant and credential partition keys across state stores | implementation | 14 | platform engineer | Partitioned cache/quota/session state | Not Started |
| [ST02.04.01.03](#id-st02-04-01-03) | [US02.04.01](#id-us02-04-01) | P0 | Build two-tenant isolation tests | security-testing | 8 | security engineer | Cross-tenant isolation suite | Not Started |
| [ST02.04.02.01](#id-st02-04-02-01) | [US02.04.02](#id-us02-04-02) | P1 | Document insider/raw-data classification and licensing rules | governance | 5 | data governance owner | Data access policy | Not Started |
| [ST02.04.02.02](#id-st02-04-02-02) | [US02.04.02](#id-us02-04-02) | P1 | Implement field-level suppress/aggregate/deny projections | implementation | 8 | backend engineer | Policy-aware response projection | Not Started |
| [ST02.04.02.03](#id-st02-04-02-03) | [US02.04.02](#id-us02-04-02) | P1 | Test raw/full entitlement and history privacy | testing | 5 | security engineer | Sensitive-view policy tests | Not Started |
| [ST02.04.03.01](#id-st02-04-03-01) | [US02.04.03](#id-us02-04-03) | P0 | Define audit event schema, retention, and access policy | design | 6 | compliance engineer | Audit logging specification | Not Started |
| [ST02.04.03.02](#id-st02-04-03-02) | [US02.04.03](#id-us02-04-03) | P0 | Emit structured audit events at authorization and tool boundaries | implementation | 10 | backend engineer | Audit event pipeline | Not Started |
| [ST02.04.03.03](#id-st02-04-03-03) | [US02.04.03](#id-us02-04-03) | P0 | Add secret/raw-content log scanning tests | security-testing | 7 | security engineer | Log privacy regression suite | Not Started |
| [ST02.05.01.01](#id-st02-05-01-01) | [US02.05.01](#id-us02-05-01) | P1 | Define canonical validators and maximum sizes by input type | design | 5 | backend engineer | Input contract matrix | Not Started |
| [ST02.05.01.02](#id-st02-05-01-02) | [US02.05.01](#id-us02-05-01) | P1 | Replace regex-only natural-query validation | implementation | 7 | backend engineer | Unicode-aware canonical validation library | Not Started |
| [ST02.05.01.03](#id-st02-05-01-03) | [US02.05.01](#id-us02-05-01) | P1 | Add fuzz/property tests and safe error rendering | security-testing | 8 | security engineer | Input fuzz suite | Not Started |
| [ST02.05.02.01](#id-st02-05-02-01) | [US02.05.02](#id-us02-05-02) | P1 | Implement provider URL allow/deny validation | security | 5 | security engineer | Outbound URL validator | Not Started |
| [ST02.05.02.02](#id-st02-05-02-02) | [US02.05.02](#id-us02-05-02) | P1 | Annotate and bound untrusted provider text | implementation | 6 | MCP engineer | Untrusted-content response contract | Not Started |
| [ST02.05.02.03](#id-st02-05-02-03) | [US02.05.02](#id-us02-05-02) | P1 | Test malicious links and instruction-like content | security-testing | 5 | security engineer | Content safety test fixtures | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E02 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| B-02 | B. Security Improvements | Add request authentication and authorization for hosted deployments, scoped access, key/token rotation, and multi-tenant isolation. | [US02.01.01](#id-us02-01-01), [US02.02.01](#id-us02-02-01), [US02.02.02](#id-us02-02-02), [US02.04.01](#id-us02-04-01) | [E02](#id-e02) | P0 | Covered | Explicit review question B2. |
| B-03 | B. Security Improvements | Apply policy and licensing controls to sensitive/high-detail insider data without unnecessary default masking of public records. | [US02.04.02](#id-us02-04-02) | [E02](#id-e02) | P1 | Covered | Explicit review question B3. |
| B-04 | B. Security Improvements | Replace brittle regex-only validation with normalized Unicode-aware input handling, control-character rejection, request-size limits, URL validation, and bounded errors. | [US02.03.02](#id-us02-03-02), [US02.05.01](#id-us02-05-01), [US02.05.02](#id-us02-05-02) | [E02](#id-e02) | P0 | Covered | Explicit review question B4. |
| B-05 | B. Security Improvements | Define production Host/Origin/CORS/TLS policies and remove wildcard behavior from MCP routes. | [US02.03.01](#id-us02-03-01) | [E02](#id-e02) | P0 | Covered | Explicit review question B5. |
| B-06 | B. Security Improvements | Add privacy-safe audit logging for sensitive tool access and hosted multi-tenant activity. | [US02.04.03](#id-us02-04-03) | [E02](#id-e02) | P0 | Covered | Explicit review question B6. |
| R-19 | Repository finding | Partition quota tracking by key/tenant and coordinate multi-instance limits rather than using one process-global tracker. | [US02.04.01](#id-us02-04-01), [US04.03.03](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-03), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01) | [E02](#id-e02) | P0 | Covered | Code-level hosted-deployment finding. |
| RF-071 | Code-review detail | Custom /mcp, /mcp/sse, and /mcp/streamable endpoints simulate rather than implement MCP and include wildcard CORS/raw-body behavior. | [US01.01.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-02), [US02.03.01](#id-us02-03-01) | [E01](./E01-mcp-transport-and-protocol-integrity.md#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-077 | Code-review detail | Local STDIO, loopback HTTP, and remote HTTP require different explicit trust boundaries. | [US02.01.01](#id-us02-01-01) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-078 | Code-review detail | Hosted HTTP lacks inbound authentication and authorization. | [US02.02.01](#id-us02-02-01), [US02.02.02](#id-us02-02-02) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-079 | Code-review detail | AllowedHosts wildcard, development AllowAnyOrigin, and unconditional wildcard headers violate a safe production origin policy. | [US02.03.01](#id-us02-03-01) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-080 | Code-review detail | No request body, inbound rate, concurrency, or bounded-queue limits protect the HTTP surface. | [US02.03.02](#id-us02-03-02) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-081 | Code-review detail | Cache and rate state are shared globally and need tenant and credential partitioning. | [US02.04.01](#id-us02-04-01), [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02), [US04.03.03](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-03) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-082 | Code-review detail | Insider names are public, but raw/full detail needs policy and user query/watch history needs privacy. | [US02.04.02](#id-us02-04-02) | [E02](#id-e02) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-083 | Code-review detail | Hosted sensitive access needs audit logging without keys, raw bodies, or unbounded content. | [US02.04.03](#id-us02-04-03) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-084 | Code-review detail | Regex-only validation rejects legitimate punctuation/Unicode, may admit CR/LF, and does not address resource exhaustion. | [US02.05.01](#id-us02-05-01), [US02.03.02](#id-us02-03-02) | [E02](#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-085 | Code-review detail | Provider URLs and news/filing prose must be treated as validated, bounded, untrusted content. | [US02.05.02](#id-us02-05-02) | [E02](#id-e02) | P1 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 2. M0B — Hardened Service

- **Priority:** P0
- **Indicative window:** Weeks 3–10
- **Primary epics:** E02, E04, E05, E07, E15
- **Goal:** Complete hosted security, quota controls, core data correctness, response bounds, and release gates.
- **Entry criteria:** M0A released; authentication/tenant model approved; financial semantics reviewed.
- **Exit criteria:** Hosted trust boundary is enforced; quota/retry/cache behavior is safe; search and calculations are correct; P0 release suite and supply-chain gates pass.
- **Delivery note:** Use feature flags and staged deployment; local-only scope can finish sooner.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E02 | Epic | — | P0 | Hosted Security, Authorization, and Tenant Isolation | See [E02](#id-e02) | See [E02](#id-e02) | — | finnhub-mcp; epic | — | Not Started |
| F02.01 | Feature | [E02](#id-e02) | P0 | Deployment Trust Profiles | See [F02.01](#id-f02-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e02 | — | Not Started |
| F02.02 | Feature | [E02](#id-e02) | P0 | Authenticated and Authorized Remote Access | See [F02.02](#id-f02-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e02 | — | Not Started |
| F02.03 | Feature | [E02](#id-e02) | P0 | HTTP Perimeter Controls | See [F02.03](#id-f02-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e02 | — | Not Started |
| F02.04 | Feature | [E02](#id-e02) | P0 | Tenant Isolation and Auditable Sensitive Access | See [F02.04](#id-f02-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e02 | — | Not Started |
| F02.05 | Feature | [E02](#id-e02) | P1 | Input and Untrusted-Content Safety | See [F02.05](#id-f02-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e02 | — | Not Started |
| US02.01.01 | Story | [F02.01](#id-f02-01) | P0 | Enforce explicit deployment security profiles | See [US02.01.01](#id-us02-01-01) | See [US02.01.01](#id-us02-01-01) | 2d | security; deployment; configuration; fail-closed | [US01.01.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-01) | Not Started |
| US02.02.01 | Story | [F02.02](#id-f02-02) | P0 | Authenticate remote MCP requests with OAuth/OIDC | See [US02.02.01](#id-us02-02-01) | See [US02.02.01](#id-us02-02-01) | 5d | security; oauth; oidc; authentication | [US02.01.01](#id-us02-01-01) | Not Started |
| US02.02.02 | Story | [F02.02](#id-f02-02) | P0 | Authorize tools, views, and premium entitlements | See [US02.02.02](#id-us02-02-02) | See [US02.02.02](#id-us02-02-02) | 4d | security; authorization; scopes; entitlements | [US02.02.01](#id-us02-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01) | Not Started |
| US02.03.01 | Story | [F02.03](#id-f02-03) | P0 | Validate Host and Origin and enforce an environment-specific CORS allowlist | See [US02.03.01](#id-us02-03-01) | See [US02.03.01](#id-us02-03-01) | 2d | security; cors; origin; host | [US02.01.01](#id-us02-01-01), [US01.01.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-02) | Not Started |
| US02.03.02 | Story | [F02.03](#id-f02-03) | P0 | Bound inbound body size, concurrency, and request rate | See [US02.03.02](#id-us02-03-02) | See [US02.03.02](#id-us02-03-02) | 3d | security; dos; rate-limit; concurrency | [US02.02.01](#id-us02-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01) | Not Started |
| US02.04.01 | Story | [F02.04](#id-f02-04) | P0 | Partition credentials, cache, quota, and sessions by tenant | See [US02.04.01](#id-us02-04-01) | See [US02.04.01](#id-us02-04-01) | 5d | security; multi-tenancy; cache; quota | [US02.02.01](#id-us02-02-01), [US03.01.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-01-01), [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02) | Not Started |
| US02.04.02 | Story | [F02.04](#id-f02-04) | P1 | Apply policy controls to sensitive insider and raw data | See [US02.04.02](#id-us02-04-02) | See [US02.04.02](#id-us02-04-02) | 3d | security; privacy; insider-data; licensing | [US02.02.02](#id-us02-02-02), [US02.04.01](#id-us02-04-01) | Not Started |
| US02.04.03 | Story | [F02.04](#id-f02-04) | P0 | Record privacy-preserving security audit events | See [US02.04.03](#id-us02-04-03) | See [US02.04.03](#id-us02-04-03) | 4d | security; audit; compliance; logging | [US02.02.01](#id-us02-02-01), [US02.02.02](#id-us02-02-02) | Not Started |
| US02.05.01 | Story | [F02.05](#id-f02-05) | P1 | Canonicalize and bound tool inputs | See [US02.05.01](#id-us02-05-01) | See [US02.05.01](#id-us02-05-01) | 3d | security; validation; unicode; fuzzing | — | Not Started |
| US02.05.02 | Story | [F02.05](#id-f02-05) | P1 | Validate outbound links and label provider text as untrusted | See [US02.05.02](#id-us02-05-02) | See [US02.05.02](#id-us02-05-02) | 2.5d | security; untrusted-content; urls; prompt-injection | [US02.05.01](#id-us02-05-01) | Not Started |
| ST02.01.01.01 | Sub-task | [US02.01.01](#id-us02-01-01) | P0 | Define profile configuration schema and threat assumptions | See [ST02.01.01.01](#id-st02-01-01-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST02.01.01.02 | Sub-task | [US02.01.01](#id-us02-01-01) | P0 | Implement profile-specific bindings and startup validation | See [ST02.01.01.02](#id-st02-01-01-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST02.01.01.03 | Sub-task | [US02.01.01](#id-us02-01-01) | P0 | Test unsafe configuration combinations | See [ST02.01.01.03](#id-st02-01-01-03) | Not applicable; see detail or parent section | 4h | finnhub-mcp; testing | — | Not Started |
| ST02.02.01.01 | Sub-task | [US02.02.01](#id-us02-02-01) | P0 | Select MCP-compatible OAuth/OIDC flow and claims model | See [ST02.02.01.01](#id-st02-02-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST02.02.01.02 | Sub-task | [US02.02.01](#id-us02-02-01) | P0 | Implement bearer validation and tenant resolution | See [ST02.02.01.02](#id-st02-02-01-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST02.02.01.03 | Sub-task | [US02.02.01](#id-us02-02-01) | P0 | Publish discovery/challenge metadata | See [ST02.02.01.03](#id-st02-02-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST02.02.01.04 | Sub-task | [US02.02.01](#id-us02-02-01) | P0 | Add token-negative integration matrix | See [ST02.02.01.04](#id-st02-02-01-04) | Not applicable; see detail or parent section | 8h | finnhub-mcp; testing | — | Not Started |
| ST02.02.02.01 | Sub-task | [US02.02.02](#id-us02-02-02) | P0 | Define scope, role, view, and entitlement matrix | See [ST02.02.02.01](#id-st02-02-02-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST02.02.02.02 | Sub-task | [US02.02.02](#id-us02-02-02) | P0 | Enforce policies in discovery, tools, resources, and views | See [ST02.02.02.02](#id-st02-02-02-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST02.02.02.03 | Sub-task | [US02.02.02](#id-us02-02-02) | P0 | Test denial without upstream side effects | See [ST02.02.02.03](#id-st02-02-02-03) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |
| ST02.03.01.01 | Sub-task | [US02.03.01](#id-us02-03-01) | P0 | Implement Host and Origin validation middleware | See [ST02.03.01.01](#id-st02-03-01-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; security | — | Not Started |
| ST02.03.01.02 | Sub-task | [US02.03.01](#id-us02-03-01) | P0 | Replace wildcard CORS with exact environment policies | See [ST02.03.01.02](#id-st02-03-01-02) | Not applicable; see detail or parent section | 4h | finnhub-mcp; implementation | — | Not Started |
| ST02.03.01.03 | Sub-task | [US02.03.01](#id-us02-03-01) | P0 | Add hostile Host/Origin integration cases | See [ST02.03.01.03](#id-st02-03-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST02.03.02.01 | Sub-task | [US02.03.02](#id-us02-03-02) | P0 | Choose body, session, concurrency, and queue defaults | See [ST02.03.02.01](#id-st02-03-02-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST02.03.02.02 | Sub-task | [US02.03.02](#id-us02-03-02) | P0 | Implement early size/rate/concurrency limiters | See [ST02.03.02.02](#id-st02-03-02-02) | Not applicable; see detail or parent section | 9h | finnhub-mcp; implementation | — | Not Started |
| ST02.03.02.03 | Sub-task | [US02.03.02](#id-us02-03-02) | P0 | Run overload and rejection-behavior tests | See [ST02.03.02.03](#id-st02-03-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; performance-testing | — | Not Started |
| ST02.04.01.01 | Sub-task | [US02.04.01](#id-us02-04-01) | P0 | Create immutable tenant execution context | See [ST02.04.01.01](#id-st02-04-01-01) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST02.04.01.02 | Sub-task | [US02.04.01](#id-us02-04-01) | P0 | Apply tenant and credential partition keys across state stores | See [ST02.04.01.02](#id-st02-04-01-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST02.04.01.03 | Sub-task | [US02.04.01](#id-us02-04-01) | P0 | Build two-tenant isolation tests | See [ST02.04.01.03](#id-st02-04-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; security-testing | — | Not Started |
| ST02.04.02.01 | Sub-task | [US02.04.02](#id-us02-04-02) | P1 | Document insider/raw-data classification and licensing rules | See [ST02.04.02.01](#id-st02-04-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; governance | — | Not Started |
| ST02.04.02.02 | Sub-task | [US02.04.02](#id-us02-04-02) | P1 | Implement field-level suppress/aggregate/deny projections | See [ST02.04.02.02](#id-st02-04-02-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST02.04.02.03 | Sub-task | [US02.04.02](#id-us02-04-02) | P1 | Test raw/full entitlement and history privacy | See [ST02.04.02.03](#id-st02-04-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST02.04.03.01 | Sub-task | [US02.04.03](#id-us02-04-03) | P0 | Define audit event schema, retention, and access policy | See [ST02.04.03.01](#id-st02-04-03-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST02.04.03.02 | Sub-task | [US02.04.03](#id-us02-04-03) | P0 | Emit structured audit events at authorization and tool boundaries | See [ST02.04.03.02](#id-st02-04-03-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST02.04.03.03 | Sub-task | [US02.04.03](#id-us02-04-03) | P0 | Add secret/raw-content log scanning tests | See [ST02.04.03.03](#id-st02-04-03-03) | Not applicable; see detail or parent section | 7h | finnhub-mcp; security-testing | — | Not Started |
| ST02.05.01.01 | Sub-task | [US02.05.01](#id-us02-05-01) | P1 | Define canonical validators and maximum sizes by input type | See [ST02.05.01.01](#id-st02-05-01-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST02.05.01.02 | Sub-task | [US02.05.01](#id-us02-05-01) | P1 | Replace regex-only natural-query validation | See [ST02.05.01.02](#id-st02-05-01-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST02.05.01.03 | Sub-task | [US02.05.01](#id-us02-05-01) | P1 | Add fuzz/property tests and safe error rendering | See [ST02.05.01.03](#id-st02-05-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; security-testing | — | Not Started |
| ST02.05.02.01 | Sub-task | [US02.05.02](#id-us02-05-02) | P1 | Implement provider URL allow/deny validation | See [ST02.05.02.01](#id-st02-05-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; security | — | Not Started |
| ST02.05.02.02 | Sub-task | [US02.05.02](#id-us02-05-02) | P1 | Annotate and bound untrusted provider text | See [ST02.05.02.02](#id-st02-05-02-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST02.05.02.03 | Sub-task | [US02.05.02](#id-us02-05-02) | P1 | Test malicious links and instruction-like content | See [ST02.05.02.03](#id-st02-05-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; security-testing | — | Not Started |

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

- [ ] Create E02 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E02.
- [ ] Create all 10 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 31 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 16 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 47 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

