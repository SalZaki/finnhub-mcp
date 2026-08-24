---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E06
title: "Financial Data Coverage and Semantics"
priority: P1
phase: "M1 — Product Expansion"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 9
  user_stories: 15
  subtasks: 44
  traceability_owned: 7
  traceability_items: 11
story_estimate_days: 74
subtask_estimate_hours: 551
---

<a id="id-e06"></a>
# E06 — Financial Data Coverage and Semantics

This is the self-contained coding-agent backlog for E06. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E06 | P1 | 9 | 15 | 44 | 74 | 551 | M1 — Product Expansion | Not Started |

## 2. Epic Definition

**Objective:** Expand the server from summary research into auditable market, fundamental, event, filing and multi-asset analysis while preserving Finnhub quota and licensing constraints.

**Business value:** Covers the highest-value workflows that currently force agents to leave the server or synthesize unsupported conclusions.

**Exit criteria:**

- [ ] Historical prices, technical indicators, corporate actions, statements, earnings and filing workflows are available with bounded responses.
- [ ] Ownership, ETF, FX and crypto extensions are entitlement-aware and follow the same response contract.
- [ ] Price volatility, insider transactions and news trend windows use documented financial semantics with golden tests.
- [ ] Every financial result identifies provenance, units, currency, time zone, freshness and data completeness.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F06.01](#id-f06-01) | P1 | Historical OHLCV tool | 1 | 5 | Not Started |
| [F06.02](#id-f06-02) | P1 | Local technical indicators | 1 | 6 | Not Started |
| [F06.03](#id-f06-03) | P1 | Corporate actions | 1 | 4 | Not Started |
| [F06.04](#id-f06-04) | P1 | Financial statements | 1 | 7 | Not Started |
| [F06.05](#id-f06-05) | P1 | Earnings and analyst events | 2 | 9 | Not Started |
| [F06.06](#id-f06-06) | P1 | SEC filing discovery and sections | 2 | 13 | Not Started |
| [F06.07](#id-f06-07) | P2 | Ownership and multi-asset coverage | 3 | 19 | Not Started |
| [F06.08](#id-f06-08) | P1 | Financial provenance contract | 1 | 5 | Not Started |
| [F06.09](#id-f06-09) | P0 | Financial-signal semantic certification | 3 | 6 | Not Started |

<a id="id-f06-01"></a>
### F06.01 — Historical OHLCV tool

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Provide custom-range, resolution-aware, adjusted historical candles with pagination and market-time semantics.

**Expected outcome:** Agents can perform reproducible price-history analysis without relying on fixed 7d/30d/90d/1y summaries.

**Stories:**

- [US06.01.01](#id-us06-01-01) — Query bounded historical candles (P1, 5d)

<a id="id-f06-02"></a>
### F06.02 — Local technical indicators

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Compute common indicators from cached OHLCV using versioned formulas and warm-up disclosure.

**Expected outcome:** High-value technical analysis adds no avoidable Finnhub calls and remains auditable.

**Stories:**

- [US06.02.01](#id-us06-02-01) — Compute indicators from cached OHLCV (P1, 6d)

<a id="id-f06-03"></a>
### F06.03 — Corporate actions

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Expose dividend and split history with ex/pay dates, ratios, currency and adjustment status.

**Expected outcome:** Total-return and event workflows no longer infer actions from price gaps.

**Stories:**

- [US06.03.01](#id-us06-03-01) — Retrieve dividends and splits (P1, 4d)

<a id="id-f06-04"></a>
### F06.04 — Financial statements

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Expose normalized income statement, balance sheet and cash-flow periods with units and source concepts.

**Expected outcome:** Agents can inspect fundamentals beyond the existing ten-metric snapshot.

**Stories:**

- [US06.04.01](#id-us06-04-01) — Retrieve normalized financial statements (P1, 7d)

<a id="id-f06-05"></a>
### F06.05 — Earnings and analyst events

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Add reported earnings, estimates, surprises, price targets and recommendation-change events.

**Expected outcome:** Research and peer workflows can reason over expectations and revisions rather than only consensus snapshots.

**Stories:**

- [US06.05.01](#id-us06-05-01) — Retrieve earnings results, estimates and surprises (P1, 5d)
- [US06.05.02](#id-us06-05-02) — Expose price targets and analyst-change events (P1, 4d)

<a id="id-f06-06"></a>
### F06.06 — SEC filing discovery and sections

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Provide filing metadata and bounded section retrieval instead of returning complete filing documents.

**Expected outcome:** Agents can locate and analyze authoritative disclosures without overflowing context windows.

**Stories:**

- [US06.06.01](#id-us06-06-01) — Search SEC filing metadata (P1, 5d)
- [US06.06.02](#id-us06-06-02) — Retrieve bounded filing sections (P1, 8d)

<a id="id-f06-07"></a>
### F06.07 — Ownership and multi-asset coverage

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P2
- **Status:** Not Started

**Description:** Add entitlement-aware ownership, ETF, foreign-exchange and crypto workflows after core equity coverage.

**Expected outcome:** The server supports broader portfolios with explicit capability and data-quality boundaries.

**Stories:**

- [US06.07.01](#id-us06-07-01) — Retrieve institutional and ownership summaries (P2, 5d)
- [US06.07.02](#id-us06-07-02) — Retrieve ETF profile and holdings (P2, 6d)
- [US06.07.03](#id-us06-07-03) — Add FX and crypto market utilities (P2, 8d)

<a id="id-f06-08"></a>
### F06.08 — Financial provenance contract

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P1
- **Status:** Not Started

**Description:** Attach source, timing, unit, cache and quota metadata consistently to derived and upstream data.

**Expected outcome:** Users can judge freshness, comparability and reproducibility of every financial answer.

**Stories:**

- [US06.08.01](#id-us06-08-01) — Attach common data provenance (P1, 5d)

<a id="id-f06-09"></a>
### F06.09 — Financial-signal semantic certification

- **Parent Epic:** [E06](#id-e06)
- **Priority:** P0
- **Status:** Not Started

**Description:** Independently certify the corrected price-volatility, insider-transaction and news-window semantics before expanding those analyses.

**Expected outcome:** Core correctness fixes have financial-domain review, independent fixtures and durable release gates.

**Stories:**

- [US06.09.01](#id-us06-09-01) — Certify corrected historical volatility semantics (P0, 2d)
- [US06.09.02](#id-us06-09-02) — Certify corrected insider-activity semantics (P0, 2d)
- [US06.09.03](#id-us06-09-03) — Certify corrected news-window and completeness semantics (P0, 2d)

## 4. User Stories and Subtasks

<a id="id-us06-01-01"></a>
### US06.01.01 — Query bounded historical candles

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.01](#id-f06-01) | P1 | 5 | 40 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Financial analyst

**User story:** As a financial analyst, I want OHLCV for an explicit symbol, range and resolution so that I can reproduce price analysis beyond fixed summary windows.

**Acceptance criteria:**

- [ ] The tool accepts validated symbol, from, to, resolution, adjusted, max_items and cursor inputs and rejects invalid market ranges before calling Finnhub.
- [ ] Candles are returned in ascending timestamp order with open, high, low, close, volume, time_zone, currency and adjustment metadata.
- [ ] The response applies a documented hard cap, deterministic cursor continuation and no-data versus upstream-error distinction.
- [ ] Tests cover daily and intraday ranges, split-adjusted data, malformed array lengths, duplicate timestamps and provider no_data responses.

**Dependencies:** [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `ohlcv` `market-data` `P1`

**Source findings:**

- Missing full historical OHLCV; current price summary only exposes fixed aggregated windows.
- Historical candles need custom range, resolution, adjusted flag, pagination and time-zone semantics.

**Subtasks:**

<a id="id-st06-01-01-01"></a>
- [ ] **ST06.01.01.01 — Define candle input/output schemas and provider mapping**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Versioned OHLCV schema, validation rules and mapping tests.
  - Status: Not Started
<a id="id-st06-01-01-02"></a>
- [ ] **ST06.01.01.02 — Implement candle service, pagination and cache policy**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Registered get-historical-ohlcv tool with canonical caching.
  - Status: Not Started
<a id="id-st06-01-01-03"></a>
- [ ] **ST06.01.01.03 — Add edge-case and integration tests**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Golden and provider-fixture test suite for intraday/daily candles.
  - Status: Not Started

<a id="id-us06-02-01"></a>
### US06.02.01 — Compute indicators from cached OHLCV

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.02](#id-f06-02) | P1 | 6 | 44 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Technical analyst

**User story:** As a technical analyst, I want common indicators computed locally so that I can analyze trends without spending one Finnhub request per indicator.

**Acceptance criteria:**

- [ ] The tool supports SMA, EMA, RSI, MACD, Bollinger Bands and ATR with validated periods and an explicit formula_version.
- [ ] Indicator calculations reuse canonical OHLCV cache entries and make no additional upstream call when required candles are cached.
- [ ] Results disclose warm_up_points, null/insufficient-data periods, input adjustment mode and source candle range.
- [ ] Golden-vector tests match documented formulas within stated numeric tolerance and cover divide-by-zero and short series.

**Dependencies:** [US06.01.01](#id-us06-01-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `technical-analysis` `derived-data` `P1`

**Source findings:**

- Technical indicators are a high-value missing capability.
- Indicators should be local derivations with formula/version and warm-up disclosure rather than extra provider calls.

**Subtasks:**

<a id="id-st06-02-01-01"></a>
- [ ] **ST06.02.01.01 — Specify indicator formulas and tolerances**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: Quant engineer
  - Deliverable/evidence: Formula-version specification with warm-up and null semantics.
  - Status: Not Started
<a id="id-st06-02-01-02"></a>
- [ ] **ST06.02.01.02 — Implement local indicator engine**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Deterministic indicator library and MCP tool adapter.
  - Status: Not Started
<a id="id-st06-02-01-03"></a>
- [ ] **ST06.02.01.03 — Validate against golden vectors**
  - Type: test
  - Estimate: 14 hours
  - Suggested owner role: Quant QA engineer
  - Deliverable/evidence: Cross-checked indicator vectors and numeric edge-case tests.
  - Status: Not Started

<a id="id-us06-03-01"></a>
### US06.03.01 — Retrieve dividends and splits

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.03](#id-f06-03) | P1 | 4 | 32 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Portfolio analyst

**User story:** As a portfolio analyst, I want dividend and split history so that I can distinguish corporate actions from operating price performance.

**Acceptance criteria:**

- [ ] The tool accepts symbol, date range, action_types, max_items and cursor and supports dividends and splits independently or together.
- [ ] Dividend results include declaration/ex/pay dates when available, amount, currency and frequency; split results include numerator, denominator and effective date.
- [ ] The response states whether price series are adjusted and never silently converts currencies or annualizes irregular payments.
- [ ] Tests cover missing dates, special dividends, reverse splits, pagination and provider entitlement errors.

**Dependencies:** [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `corporate-actions` `dividends` `splits` `P1`

**Source findings:**

- Dividend and split data is missing and needed for total-return analysis.

**Subtasks:**

<a id="id-st06-03-01-01"></a>
- [ ] **ST06.03.01.01 — Map dividend and split provider payloads**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Corporate-action domain models and provider client.
  - Status: Not Started
<a id="id-st06-03-01-02"></a>
- [ ] **ST06.03.01.02 — Implement bounded corporate-actions tool**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Registered tool with filtering, pagination and provenance.
  - Status: Not Started
<a id="id-st06-03-01-03"></a>
- [ ] **ST06.03.01.03 — Test special and reverse actions**
  - Type: test
  - Estimate: 6 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Fixtures covering irregular dividends and split ratios.
  - Status: Not Started

<a id="id-us06-04-01"></a>
### US06.04.01 — Retrieve normalized financial statements

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.04](#id-f06-04) | P1 | 7 | 52 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Fundamental analyst

**User story:** As a fundamental analyst, I want statement line items by period so that I can inspect the drivers hidden by the ten-metric snapshot.

**Acceptance criteria:**

- [ ] The tool supports income, balance_sheet and cash_flow statements with annual or quarterly periods and bounded period count.
- [ ] Each value includes period/end_date, fiscal_year, fiscal_quarter, unit, currency, source_concept and restatement status when available.
- [ ] Missing line items remain null with completeness metadata and are not coerced to zero.
- [ ] Cross-statement and period ordering tests cover amended values, mixed units and premium-required responses.

**Dependencies:** [US07.01.02](./E07-bounded-response-and-token-contract.md#id-us07-01-02), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `fundamentals` `statements` `P1`

**Source findings:**

- The existing financial snapshot covers only ten metrics; complete statements are a high-value gap.

**Subtasks:**

<a id="id-st06-04-01-01"></a>
- [ ] **ST06.04.01.01 — Design statement normalization model**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: Statement taxonomy, unit/currency and missing-data contract.
  - Status: Not Started
<a id="id-st06-04-01-02"></a>
- [ ] **ST06.04.01.02 — Implement statement provider and projection**
  - Type: implementation
  - Estimate: 28 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Financial-statements service and tool.
  - Status: Not Started
<a id="id-st06-04-01-03"></a>
- [ ] **ST06.04.01.03 — Add restatement and period tests**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Fixture suite for annual, quarterly, amended and mixed-unit data.
  - Status: Not Started

<a id="id-us06-05-01"></a>
### US06.05.01 — Retrieve earnings results, estimates and surprises

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.05](#id-f06-05) | P1 | 5 | 34 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Equity researcher

**User story:** As an equity researcher, I want reported earnings beside estimates and surprises so that I can evaluate expectation changes accurately.

**Acceptance criteria:**

- [ ] The tool returns reported and estimated EPS and revenue, surprise value/percent, period, report date and timing when provided.
- [ ] The API distinguishes future estimates from historical reported events and supports bounded past/future ranges.
- [ ] Currency, units, source timestamp and missing-estimate semantics are explicit.
- [ ] Tests cover negative estimates, zero-estimate percentage handling, revised reports and no-data responses.

**Dependencies:** [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `earnings` `estimates` `P1`

**Source findings:**

- Earnings results, estimates and surprises are missing high-value research inputs.

**Subtasks:**

<a id="id-st06-05-01-01"></a>
- [ ] **ST06.05.01.01 — Implement earnings event mapping**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Earnings/estimates/surprise domain service and tool.
  - Status: Not Started
<a id="id-st06-05-01-02"></a>
- [ ] **ST06.05.01.02 — Define zero and missing estimate math**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: Documented surprise and completeness semantics.
  - Status: Not Started
<a id="id-st06-05-01-03"></a>
- [ ] **ST06.05.01.03 — Test historical and forward events**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Boundary and revision test suite.
  - Status: Not Started

<a id="id-us06-05-02"></a>
### US06.05.02 — Expose price targets and analyst-change events

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.05](#id-f06-05) | P1 | 4 | 30 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Equity researcher

**User story:** As an equity researcher, I want price targets and upgrade/downgrade events so that I can inspect changes behind a consensus recommendation.

**Acceptance criteria:**

- [ ] The tool returns target high/low/mean/median with currency and as_of, plus bounded upgrade/downgrade/initiation events.
- [ ] Events identify firm, from_grade, to_grade, action and date without inferring sentiment when fields are absent.
- [ ] The tool integrates with existing recommendations while avoiding duplicate provider calls for the same canonical payload.
- [ ] Tests cover mixed currencies, unchanged/reiterated ratings, missing firms and entitlement failures.

**Dependencies:** [US06.08.01](#id-us06-08-01), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01)

**Labels:** `tool` `analyst` `price-targets` `P1`

**Source findings:**

- Existing recommendations expose consensus but not price targets, upgrades or downgrades.

**Subtasks:**

<a id="id-st06-05-02-01"></a>
- [ ] **ST06.05.02.01 — Implement target and rating-change endpoints**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Analyst-event service integrated with recommendations cache.
  - Status: Not Started
<a id="id-st06-05-02-02"></a>
- [ ] **ST06.05.02.02 — Add currency and action classification tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Analyst-event mapping regression suite.
  - Status: Not Started

<a id="id-us06-06-01"></a>
### US06.06.01 — Search SEC filing metadata

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.06](#id-f06-06) | P1 | 5 | 38 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Risk analyst

**User story:** As a risk analyst, I want filing metadata and authoritative links so that I can locate the relevant disclosure without downloading every document.

**Acceptance criteria:**

- [ ] The tool filters by symbol/CIK, form types and date range and returns accession number, filed/report dates, form, description and validated SEC/Finnhub source URL.
- [ ] Results are pageable, newest-first by default and include amended-form relationships where available.
- [ ] External URLs are HTTPS allowlisted and filing text is treated as untrusted content rather than instructions.
- [ ] Tests cover 10-K, 10-Q, 8-K, amendments, malformed URLs and no-match behavior.

**Dependencies:** [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `sec` `filings` `P1`

**Source findings:**

- SEC filing discovery and links are missing.
- Filing URLs and text must be validated and treated as untrusted external data.

**Subtasks:**

<a id="id-st06-06-01-01"></a>
- [ ] **ST06.06.01.01 — Implement filing search and URL validation**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: SEC filing metadata service and allowlisted link validator.
  - Status: Not Started
<a id="id-st06-06-01-02"></a>
- [ ] **ST06.06.01.02 — Add filing pagination and amendment mapping**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Bounded filing-discovery tool.
  - Status: Not Started
<a id="id-st06-06-01-03"></a>
- [ ] **ST06.06.01.03 — Threat-test URLs and untrusted metadata**
  - Type: security-test
  - Estimate: 8 hours
  - Suggested owner role: Security engineer
  - Deliverable/evidence: SSRF, malformed URL and prompt-injection fixture tests.
  - Status: Not Started

<a id="id-us06-06-02"></a>
### US06.06.02 — Retrieve bounded filing sections

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.06](#id-f06-06) | P1 | 8 | 60 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Risk analyst

**User story:** As a risk analyst, I want selected filing sections in chunks so that I can analyze disclosures within my context budget.

**Acceptance criteria:**

- [ ] The tool requires an accession number and section identifier and never returns a complete 10-K by default.
- [ ] Output supports chunk cursor, max_output_tokens and stable section/chunk identifiers with source offsets or headings.
- [ ] Responses flag extraction confidence, omitted content, document amendments and untrusted-content boundaries.
- [ ] Tests cover section aliases, tables, unavailable sections, oversized sections and deterministic continuation.

**Dependencies:** [US06.06.01](#id-us06-06-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01)

**Labels:** `tool` `sec` `document-chunking` `P1`

**Source findings:**

- Filing retrieval should return metadata and chunked sections, not an entire 10-K in one response.

**Subtasks:**

<a id="id-st06-06-02-01"></a>
- [ ] **ST06.06.02.01 — Design section/chunk extraction contract**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: Document-processing engineer
  - Deliverable/evidence: Section aliases, stable chunk identifiers and confidence schema.
  - Status: Not Started
<a id="id-st06-06-02-02"></a>
- [ ] **ST06.06.02.02 — Implement bounded extraction pipeline**
  - Type: implementation
  - Estimate: 32 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Filing section retrieval service with cursor continuation.
  - Status: Not Started
<a id="id-st06-06-02-03"></a>
- [ ] **ST06.06.02.03 — Test tables, large sections and amendments**
  - Type: test
  - Estimate: 16 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Representative filing extraction corpus and regression tests.
  - Status: Not Started

<a id="id-us06-07-01"></a>
### US06.07.01 — Retrieve institutional and ownership summaries

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.07](#id-f06-07) | P2 | 5 | 36 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Portfolio analyst

**User story:** As a portfolio analyst, I want bounded ownership snapshots and changes so that I can assess concentration and investor movement.

**Acceptance criteria:**

- [ ] The tool separates aggregate ownership, institutional holders and ownership changes and labels reporting-period lag.
- [ ] Holder lists are pageable and include position, change, report date and source when licensed.
- [ ] Entitlement and access-policy checks occur before provider calls and return PremiumRequired or PermissionDenied distinctly.
- [ ] Tests cover stale reporting periods, duplicate holders, missing changes and entitlement denial.

**Dependencies:** [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `ownership` `P2`

**Source findings:**

- Ownership is a useful long-term extension and must be access/entitlement aware.

**Subtasks:**

<a id="id-st06-07-01-01"></a>
- [ ] **ST06.07.01.01 — Define ownership data and access contract**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: Ownership domain schema and entitlement matrix.
  - Status: Not Started
<a id="id-st06-07-01-02"></a>
- [ ] **ST06.07.01.02 — Implement bounded ownership tool**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Ownership summary/holder service with policy checks.
  - Status: Not Started
<a id="id-st06-07-01-03"></a>
- [ ] **ST06.07.01.03 — Test lag, duplicates and entitlement**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Ownership completeness and authorization test suite.
  - Status: Not Started

<a id="id-us06-07-02"></a>
### US06.07.02 — Retrieve ETF profile and holdings

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.07](#id-f06-07) | P2 | 6 | 42 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** ETF researcher

**User story:** As an ETF researcher, I want fund metadata and bounded holdings so that I can compare exposures without treating an ETF as a company.

**Acceptance criteria:**

- [ ] The tool returns fund profile, benchmark, asset class, expense ratio and as_of metadata when available.
- [ ] Holdings are pageable and expose weight, units/market value, currency and reporting date without normalizing unknown values to zero.
- [ ] Symbol resolution identifies ETFs and routes company-only tools away with an actionable alternative.
- [ ] Tests cover stale holdings, total weights not equal to 100 percent, missing benchmark and entitlement errors.

**Dependencies:** [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01)

**Labels:** `tool` `etf` `P2`

**Source findings:**

- ETF-specific data is a useful long-term capability absent from the equity-focused tool set.

**Subtasks:**

<a id="id-st06-07-02-01"></a>
- [ ] **ST06.07.02.01 — Model ETF profile and holdings**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: ETF-specific profile and holdings schema.
  - Status: Not Started
<a id="id-st06-07-02-02"></a>
- [ ] **ST06.07.02.02 — Implement ETF tool and symbol routing**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: ETF profile/holdings service and company-tool guardrails.
  - Status: Not Started
<a id="id-st06-07-02-03"></a>
- [ ] **ST06.07.02.03 — Test stale and incomplete holdings**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: ETF data-quality regression tests.
  - Status: Not Started

<a id="id-us06-07-03"></a>
### US06.07.03 — Add FX and crypto market utilities

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.07](#id-f06-07) | P2 | 8 | 60 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Multi-asset analyst

**User story:** As a multi-asset analyst, I want explicit FX and crypto quote/history utilities so that symbols, sessions and volume semantics are not forced through equity assumptions.

**Acceptance criteria:**

- [ ] Capabilities include pair/venue discovery plus quote and OHLCV paths that reuse the common bounded contracts.
- [ ] Responses identify base, quote, venue, market hours or 24x7 status, price precision and volume meaning.
- [ ] Unsupported venues and premium feeds fail before unrelated provider calls with actionable capability guidance.
- [ ] Tests cover inverted FX pairs, crypto venue ambiguity, 24x7 dates and symbols shared with equities.

**Dependencies:** [US06.01.01](#id-us06-01-01), [US06.08.01](#id-us06-08-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02)

**Labels:** `tool` `fx` `crypto` `P2`

**Source findings:**

- FX and crypto coverage are possible P2 extensions but require asset-specific semantics.

**Subtasks:**

<a id="id-st06-07-03-01"></a>
- [ ] **ST06.07.03.01 — Define FX/crypto identifiers and semantics**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: Market data engineer
  - Deliverable/evidence: Pair, venue, precision and volume contract.
  - Status: Not Started
<a id="id-st06-07-03-02"></a>
- [ ] **ST06.07.03.02 — Implement discovery, quote and history adapters**
  - Type: implementation
  - Estimate: 36 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: FX/crypto market utilities reusing common contracts.
  - Status: Not Started
<a id="id-st06-07-03-03"></a>
- [ ] **ST06.07.03.03 — Test ambiguity and 24x7 behavior**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Venue/pair/date boundary suite.
  - Status: Not Started

<a id="id-us06-08-01"></a>
### US06.08.01 — Attach common data provenance

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.08](#id-f06-08) | P1 | 5 | 40 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Any financial-data consumer

**User story:** As a consumer, I want provenance and freshness on every result so that I can decide whether data is fit for a financial conclusion.

**Acceptance criteria:**

- [ ] Every tool reports provider/source endpoint identifier, fetched_at, as_of, cache_age, cache_status and quota_cost without exposing credentials.
- [ ] Applicable numeric series report currency, unit, exchange time_zone, adjustment status and known delay.
- [ ] Derived results identify input ranges and a calculation/formula version; partial inputs set completeness warnings.
- [ ] Contract tests assert provenance presence and ISO-8601/ISO-4217 formatting across all tools.

**Dependencies:** —

**Labels:** `cross-cutting` `provenance` `data-quality` `P1`

**Source findings:**

- Responses need source, endpoint, fetched/as-of time, currency, time zone, delay, cache age and quota cost.

**Subtasks:**

<a id="id-st06-08-01-01"></a>
- [ ] **ST06.08.01.01 — Define provenance metadata schema**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Common provenance and completeness contract.
  - Status: Not Started
<a id="id-st06-08-01-02"></a>
- [ ] **ST06.08.01.02 — Integrate provenance into tool middleware**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Shared metadata builder applied across tools.
  - Status: Not Started
<a id="id-st06-08-01-03"></a>
- [ ] **ST06.08.01.03 — Add all-tool provenance contract tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Schema and formatting assertions for every result.
  - Status: Not Started

<a id="id-us06-09-01"></a>
### US06.09.01 — Certify corrected historical volatility semantics

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.09](#id-f06-09) | P0 | 2 | 14 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Financial-model reviewer

**User story:** As a financial-model reviewer, I want the corrected volatility implementation independently certified so that regressions cannot reintroduce price-level dispersion as investment risk.

**Acceptance criteria:**

- [ ] get-price-summary computes annualized standard deviation of log or simple returns using a documented period and annualization convention, or renames the old value to price_dispersion.
- [ ] The response exposes formula_version, observation_count, return_interval and annualization_factor.
- [ ] Calculation rejects/masks zero or negative prices, misaligned arrays and unordered/duplicate timestamps with completeness warnings rather than invalid arithmetic.
- [ ] Golden tests cover constant prices, one observation, gaps, zero prices and a hand-calculated return series.

**Dependencies:** [US05.03.01](./E05-financial-and-symbol-data-correctness.md#id-us05-03-01), [US05.03.02](./E05-financial-and-symbol-data-correctness.md#id-us05-03-02), [US06.08.01](#id-us06-08-01)

**Labels:** `financial-correctness` `price-summary` `certification` `P0`

**Source findings:**

- The current vol is population standard deviation of closing price levels rather than returns and is financially misleading.
- Price math needs zero, array-length and timestamp guards.

**Subtasks:**

<a id="id-st06-09-01-01"></a>
- [ ] **ST06.09.01.01 — Review the implemented return-volatility specification**
  - Type: review
  - Estimate: 4 hours
  - Suggested owner role: Quant engineer
  - Deliverable/evidence: Independent sign-off on formula, interval, units and annualization.
  - Status: Not Started
<a id="id-st06-09-01-02"></a>
- [ ] **ST06.09.01.02 — Reconcile implementation against independent golden vectors**
  - Type: verification
  - Estimate: 6 hours
  - Suggested owner role: Quant QA engineer
  - Deliverable/evidence: Reconciliation report proving agreement with external vectors.
  - Status: Not Started
<a id="id-st06-09-01-03"></a>
- [ ] **ST06.09.01.03 — Publish the volatility semantic certification gate**
  - Type: test
  - Estimate: 4 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: CI gate and review record for normal and edge cases.
  - Status: Not Started

<a id="id-us06-09-02"></a>
### US06.09.02 — Certify corrected insider-activity semantics

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.09](#id-f06-09) | P0 | 2 | 15 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Financial-data reviewer

**User story:** As a financial-data reviewer, I want the corrected insider taxonomy independently certified so that future changes cannot fold grants, gifts or tax withholding back into open-market conviction.

**Acceptance criteria:**

- [ ] The primary net_buy_sell signal includes documented open-market purchase/sale codes and reports other Form 4 classes separately for grants, gifts, option exercises and tax withholding.
- [ ] The field name or metadata reflects the requested window_days and never remains net_buy_sell_30d for a 90-day request.
- [ ] Counts, value methodology, unknown-code count and completeness are reported without coercing unclassified transactions into buys or sells.
- [ ] Fixtures cover P, S, A, G, M, F and unknown transaction codes plus 30/90-day boundaries.

**Dependencies:** [US05.04.01](./E05-financial-and-symbol-data-correctness.md#id-us05-04-01), [US05.04.02](./E05-financial-and-symbol-data-correctness.md#id-us05-04-02), [US06.08.01](#id-us06-08-01)

**Labels:** `financial-correctness` `insider` `certification` `P0`

**Source findings:**

- net_buy_sell_30d currently sums all signed Form 4 changes, including grants, gifts, options and tax transactions.
- The 30d field name remains even when a custom 90-day window is requested.

**Subtasks:**

<a id="id-st06-09-02-01"></a>
- [ ] **ST06.09.02.01 — Audit the implemented Form 4 transaction taxonomy**
  - Type: review
  - Estimate: 5 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: Independent audit of open-market and non-open-market mappings.
  - Status: Not Started
<a id="id-st06-09-02-02"></a>
- [ ] **ST06.09.02.02 — Reconcile insider output against independent fixtures**
  - Type: verification
  - Estimate: 6 hours
  - Suggested owner role: Financial data QA engineer
  - Deliverable/evidence: Reconciliation report for classifications, windows and units.
  - Status: Not Started
<a id="id-st06-09-02-03"></a>
- [ ] **ST06.09.02.03 — Publish the insider semantic certification gate**
  - Type: test
  - Estimate: 4 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: CI gate and review record for Form codes and date boundaries.
  - Status: Not Started

<a id="id-us06-09-03"></a>
### US06.09.03 — Certify corrected news-window and completeness semantics

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F06.09](#id-f06-09) | P0 | 2 | 14 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** News-data reviewer

**User story:** As a news-data reviewer, I want the corrected news behavior independently certified so that boundary overlap or volume-as-sentiment regressions cannot ship.

**Acceptance criteria:**

- [ ] Current and comparison windows have non-overlapping inclusive/exclusive boundaries and disclose exact UTC ranges.
- [ ] Article-count change is labeled volume trend; sentiment trend is calculated only from comparable sentiment values.
- [ ] Optional sentiment degradation handles PremiumRequired and other classified recoverable failures while still returning articles with partial/completeness metadata.
- [ ] Independent upstream work starts concurrently only after quota reservation and respects cancellation and per-call bounds.
- [ ] Tests cover boundary-day articles, missing sentiment, recoverable provider failure and partial-result ordering.

**Dependencies:** [US05.05.01](./E05-financial-and-symbol-data-correctness.md#id-us05-05-01), [US05.05.02](./E05-financial-and-symbol-data-correctness.md#id-us05-05-02), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01), [US06.08.01](#id-us06-08-01)

**Labels:** `financial-correctness` `news` `certification` `P0`

**Source findings:**

- News date windows share a boundary and can double count.
- Article-count delta is volume, not sentiment.
- Optional sentiment only degrades gracefully for PremiumRequired; recoverable failures should preserve partial news with completeness.

**Subtasks:**

<a id="id-st06-09-03-01"></a>
- [ ] **ST06.09.03.01 — Audit implemented news window and completeness semantics**
  - Type: review
  - Estimate: 4 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: Independent audit of time windows and partial-result behavior.
  - Status: Not Started
<a id="id-st06-09-03-02"></a>
- [ ] **ST06.09.03.02 — Reconcile news output against boundary and failure fixtures**
  - Type: verification
  - Estimate: 6 hours
  - Suggested owner role: Financial data QA engineer
  - Deliverable/evidence: Reconciliation report for boundaries, labels and degraded components.
  - Status: Not Started
<a id="id-st06-09-03-03"></a>
- [ ] **ST06.09.03.03 — Publish the news semantic certification gate**
  - Type: test
  - Estimate: 4 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: CI gate and review record for windows and partial sentiment.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST06.01.01.01](#id-st06-01-01-01) | [US06.01.01](#id-us06-01-01) | P1 | Define candle input/output schemas and provider mapping | design | 8 | Backend engineer | Versioned OHLCV schema, validation rules and mapping tests. | Not Started |
| [ST06.01.01.02](#id-st06-01-01-02) | [US06.01.01](#id-us06-01-01) | P1 | Implement candle service, pagination and cache policy | implementation | 20 | Backend engineer | Registered get-historical-ohlcv tool with canonical caching. | Not Started |
| [ST06.01.01.03](#id-st06-01-01-03) | [US06.01.01](#id-us06-01-01) | P1 | Add edge-case and integration tests | test | 12 | QA automation engineer | Golden and provider-fixture test suite for intraday/daily candles. | Not Started |
| [ST06.02.01.01](#id-st06-02-01-01) | [US06.02.01](#id-us06-02-01) | P1 | Specify indicator formulas and tolerances | design | 10 | Quant engineer | Formula-version specification with warm-up and null semantics. | Not Started |
| [ST06.02.01.02](#id-st06-02-01-02) | [US06.02.01](#id-us06-02-01) | P1 | Implement local indicator engine | implementation | 20 | Backend engineer | Deterministic indicator library and MCP tool adapter. | Not Started |
| [ST06.02.01.03](#id-st06-02-01-03) | [US06.02.01](#id-us06-02-01) | P1 | Validate against golden vectors | test | 14 | Quant QA engineer | Cross-checked indicator vectors and numeric edge-case tests. | Not Started |
| [ST06.03.01.01](#id-st06-03-01-01) | [US06.03.01](#id-us06-03-01) | P1 | Map dividend and split provider payloads | implementation | 16 | Backend engineer | Corporate-action domain models and provider client. | Not Started |
| [ST06.03.01.02](#id-st06-03-01-02) | [US06.03.01](#id-us06-03-01) | P1 | Implement bounded corporate-actions tool | implementation | 10 | Backend engineer | Registered tool with filtering, pagination and provenance. | Not Started |
| [ST06.03.01.03](#id-st06-03-01-03) | [US06.03.01](#id-us06-03-01) | P1 | Test special and reverse actions | test | 6 | QA automation engineer | Fixtures covering irregular dividends and split ratios. | Not Started |
| [ST06.04.01.01](#id-st06-04-01-01) | [US06.04.01](#id-us06-04-01) | P1 | Design statement normalization model | design | 12 | Financial data engineer | Statement taxonomy, unit/currency and missing-data contract. | Not Started |
| [ST06.04.01.02](#id-st06-04-01-02) | [US06.04.01](#id-us06-04-01) | P1 | Implement statement provider and projection | implementation | 28 | Backend engineer | Financial-statements service and tool. | Not Started |
| [ST06.04.01.03](#id-st06-04-01-03) | [US06.04.01](#id-us06-04-01) | P1 | Add restatement and period tests | test | 12 | QA automation engineer | Fixture suite for annual, quarterly, amended and mixed-unit data. | Not Started |
| [ST06.05.01.01](#id-st06-05-01-01) | [US06.05.01](#id-us06-05-01) | P1 | Implement earnings event mapping | implementation | 20 | Backend engineer | Earnings/estimates/surprise domain service and tool. | Not Started |
| [ST06.05.01.02](#id-st06-05-01-02) | [US06.05.01](#id-us06-05-01) | P1 | Define zero and missing estimate math | design | 6 | Financial data engineer | Documented surprise and completeness semantics. | Not Started |
| [ST06.05.01.03](#id-st06-05-01-03) | [US06.05.01](#id-us06-05-01) | P1 | Test historical and forward events | test | 8 | QA automation engineer | Boundary and revision test suite. | Not Started |
| [ST06.05.02.01](#id-st06-05-02-01) | [US06.05.02](#id-us06-05-02) | P1 | Implement target and rating-change endpoints | implementation | 20 | Backend engineer | Analyst-event service integrated with recommendations cache. | Not Started |
| [ST06.05.02.02](#id-st06-05-02-02) | [US06.05.02](#id-us06-05-02) | P1 | Add currency and action classification tests | test | 10 | QA automation engineer | Analyst-event mapping regression suite. | Not Started |
| [ST06.06.01.01](#id-st06-06-01-01) | [US06.06.01](#id-us06-06-01) | P1 | Implement filing search and URL validation | implementation | 20 | Backend engineer | SEC filing metadata service and allowlisted link validator. | Not Started |
| [ST06.06.01.02](#id-st06-06-01-02) | [US06.06.01](#id-us06-06-01) | P1 | Add filing pagination and amendment mapping | implementation | 10 | Backend engineer | Bounded filing-discovery tool. | Not Started |
| [ST06.06.01.03](#id-st06-06-01-03) | [US06.06.01](#id-us06-06-01) | P1 | Threat-test URLs and untrusted metadata | security-test | 8 | Security engineer | SSRF, malformed URL and prompt-injection fixture tests. | Not Started |
| [ST06.06.02.01](#id-st06-06-02-01) | [US06.06.02](#id-us06-06-02) | P1 | Design section/chunk extraction contract | design | 12 | Document-processing engineer | Section aliases, stable chunk identifiers and confidence schema. | Not Started |
| [ST06.06.02.02](#id-st06-06-02-02) | [US06.06.02](#id-us06-06-02) | P1 | Implement bounded extraction pipeline | implementation | 32 | Backend engineer | Filing section retrieval service with cursor continuation. | Not Started |
| [ST06.06.02.03](#id-st06-06-02-03) | [US06.06.02](#id-us06-06-02) | P1 | Test tables, large sections and amendments | test | 16 | QA automation engineer | Representative filing extraction corpus and regression tests. | Not Started |
| [ST06.07.01.01](#id-st06-07-01-01) | [US06.07.01](#id-us06-07-01) | P2 | Define ownership data and access contract | design | 8 | Financial data engineer | Ownership domain schema and entitlement matrix. | Not Started |
| [ST06.07.01.02](#id-st06-07-01-02) | [US06.07.01](#id-us06-07-01) | P2 | Implement bounded ownership tool | implementation | 20 | Backend engineer | Ownership summary/holder service with policy checks. | Not Started |
| [ST06.07.01.03](#id-st06-07-01-03) | [US06.07.01](#id-us06-07-01) | P2 | Test lag, duplicates and entitlement | test | 8 | QA automation engineer | Ownership completeness and authorization test suite. | Not Started |
| [ST06.07.02.01](#id-st06-07-02-01) | [US06.07.02](#id-us06-07-02) | P2 | Model ETF profile and holdings | design | 8 | Financial data engineer | ETF-specific profile and holdings schema. | Not Started |
| [ST06.07.02.02](#id-st06-07-02-02) | [US06.07.02](#id-us06-07-02) | P2 | Implement ETF tool and symbol routing | implementation | 24 | Backend engineer | ETF profile/holdings service and company-tool guardrails. | Not Started |
| [ST06.07.02.03](#id-st06-07-02-03) | [US06.07.02](#id-us06-07-02) | P2 | Test stale and incomplete holdings | test | 10 | QA automation engineer | ETF data-quality regression tests. | Not Started |
| [ST06.07.03.01](#id-st06-07-03-01) | [US06.07.03](#id-us06-07-03) | P2 | Define FX/crypto identifiers and semantics | design | 12 | Market data engineer | Pair, venue, precision and volume contract. | Not Started |
| [ST06.07.03.02](#id-st06-07-03-02) | [US06.07.03](#id-us06-07-03) | P2 | Implement discovery, quote and history adapters | implementation | 36 | Backend engineer | FX/crypto market utilities reusing common contracts. | Not Started |
| [ST06.07.03.03](#id-st06-07-03-03) | [US06.07.03](#id-us06-07-03) | P2 | Test ambiguity and 24x7 behavior | test | 12 | QA automation engineer | Venue/pair/date boundary suite. | Not Started |
| [ST06.08.01.01](#id-st06-08-01-01) | [US06.08.01](#id-us06-08-01) | P1 | Define provenance metadata schema | design | 10 | API architect | Common provenance and completeness contract. | Not Started |
| [ST06.08.01.02](#id-st06-08-01-02) | [US06.08.01](#id-us06-08-01) | P1 | Integrate provenance into tool middleware | implementation | 20 | Backend engineer | Shared metadata builder applied across tools. | Not Started |
| [ST06.08.01.03](#id-st06-08-01-03) | [US06.08.01](#id-us06-08-01) | P1 | Add all-tool provenance contract tests | test | 10 | QA automation engineer | Schema and formatting assertions for every result. | Not Started |
| [ST06.09.01.01](#id-st06-09-01-01) | [US06.09.01](#id-us06-09-01) | P0 | Review the implemented return-volatility specification | review | 4 | Quant engineer | Independent sign-off on formula, interval, units and annualization. | Not Started |
| [ST06.09.01.02](#id-st06-09-01-02) | [US06.09.01](#id-us06-09-01) | P0 | Reconcile implementation against independent golden vectors | verification | 6 | Quant QA engineer | Reconciliation report proving agreement with external vectors. | Not Started |
| [ST06.09.01.03](#id-st06-09-01-03) | [US06.09.01](#id-us06-09-01) | P0 | Publish the volatility semantic certification gate | test | 4 | QA automation engineer | CI gate and review record for normal and edge cases. | Not Started |
| [ST06.09.02.01](#id-st06-09-02-01) | [US06.09.02](#id-us06-09-02) | P0 | Audit the implemented Form 4 transaction taxonomy | review | 5 | Financial data engineer | Independent audit of open-market and non-open-market mappings. | Not Started |
| [ST06.09.02.02](#id-st06-09-02-02) | [US06.09.02](#id-us06-09-02) | P0 | Reconcile insider output against independent fixtures | verification | 6 | Financial data QA engineer | Reconciliation report for classifications, windows and units. | Not Started |
| [ST06.09.02.03](#id-st06-09-02-03) | [US06.09.02](#id-us06-09-02) | P0 | Publish the insider semantic certification gate | test | 4 | QA automation engineer | CI gate and review record for Form codes and date boundaries. | Not Started |
| [ST06.09.03.01](#id-st06-09-03-01) | [US06.09.03](#id-us06-09-03) | P0 | Audit implemented news window and completeness semantics | review | 4 | Financial data engineer | Independent audit of time windows and partial-result behavior. | Not Started |
| [ST06.09.03.02](#id-st06-09-03-02) | [US06.09.03](#id-us06-09-03) | P0 | Reconcile news output against boundary and failure fixtures | verification | 6 | Financial data QA engineer | Reconciliation report for boundaries, labels and degraded components. | Not Started |
| [ST06.09.03.03](#id-st06-09-03-03) | [US06.09.03](#id-us06-09-03) | P0 | Publish the news semantic certification gate | test | 4 | QA automation engineer | CI gate and review record for windows and partial sentiment. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E06 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| A-01 | A. Feature Enhancements | Assess coverage of the 12 tools and add high-value gaps such as full historical OHLCV, technical indicators, dividends/splits, financial statements, earnings intelligence, and SEC filings. | [US06.01.01](#id-us06-01-01), [US06.02.01](#id-us06-02-01), [US06.03.01](#id-us06-03-01), [US06.04.01](#id-us06-04-01), [US06.05.01](#id-us06-05-01), [US06.05.02](#id-us06-05-02), [US06.06.01](#id-us06-06-01), [US06.06.02](#id-us06-06-02), [US06.07.01](#id-us06-07-01), [US06.07.02](#id-us06-07-02), [US06.07.03](#id-us06-07-03) | [E06](#id-e06) | P1 | Covered | Explicit review question A1. |
| A-02 | A. Feature Enhancements | Improve the summary/standard/full view design with fields, limits, pagination, truncation metadata, provenance, and token budgets. | [US07.01.01](./E07-bounded-response-and-token-contract.md#id-us07-01-01), [US07.01.02](./E07-bounded-response-and-token-contract.md#id-us07-01-02), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01), [US07.03.02](./E07-bounded-response-and-token-contract.md#id-us07-03-02), [US06.08.01](#id-us06-08-01) | [E07](./E07-bounded-response-and-token-contract.md#id-e07) | P0 | Covered | Explicit review question A2. |
| R-09 | Repository finding | Correct get-price-summary volatility semantics, guards, units, array validation, timestamps, and test fixtures. | [US05.03.01](./E05-financial-and-symbol-data-correctness.md#id-us05-03-01), [US05.03.02](./E05-financial-and-symbol-data-correctness.md#id-us05-03-02), [US06.09.01](#id-us06-09-01) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Code-level financial-accuracy finding. |
| R-10 | Repository finding | Classify insider transactions correctly, separate open-market purchase/sale from grants/options/gifts/tax events, and align output window names. | [US05.04.01](./E05-financial-and-symbol-data-correctness.md#id-us05-04-01), [US05.04.02](./E05-financial-and-symbol-data-correctness.md#id-us05-04-02), [US06.09.02](#id-us06-09-02) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Code-level financial-accuracy finding. |
| R-11 | Repository finding | Use non-overlapping news windows, resilient optional sentiment behavior, completeness metadata, and quota-aware concurrency. | [US05.05.01](./E05-financial-and-symbol-data-correctness.md#id-us05-05-01), [US05.05.02](./E05-financial-and-symbol-data-correctness.md#id-us05-05-02), [US06.09.03](#id-us06-09-03) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Code-level financial/UX finding. |
| R-28 | Repository finding | Return source endpoint, fetched/as-of timestamps, currency, timezone, delay status, cache age, and quota cost with financial outputs. | [US06.08.01](#id-us06-08-01) | [E06](#id-e06) | P1 | Covered | Financial provenance finding. |
| RF-112 | Code-review detail | A1 - Missing historical OHLCV, technical indicators, dividends/splits, statements, earnings/estimates, filings and broader asset tools | [US06.01.01](#id-us06-01-01), [US06.02.01](#id-us06-02-01), [US06.03.01](#id-us06-03-01), [US06.04.01](#id-us06-04-01), [US06.05.01](#id-us06-05-01), [US06.05.02](#id-us06-05-02), [US06.06.01](#id-us06-06-01), [US06.06.02](#id-us06-06-02), [US06.07.01](#id-us06-07-01), [US06.07.02](#id-us06-07-02), [US06.07.03](#id-us06-07-03) | [E06](#id-e06) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-130 | Code-review detail | Cross-cutting - every financial payload needs provenance, units, freshness, completeness, cache and quota metadata | [US06.08.01](#id-us06-08-01), [US07.03.02](./E07-bounded-response-and-token-contract.md#id-us07-03-02) | [E06](#id-e06) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-133 | Code-review detail | Observed financial defect - price-level standard deviation is mislabeled as volatility | [US06.09.01](#id-us06-09-01) | [E06](#id-e06) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-134 | Code-review detail | Observed financial defect - insider net buy/sell mixes grants, gifts, options and tax transactions and hard-codes a 30-day label | [US06.09.02](#id-us06-09-02) | [E06](#id-e06) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-135 | Code-review detail | Observed financial defect - news windows overlap and article-volume change can be mislabeled as sentiment | [US06.09.03](#id-us06-09-03), [US08.04.01](./E08-intelligent-discovery-and-context-engineering.md#id-us08-04-01) | [E06](#id-e06) | P0 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 3. M1 — Contract & Core Data

- **Priority:** P1
- **Indicative window:** Weeks 8–18
- **Primary epics:** E06–E09
- **Goal:** Stabilize output contracts and add the highest-value financial workflows.
- **Entry criteria:** M0B exit gates met; output budget and provider-entitlement policies approved.
- **Exit criteria:** View/fields/pagination/provenance shipped; OHLCV, indicators, corporate actions, statements, filings, and bounded batches available.
- **Delivery note:** Sequence premium-dependent endpoints behind capability flags.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E06 | Epic | — | P1 | Financial Data Coverage and Semantics | See [E06](#id-e06) | See [E06](#id-e06) | — | finnhub-mcp; epic | — | Not Started |
| F06.01 | Feature | [E06](#id-e06) | P1 | Historical OHLCV tool | See [F06.01](#id-f06-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.02 | Feature | [E06](#id-e06) | P1 | Local technical indicators | See [F06.02](#id-f06-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.03 | Feature | [E06](#id-e06) | P1 | Corporate actions | See [F06.03](#id-f06-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.04 | Feature | [E06](#id-e06) | P1 | Financial statements | See [F06.04](#id-f06-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.05 | Feature | [E06](#id-e06) | P1 | Earnings and analyst events | See [F06.05](#id-f06-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.06 | Feature | [E06](#id-e06) | P1 | SEC filing discovery and sections | See [F06.06](#id-f06-06) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.07 | Feature | [E06](#id-e06) | P2 | Ownership and multi-asset coverage | See [F06.07](#id-f06-07) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.08 | Feature | [E06](#id-e06) | P1 | Financial provenance contract | See [F06.08](#id-f06-08) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| F06.09 | Feature | [E06](#id-e06) | P0 | Financial-signal semantic certification | See [F06.09](#id-f06-09) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e06 | — | Not Started |
| US06.01.01 | Story | [F06.01](#id-f06-01) | P1 | Query bounded historical candles | See [US06.01.01](#id-us06-01-01) | See [US06.01.01](#id-us06-01-01) | 5d | tool; ohlcv; market-data; P1 | [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.02.01 | Story | [F06.02](#id-f06-02) | P1 | Compute indicators from cached OHLCV | See [US06.02.01](#id-us06-02-01) | See [US06.02.01](#id-us06-02-01) | 6d | tool; technical-analysis; derived-data; P1 | [US06.01.01](#id-us06-01-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.03.01 | Story | [F06.03](#id-f06-03) | P1 | Retrieve dividends and splits | See [US06.03.01](#id-us06-03-01) | See [US06.03.01](#id-us06-03-01) | 4d | tool; corporate-actions; dividends; splits; P1 | [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.04.01 | Story | [F06.04](#id-f06-04) | P1 | Retrieve normalized financial statements | See [US06.04.01](#id-us06-04-01) | See [US06.04.01](#id-us06-04-01) | 7d | tool; fundamentals; statements; P1 | [US07.01.02](./E07-bounded-response-and-token-contract.md#id-us07-01-02), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.05.01 | Story | [F06.05](#id-f06-05) | P1 | Retrieve earnings results, estimates and surprises | See [US06.05.01](#id-us06-05-01) | See [US06.05.01](#id-us06-05-01) | 5d | tool; earnings; estimates; P1 | [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.05.02 | Story | [F06.05](#id-f06-05) | P1 | Expose price targets and analyst-change events | See [US06.05.02](#id-us06-05-02) | See [US06.05.02](#id-us06-05-02) | 4d | tool; analyst; price-targets; P1 | [US06.08.01](#id-us06-08-01), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01) | Not Started |
| US06.06.01 | Story | [F06.06](#id-f06-06) | P1 | Search SEC filing metadata | See [US06.06.01](#id-us06-06-01) | See [US06.06.01](#id-us06-06-01) | 5d | tool; sec; filings; P1 | [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.06.02 | Story | [F06.06](#id-f06-06) | P1 | Retrieve bounded filing sections | See [US06.06.02](#id-us06-06-02) | See [US06.06.02](#id-us06-06-02) | 8d | tool; sec; document-chunking; P1 | [US06.06.01](#id-us06-06-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01) | Not Started |
| US06.07.01 | Story | [F06.07](#id-f06-07) | P2 | Retrieve institutional and ownership summaries | See [US06.07.01](#id-us06-07-01) | See [US06.07.01](#id-us06-07-01) | 5d | tool; ownership; P2 | [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.07.02 | Story | [F06.07](#id-f06-07) | P2 | Retrieve ETF profile and holdings | See [US06.07.02](#id-us06-07-02) | See [US06.07.02](#id-us06-07-02) | 6d | tool; etf; P2 | [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.07.03 | Story | [F06.07](#id-f06-07) | P2 | Add FX and crypto market utilities | See [US06.07.03](#id-us06-07-03) | See [US06.07.03](#id-us06-07-03) | 8d | tool; fx; crypto; P2 | [US06.01.01](#id-us06-01-01), [US06.08.01](#id-us06-08-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02) | Not Started |
| US06.08.01 | Story | [F06.08](#id-f06-08) | P1 | Attach common data provenance | See [US06.08.01](#id-us06-08-01) | See [US06.08.01](#id-us06-08-01) | 5d | cross-cutting; provenance; data-quality; P1 | — | Not Started |
| US06.09.01 | Story | [F06.09](#id-f06-09) | P0 | Certify corrected historical volatility semantics | See [US06.09.01](#id-us06-09-01) | See [US06.09.01](#id-us06-09-01) | 2d | financial-correctness; price-summary; certification; P0 | [US05.03.01](./E05-financial-and-symbol-data-correctness.md#id-us05-03-01), [US05.03.02](./E05-financial-and-symbol-data-correctness.md#id-us05-03-02), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.09.02 | Story | [F06.09](#id-f06-09) | P0 | Certify corrected insider-activity semantics | See [US06.09.02](#id-us06-09-02) | See [US06.09.02](#id-us06-09-02) | 2d | financial-correctness; insider; certification; P0 | [US05.04.01](./E05-financial-and-symbol-data-correctness.md#id-us05-04-01), [US05.04.02](./E05-financial-and-symbol-data-correctness.md#id-us05-04-02), [US06.08.01](#id-us06-08-01) | Not Started |
| US06.09.03 | Story | [F06.09](#id-f06-09) | P0 | Certify corrected news-window and completeness semantics | See [US06.09.03](#id-us06-09-03) | See [US06.09.03](#id-us06-09-03) | 2d | financial-correctness; news; certification; P0 | [US05.05.01](./E05-financial-and-symbol-data-correctness.md#id-us05-05-01), [US05.05.02](./E05-financial-and-symbol-data-correctness.md#id-us05-05-02), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01), [US06.08.01](#id-us06-08-01) | Not Started |
| ST06.01.01.01 | Sub-task | [US06.01.01](#id-us06-01-01) | P1 | Define candle input/output schemas and provider mapping | See [ST06.01.01.01](#id-st06-01-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST06.01.01.02 | Sub-task | [US06.01.01](#id-us06-01-01) | P1 | Implement candle service, pagination and cache policy | See [ST06.01.01.02](#id-st06-01-01-02) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.01.01.03 | Sub-task | [US06.01.01](#id-us06-01-01) | P1 | Add edge-case and integration tests | See [ST06.01.01.03](#id-st06-01-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |
| ST06.02.01.01 | Sub-task | [US06.02.01](#id-us06-02-01) | P1 | Specify indicator formulas and tolerances | See [ST06.02.01.01](#id-st06-02-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST06.02.01.02 | Sub-task | [US06.02.01](#id-us06-02-01) | P1 | Implement local indicator engine | See [ST06.02.01.02](#id-st06-02-01-02) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.02.01.03 | Sub-task | [US06.02.01](#id-us06-02-01) | P1 | Validate against golden vectors | See [ST06.02.01.03](#id-st06-02-01-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; test | — | Not Started |
| ST06.03.01.01 | Sub-task | [US06.03.01](#id-us06-03-01) | P1 | Map dividend and split provider payloads | See [ST06.03.01.01](#id-st06-03-01-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST06.03.01.02 | Sub-task | [US06.03.01](#id-us06-03-01) | P1 | Implement bounded corporate-actions tool | See [ST06.03.01.02](#id-st06-03-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST06.03.01.03 | Sub-task | [US06.03.01](#id-us06-03-01) | P1 | Test special and reverse actions | See [ST06.03.01.03](#id-st06-03-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; test | — | Not Started |
| ST06.04.01.01 | Sub-task | [US06.04.01](#id-us06-04-01) | P1 | Design statement normalization model | See [ST06.04.01.01](#id-st06-04-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST06.04.01.02 | Sub-task | [US06.04.01](#id-us06-04-01) | P1 | Implement statement provider and projection | See [ST06.04.01.02](#id-st06-04-01-02) | Not applicable; see detail or parent section | 28h | finnhub-mcp; implementation | — | Not Started |
| ST06.04.01.03 | Sub-task | [US06.04.01](#id-us06-04-01) | P1 | Add restatement and period tests | See [ST06.04.01.03](#id-st06-04-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |
| ST06.05.01.01 | Sub-task | [US06.05.01](#id-us06-05-01) | P1 | Implement earnings event mapping | See [ST06.05.01.01](#id-st06-05-01-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.05.01.02 | Sub-task | [US06.05.01](#id-us06-05-01) | P1 | Define zero and missing estimate math | See [ST06.05.01.02](#id-st06-05-01-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST06.05.01.03 | Sub-task | [US06.05.01](#id-us06-05-01) | P1 | Test historical and forward events | See [ST06.05.01.03](#id-st06-05-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST06.05.02.01 | Sub-task | [US06.05.02](#id-us06-05-02) | P1 | Implement target and rating-change endpoints | See [ST06.05.02.01](#id-st06-05-02-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.05.02.02 | Sub-task | [US06.05.02](#id-us06-05-02) | P1 | Add currency and action classification tests | See [ST06.05.02.02](#id-st06-05-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST06.06.01.01 | Sub-task | [US06.06.01](#id-us06-06-01) | P1 | Implement filing search and URL validation | See [ST06.06.01.01](#id-st06-06-01-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.06.01.02 | Sub-task | [US06.06.01](#id-us06-06-01) | P1 | Add filing pagination and amendment mapping | See [ST06.06.01.02](#id-st06-06-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST06.06.01.03 | Sub-task | [US06.06.01](#id-us06-06-01) | P1 | Threat-test URLs and untrusted metadata | See [ST06.06.01.03](#id-st06-06-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; security-test | — | Not Started |
| ST06.06.02.01 | Sub-task | [US06.06.02](#id-us06-06-02) | P1 | Design section/chunk extraction contract | See [ST06.06.02.01](#id-st06-06-02-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST06.06.02.02 | Sub-task | [US06.06.02](#id-us06-06-02) | P1 | Implement bounded extraction pipeline | See [ST06.06.02.02](#id-st06-06-02-02) | Not applicable; see detail or parent section | 32h | finnhub-mcp; implementation | — | Not Started |
| ST06.06.02.03 | Sub-task | [US06.06.02](#id-us06-06-02) | P1 | Test tables, large sections and amendments | See [ST06.06.02.03](#id-st06-06-02-03) | Not applicable; see detail or parent section | 16h | finnhub-mcp; test | — | Not Started |
| ST06.07.01.01 | Sub-task | [US06.07.01](#id-us06-07-01) | P2 | Define ownership data and access contract | See [ST06.07.01.01](#id-st06-07-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST06.07.01.02 | Sub-task | [US06.07.01](#id-us06-07-01) | P2 | Implement bounded ownership tool | See [ST06.07.01.02](#id-st06-07-01-02) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.07.01.03 | Sub-task | [US06.07.01](#id-us06-07-01) | P2 | Test lag, duplicates and entitlement | See [ST06.07.01.03](#id-st06-07-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST06.07.02.01 | Sub-task | [US06.07.02](#id-us06-07-02) | P2 | Model ETF profile and holdings | See [ST06.07.02.01](#id-st06-07-02-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST06.07.02.02 | Sub-task | [US06.07.02](#id-us06-07-02) | P2 | Implement ETF tool and symbol routing | See [ST06.07.02.02](#id-st06-07-02-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST06.07.02.03 | Sub-task | [US06.07.02](#id-us06-07-02) | P2 | Test stale and incomplete holdings | See [ST06.07.02.03](#id-st06-07-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST06.07.03.01 | Sub-task | [US06.07.03](#id-us06-07-03) | P2 | Define FX/crypto identifiers and semantics | See [ST06.07.03.01](#id-st06-07-03-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST06.07.03.02 | Sub-task | [US06.07.03](#id-us06-07-03) | P2 | Implement discovery, quote and history adapters | See [ST06.07.03.02](#id-st06-07-03-02) | Not applicable; see detail or parent section | 36h | finnhub-mcp; implementation | — | Not Started |
| ST06.07.03.03 | Sub-task | [US06.07.03](#id-us06-07-03) | P2 | Test ambiguity and 24x7 behavior | See [ST06.07.03.03](#id-st06-07-03-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |
| ST06.08.01.01 | Sub-task | [US06.08.01](#id-us06-08-01) | P1 | Define provenance metadata schema | See [ST06.08.01.01](#id-st06-08-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST06.08.01.02 | Sub-task | [US06.08.01](#id-us06-08-01) | P1 | Integrate provenance into tool middleware | See [ST06.08.01.02](#id-st06-08-01-02) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST06.08.01.03 | Sub-task | [US06.08.01](#id-us06-08-01) | P1 | Add all-tool provenance contract tests | See [ST06.08.01.03](#id-st06-08-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST06.09.01.01 | Sub-task | [US06.09.01](#id-us06-09-01) | P0 | Review the implemented return-volatility specification | See [ST06.09.01.01](#id-st06-09-01-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; review | — | Not Started |
| ST06.09.01.02 | Sub-task | [US06.09.01](#id-us06-09-01) | P0 | Reconcile implementation against independent golden vectors | See [ST06.09.01.02](#id-st06-09-01-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; verification | — | Not Started |
| ST06.09.01.03 | Sub-task | [US06.09.01](#id-us06-09-01) | P0 | Publish the volatility semantic certification gate | See [ST06.09.01.03](#id-st06-09-01-03) | Not applicable; see detail or parent section | 4h | finnhub-mcp; test | — | Not Started |
| ST06.09.02.01 | Sub-task | [US06.09.02](#id-us06-09-02) | P0 | Audit the implemented Form 4 transaction taxonomy | See [ST06.09.02.01](#id-st06-09-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; review | — | Not Started |
| ST06.09.02.02 | Sub-task | [US06.09.02](#id-us06-09-02) | P0 | Reconcile insider output against independent fixtures | See [ST06.09.02.02](#id-st06-09-02-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; verification | — | Not Started |
| ST06.09.02.03 | Sub-task | [US06.09.02](#id-us06-09-02) | P0 | Publish the insider semantic certification gate | See [ST06.09.02.03](#id-st06-09-02-03) | Not applicable; see detail or parent section | 4h | finnhub-mcp; test | — | Not Started |
| ST06.09.03.01 | Sub-task | [US06.09.03](#id-us06-09-03) | P0 | Audit implemented news window and completeness semantics | See [ST06.09.03.01](#id-st06-09-03-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; review | — | Not Started |
| ST06.09.03.02 | Sub-task | [US06.09.03](#id-us06-09-03) | P0 | Reconcile news output against boundary and failure fixtures | See [ST06.09.03.02](#id-st06-09-03-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; verification | — | Not Started |
| ST06.09.03.03 | Sub-task | [US06.09.03](#id-us06-09-03) | P0 | Publish the news semantic certification gate | See [ST06.09.03.03](#id-st06-09-03-03) | Not applicable; see detail or parent section | 4h | finnhub-mcp; test | — | Not Started |

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

- [ ] Create E06 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 9 Features under E06.
- [ ] Create all 15 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 44 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 11 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 69 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

