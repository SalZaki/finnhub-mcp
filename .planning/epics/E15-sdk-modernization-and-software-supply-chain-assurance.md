---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E15
title: "SDK Modernization and Software Supply-Chain Assurance"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 9
  subtasks: 27
  traceability_owned: 7
  traceability_items: 14
story_estimate_days: 40
subtask_estimate_hours: 292
---

<a id="id-e15"></a>
# E15 — SDK Modernization and Software Supply-Chain Assurance

This is the self-contained coding-agent backlog for E15. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E15 | P0 | 5 | 9 | 27 | 40 | 292 | M0 — Hardened Core | Not Started |

## 2. Epic Definition

**Objective:** Upgrade the MCP foundation safely and make every merge and release pass repeatable protocol, security, dependency, and artifact-integrity controls.

**Business value:** Prevents regressions and compromised releases, gives maintainers confidence to upgrade dependencies, and makes distributable artifacts suitable for production use.

**Exit criteria:**

- [ ] The MCP SDK upgrade is protected by real HTTP and STDIO protocol conformance tests and a documented rollback path.
- [ ] Dependency update, SAST, secret scanning, tests, build, coverage, and artifact-integrity checks are required release gates.
- [ ] Every release publishes checksums, an SBOM, provenance, and a scanned non-root container image.
- [ ] Installer fallbacks and configuration assets are covered by integrity verification on Windows, Linux, and macOS.
- [ ] Release automation cannot publish from a commit that has not passed the same reusable validation workflow as pull requests.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F15.01](#id-f15-01) | P1 | MCP SDK Upgrade and Compatibility | 2 | 10 | Not Started |
| [F15.02](#id-f15-02) | P0 | Protocol-Level Regression Testing | 1 | 5 | Not Started |
| [F15.03](#id-f15-03) | P0 | Automated Dependency and Security Analysis | 2 | 7 | Not Started |
| [F15.04](#id-f15-04) | P1 | Reproducible and Verifiable Artifacts | 3 | 13 | Not Started |
| [F15.05](#id-f15-05) | P0 | Release Gating and Branch Policy | 1 | 5 | Not Started |

<a id="id-f15-01"></a>
### F15.01 — MCP SDK Upgrade and Compatibility

- **Parent Epic:** [E15](#id-e15)
- **Priority:** P1
- **Status:** Not Started

**Description:** Move from the pinned MCP C# SDK baseline to the current supported release through an explicit compatibility and rollback plan.

**Expected outcome:** The server benefits from current protocol behavior without an uncontrolled transport or serialization regression.

**Stories:**

- [US15.01.01](#id-us15-01-01) — Define an MCP SDK migration and rollback plan (P1, 4d)
- [US15.01.02](#id-us15-01-02) — Upgrade and certify the MCP SDK (P1, 6d)

<a id="id-f15-02"></a>
### F15.02 — Protocol-Level Regression Testing

- **Parent Epic:** [E15](#id-e15)
- **Priority:** P0
- **Status:** Not Started

**Description:** Replace misleading direct-tool smoke confidence with client-visible HTTP and STDIO lifecycle tests.

**Expected outcome:** CI detects route, initialization, listing, invocation, serialization, middleware, CORS, authentication, resource, and prompt regressions.

**Stories:**

- [US15.02.01](#id-us15-02-01) — Test the real HTTP and STDIO MCP lifecycle (P0, 5d)

<a id="id-f15-03"></a>
### F15.03 — Automated Dependency and Security Analysis

- **Parent Epic:** [E15](#id-e15)
- **Priority:** P0
- **Status:** Not Started

**Description:** Automate dependency updates, source analysis, secret detection, vulnerability handling, and disclosure policy.

**Expected outcome:** Known dependency, code, or credential risks are discovered before release and handled through a documented process.

**Stories:**

- [US15.03.01](#id-us15-03-01) — Automate dependency updates across all ecosystems (P1, 3d)
- [US15.03.02](#id-us15-03-02) — Add code, secret, and disclosure security controls (P0, 4d)

<a id="id-f15-04"></a>
### F15.04 — Reproducible and Verifiable Artifacts

- **Parent Epic:** [E15](#id-e15)
- **Priority:** P1
- **Status:** Not Started

**Description:** Produce SBOMs, provenance, checksums, hardened containers, and verified cross-platform installers.

**Expected outcome:** Consumers can verify what was built, from which source, and whether distributed files were altered.

**Stories:**

- [US15.04.01](#id-us15-04-01) — Publish SBOM, checksums, and build provenance (P1, 4d)
- [US15.04.02](#id-us15-04-02) — Ship a hardened multi-platform container image (P1, 5d)
- [US15.04.03](#id-us15-04-03) — Verify cross-platform installers and fallback downloads (P0, 4d)

<a id="id-f15-05"></a>
### F15.05 — Release Gating and Branch Policy

- **Parent Epic:** [E15](#id-e15)
- **Priority:** P0
- **Status:** Not Started

**Description:** Make reusable validation and protected-branch requirements authoritative for pull requests and release automation.

**Expected outcome:** No release can bypass the quality and security checks required of normal changes.

**Stories:**

- [US15.05.01](#id-us15-05-01) — Make one reusable validation workflow a release gate (P0, 5d)

## 4. User Stories and Subtasks

<a id="id-us15-01-01"></a>
### US15.01.01 — Define an MCP SDK migration and rollback plan

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.01](#id-f15-01) | P1 | 4 | 26 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Platform maintainer

**User story:** As a platform maintainer, I want an evidence-based SDK migration plan so that updating from the pinned release does not silently change routes or protocol behavior.

**Acceptance criteria:**

- [ ] The plan inventories current SDK v1.4 usage, generated schemas, MapMcp routing, stateful/stateless session behavior, tool wrapping, structured content, resources, prompts, logging, cancellation, and test helpers.
- [ ] A version comparison records breaking changes through the selected current supported SDK release, including protocol revision and supported .NET runtime requirements.
- [ ] A compatibility matrix names supported clients and verifies initialize, notifications, tools, resources, prompts, errors, cancellation, and HTTP session behavior before and after upgrade.
- [ ] The rollout uses a feature branch, pinned package version, lockfile diff, canary artifacts, documented rollback command, and no simultaneous unrelated transport redesign.
- [ ] An ADR records the selected target, rejected alternatives, known risks, and exit metrics.

**Dependencies:** [US15.02.01](#id-us15-02-01)

**Labels:** `mcp-sdk` `upgrade` `compatibility` `adr`

**Source findings:**

- The repository pins MCP C# SDK 1.4 while the reviewed current official SDK is materially newer.
- Upgrade should occur only after real protocol tests establish the existing behavior and desired endpoint contract.

**Subtasks:**

<a id="id-st15-01-01-01"></a>
- [ ] **ST15.01.01.01 — Inventory SDK integration and current behavior**
  - Type: analysis
  - Estimate: 10 hours
  - Suggested owner role: Senior C# engineer
  - Deliverable/evidence: v1.4 usage map and behavioral baseline for routes, sessions, schemas, wrapping, content, resources, prompts, and cancellation.
  - Status: Not Started
<a id="id-st15-01-01-02"></a>
- [ ] **ST15.01.01.02 — Analyze target SDK changes and client matrix**
  - Type: research
  - Estimate: 10 hours
  - Suggested owner role: MCP protocol engineer
  - Deliverable/evidence: Breaking-change register, target recommendation, client/runtime matrix, and performance thresholds.
  - Status: Not Started
<a id="id-st15-01-01-03"></a>
- [ ] **ST15.01.01.03 — Approve migration ADR and rollback plan**
  - Type: governance
  - Estimate: 6 hours
  - Suggested owner role: Solution architect
  - Deliverable/evidence: ADR, isolated rollout plan, canary and rollback procedure, and exit metrics.
  - Status: Not Started

<a id="id-us15-01-02"></a>
### US15.01.02 — Upgrade and certify the MCP SDK

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.01](#id-f15-01) | P1 | 6 | 48 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As a client developer, I want the server on a supported SDK with verified compatibility so that protocol behavior is current and predictable.

**Acceptance criteria:**

- [ ] The selected MCP SDK version is pinned and all compile, analyzer, deprecation, serialization, registration, routing, resource, and prompt changes are resolved explicitly.
- [ ] HTTP and STDIO conformance suites pass against all supported clients and no endpoint path, capability, schema, error, or output-shape change is undocumented.
- [ ] Structured content and output schemas are enabled or a documented compatibility rationale and deprecation plan is approved.
- [ ] Performance and allocation baselines for initialize, tools/list, and representative tools/call show no unapproved regression above the documented threshold.
- [ ] Release notes and compatibility documentation contain migration instructions, known issues, and rollback version.

**Dependencies:** [US15.01.01](#id-us15-01-01), [US15.02.01](#id-us15-02-01), [US15.05.01](#id-us15-05-01)

**Labels:** `mcp-sdk` `upgrade` `protocol` `performance`

**Source findings:**

- A controlled SDK upgrade is needed after transport and protocol E2E coverage.
- Current wrapped tools default structured content off and can return domain failures as protocol successes.

**Subtasks:**

<a id="id-st15-01-02-01"></a>
- [ ] **ST15.01.02.01 — Upgrade SDK and adapt registrations/contracts**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Senior C# engineer
  - Deliverable/evidence: Pinned upgraded SDK with explicit route, session, schema, structured output, and error behavior.
  - Status: Not Started
<a id="id-st15-01-02-02"></a>
- [ ] **ST15.01.02.02 — Run compatibility and performance certification**
  - Type: test
  - Estimate: 16 hours
  - Suggested owner role: Performance test engineer
  - Deliverable/evidence: Passing client/transport matrix and approved latency/allocation comparison.
  - Status: Not Started
<a id="id-st15-01-02-03"></a>
- [ ] **ST15.01.02.03 — Publish migration and rollback notes**
  - Type: documentation
  - Estimate: 8 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Release/compatibility documentation with breaking changes, known issues, and rollback version.
  - Status: Not Started

<a id="id-us15-02-01"></a>
### US15.02.01 — Test the real HTTP and STDIO MCP lifecycle

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.02](#id-f15-02) | P0 | 5 | 44 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Release engineer

**User story:** As a release engineer, I want client-visible protocol tests so that a green build proves the server a user launches can initialize and use every advertised surface.

**Acceptance criteria:**

- [ ] Tests launch the built server process and use an official or independent MCP client over STDIO and Streamable HTTP rather than constructing tool classes directly.
- [ ] Both transports verify initialize negotiation, initialized notification, tools/list, representative tools/call success and domain failure, resources/list/read, prompts/list/get, cancellation, shutdown, and deterministic serialization.
- [ ] HTTP tests verify the documented endpoint has no route ambiguity and cover GET/POST/DELETE or session behavior as applicable, Origin/Host/CORS policy, authentication profile, content type, body limit, invalid session, and concurrent calls.
- [ ] The suite asserts tool/resource/prompt counts and names, input and output schema shape, structured content, IsError behavior, next_actions argument validity, and absence of secrets or raw request bodies in logs.
- [ ] The old direct-tool live smoke is renamed as an application integration test and no longer serves as evidence for transport or protocol compatibility.

**Dependencies:** [US01.01.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-01), [US01.01.03](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-03), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02), [US01.03.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-03-01)

**Labels:** `e2e` `stdio` `streamable-http` `protocol` `p0`

**Source findings:**

- Existing live smoke boots the host but calls tools directly, bypassing transport, initialize/list, serialization, middleware, CORS, auth, and routing.
- Program.cs has documented/custom endpoint behavior that needs a real client to catch.
- Program and DI bootstrap are effectively outside meaningful current coverage.

**Subtasks:**

<a id="id-st15-02-01-01"></a>
- [ ] **ST15.02.01.01 — Build process-level STDIO protocol suite**
  - Type: test
  - Estimate: 14 hours
  - Suggested owner role: Test automation engineer
  - Deliverable/evidence: STDIO lifecycle tests for initialization, listings, calls, failures, cancellation, and shutdown.
  - Status: Not Started
<a id="id-st15-02-01-02"></a>
- [ ] **ST15.02.01.02 — Build hosted Streamable HTTP protocol suite**
  - Type: test
  - Estimate: 18 hours
  - Suggested owner role: Test automation engineer
  - Deliverable/evidence: HTTP lifecycle, endpoint, method/session, content, origin/host/CORS/auth/body/concurrency tests.
  - Status: Not Started
<a id="id-st15-02-01-03"></a>
- [ ] **ST15.02.01.03 — Assert catalogue, schema, error, and log contracts**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: MCP protocol engineer
  - Deliverable/evidence: Cross-transport snapshots for counts, schemas, structured content, IsError, next-actions, and redaction.
  - Status: Not Started

<a id="id-us15-03-01"></a>
### US15.03.01 — Automate dependency updates across all ecosystems

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.03](#id-f15-03) | P1 | 3 | 19 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Dependency maintainer

**User story:** As a dependency maintainer, I want bounded automated update pull requests so that NuGet, npm, Actions, and container dependencies do not age unnoticed.

**Acceptance criteria:**

- [ ] Dependabot or equivalent is configured for NuGet, npm, GitHub Actions, and Docker with weekly cadence, grouped patch/minor updates, and separate high-risk SDK/runtime majors.
- [ ] Pull requests include release notes, compatibility risk labels, dependency-review output, and execute the complete reusable validation workflow.
- [ ] Automerge is limited to approved low-risk patch groups after tests, analysis, license policy, and vulnerability checks pass; major MCP SDK and .NET updates require human approval.
- [ ] A dependency inventory records owner, update policy, support window, license, and exception expiry for every direct production dependency.
- [ ] Stale or vulnerable dependency SLAs are reported on the security dashboard without exposing private advisory details.

**Dependencies:** [US15.05.01](#id-us15-05-01)

**Labels:** `dependabot` `dependencies` `nuget` `npm` `github-actions`

**Source findings:**

- Actions are strongly SHA-pinned and npm release provenance exists, but NuGet and npm dependency update automation should be added.
- SDK/runtime major updates need different review from routine patches.

**Subtasks:**

<a id="id-st15-03-01-01"></a>
- [ ] **ST15.03.01.01 — Configure multi-ecosystem Dependabot**
  - Type: automation
  - Estimate: 6 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: NuGet, npm, Actions, and Docker update configuration with grouping and cadence.
  - Status: Not Started
<a id="id-st15-03-01-02"></a>
- [ ] **ST15.03.01.02 — Add dependency review, license policy, and ownership**
  - Type: security-automation
  - Estimate: 8 hours
  - Suggested owner role: Supply-chain security engineer
  - Deliverable/evidence: Blocking review policy, dependency inventory, owner/support window, and exception process.
  - Status: Not Started
<a id="id-st15-03-01-03"></a>
- [ ] **ST15.03.01.03 — Define safe update and automerge policy**
  - Type: governance
  - Estimate: 5 hours
  - Suggested owner role: Release manager
  - Deliverable/evidence: Risk-tiered update, automerge, vulnerability SLA, and major SDK/runtime approval rules.
  - Status: Not Started

<a id="id-us15-03-02"></a>
### US15.03.02 — Add code, secret, and disclosure security controls

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.03](#id-f15-03) | P0 | 4 | 26 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Security maintainer

**User story:** As a security maintainer, I want automated analysis and a clear disclosure path so that vulnerabilities and exposed credentials are caught and handled before publication.

**Acceptance criteria:**

- [ ] CodeQL scans C# and website JavaScript/TypeScript on pull requests, protected branches, and a weekly schedule with blocking severity policy and documented suppressions.
- [ ] Secret scanning with push protection or an equivalent scanner covers source, history, fixtures, docs, build output, logs, and example environment files using test-token allowlists only.
- [ ] SECURITY.md defines supported versions, private reporting route, acknowledgement and remediation targets, disclosure process, scope, safe-harbor expectations, and credential-rotation procedure.
- [ ] Dependency review and license policy fail changes introducing denied licenses or known exploitable vulnerabilities above the approved threshold unless a time-bounded exception is recorded.
- [ ] Security findings are access-controlled, assigned an owner and SLA, and must be resolved or explicitly waived before release.

**Dependencies:** [US15.05.01](#id-us15-05-01)

**Labels:** `codeql` `secret-scanning` `security-policy` `p0`

**Source findings:**

- CodeQL, secret scanning, and a SECURITY policy are missing from an otherwise strong pinned-action release setup.
- The repository handles a Finnhub credential and publishes examples, fixtures, logs, and packages that require scanning.

**Subtasks:**

<a id="id-st15-03-02-01"></a>
- [ ] **ST15.03.02.01 — Configure CodeQL and dependency security gates**
  - Type: security-automation
  - Estimate: 10 hours
  - Suggested owner role: Application security engineer
  - Deliverable/evidence: C#/JavaScript CodeQL, severity gates, schedule, suppression, and vulnerability/license review.
  - Status: Not Started
<a id="id-st15-03-02-02"></a>
- [ ] **ST15.03.02.02 — Configure secret scanning and push protection**
  - Type: security-automation
  - Estimate: 8 hours
  - Suggested owner role: Security engineer
  - Deliverable/evidence: Secret detection across history, source, fixtures, docs, outputs, logs, and example configs.
  - Status: Not Started
<a id="id-st15-03-02-03"></a>
- [ ] **ST15.03.02.03 — Publish SECURITY policy and triage workflow**
  - Type: security-governance
  - Estimate: 8 hours
  - Suggested owner role: Security maintainer
  - Deliverable/evidence: Supported-version, private-reporting, SLA, disclosure, safe-harbor, rotation, ownership, and waiver policy.
  - Status: Not Started

<a id="id-us15-04-01"></a>
### US15.04.01 — Publish SBOM, checksums, and build provenance

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.04](#id-f15-04) | P1 | 4 | 28 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Artifact consumer

**User story:** As an artifact consumer, I want verifiable release metadata so that I can audit dependencies and confirm binaries came from the tagged source.

**Acceptance criteria:**

- [ ] Every release generates CycloneDX or SPDX SBOMs for .NET, website/npm, and container contents and attaches them to the release.
- [ ] SHA-256 checksums cover all binaries, archives, installers, appsettings/configuration assets, SBOMs, and container digest manifests in one signed manifest.
- [ ] Build provenance identifies repository, immutable commit, workflow, runner, inputs, dependency lock state, artifact digests, and reproducible build command using an approved attestation format.
- [ ] npm provenance remains enabled and equivalent attestations are published for GitHub release and container artifacts.
- [ ] A verification document and CI smoke job prove a consumer can validate signature, checksum, provenance subject, and SBOM-to-artifact relationship.

**Dependencies:** [US15.05.01](#id-us15-05-01)

**Labels:** `sbom` `provenance` `checksums` `supply-chain`

**Source findings:**

- The release uses pinned Actions, lock/checksum controls, and npm provenance but lacks a complete SBOM and provenance set.
- appsettings is not currently included in integrity checks.

**Subtasks:**

<a id="id-st15-04-01-01"></a>
- [ ] **ST15.04.01.01 — Generate multi-ecosystem SBOMs**
  - Type: supply-chain
  - Estimate: 8 hours
  - Suggested owner role: Build engineer
  - Deliverable/evidence: CycloneDX/SPDX documents for .NET, npm/site, and container artifacts.
  - Status: Not Started
<a id="id-st15-04-01-02"></a>
- [ ] **ST15.04.01.02 — Create signed checksum and provenance manifests**
  - Type: supply-chain
  - Estimate: 12 hours
  - Suggested owner role: Supply-chain security engineer
  - Deliverable/evidence: Signed complete artifact manifest and build attestations tied to commit and lock state.
  - Status: Not Started
<a id="id-st15-04-01-03"></a>
- [ ] **ST15.04.01.03 — Test consumer artifact verification**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Release engineer
  - Deliverable/evidence: Verification guide and CI proof for signature, checksum, provenance, and SBOM relationships.
  - Status: Not Started

<a id="id-us15-04-02"></a>
### US15.04.02 — Ship a hardened multi-platform container image

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.04](#id-f15-04) | P1 | 5 | 36 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Container operator

**User story:** As a container operator, I want a minimal, signed image so that I can deploy the server without maintaining my own insecure packaging layer.

**Acceptance criteria:**

- [ ] A multi-stage Dockerfile produces linux/amd64 and linux/arm64 images with pinned digest bases, non-root user, read-only root compatibility, explicit writable temp path, no shell or build tool in runtime, and a documented health endpoint.
- [ ] The image accepts secrets only at runtime, contains no .env/user-secret/build credentials, exposes only the documented port, and defaults to the safest applicable HTTP profile.
- [ ] CI scans OS and application packages, generates an image SBOM, fails on policy-threshold vulnerabilities, and records time-bounded exceptions.
- [ ] Release publishes immutable semantic-version and digest references, signs the manifest, and never relies on mutable latest in deployment examples.
- [ ] A smoke test runs initialize, tools/list, one fixture-backed tools/call, graceful shutdown, and non-root/read-only checks on both target architectures where runners permit.

**Dependencies:** [US15.02.01](#id-us15-02-01), [US15.04.01](#id-us15-04-01), [US02.03.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-01), [US02.03.02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-02), [US03.03.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-03-01)

**Labels:** `container` `docker` `hardening` `multi-arch`

**Source findings:**

- A production container artifact and scan/signing controls are missing.
- Hosted deployment requires safe defaults and real protocol smoke verification.

**Subtasks:**

<a id="id-st15-04-02-01"></a>
- [ ] **ST15.04.02.01 — Create minimal non-root multi-stage image**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: Container engineer
  - Deliverable/evidence: Pinned-digest multi-arch runtime image compatible with non-root and read-only operation.
  - Status: Not Started
<a id="id-st15-04-02-02"></a>
- [ ] **ST15.04.02.02 — Add container scan, SBOM, signing, and publishing**
  - Type: supply-chain
  - Estimate: 12 hours
  - Suggested owner role: DevSecOps engineer
  - Deliverable/evidence: Scanned, SBOM-attached, signed immutable manifests for release tags and digests.
  - Status: Not Started
<a id="id-st15-04-02-03"></a>
- [ ] **ST15.04.02.03 — Run multi-architecture security and protocol smoke**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: Test engineer
  - Deliverable/evidence: Non-root/read-only/no-secret/port and initialize/list/call/shutdown tests.
  - Status: Not Started

<a id="id-us15-04-03"></a>
### US15.04.03 — Verify cross-platform installers and fallback downloads

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.04](#id-f15-04) | P0 | 4 | 25 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Desktop installer user

**User story:** As a desktop user, I want installers and fallback downloads validated on my operating system so that installation does not fail or accept an unverified file.

**Acceptance criteria:**

- [ ] Windows install fallback resolves the actual .exe artifact name and extension and is exercised in a clean Windows CI environment.
- [ ] Linux, macOS, and Windows installation paths download only version-pinned artifacts and verify the signed checksum manifest before execution or extraction.
- [ ] All bundled and separately downloaded configuration files, including appsettings, are checksummed and validated before use.
- [ ] Install, upgrade, downgrade, uninstall, path-with-spaces, and no-admin scenarios are smoke-tested with logs redacted of credentials and user paths where feasible.
- [ ] A release is blocked when an installer URL, archive member, executable bit, checksum, signature, or post-install STDIO initialize check fails.

**Dependencies:** [US15.04.01](#id-us15-04-01), [US15.02.01](#id-us15-02-01)

**Labels:** `installer` `windows` `integrity` `p0`

**Source findings:**

- The Windows installer fallback URL omits the .exe suffix.
- appsettings is omitted from checksum coverage.
- Cross-platform release verification should test the installed server rather than only archive creation.

**Subtasks:**

<a id="id-st15-04-03-01"></a>
- [ ] **ST15.04.03.01 — Repair Windows fallback artifact resolution**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: Release engineer
  - Deliverable/evidence: Version-pinned .exe fallback URL and verified Windows installation path.
  - Status: Not Started
<a id="id-st15-04-03-02"></a>
- [ ] **ST15.04.03.02 — Extend integrity manifest to all distributed files**
  - Type: supply-chain
  - Estimate: 6 hours
  - Suggested owner role: Build engineer
  - Deliverable/evidence: Checksums and signature verification covering appsettings and every installer/archive member.
  - Status: Not Started
<a id="id-st15-04-03-03"></a>
- [ ] **ST15.04.03.03 — Build cross-platform lifecycle tests**
  - Type: test
  - Estimate: 14 hours
  - Suggested owner role: Test automation engineer
  - Deliverable/evidence: Clean Windows/Linux/macOS install, upgrade, downgrade, uninstall, space-path, and STDIO initialize gates.
  - Status: Not Started

<a id="id-us15-05-01"></a>
### US15.05.01 — Make one reusable validation workflow a release gate

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F15.05](#id-f15-05) | P0 | 5 | 40 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Release manager

**User story:** As a release manager, I want release automation to consume the same required validation as pull requests so that release-please cannot publish an untested commit.

**Acceptance criteria:**

- [ ] A reusable workflow performs restore with locked dependencies, format/analyzers, build, unit/integration/protocol tests, coverage thresholds, docs/schema diff, website checks, dependency review, CodeQL, secret scan, package validation, and artifact smoke tests.
- [ ] Pull requests, pushes to protected branches, release-please, manual release, container publication, and website production deployment all call or verify the immutable result of that workflow.
- [ ] Branch protection requires named validation checks, signed or approved commits as policy dictates, review for code-owner areas, resolved conversations, and no force-push to release branches.
- [ ] Release jobs verify that the exact commit and dependency lock state passed validation and cannot rebuild from a moving branch or publish when a required check is skipped or neutral.
- [ ] A deliberately failing canary proves each gate blocks publication, and rollback/revocation procedures are exercised at least twice yearly.

**Dependencies:** —

**Labels:** `ci-cd` `release-please` `branch-protection` `p0`

**Source findings:**

- The release workflow can be independent of test success unless branch protection is configured correctly.
- Release automation should consume a reusable validation workflow rather than assume another workflow ran.
- Existing SHA-pinned Actions and npm provenance are strong foundations to retain.

**Subtasks:**

<a id="id-st15-05-01-01"></a>
- [ ] **ST15.05.01.01 — Implement reusable complete validation workflow**
  - Type: devops
  - Estimate: 20 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: One versioned workflow for code, protocol, docs, web, security, package, and artifact validation.
  - Status: Not Started
<a id="id-st15-05-01-02"></a>
- [ ] **ST15.05.01.02 — Enforce protected branch and exact-commit release policy**
  - Type: governance
  - Estimate: 10 hours
  - Suggested owner role: Repository administrator
  - Deliverable/evidence: Required checks, code owners, no bypass, exact SHA/lock verification, and protected release branches.
  - Status: Not Started
<a id="id-st15-05-01-03"></a>
- [ ] **ST15.05.01.03 — Exercise failure gates and rollback**
  - Type: resilience-test
  - Estimate: 10 hours
  - Suggested owner role: Release manager
  - Deliverable/evidence: Canary evidence that every failed/neutral check blocks publication plus rehearsed revocation and rollback record.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST15.01.01.01](#id-st15-01-01-01) | [US15.01.01](#id-us15-01-01) | P1 | Inventory SDK integration and current behavior | analysis | 10 | Senior C# engineer | v1.4 usage map and behavioral baseline for routes, sessions, schemas, wrapping, content, resources, prompts, and cancellation. | Not Started |
| [ST15.01.01.02](#id-st15-01-01-02) | [US15.01.01](#id-us15-01-01) | P1 | Analyze target SDK changes and client matrix | research | 10 | MCP protocol engineer | Breaking-change register, target recommendation, client/runtime matrix, and performance thresholds. | Not Started |
| [ST15.01.01.03](#id-st15-01-01-03) | [US15.01.01](#id-us15-01-01) | P1 | Approve migration ADR and rollback plan | governance | 6 | Solution architect | ADR, isolated rollout plan, canary and rollback procedure, and exit metrics. | Not Started |
| [ST15.01.02.01](#id-st15-01-02-01) | [US15.01.02](#id-us15-01-02) | P1 | Upgrade SDK and adapt registrations/contracts | implementation | 24 | Senior C# engineer | Pinned upgraded SDK with explicit route, session, schema, structured output, and error behavior. | Not Started |
| [ST15.01.02.02](#id-st15-01-02-02) | [US15.01.02](#id-us15-01-02) | P1 | Run compatibility and performance certification | test | 16 | Performance test engineer | Passing client/transport matrix and approved latency/allocation comparison. | Not Started |
| [ST15.01.02.03](#id-st15-01-02-03) | [US15.01.02](#id-us15-01-02) | P1 | Publish migration and rollback notes | documentation | 8 | Technical writer | Release/compatibility documentation with breaking changes, known issues, and rollback version. | Not Started |
| [ST15.02.01.01](#id-st15-02-01-01) | [US15.02.01](#id-us15-02-01) | P0 | Build process-level STDIO protocol suite | test | 14 | Test automation engineer | STDIO lifecycle tests for initialization, listings, calls, failures, cancellation, and shutdown. | Not Started |
| [ST15.02.01.02](#id-st15-02-01-02) | [US15.02.01](#id-us15-02-01) | P0 | Build hosted Streamable HTTP protocol suite | test | 18 | Test automation engineer | HTTP lifecycle, endpoint, method/session, content, origin/host/CORS/auth/body/concurrency tests. | Not Started |
| [ST15.02.01.03](#id-st15-02-01-03) | [US15.02.01](#id-us15-02-01) | P0 | Assert catalogue, schema, error, and log contracts | test | 12 | MCP protocol engineer | Cross-transport snapshots for counts, schemas, structured content, IsError, next-actions, and redaction. | Not Started |
| [ST15.03.01.01](#id-st15-03-01-01) | [US15.03.01](#id-us15-03-01) | P1 | Configure multi-ecosystem Dependabot | automation | 6 | DevOps engineer | NuGet, npm, Actions, and Docker update configuration with grouping and cadence. | Not Started |
| [ST15.03.01.02](#id-st15-03-01-02) | [US15.03.01](#id-us15-03-01) | P1 | Add dependency review, license policy, and ownership | security-automation | 8 | Supply-chain security engineer | Blocking review policy, dependency inventory, owner/support window, and exception process. | Not Started |
| [ST15.03.01.03](#id-st15-03-01-03) | [US15.03.01](#id-us15-03-01) | P1 | Define safe update and automerge policy | governance | 5 | Release manager | Risk-tiered update, automerge, vulnerability SLA, and major SDK/runtime approval rules. | Not Started |
| [ST15.03.02.01](#id-st15-03-02-01) | [US15.03.02](#id-us15-03-02) | P0 | Configure CodeQL and dependency security gates | security-automation | 10 | Application security engineer | C#/JavaScript CodeQL, severity gates, schedule, suppression, and vulnerability/license review. | Not Started |
| [ST15.03.02.02](#id-st15-03-02-02) | [US15.03.02](#id-us15-03-02) | P0 | Configure secret scanning and push protection | security-automation | 8 | Security engineer | Secret detection across history, source, fixtures, docs, outputs, logs, and example configs. | Not Started |
| [ST15.03.02.03](#id-st15-03-02-03) | [US15.03.02](#id-us15-03-02) | P0 | Publish SECURITY policy and triage workflow | security-governance | 8 | Security maintainer | Supported-version, private-reporting, SLA, disclosure, safe-harbor, rotation, ownership, and waiver policy. | Not Started |
| [ST15.04.01.01](#id-st15-04-01-01) | [US15.04.01](#id-us15-04-01) | P1 | Generate multi-ecosystem SBOMs | supply-chain | 8 | Build engineer | CycloneDX/SPDX documents for .NET, npm/site, and container artifacts. | Not Started |
| [ST15.04.01.02](#id-st15-04-01-02) | [US15.04.01](#id-us15-04-01) | P1 | Create signed checksum and provenance manifests | supply-chain | 12 | Supply-chain security engineer | Signed complete artifact manifest and build attestations tied to commit and lock state. | Not Started |
| [ST15.04.01.03](#id-st15-04-01-03) | [US15.04.01](#id-us15-04-01) | P1 | Test consumer artifact verification | test | 8 | Release engineer | Verification guide and CI proof for signature, checksum, provenance, and SBOM relationships. | Not Started |
| [ST15.04.02.01](#id-st15-04-02-01) | [US15.04.02](#id-us15-04-02) | P1 | Create minimal non-root multi-stage image | implementation | 14 | Container engineer | Pinned-digest multi-arch runtime image compatible with non-root and read-only operation. | Not Started |
| [ST15.04.02.02](#id-st15-04-02-02) | [US15.04.02](#id-us15-04-02) | P1 | Add container scan, SBOM, signing, and publishing | supply-chain | 12 | DevSecOps engineer | Scanned, SBOM-attached, signed immutable manifests for release tags and digests. | Not Started |
| [ST15.04.02.03](#id-st15-04-02-03) | [US15.04.02](#id-us15-04-02) | P1 | Run multi-architecture security and protocol smoke | test | 10 | Test engineer | Non-root/read-only/no-secret/port and initialize/list/call/shutdown tests. | Not Started |
| [ST15.04.03.01](#id-st15-04-03-01) | [US15.04.03](#id-us15-04-03) | P0 | Repair Windows fallback artifact resolution | implementation | 5 | Release engineer | Version-pinned .exe fallback URL and verified Windows installation path. | Not Started |
| [ST15.04.03.02](#id-st15-04-03-02) | [US15.04.03](#id-us15-04-03) | P0 | Extend integrity manifest to all distributed files | supply-chain | 6 | Build engineer | Checksums and signature verification covering appsettings and every installer/archive member. | Not Started |
| [ST15.04.03.03](#id-st15-04-03-03) | [US15.04.03](#id-us15-04-03) | P0 | Build cross-platform lifecycle tests | test | 14 | Test automation engineer | Clean Windows/Linux/macOS install, upgrade, downgrade, uninstall, space-path, and STDIO initialize gates. | Not Started |
| [ST15.05.01.01](#id-st15-05-01-01) | [US15.05.01](#id-us15-05-01) | P0 | Implement reusable complete validation workflow | devops | 20 | DevOps engineer | One versioned workflow for code, protocol, docs, web, security, package, and artifact validation. | Not Started |
| [ST15.05.01.02](#id-st15-05-01-02) | [US15.05.01](#id-us15-05-01) | P0 | Enforce protected branch and exact-commit release policy | governance | 10 | Repository administrator | Required checks, code owners, no bypass, exact SHA/lock verification, and protected release branches. | Not Started |
| [ST15.05.01.03](#id-st15-05-01-03) | [US15.05.01](#id-us15-05-01) | P0 | Exercise failure gates and rollback | resilience-test | 10 | Release manager | Canary evidence that every failed/neutral check blocks publication plus rehearsed revocation and rollback record. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E15 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| I-01 | I. Prioritisation | Maintain a P0/P1/P2 roadmap with estimates, dependencies, milestones, and explicit release exit criteria. | [US12.04.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-04-02), [US15.05.01](#id-us15-05-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Explicit review question I. |
| R-24 | Repository finding | Replace direct-construction 'live smoke' coverage with real SDK client initialization/list/call/resource/prompt transport tests and correct route/streaming/field claims. | [US01.03.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-03-01), [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US15.02.01](#id-us15-02-01) | [E01](./E01-mcp-transport-and-protocol-integrity.md#id-e01) | P0 | Covered | Testing/documentation finding. |
| R-25 | Repository finding | Upgrade the pinned MCP C# SDK through a controlled compatibility program after protocol tests exist. | [US01.03.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-03-02), [US15.01.01](#id-us15-01-01), [US15.01.02](#id-us15-01-02) | [E01](./E01-mcp-transport-and-protocol-integrity.md#id-e01) | P1 | Covered | Dependency lifecycle finding. |
| R-26 | Repository finding | Add Dependabot, CodeQL, secret scanning, SBOM, SECURITY.md, container hardening, and release validation gates. | [US15.03.01](#id-us15-03-01), [US15.03.02](#id-us15-03-02), [US15.04.01](#id-us15-04-01), [US15.04.02](#id-us15-04-02), [US15.05.01](#id-us15-05-01) | [E15](#id-e15) | P0 | Covered | Supply-chain finding. |
| R-27 | Repository finding | Fix Windows install fallback asset naming and checksum all runtime configuration artifacts. | [US15.04.01](#id-us15-04-01), [US15.04.03](#id-us15-04-03) | [E15](#id-e15) | P0 | Covered | Distribution finding. |
| RF-141 | Code-review detail | README transport route, SSE/streaming, and SDK mapping claims are inaccurate or ambiguous. | [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US12.03.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-03-02), [US15.02.01](#id-us15-02-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-142 | Code-review detail | README overstates fields projection, limit behavior, confidence/exact match, lazy schemas, full/raw views, and live-smoke coverage. | [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01), [US15.02.01](#id-us15-02-01), [US14.02.01](./E14-ecosystem-positioning-adoption-and-community.md#id-us14-02-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-150 | Code-review detail | Contributor release-branch guidance drifts and ignored .planning files are treated as authoritative. | [US12.04.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-04-02), [US15.05.01](#id-us15-05-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-163 | Code-review detail | MCP SDK is pinned to v1.4 and should be upgraded only after protocol baselines and through controlled compatibility review. | [US15.01.01](#id-us15-01-01), [US15.01.02](#id-us15-01-02), [US15.02.01](#id-us15-02-01) | [E15](#id-e15) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-164 | Code-review detail | Current live smoke bypasses real transport, initialization, listings, serialization, middleware, CORS, auth, route, resource, and prompt behavior. | [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US15.02.01](#id-us15-02-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-165 | Code-review detail | NuGet/npm dependency automation, CodeQL, secret scanning, security policy, and license/vulnerability gates are missing. | [US15.03.01](#id-us15-03-01), [US15.03.02](#id-us15-03-02) | [E15](#id-e15) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-166 | Code-review detail | Releases need SBOMs, provenance, complete checksums, and a hardened signed container while retaining SHA-pinned Actions and npm provenance. | [US15.04.01](#id-us15-04-01), [US15.04.02](#id-us15-04-02) | [E15](#id-e15) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-167 | Code-review detail | Windows fallback omits .exe and appsettings lacks checksum coverage. | [US15.04.01](#id-us15-04-01), [US15.04.03](#id-us15-04-03) | [E15](#id-e15) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-168 | Code-review detail | Release automation may bypass tests unless exact-commit reusable validation and branch protection are enforced. | [US15.05.01](#id-us15-05-01) | [E15](#id-e15) | P0 | Covered | Detailed finding retained from the repository review. |

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

### 4. M2 — Operability & Context

- **Priority:** P1
- **Indicative window:** Weeks 12–22
- **Primary epics:** E08, E10–E12, E14, E15
- **Goal:** Improve discovery quality, observability, documentation, and delivery controls.
- **Entry criteria:** Stable tool schemas and telemetry taxonomy.
- **Exit criteria:** Golden intent evals run in CI; metrics/health are live; API docs are generated; release and supply-chain controls are enforced.
- **Delivery note:** Can proceed in parallel with later M1 stories.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E15 | Epic | — | P0 | SDK Modernization and Software Supply-Chain Assurance | See [E15](#id-e15) | See [E15](#id-e15) | — | finnhub-mcp; epic | — | Not Started |
| F15.01 | Feature | [E15](#id-e15) | P1 | MCP SDK Upgrade and Compatibility | See [F15.01](#id-f15-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e15 | — | Not Started |
| F15.02 | Feature | [E15](#id-e15) | P0 | Protocol-Level Regression Testing | See [F15.02](#id-f15-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e15 | — | Not Started |
| F15.03 | Feature | [E15](#id-e15) | P0 | Automated Dependency and Security Analysis | See [F15.03](#id-f15-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e15 | — | Not Started |
| F15.04 | Feature | [E15](#id-e15) | P1 | Reproducible and Verifiable Artifacts | See [F15.04](#id-f15-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e15 | — | Not Started |
| F15.05 | Feature | [E15](#id-e15) | P0 | Release Gating and Branch Policy | See [F15.05](#id-f15-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e15 | — | Not Started |
| US15.01.01 | Story | [F15.01](#id-f15-01) | P1 | Define an MCP SDK migration and rollback plan | See [US15.01.01](#id-us15-01-01) | See [US15.01.01](#id-us15-01-01) | 4d | mcp-sdk; upgrade; compatibility; adr | [US15.02.01](#id-us15-02-01) | Not Started |
| US15.01.02 | Story | [F15.01](#id-f15-01) | P1 | Upgrade and certify the MCP SDK | See [US15.01.02](#id-us15-01-02) | See [US15.01.02](#id-us15-01-02) | 6d | mcp-sdk; upgrade; protocol; performance | [US15.01.01](#id-us15-01-01), [US15.02.01](#id-us15-02-01), [US15.05.01](#id-us15-05-01) | Not Started |
| US15.02.01 | Story | [F15.02](#id-f15-02) | P0 | Test the real HTTP and STDIO MCP lifecycle | See [US15.02.01](#id-us15-02-01) | See [US15.02.01](#id-us15-02-01) | 5d | e2e; stdio; streamable-http; protocol; p0 | [US01.01.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-01), [US01.01.03](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-03), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02), [US01.03.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-03-01) | Not Started |
| US15.03.01 | Story | [F15.03](#id-f15-03) | P1 | Automate dependency updates across all ecosystems | See [US15.03.01](#id-us15-03-01) | See [US15.03.01](#id-us15-03-01) | 3d | dependabot; dependencies; nuget; npm; github-actions | [US15.05.01](#id-us15-05-01) | Not Started |
| US15.03.02 | Story | [F15.03](#id-f15-03) | P0 | Add code, secret, and disclosure security controls | See [US15.03.02](#id-us15-03-02) | See [US15.03.02](#id-us15-03-02) | 4d | codeql; secret-scanning; security-policy; p0 | [US15.05.01](#id-us15-05-01) | Not Started |
| US15.04.01 | Story | [F15.04](#id-f15-04) | P1 | Publish SBOM, checksums, and build provenance | See [US15.04.01](#id-us15-04-01) | See [US15.04.01](#id-us15-04-01) | 4d | sbom; provenance; checksums; supply-chain | [US15.05.01](#id-us15-05-01) | Not Started |
| US15.04.02 | Story | [F15.04](#id-f15-04) | P1 | Ship a hardened multi-platform container image | See [US15.04.02](#id-us15-04-02) | See [US15.04.02](#id-us15-04-02) | 5d | container; docker; hardening; multi-arch | [US15.02.01](#id-us15-02-01), [US15.04.01](#id-us15-04-01), [US02.03.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-01), [US02.03.02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-02), [US03.03.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-03-01) | Not Started |
| US15.04.03 | Story | [F15.04](#id-f15-04) | P0 | Verify cross-platform installers and fallback downloads | See [US15.04.03](#id-us15-04-03) | See [US15.04.03](#id-us15-04-03) | 4d | installer; windows; integrity; p0 | [US15.04.01](#id-us15-04-01), [US15.02.01](#id-us15-02-01) | Not Started |
| US15.05.01 | Story | [F15.05](#id-f15-05) | P0 | Make one reusable validation workflow a release gate | See [US15.05.01](#id-us15-05-01) | See [US15.05.01](#id-us15-05-01) | 5d | ci-cd; release-please; branch-protection; p0 | — | Not Started |
| ST15.01.01.01 | Sub-task | [US15.01.01](#id-us15-01-01) | P1 | Inventory SDK integration and current behavior | See [ST15.01.01.01](#id-st15-01-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; analysis | — | Not Started |
| ST15.01.01.02 | Sub-task | [US15.01.01](#id-us15-01-01) | P1 | Analyze target SDK changes and client matrix | See [ST15.01.01.02](#id-st15-01-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; research | — | Not Started |
| ST15.01.01.03 | Sub-task | [US15.01.01](#id-us15-01-01) | P1 | Approve migration ADR and rollback plan | See [ST15.01.01.03](#id-st15-01-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; governance | — | Not Started |
| ST15.01.02.01 | Sub-task | [US15.01.02](#id-us15-01-02) | P1 | Upgrade SDK and adapt registrations/contracts | See [ST15.01.02.01](#id-st15-01-02-01) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST15.01.02.02 | Sub-task | [US15.01.02](#id-us15-01-02) | P1 | Run compatibility and performance certification | See [ST15.01.02.02](#id-st15-01-02-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; test | — | Not Started |
| ST15.01.02.03 | Sub-task | [US15.01.02](#id-us15-01-02) | P1 | Publish migration and rollback notes | See [ST15.01.02.03](#id-st15-01-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; documentation | — | Not Started |
| ST15.02.01.01 | Sub-task | [US15.02.01](#id-us15-02-01) | P0 | Build process-level STDIO protocol suite | See [ST15.02.01.01](#id-st15-02-01-01) | Not applicable; see detail or parent section | 14h | finnhub-mcp; test | — | Not Started |
| ST15.02.01.02 | Sub-task | [US15.02.01](#id-us15-02-01) | P0 | Build hosted Streamable HTTP protocol suite | See [ST15.02.01.02](#id-st15-02-01-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; test | — | Not Started |
| ST15.02.01.03 | Sub-task | [US15.02.01](#id-us15-02-01) | P0 | Assert catalogue, schema, error, and log contracts | See [ST15.02.01.03](#id-st15-02-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |
| ST15.03.01.01 | Sub-task | [US15.03.01](#id-us15-03-01) | P1 | Configure multi-ecosystem Dependabot | See [ST15.03.01.01](#id-st15-03-01-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; automation | — | Not Started |
| ST15.03.01.02 | Sub-task | [US15.03.01](#id-us15-03-01) | P1 | Add dependency review, license policy, and ownership | See [ST15.03.01.02](#id-st15-03-01-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; security-automation | — | Not Started |
| ST15.03.01.03 | Sub-task | [US15.03.01](#id-us15-03-01) | P1 | Define safe update and automerge policy | See [ST15.03.01.03](#id-st15-03-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; governance | — | Not Started |
| ST15.03.02.01 | Sub-task | [US15.03.02](#id-us15-03-02) | P0 | Configure CodeQL and dependency security gates | See [ST15.03.02.01](#id-st15-03-02-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; security-automation | — | Not Started |
| ST15.03.02.02 | Sub-task | [US15.03.02](#id-us15-03-02) | P0 | Configure secret scanning and push protection | See [ST15.03.02.02](#id-st15-03-02-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; security-automation | — | Not Started |
| ST15.03.02.03 | Sub-task | [US15.03.02](#id-us15-03-02) | P0 | Publish SECURITY policy and triage workflow | See [ST15.03.02.03](#id-st15-03-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; security-governance | — | Not Started |
| ST15.04.01.01 | Sub-task | [US15.04.01](#id-us15-04-01) | P1 | Generate multi-ecosystem SBOMs | See [ST15.04.01.01](#id-st15-04-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; supply-chain | — | Not Started |
| ST15.04.01.02 | Sub-task | [US15.04.01](#id-us15-04-01) | P1 | Create signed checksum and provenance manifests | See [ST15.04.01.02](#id-st15-04-01-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; supply-chain | — | Not Started |
| ST15.04.01.03 | Sub-task | [US15.04.01](#id-us15-04-01) | P1 | Test consumer artifact verification | See [ST15.04.01.03](#id-st15-04-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST15.04.02.01 | Sub-task | [US15.04.02](#id-us15-04-02) | P1 | Create minimal non-root multi-stage image | See [ST15.04.02.01](#id-st15-04-02-01) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST15.04.02.02 | Sub-task | [US15.04.02](#id-us15-04-02) | P1 | Add container scan, SBOM, signing, and publishing | See [ST15.04.02.02](#id-st15-04-02-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; supply-chain | — | Not Started |
| ST15.04.02.03 | Sub-task | [US15.04.02](#id-us15-04-02) | P1 | Run multi-architecture security and protocol smoke | See [ST15.04.02.03](#id-st15-04-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST15.04.03.01 | Sub-task | [US15.04.03](#id-us15-04-03) | P0 | Repair Windows fallback artifact resolution | See [ST15.04.03.01](#id-st15-04-03-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST15.04.03.02 | Sub-task | [US15.04.03](#id-us15-04-03) | P0 | Extend integrity manifest to all distributed files | See [ST15.04.03.02](#id-st15-04-03-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; supply-chain | — | Not Started |
| ST15.04.03.03 | Sub-task | [US15.04.03](#id-us15-04-03) | P0 | Build cross-platform lifecycle tests | See [ST15.04.03.03](#id-st15-04-03-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; test | — | Not Started |
| ST15.05.01.01 | Sub-task | [US15.05.01](#id-us15-05-01) | P0 | Implement reusable complete validation workflow | See [ST15.05.01.01](#id-st15-05-01-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; devops | — | Not Started |
| ST15.05.01.02 | Sub-task | [US15.05.01](#id-us15-05-01) | P0 | Enforce protected branch and exact-commit release policy | See [ST15.05.01.02](#id-st15-05-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; governance | — | Not Started |
| ST15.05.01.03 | Sub-task | [US15.05.01](#id-us15-05-01) | P0 | Exercise failure gates and rollback | See [ST15.05.01.03](#id-st15-05-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; resilience-test | — | Not Started |

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

- [ ] Create E15 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E15.
- [ ] Create all 9 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 27 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 14 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 42 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

