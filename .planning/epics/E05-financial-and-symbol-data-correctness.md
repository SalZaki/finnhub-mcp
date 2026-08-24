---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E05
title: "Financial and Symbol Data Correctness"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 11
  subtasks: 34
  traceability_owned: 18
  traceability_items: 20
story_estimate_days: 32.5
subtask_estimate_hours: 205
---

<a id="id-e05"></a>
# E05 — Financial and Symbol Data Correctness

This is the self-contained coding-agent backlog for E05. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E05 | P0 | 5 | 11 | 34 | 32.5 | 205 | M0 — Hardened Core | Not Started |

## 2. Epic Definition

**Objective:** Correct search behavior and financial calculations so that every exposed field has accurate semantics, time windows, provenance, and deterministic behavior.

**Business value:** Prevents agents and users from making decisions from silently ignored parameters, invalid match confidence, or mislabeled financial signals.

**Exit criteria:**

- [ ] search-symbol honors limit and fields, scores candidates deterministically, and does not duplicate upstream work.
- [ ] Price volatility is defined and computed from returns or explicitly renamed as price dispersion.
- [ ] Insider signals distinguish open-market activity from other Form 4 transaction types and label the requested window correctly.
- [ ] News periods do not overlap and partial sentiment availability is explicitly reported.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F05.01](#id-f05-01) | P0 | Search Parameter Fidelity | 2 | 4.5 | Not Started |
| [F05.02](#id-f05-02) | P0 | Deterministic Symbol Resolution | 3 | 9.5 | Not Started |
| [F05.03](#id-f05-03) | P0 | Defensible Historical Price Statistics | 2 | 6 | Not Started |
| [F05.04](#id-f05-04) | P0 | Transaction-Aware Insider Signals | 2 | 6.5 | Not Started |
| [F05.05](#id-f05-05) | P1 | Non-Overlapping and Degradable News Pulse | 2 | 6 | Not Started |

<a id="id-f05-01"></a>
### F05.01 — Search Parameter Fidelity

- **Parent Epic:** [E05](#id-e05)
- **Priority:** P0
- **Status:** Not Started

**Description:** Make search-symbol limit and fields behavior truthful, validated, and observable.

**Expected outcome:** Every accepted parameter materially affects the response exactly as documented.

**Stories:**

- [US05.01.01](#id-us05-01-01) — Honor search limit and field projection (P0, 2.5d)
- [US05.01.02](#id-us05-01-02) — Separate cached provider candidates from per-call search metadata (P0, 2d)

<a id="id-f05-02"></a>
### F05.02 — Deterministic Symbol Resolution

- **Parent Epic:** [E05](#id-e05)
- **Priority:** P0
- **Status:** Not Started

**Description:** Calculate meaningful confidence and exact-match state across ticker, name, ISIN, CUSIP, exchange, and share-class inputs.

**Expected outcome:** Resolution and next actions use the best candidate once, with deterministic and explainable matching.

**Stories:**

- [US05.02.01](#id-us05-02-01) — Compute explainable confidence and exact-match values (P0, 4d)
- [US05.02.02](#id-us05-02-02) — Resolve exchange and share-class symbols without misparsing (P0, 3d)
- [US05.02.03](#id-us05-02-03) — Reuse ranked search results for resolution and next actions (P0, 2.5d)

<a id="id-f05-03"></a>
### F05.03 — Defensible Historical Price Statistics

- **Parent Epic:** [E05](#id-e05)
- **Priority:** P0
- **Status:** Not Started

**Description:** Define volatility correctly and validate candle data before computing aggregate price statistics.

**Expected outcome:** Price metrics have documented formulas, units, sample requirements, and safe behavior for malformed or sparse series.

**Stories:**

- [US05.03.01](#id-us05-03-01) — Define and calculate return-based volatility (P0, 3d)
- [US05.03.02](#id-us05-03-02) — Validate candle series before aggregating price statistics (P0, 3d)

<a id="id-f05-04"></a>
### F05.04 — Transaction-Aware Insider Signals

- **Parent Epic:** [E05](#id-e05)
- **Priority:** P0
- **Status:** Not Started

**Description:** Classify Form 4 activity and label signal windows dynamically rather than treating all signed changes as open-market trades.

**Expected outcome:** Insider signals reflect economically comparable activity and expose exclusions and completeness.

**Stories:**

- [US05.04.01](#id-us05-04-01) — Classify Form 4 transactions before computing an insider signal (P0, 4d)
- [US05.04.02](#id-us05-04-02) — Label insider periods and values from the actual request window (P0, 2.5d)

<a id="id-f05-05"></a>
### F05.05 — Non-Overlapping and Degradable News Pulse

- **Parent Epic:** [E05](#id-e05)
- **Priority:** P1
- **Status:** Not Started

**Description:** Use non-overlapping date periods, concurrency bounded by quota, and explicit fallback behavior when sentiment data is unavailable.

**Expected outcome:** News trends are mathematically comparable and partial provider capabilities do not silently distort the result.

**Stories:**

- [US05.05.01](#id-us05-05-01) — Use non-overlapping comparable news windows (P1, 2.5d)
- [US05.05.02](#id-us05-05-02) — Degrade news sentiment consistently under bounded concurrency (P1, 3.5d)

## 4. User Stories and Subtasks

<a id="id-us05-01-01"></a>
### US05.01.01 — Honor search limit and field projection

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.01](#id-f05-01) | P0 | 2.5 | 15 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP user

**User story:** As an MCP user, I want search-symbol limit and fields parameters to change the response as documented so that I can control relevance and token cost.

**Acceptance criteria:**

- [ ] The normalized limit is applied deterministically after ranking and before response serialization, with a documented allowed range.
- [ ] The fields allowlist contains only real output properties and includes all supported properties using one documented naming convention.
- [ ] Supplying fields returns only mandatory envelope metadata plus the requested fields; unsupported fields fail validation before an upstream call.
- [ ] If projection is not retained, the fields parameter and all related claims are removed instead of being silently ignored.
- [ ] Tests prove limit values and each field projection affect output and token estimate without changing the upstream candidate cache key unnecessarily.

**Dependencies:** [US02.05.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-05-01)

**Labels:** `search-symbol` `parameters` `projection` `correctness`

**Source findings:**

- fields is validated and then discarded; its allowlist includes stale/nonexistent count, result, and is_exact_match entries and misses actual fields.
- limit is accepted and cache-keyed but never sent upstream or applied to the response.

**Subtasks:**

<a id="id-st05-01-01-01"></a>
- [ ] **ST05.01.01.01 — Reconcile fields allowlist with actual response schema**
  - Type: analysis
  - Estimate: 3 hours
  - Suggested owner role: API engineer
  - Deliverable/evidence: Supported search field list
  - Status: Not Started
<a id="id-st05-01-01-02"></a>
- [ ] **ST05.01.01.02 — Implement deterministic limit and projection**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Functional search parameters
  - Status: Not Started
<a id="id-st05-01-01-03"></a>
- [ ] **ST05.01.01.03 — Add parameter-effect and token-projection tests**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Search parameter contract tests
  - Status: Not Started

<a id="id-us05-01-02"></a>
### US05.01.02 — Separate cached provider candidates from per-call search metadata

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.01](#id-f05-01) | P0 | 2 | 14 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** application developer

**User story:** As an application developer, I want provider candidates cached independently from request metadata so that cache hits do not return an earlier caller's query id, timestamp, or duration.

**Acceptance criteria:**

- [ ] The search cache value contains only normalized Finnhub candidate data and provider fetch metadata.
- [ ] Every invocation generates a fresh query id, request timestamp, elapsed duration, projection, and token estimate after cache retrieval.
- [ ] Cache keys use canonical query normalization but do not include response-only view or limit values.
- [ ] The response indicates hit/miss and provider fetched_at separately from request generated_at.
- [ ] A two-call test proves the second response reuses candidates but has distinct request metadata.

**Dependencies:** —

**Labels:** `search-symbol` `cache` `metadata` `correctness`

**Source findings:**

- SearchService caches the full response including per-request QueryId, timestamp, and duration, causing stale caller metadata on a cache hit.

**Subtasks:**

<a id="id-st05-01-02-01"></a>
- [ ] **ST05.01.02.01 — Create provider-candidate cache DTO**
  - Type: refactoring
  - Estimate: 5 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Metadata-free cached candidate model
  - Status: Not Started
<a id="id-st05-01-02-02"></a>
- [ ] **ST05.01.02.02 — Generate per-invocation metadata after cache retrieval**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Fresh search response metadata
  - Status: Not Started
<a id="id-st05-01-02-03"></a>
- [ ] **ST05.01.02.03 — Test repeated-query cache metadata isolation**
  - Type: testing
  - Estimate: 4 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Search cache regression tests
  - Status: Not Started

<a id="id-us05-02-01"></a>
### US05.02.01 — Compute explainable confidence and exact-match values

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.02](#id-f05-02) | P0 | 4 | 24 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** AI agent

**User story:** As an AI agent, I want candidate scores and exact-match flags based on normalized identifiers and names so that I can safely decide whether to continue research without guessing.

**Acceptance criteria:**

- [ ] Every candidate receives a deterministic confidence score in the documented range and an exact-match boolean.
- [ ] Exact ticker, ISIN, and CUSIP matches outrank prefix, normalized company-name, and fuzzy matches according to a documented scoring table.
- [ ] Ties use deterministic secondary keys and never rely on provider order alone.
- [ ] The result exposes match_reason or matched_fields so the score is explainable and is not described as a calibrated probability unless validated.
- [ ] Golden tests cover exact ticker, company name, ISIN, CUSIP, ambiguous names, no-match, and case/punctuation normalization.

**Dependencies:** [US05.01.02](#id-us05-01-02)

**Labels:** `search-symbol` `ranking` `confidence` `exact-match`

**Source findings:**

- MapToSymbolResult leaves ConfidenceScore at zero and does not set IsExactMatch, so ambiguous resolution sorts all candidates equally and a >=0.95 threshold is unreachable for name/identifier searches.

**Subtasks:**

<a id="id-st05-02-01-01"></a>
- [ ] **ST05.02.01.01 — Specify identifier/name match scoring rules**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: search engineer
  - Deliverable/evidence: Explainable scoring specification
  - Status: Not Started
<a id="id-st05-02-01-02"></a>
- [ ] **ST05.02.01.02 — Implement confidence, exact flag, and match reasons**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: search engineer
  - Deliverable/evidence: Deterministic candidate ranker
  - Status: Not Started
<a id="id-st05-02-01-03"></a>
- [ ] **ST05.02.01.03 — Create golden identifier and ambiguity corpus**
  - Type: testing
  - Estimate: 8 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Search resolution golden tests
  - Status: Not Started

<a id="id-us05-02-02"></a>
### US05.02.02 — Resolve exchange and share-class symbols without misparsing

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.02](#id-f05-02) | P0 | 3 | 17 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** financial analyst

**User story:** As a financial analyst, I want dotted share classes and exchange-qualified symbols parsed against known exchanges so that securities such as BRK.A are not interpreted as exchange suffixes.

**Acceptance criteria:**

- [ ] A suffix is treated as an exchange qualifier only when it matches the maintained exchange/code allowlist and syntax.
- [ ] Known share-class punctuation is preserved in provider queries and normalized output.
- [ ] The parser supports explicit exchange input independently from ticker punctuation to resolve ambiguous cases.
- [ ] Tests cover BRK.A, BRK.B, region/exchange-qualified tickers, symbols with hyphens, and invalid suffixes.
- [ ] The same normalization routine is shared by search, resolver, quote, and follow-up argument generation.

**Dependencies:** [US02.05.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-05-01)

**Labels:** `search-symbol` `ticker` `exchange` `share-class`

**Source findings:**

- The current resolver knowingly misparses dotted share classes such as BRK.A as exchange-qualified symbols.

**Subtasks:**

<a id="id-st05-02-02-01"></a>
- [ ] **ST05.02.02.01 — Define exchange suffix and share-class grammar**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: financial data engineer
  - Deliverable/evidence: Symbol grammar specification
  - Status: Not Started
<a id="id-st05-02-02-02"></a>
- [ ] **ST05.02.02.02 — Implement shared exchange-aware symbol parser**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Canonical symbol parser
  - Status: Not Started
<a id="id-st05-02-02-03"></a>
- [ ] **ST05.02.02.03 — Add share-class and suffix fixture tests**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Symbol parsing regression suite
  - Status: Not Started

<a id="id-us05-02-03"></a>
### US05.02.03 — Reuse ranked search results for resolution and next actions

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.02](#id-f05-02) | P0 | 2.5 | 17 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** quota-conscious MCP user

**User story:** As a quota-conscious MCP user, I want one search request reused through resolution and follow-up generation so that a single tool call does not unnecessarily spend Finnhub quota twice.

**Acceptance criteria:**

- [ ] The resolver accepts the already fetched normalized candidate set instead of issuing a second direct Finnhub lookup.
- [ ] One normal search-symbol invocation performs at most one provider search call, excluding an explicitly documented enrichment request.
- [ ] next_actions are emitted only when the winning candidate meets the corrected exact/confidence policy and use the canonical resolved symbol.
- [ ] No-match and ambiguous results recommend clarification rather than an arbitrary ticker.
- [ ] A handler-counting test verifies one upstream call and validates follow-up arguments against target tool schemas.

**Dependencies:** [US05.02.01](#id-us05-02-01), [US05.02.02](#id-us05-02-02)

**Labels:** `search-symbol` `quota` `resolver` `next-actions`

**Source findings:**

- SearchService and the symbol resolver can issue two Finnhub calls using different cache keys, and broken confidence prevents expected next actions.

**Subtasks:**

<a id="id-st05-02-03-01"></a>
- [ ] **ST05.02.03.01 — Refactor resolver to consume existing candidates**
  - Type: refactoring
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Single-fetch resolution flow
  - Status: Not Started
<a id="id-st05-02-03-02"></a>
- [ ] **ST05.02.03.02 — Update ambiguity and next-action policy**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Confidence-aware follow-ups
  - Status: Not Started
<a id="id-st05-02-03-03"></a>
- [ ] **ST05.02.03.03 — Count upstream calls and validate follow-up schemas**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Search quota and action tests
  - Status: Not Started

<a id="id-us05-03-01"></a>
### US05.03.01 — Define and calculate return-based volatility

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.03](#id-f05-03) | P0 | 3 | 18 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** quantitative analyst

**User story:** As a quantitative analyst, I want volatility computed from returns with an explicit formula so that the metric is comparable across symbols and time windows.

**Acceptance criteria:**

- [ ] The contract either renames the current standard deviation of close levels to price_dispersion or replaces it with volatility computed from simple or log returns.
- [ ] The selected return formula, sample/population convention, annualization factor, interval assumptions, units, and minimum observations are documented.
- [ ] Annualization uses the actual candle resolution/trading-period convention rather than a hard-coded factor that conflicts with the input.
- [ ] Golden vectors are checked against an independent implementation within a documented numerical tolerance.
- [ ] Breaking field changes use versioning or a deprecation window and never silently change units.

**Dependencies:** —

**Labels:** `financial-correctness` `volatility` `price-summary` `math`

**Source findings:**

- The existing vol field is the population standard deviation of closing price levels, not financial return volatility.

**Subtasks:**

<a id="id-st05-03-01-01"></a>
- [ ] **ST05.03.01.01 — Select formula, naming, units, and compatibility plan**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: quantitative engineer
  - Deliverable/evidence: Volatility calculation specification
  - Status: Not Started
<a id="id-st05-03-01-02"></a>
- [ ] **ST05.03.01.02 — Implement return-based volatility or renamed dispersion**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: quantitative engineer
  - Deliverable/evidence: Correct price metric implementation
  - Status: Not Started
<a id="id-st05-03-01-03"></a>
- [ ] **ST05.03.01.03 — Verify independent golden vectors and numerical tolerance**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: quantitative QA engineer
  - Deliverable/evidence: Financial formula verification suite
  - Status: Not Started

<a id="id-us05-03-02"></a>
### US05.03.02 — Validate candle series before aggregating price statistics

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.03](#id-f05-03) | P0 | 3 | 22 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** financial researcher

**User story:** As a financial researcher, I want malformed or sparse candle data detected before calculations so that divide-by-zero, misaligned arrays, or invalid timestamps cannot produce plausible-looking nonsense.

**Acceptance criteria:**

- [ ] Open, high, low, close, volume, and timestamp arrays have compatible lengths before indexed calculations.
- [ ] Timestamps are valid, ordered, deduplicated according to policy, and converted with an explicit timezone/session convention.
- [ ] NaN, infinity, non-positive prices where invalid, zero denominators, and insufficient observations produce a defined null/NotFound/InvalidResponse outcome.
- [ ] Range high/low, return, drawdown, volume, and volatility calculations state their included endpoints and units.
- [ ] Property tests cover empty, one-point, mismatched, duplicate, unordered, zero-price, and extreme-value series.

**Dependencies:** [US05.03.01](#id-us05-03-01)

**Labels:** `financial-correctness` `candles` `validation` `edge-cases`

**Source findings:**

- Price aggregation needs guards for zero division, mismatched arrays, timestamps, sparse data, and non-finite values.

**Subtasks:**

<a id="id-st05-03-02-01"></a>
- [ ] **ST05.03.02.01 — Implement candle normalization and validation stage**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: financial data engineer
  - Deliverable/evidence: Validated candle-series model
  - Status: Not Started
<a id="id-st05-03-02-02"></a>
- [ ] **ST05.03.02.02 — Harden aggregate formulas and missing-data outcomes**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: quantitative engineer
  - Deliverable/evidence: Defensive price calculations
  - Status: Not Started
<a id="id-st05-03-02-03"></a>
- [ ] **ST05.03.02.03 — Add malformed-series property tests**
  - Type: testing
  - Estimate: 8 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Candle edge-case test suite
  - Status: Not Started

<a id="id-us05-04-01"></a>
### US05.04.01 — Classify Form 4 transactions before computing an insider signal

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.04](#id-f05-04) | P0 | 4 | 23 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** equity researcher

**User story:** As an equity researcher, I want open-market purchases and sales separated from grants, gifts, option exercises, and tax withholding so that the signal reflects intentional trading rather than all signed share changes.

**Acceptance criteria:**

- [ ] Provider transaction codes and acquisition/disposition flags map through a documented classification table.
- [ ] The primary net open-market signal includes only qualifying purchase/sale classes and reports excluded-class counts and values.
- [ ] Grants, awards, gifts, exercises, conversions, and tax transactions are exposed separately or excluded explicitly rather than silently folded into net buying/selling.
- [ ] Unknown codes do not enter the primary signal and reduce completeness with a warning.
- [ ] Golden fixtures cover representative purchase, sale, award, gift, option, tax, and unknown transactions.

**Dependencies:** [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02)

**Labels:** `financial-correctness` `insider` `form-4` `classification`

**Source findings:**

- net_buy_sell_30d currently sums all signed Form 4 changes, which can treat grants, gifts, options, and tax transactions as open-market conviction.

**Subtasks:**

<a id="id-st05-04-01-01"></a>
- [ ] **ST05.04.01.01 — Research and document Finnhub/Form 4 transaction-code mapping**
  - Type: analysis
  - Estimate: 7 hours
  - Suggested owner role: financial data analyst
  - Deliverable/evidence: Reviewed transaction classification table
  - Status: Not Started
<a id="id-st05-04-01-02"></a>
- [ ] **ST05.04.01.02 — Implement classified aggregation and exclusions**
  - Type: implementation
  - Estimate: 9 hours
  - Suggested owner role: financial data engineer
  - Deliverable/evidence: Transaction-aware insider signal
  - Status: Not Started
<a id="id-st05-04-01-03"></a>
- [ ] **ST05.04.01.03 — Build representative code fixtures and golden results**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: financial QA engineer
  - Deliverable/evidence: Insider classification tests
  - Status: Not Started

<a id="id-us05-04-02"></a>
### US05.04.02 — Label insider periods and values from the actual request window

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.04](#id-f05-04) | P0 | 2.5 | 15 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP user

**User story:** As an MCP user, I want insider signal fields to reflect the period I requested so that a 90-day calculation is never labeled 30-day.

**Acceptance criteria:**

- [ ] The response uses window_days plus neutral metric names or dynamically generated labels rather than a fixed net_buy_sell_30d field.
- [ ] The exact inclusive/exclusive start and end dates and timezone are returned.
- [ ] Share and monetary values are separated, with currency and missing-price handling explicitly stated.
- [ ] Summary, standard, and full views use the same underlying classified transactions and window.
- [ ] Tests cover default 30-day and custom 7-, 90-, and boundary-date windows.

**Dependencies:** [US05.04.01](#id-us05-04-01)

**Labels:** `financial-correctness` `insider` `date-window` `schema`

**Source findings:**

- The insider response field remains net_buy_sell_30d even when a caller requests a different period such as 90 days.

**Subtasks:**

<a id="id-st05-04-02-01"></a>
- [ ] **ST05.04.02.01 — Revise insider response schema for neutral windowed fields**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Window-correct insider schema
  - Status: Not Started
<a id="id-st05-04-02-02"></a>
- [ ] **ST05.04.02.02 — Implement dates, currency, and dynamic window metadata**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: financial data engineer
  - Deliverable/evidence: Correctly labeled insider response
  - Status: Not Started
<a id="id-st05-04-02-03"></a>
- [ ] **ST05.04.02.03 — Test default, custom, and boundary periods**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Insider window test suite
  - Status: Not Started

<a id="id-us05-05-01"></a>
### US05.05.01 — Use non-overlapping comparable news windows

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.05](#id-f05-05) | P1 | 2.5 | 15 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** news analyst

**User story:** As a news analyst, I want current and comparison windows with no shared boundary day so that article counts and sentiment changes are not double-counted.

**Acceptance criteria:**

- [ ] Current and previous intervals are defined with explicit inclusive/exclusive boundaries and share no date or timestamp.
- [ ] Both windows have equal duration unless the response explicitly labels and normalizes unequal coverage.
- [ ] Timezone and market-date conversion are documented and returned.
- [ ] Trend fields distinguish article-volume change from sentiment-score change and never call count delta sentiment.
- [ ] Boundary-focused tests place articles exactly at start/end timestamps and prove each appears in at most one interval.

**Dependencies:** —

**Labels:** `financial-correctness` `news` `date-window` `sentiment`

**Source findings:**

- News comparison periods share a boundary and can double-count; article-count change must not be described as sentiment trend.

**Subtasks:**

<a id="id-st05-05-01-01"></a>
- [ ] **ST05.05.01.01 — Define non-overlapping interval and timezone convention**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: financial data engineer
  - Deliverable/evidence: News window specification
  - Status: Not Started
<a id="id-st05-05-01-02"></a>
- [ ] **ST05.05.01.02 — Implement period generation and distinct trend fields**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Correct news comparison model
  - Status: Not Started
<a id="id-st05-05-01-03"></a>
- [ ] **ST05.05.01.03 — Add boundary-timestamp tests**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Non-overlap news tests
  - Status: Not Started

<a id="id-us05-05-02"></a>
### US05.05.02 — Degrade news sentiment consistently under bounded concurrency

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F05.05](#id-f05-05) | P1 | 3.5 | 25 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** quota-conscious researcher

**User story:** As a quota-conscious researcher, I want independent news components fetched concurrently only when budget permits and failures reported consistently so that I get timely usable partial results.

**Acceptance criteria:**

- [ ] Quota for all mandatory and optional news calls is reserved before starting concurrent work.
- [ ] Independent current-news, comparison-news, and sentiment calls may run concurrently under a configured per-tool concurrency cap.
- [ ] PremiumRequired and other documented recoverable sentiment failures return article analysis with explicit sentiment_unavailable and component error metadata.
- [ ] Mandatory news-fetch failure follows the partial/total failure contract rather than silently returning empty data.
- [ ] Tests cover full success, premium sentiment, timeout, rate limit, partial window failure, cancellation, and reserved-budget denial.

**Dependencies:** [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02), [US04.03.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-02), [US05.05.01](#id-us05-05-01)

**Labels:** `financial-correctness` `news` `concurrency` `degradation`

**Source findings:**

- Only PremiumRequired for optional sentiment degrades cleanly; other recoverable failures can fail the whole tool, and independent calls can be concurrent only under quota control.

**Subtasks:**

<a id="id-st05-05-02-01"></a>
- [ ] **ST05.05.02.01 — Model news component costs and failure policy**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: News execution/degradation plan
  - Status: Not Started
<a id="id-st05-05-02-02"></a>
- [ ] **ST05.05.02.02 — Implement reserved bounded parallel execution**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Quota-aware news orchestration
  - Status: Not Started
<a id="id-st05-05-02-03"></a>
- [ ] **ST05.05.02.03 — Apply partial-result metadata to sentiment degradation**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Explicit news completeness response
  - Status: Not Started
<a id="id-st05-05-02-04"></a>
- [ ] **ST05.05.02.04 — Test full failure/degradation/cancellation matrix**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: News resilience tests
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST05.01.01.01](#id-st05-01-01-01) | [US05.01.01](#id-us05-01-01) | P0 | Reconcile fields allowlist with actual response schema | analysis | 3 | API engineer | Supported search field list | Not Started |
| [ST05.01.01.02](#id-st05-01-01-02) | [US05.01.01](#id-us05-01-01) | P0 | Implement deterministic limit and projection | implementation | 7 | backend engineer | Functional search parameters | Not Started |
| [ST05.01.01.03](#id-st05-01-01-03) | [US05.01.01](#id-us05-01-01) | P0 | Add parameter-effect and token-projection tests | testing | 5 | test engineer | Search parameter contract tests | Not Started |
| [ST05.01.02.01](#id-st05-01-02-01) | [US05.01.02](#id-us05-01-02) | P0 | Create provider-candidate cache DTO | refactoring | 5 | backend engineer | Metadata-free cached candidate model | Not Started |
| [ST05.01.02.02](#id-st05-01-02-02) | [US05.01.02](#id-us05-01-02) | P0 | Generate per-invocation metadata after cache retrieval | implementation | 5 | backend engineer | Fresh search response metadata | Not Started |
| [ST05.01.02.03](#id-st05-01-02-03) | [US05.01.02](#id-us05-01-02) | P0 | Test repeated-query cache metadata isolation | testing | 4 | test engineer | Search cache regression tests | Not Started |
| [ST05.02.01.01](#id-st05-02-01-01) | [US05.02.01](#id-us05-02-01) | P0 | Specify identifier/name match scoring rules | design | 6 | search engineer | Explainable scoring specification | Not Started |
| [ST05.02.01.02](#id-st05-02-01-02) | [US05.02.01](#id-us05-02-01) | P0 | Implement confidence, exact flag, and match reasons | implementation | 10 | search engineer | Deterministic candidate ranker | Not Started |
| [ST05.02.01.03](#id-st05-02-01-03) | [US05.02.01](#id-us05-02-01) | P0 | Create golden identifier and ambiguity corpus | testing | 8 | test engineer | Search resolution golden tests | Not Started |
| [ST05.02.02.01](#id-st05-02-02-01) | [US05.02.02](#id-us05-02-02) | P0 | Define exchange suffix and share-class grammar | design | 4 | financial data engineer | Symbol grammar specification | Not Started |
| [ST05.02.02.02](#id-st05-02-02-02) | [US05.02.02](#id-us05-02-02) | P0 | Implement shared exchange-aware symbol parser | implementation | 8 | backend engineer | Canonical symbol parser | Not Started |
| [ST05.02.02.03](#id-st05-02-02-03) | [US05.02.02](#id-us05-02-02) | P0 | Add share-class and suffix fixture tests | testing | 5 | test engineer | Symbol parsing regression suite | Not Started |
| [ST05.02.03.01](#id-st05-02-03-01) | [US05.02.03](#id-us05-02-03) | P0 | Refactor resolver to consume existing candidates | refactoring | 7 | backend engineer | Single-fetch resolution flow | Not Started |
| [ST05.02.03.02](#id-st05-02-03-02) | [US05.02.03](#id-us05-02-03) | P0 | Update ambiguity and next-action policy | implementation | 5 | MCP engineer | Confidence-aware follow-ups | Not Started |
| [ST05.02.03.03](#id-st05-02-03-03) | [US05.02.03](#id-us05-02-03) | P0 | Count upstream calls and validate follow-up schemas | testing | 5 | test engineer | Search quota and action tests | Not Started |
| [ST05.03.01.01](#id-st05-03-01-01) | [US05.03.01](#id-us05-03-01) | P0 | Select formula, naming, units, and compatibility plan | design | 5 | quantitative engineer | Volatility calculation specification | Not Started |
| [ST05.03.01.02](#id-st05-03-01-02) | [US05.03.01](#id-us05-03-01) | P0 | Implement return-based volatility or renamed dispersion | implementation | 7 | quantitative engineer | Correct price metric implementation | Not Started |
| [ST05.03.01.03](#id-st05-03-01-03) | [US05.03.01](#id-us05-03-01) | P0 | Verify independent golden vectors and numerical tolerance | testing | 6 | quantitative QA engineer | Financial formula verification suite | Not Started |
| [ST05.03.02.01](#id-st05-03-02-01) | [US05.03.02](#id-us05-03-02) | P0 | Implement candle normalization and validation stage | implementation | 8 | financial data engineer | Validated candle-series model | Not Started |
| [ST05.03.02.02](#id-st05-03-02-02) | [US05.03.02](#id-us05-03-02) | P0 | Harden aggregate formulas and missing-data outcomes | implementation | 6 | quantitative engineer | Defensive price calculations | Not Started |
| [ST05.03.02.03](#id-st05-03-02-03) | [US05.03.02](#id-us05-03-02) | P0 | Add malformed-series property tests | testing | 8 | test engineer | Candle edge-case test suite | Not Started |
| [ST05.04.01.01](#id-st05-04-01-01) | [US05.04.01](#id-us05-04-01) | P0 | Research and document Finnhub/Form 4 transaction-code mapping | analysis | 7 | financial data analyst | Reviewed transaction classification table | Not Started |
| [ST05.04.01.02](#id-st05-04-01-02) | [US05.04.01](#id-us05-04-01) | P0 | Implement classified aggregation and exclusions | implementation | 9 | financial data engineer | Transaction-aware insider signal | Not Started |
| [ST05.04.01.03](#id-st05-04-01-03) | [US05.04.01](#id-us05-04-01) | P0 | Build representative code fixtures and golden results | testing | 7 | financial QA engineer | Insider classification tests | Not Started |
| [ST05.04.02.01](#id-st05-04-02-01) | [US05.04.02](#id-us05-04-02) | P0 | Revise insider response schema for neutral windowed fields | design | 4 | API architect | Window-correct insider schema | Not Started |
| [ST05.04.02.02](#id-st05-04-02-02) | [US05.04.02](#id-us05-04-02) | P0 | Implement dates, currency, and dynamic window metadata | implementation | 6 | financial data engineer | Correctly labeled insider response | Not Started |
| [ST05.04.02.03](#id-st05-04-02-03) | [US05.04.02](#id-us05-04-02) | P0 | Test default, custom, and boundary periods | testing | 5 | test engineer | Insider window test suite | Not Started |
| [ST05.05.01.01](#id-st05-05-01-01) | [US05.05.01](#id-us05-05-01) | P1 | Define non-overlapping interval and timezone convention | design | 4 | financial data engineer | News window specification | Not Started |
| [ST05.05.01.02](#id-st05-05-01-02) | [US05.05.01](#id-us05-05-01) | P1 | Implement period generation and distinct trend fields | implementation | 6 | backend engineer | Correct news comparison model | Not Started |
| [ST05.05.01.03](#id-st05-05-01-03) | [US05.05.01](#id-us05-05-01) | P1 | Add boundary-timestamp tests | testing | 5 | test engineer | Non-overlap news tests | Not Started |
| [ST05.05.02.01](#id-st05-05-02-01) | [US05.05.02](#id-us05-05-02) | P1 | Model news component costs and failure policy | design | 4 | backend engineer | News execution/degradation plan | Not Started |
| [ST05.05.02.02](#id-st05-05-02-02) | [US05.05.02](#id-us05-05-02) | P1 | Implement reserved bounded parallel execution | implementation | 8 | backend engineer | Quota-aware news orchestration | Not Started |
| [ST05.05.02.03](#id-st05-05-02-03) | [US05.05.02](#id-us05-05-02) | P1 | Apply partial-result metadata to sentiment degradation | implementation | 6 | MCP engineer | Explicit news completeness response | Not Started |
| [ST05.05.02.04](#id-st05-05-02-04) | [US05.05.02](#id-us05-05-02) | P1 | Test full failure/degradation/cancellation matrix | testing | 7 | test engineer | News resilience tests | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E05 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| R-04 | Repository finding | Implement or remove search-symbol fields projection and enforce the accepted limit; correct the stale field allowlist. | [US05.01.01](#id-us05-01-01), [US07.01.02](./E07-bounded-response-and-token-contract.md#id-us07-01-02) | [E05](#id-e05) | P0 | Covered | Code-level core-functionality finding. |
| R-05 | Repository finding | Calculate deterministic confidence and exact-match signals so name/ISIN/CUSIP ambiguity resolution and next_actions work. | [US05.02.01](#id-us05-02-01), [US05.02.03](#id-us05-02-03), [US08.05.01](./E08-intelligent-discovery-and-context-engineering.md#id-us08-05-01) | [E05](#id-e05) | P0 | Covered | Code-level core-functionality finding. |
| R-06 | Repository finding | Avoid duplicate symbol-search provider calls and keep per-call query IDs/timing outside cached provider data. | [US05.01.02](#id-us05-01-02), [US05.02.03](#id-us05-02-03) | [E05](#id-e05) | P0 | Covered | Code-level efficiency/correctness finding. |
| R-07 | Repository finding | Support exchange-qualified and share-class symbols such as BRK.A without destructive parsing. | [US05.02.02](#id-us05-02-02) | [E05](#id-e05) | P0 | Covered | Code-level symbol-model finding. |
| R-09 | Repository finding | Correct get-price-summary volatility semantics, guards, units, array validation, timestamps, and test fixtures. | [US05.03.01](#id-us05-03-01), [US05.03.02](#id-us05-03-02), [US06.09.01](./E06-financial-data-coverage-and-semantics.md#id-us06-09-01) | [E05](#id-e05) | P0 | Covered | Code-level financial-accuracy finding. |
| R-10 | Repository finding | Classify insider transactions correctly, separate open-market purchase/sale from grants/options/gifts/tax events, and align output window names. | [US05.04.01](#id-us05-04-01), [US05.04.02](#id-us05-04-02), [US06.09.02](./E06-financial-data-coverage-and-semantics.md#id-us06-09-02) | [E05](#id-e05) | P0 | Covered | Code-level financial-accuracy finding. |
| R-11 | Repository finding | Use non-overlapping news windows, resilient optional sentiment behavior, completeness metadata, and quota-aware concurrency. | [US05.05.01](#id-us05-05-01), [US05.05.02](#id-us05-05-02), [US06.09.03](./E06-financial-data-coverage-and-semantics.md#id-us06-09-03) | [E05](#id-e05) | P0 | Covered | Code-level financial/UX finding. |
| RF-093 | Code-review detail | Multi-source tool degradation is inconsistent and lacks component completeness/provenance. | [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02), [US05.05.02](#id-us05-05-02) | [E04](./E04-resilience-quota-governance-and-cache-correctness.md#id-e04) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-098 | Code-review detail | Cache entries should contain canonical provider data, not per-call metadata or view projection. | [US04.04.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-01), [US05.01.02](#id-us05-01-02) | [E04](./E04-resilience-quota-governance-and-cache-correctness.md#id-e04) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-101 | Code-review detail | search-symbol validates then discards fields, uses a stale allowlist, and accepts but does not apply limit. | [US05.01.01](#id-us05-01-01) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-102 | Code-review detail | Search cache hits can return a previous request's QueryId, timestamp, and duration. | [US05.01.02](#id-us05-01-02), [US04.04.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-01) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-103 | Code-review detail | Candidate confidence remains zero and exact-match is unset, breaking ambiguous resolution and the >=0.95 follow-up threshold. | [US05.02.01](#id-us05-02-01), [US05.02.03](#id-us05-02-03) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-104 | Code-review detail | Dotted share classes such as BRK.A are misparsed as exchange suffixes. | [US05.02.02](#id-us05-02-02) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-105 | Code-review detail | Search and resolver can spend two Finnhub calls for one invocation under different cache keys. | [US05.02.03](#id-us05-02-03) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-106 | Code-review detail | Price vol is population standard deviation of close levels, not return volatility. | [US05.03.01](#id-us05-03-01) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-107 | Code-review detail | Candle calculations need guards for array lengths, zero denominators, timestamps, sparse data, and non-finite values. | [US05.03.02](#id-us05-03-02) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-108 | Code-review detail | Insider net activity combines all Form 4 signed changes, including grants, gifts, options, and tax transactions. | [US05.04.01](#id-us05-04-01) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-109 | Code-review detail | The insider net_buy_sell_30d field remains fixed when a custom period such as 90 days is requested. | [US05.04.02](#id-us05-04-02) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-110 | Code-review detail | News current and comparison windows overlap at a boundary and article-volume change can be mislabeled as sentiment trend. | [US05.05.01](#id-us05-05-01) | [E05](#id-e05) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-111 | Code-review detail | News sentiment degrades only for PremiumRequired; other recoverable failures and sequential independent calls need a quota-aware partial-result policy. | [US05.05.02](#id-us05-05-02), [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02), [US04.03.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-02) | [E05](#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |

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
| E05 | Epic | — | P0 | Financial and Symbol Data Correctness | See [E05](#id-e05) | See [E05](#id-e05) | — | finnhub-mcp; epic | — | Not Started |
| F05.01 | Feature | [E05](#id-e05) | P0 | Search Parameter Fidelity | See [F05.01](#id-f05-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e05 | — | Not Started |
| F05.02 | Feature | [E05](#id-e05) | P0 | Deterministic Symbol Resolution | See [F05.02](#id-f05-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e05 | — | Not Started |
| F05.03 | Feature | [E05](#id-e05) | P0 | Defensible Historical Price Statistics | See [F05.03](#id-f05-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e05 | — | Not Started |
| F05.04 | Feature | [E05](#id-e05) | P0 | Transaction-Aware Insider Signals | See [F05.04](#id-f05-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e05 | — | Not Started |
| F05.05 | Feature | [E05](#id-e05) | P1 | Non-Overlapping and Degradable News Pulse | See [F05.05](#id-f05-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e05 | — | Not Started |
| US05.01.01 | Story | [F05.01](#id-f05-01) | P0 | Honor search limit and field projection | See [US05.01.01](#id-us05-01-01) | See [US05.01.01](#id-us05-01-01) | 2.5d | search-symbol; parameters; projection; correctness | [US02.05.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-05-01) | Not Started |
| US05.01.02 | Story | [F05.01](#id-f05-01) | P0 | Separate cached provider candidates from per-call search metadata | See [US05.01.02](#id-us05-01-02) | See [US05.01.02](#id-us05-01-02) | 2d | search-symbol; cache; metadata; correctness | — | Not Started |
| US05.02.01 | Story | [F05.02](#id-f05-02) | P0 | Compute explainable confidence and exact-match values | See [US05.02.01](#id-us05-02-01) | See [US05.02.01](#id-us05-02-01) | 4d | search-symbol; ranking; confidence; exact-match | [US05.01.02](#id-us05-01-02) | Not Started |
| US05.02.02 | Story | [F05.02](#id-f05-02) | P0 | Resolve exchange and share-class symbols without misparsing | See [US05.02.02](#id-us05-02-02) | See [US05.02.02](#id-us05-02-02) | 3d | search-symbol; ticker; exchange; share-class | [US02.05.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-05-01) | Not Started |
| US05.02.03 | Story | [F05.02](#id-f05-02) | P0 | Reuse ranked search results for resolution and next actions | See [US05.02.03](#id-us05-02-03) | See [US05.02.03](#id-us05-02-03) | 2.5d | search-symbol; quota; resolver; next-actions | [US05.02.01](#id-us05-02-01), [US05.02.02](#id-us05-02-02) | Not Started |
| US05.03.01 | Story | [F05.03](#id-f05-03) | P0 | Define and calculate return-based volatility | See [US05.03.01](#id-us05-03-01) | See [US05.03.01](#id-us05-03-01) | 3d | financial-correctness; volatility; price-summary; math | — | Not Started |
| US05.03.02 | Story | [F05.03](#id-f05-03) | P0 | Validate candle series before aggregating price statistics | See [US05.03.02](#id-us05-03-02) | See [US05.03.02](#id-us05-03-02) | 3d | financial-correctness; candles; validation; edge-cases | [US05.03.01](#id-us05-03-01) | Not Started |
| US05.04.01 | Story | [F05.04](#id-f05-04) | P0 | Classify Form 4 transactions before computing an insider signal | See [US05.04.01](#id-us05-04-01) | See [US05.04.01](#id-us05-04-01) | 4d | financial-correctness; insider; form-4; classification | [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02) | Not Started |
| US05.04.02 | Story | [F05.04](#id-f05-04) | P0 | Label insider periods and values from the actual request window | See [US05.04.02](#id-us05-04-02) | See [US05.04.02](#id-us05-04-02) | 2.5d | financial-correctness; insider; date-window; schema | [US05.04.01](#id-us05-04-01) | Not Started |
| US05.05.01 | Story | [F05.05](#id-f05-05) | P1 | Use non-overlapping comparable news windows | See [US05.05.01](#id-us05-05-01) | See [US05.05.01](#id-us05-05-01) | 2.5d | financial-correctness; news; date-window; sentiment | — | Not Started |
| US05.05.02 | Story | [F05.05](#id-f05-05) | P1 | Degrade news sentiment consistently under bounded concurrency | See [US05.05.02](#id-us05-05-02) | See [US05.05.02](#id-us05-05-02) | 3.5d | financial-correctness; news; concurrency; degradation | [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02), [US04.03.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-02), [US05.05.01](#id-us05-05-01) | Not Started |
| ST05.01.01.01 | Sub-task | [US05.01.01](#id-us05-01-01) | P0 | Reconcile fields allowlist with actual response schema | See [ST05.01.01.01](#id-st05-01-01-01) | Not applicable; see detail or parent section | 3h | finnhub-mcp; analysis | — | Not Started |
| ST05.01.01.02 | Sub-task | [US05.01.01](#id-us05-01-01) | P0 | Implement deterministic limit and projection | See [ST05.01.01.02](#id-st05-01-01-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST05.01.01.03 | Sub-task | [US05.01.01](#id-us05-01-01) | P0 | Add parameter-effect and token-projection tests | See [ST05.01.01.03](#id-st05-01-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST05.01.02.01 | Sub-task | [US05.01.02](#id-us05-01-02) | P0 | Create provider-candidate cache DTO | See [ST05.01.02.01](#id-st05-01-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; refactoring | — | Not Started |
| ST05.01.02.02 | Sub-task | [US05.01.02](#id-us05-01-02) | P0 | Generate per-invocation metadata after cache retrieval | See [ST05.01.02.02](#id-st05-01-02-02) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST05.01.02.03 | Sub-task | [US05.01.02](#id-us05-01-02) | P0 | Test repeated-query cache metadata isolation | See [ST05.01.02.03](#id-st05-01-02-03) | Not applicable; see detail or parent section | 4h | finnhub-mcp; testing | — | Not Started |
| ST05.02.01.01 | Sub-task | [US05.02.01](#id-us05-02-01) | P0 | Specify identifier/name match scoring rules | See [ST05.02.01.01](#id-st05-02-01-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST05.02.01.02 | Sub-task | [US05.02.01](#id-us05-02-01) | P0 | Implement confidence, exact flag, and match reasons | See [ST05.02.01.02](#id-st05-02-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST05.02.01.03 | Sub-task | [US05.02.01](#id-us05-02-01) | P0 | Create golden identifier and ambiguity corpus | See [ST05.02.01.03](#id-st05-02-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; testing | — | Not Started |
| ST05.02.02.01 | Sub-task | [US05.02.02](#id-us05-02-02) | P0 | Define exchange suffix and share-class grammar | See [ST05.02.02.01](#id-st05-02-02-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST05.02.02.02 | Sub-task | [US05.02.02](#id-us05-02-02) | P0 | Implement shared exchange-aware symbol parser | See [ST05.02.02.02](#id-st05-02-02-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST05.02.02.03 | Sub-task | [US05.02.02](#id-us05-02-02) | P0 | Add share-class and suffix fixture tests | See [ST05.02.02.03](#id-st05-02-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST05.02.03.01 | Sub-task | [US05.02.03](#id-us05-02-03) | P0 | Refactor resolver to consume existing candidates | See [ST05.02.03.01](#id-st05-02-03-01) | Not applicable; see detail or parent section | 7h | finnhub-mcp; refactoring | — | Not Started |
| ST05.02.03.02 | Sub-task | [US05.02.03](#id-us05-02-03) | P0 | Update ambiguity and next-action policy | See [ST05.02.03.02](#id-st05-02-03-02) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST05.02.03.03 | Sub-task | [US05.02.03](#id-us05-02-03) | P0 | Count upstream calls and validate follow-up schemas | See [ST05.02.03.03](#id-st05-02-03-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST05.03.01.01 | Sub-task | [US05.03.01](#id-us05-03-01) | P0 | Select formula, naming, units, and compatibility plan | See [ST05.03.01.01](#id-st05-03-01-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST05.03.01.02 | Sub-task | [US05.03.01](#id-us05-03-01) | P0 | Implement return-based volatility or renamed dispersion | See [ST05.03.01.02](#id-st05-03-01-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST05.03.01.03 | Sub-task | [US05.03.01](#id-us05-03-01) | P0 | Verify independent golden vectors and numerical tolerance | See [ST05.03.01.03](#id-st05-03-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST05.03.02.01 | Sub-task | [US05.03.02](#id-us05-03-02) | P0 | Implement candle normalization and validation stage | See [ST05.03.02.01](#id-st05-03-02-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST05.03.02.02 | Sub-task | [US05.03.02](#id-us05-03-02) | P0 | Harden aggregate formulas and missing-data outcomes | See [ST05.03.02.02](#id-st05-03-02-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST05.03.02.03 | Sub-task | [US05.03.02](#id-us05-03-02) | P0 | Add malformed-series property tests | See [ST05.03.02.03](#id-st05-03-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; testing | — | Not Started |
| ST05.04.01.01 | Sub-task | [US05.04.01](#id-us05-04-01) | P0 | Research and document Finnhub/Form 4 transaction-code mapping | See [ST05.04.01.01](#id-st05-04-01-01) | Not applicable; see detail or parent section | 7h | finnhub-mcp; analysis | — | Not Started |
| ST05.04.01.02 | Sub-task | [US05.04.01](#id-us05-04-01) | P0 | Implement classified aggregation and exclusions | See [ST05.04.01.02](#id-st05-04-01-02) | Not applicable; see detail or parent section | 9h | finnhub-mcp; implementation | — | Not Started |
| ST05.04.01.03 | Sub-task | [US05.04.01](#id-us05-04-01) | P0 | Build representative code fixtures and golden results | See [ST05.04.01.03](#id-st05-04-01-03) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |
| ST05.04.02.01 | Sub-task | [US05.04.02](#id-us05-04-02) | P0 | Revise insider response schema for neutral windowed fields | See [ST05.04.02.01](#id-st05-04-02-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST05.04.02.02 | Sub-task | [US05.04.02](#id-us05-04-02) | P0 | Implement dates, currency, and dynamic window metadata | See [ST05.04.02.02](#id-st05-04-02-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST05.04.02.03 | Sub-task | [US05.04.02](#id-us05-04-02) | P0 | Test default, custom, and boundary periods | See [ST05.04.02.03](#id-st05-04-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST05.05.01.01 | Sub-task | [US05.05.01](#id-us05-05-01) | P1 | Define non-overlapping interval and timezone convention | See [ST05.05.01.01](#id-st05-05-01-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST05.05.01.02 | Sub-task | [US05.05.01](#id-us05-05-01) | P1 | Implement period generation and distinct trend fields | See [ST05.05.01.02](#id-st05-05-01-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST05.05.01.03 | Sub-task | [US05.05.01](#id-us05-05-01) | P1 | Add boundary-timestamp tests | See [ST05.05.01.03](#id-st05-05-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST05.05.02.01 | Sub-task | [US05.05.02](#id-us05-05-02) | P1 | Model news component costs and failure policy | See [ST05.05.02.01](#id-st05-05-02-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST05.05.02.02 | Sub-task | [US05.05.02](#id-us05-05-02) | P1 | Implement reserved bounded parallel execution | See [ST05.05.02.02](#id-st05-05-02-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST05.05.02.03 | Sub-task | [US05.05.02](#id-us05-05-02) | P1 | Apply partial-result metadata to sentiment degradation | See [ST05.05.02.03](#id-st05-05-02-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST05.05.02.04 | Sub-task | [US05.05.02](#id-us05-05-02) | P1 | Test full failure/degradation/cancellation matrix | See [ST05.05.02.04](#id-st05-05-02-04) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |

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

- [ ] Create E05 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E05.
- [ ] Create all 11 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 34 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 20 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 51 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

