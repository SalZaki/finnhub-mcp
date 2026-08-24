---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E08
title: "Intelligent Discovery and Context Engineering"
priority: P1
phase: "M2 — Operability & Adoption"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 11
  subtasks: 32
  traceability_owned: 18
  traceability_items: 21
story_estimate_days: 57
subtask_estimate_hours: 407
---

<a id="id-e08"></a>
# E08 — Intelligent Discovery and Context Engineering

This is the self-contained coding-agent backlog for E08. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E08 | P1 | 5 | 11 | 32 | 57 | 407 | M2 — Operability & Adoption | Not Started |

## 2. Epic Definition

**Objective:** Improve tool selection, follow-up recommendations and prompt workflows using measured retrieval quality rather than untested description growth.

**Business value:** Raises task completion while reducing invalid calls, schema tokens, wasted quota and model-specific routing failures.

**Exit criteria:**

- [ ] Search and routing are evaluated against a versioned multilingual intent corpus with published metrics.
- [ ] Next actions are valid, context-aware, quota-aware and bounded to useful recommendations.
- [ ] Tool descriptions and prompts meet token budgets and pass cross-model task evaluations.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F08.01](#id-f08-01) | P1 | Context-aware next actions | 2 | 9 | Not Started |
| [F08.02](#id-f08-02) | P1 | Measured BM25 discovery | 3 | 18 | Not Started |
| [F08.03](#id-f08-03) | P1 | Tool-description and model evaluation | 3 | 15 | Not Started |
| [F08.04](#id-f08-04) | P1 | Correct, configurable prompt workflows | 2 | 10 | Not Started |
| [F08.05](#id-f08-05) | P0 | Reliable symbol resolution | 1 | 5 | Not Started |

<a id="id-f08-01"></a>
### F08.01 — Context-aware next actions

- **Parent Epic:** [E08](#id-e08)
- **Priority:** P1
- **Status:** Not Started

**Description:** Rank valid follow-up actions using task state, cost, entitlement, cache and remaining context.

**Expected outcome:** Agents receive fewer, more useful and executable follow-ups.

**Stories:**

- [US08.01.01](#id-us08-01-01) — Rank context-aware next actions (P1, 6d)
- [US08.01.02](#id-us08-01-02) — Guarantee executable next-action schemas (P0, 3d)

<a id="id-f08-02"></a>
### F08.02 — Measured BM25 discovery

- **Parent Epic:** [E08](#id-e08)
- **Priority:** P1
- **Status:** Not Started

**Description:** Build a repeatable search evaluation, language normalization and controlled index lifecycle.

**Expected outcome:** Tool discovery quality is measurable and improves safely as the catalogue evolves.

**Stories:**

- [US08.02.01](#id-us08-02-01) — Build a labeled tool-discovery benchmark (P1, 6d)
- [US08.02.02](#id-us08-02-02) — Improve multilingual and fuzzy intent matching (P1, 7d)
- [US08.02.03](#id-us08-02-03) — Control index rebuild and feedback learning (P1, 5d)

<a id="id-f08-03"></a>
### F08.03 — Tool-description and model evaluation

- **Parent Epic:** [E08](#id-e08)
- **Priority:** P1
- **Status:** Not Started

**Description:** Constrain schema text and test neutral descriptions across Claude, GPT and Gemini families.

**Expected outcome:** Tool routing improves without paying an unbounded schema-token tax or maintaining folklore-based variants.

**Stories:**

- [US08.03.01](#id-us08-03-01) — Set concise tool-description budgets (P1, 4d)
- [US08.03.02](#id-us08-03-02) — Run cross-model tool-use evaluations (P1, 8d)
- [US08.03.03](#id-us08-03-03) — Add targeted contrastive few-shot guidance (P2, 3d)

<a id="id-f08-04"></a>
### F08.04 — Correct, configurable prompt workflows

- **Parent Epic:** [E08](#id-e08)
- **Priority:** P1
- **Status:** Not Started

**Description:** Align prompts with available data and support bounded, versioned workflow templates.

**Expected outcome:** Built-in research prompts make supportable claims and adapt to user depth, period and budget.

**Stories:**

- [US08.04.01](#id-us08-04-01) — Correct built-in prompt workflows (P0, 3d)
- [US08.04.02](#id-us08-04-02) — Support bounded workflow templates (P1, 7d)

<a id="id-f08-05"></a>
### F08.05 — Reliable symbol resolution

- **Parent Epic:** [E08](#id-e08)
- **Priority:** P0
- **Status:** Not Started

**Description:** Make name, ISIN, CUSIP and share-class resolution scored, deterministic, cache-efficient and reusable by follow-up logic.

**Expected outcome:** Agents resolve instruments accurately without duplicate Finnhub calls or false confidence.

**Stories:**

- [US08.05.01](#id-us08-05-01) — Score and reuse symbol-search candidates (P0, 5d)

## 4. User Stories and Subtasks

<a id="id-us08-01-01"></a>
### US08.01.01 — Rank context-aware next actions

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.01](#id-f08-01) | P1 | 6 | 48 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** AI research agent

**User story:** As an AI research agent, I want follow-ups ranked against my current task and constraints so that I do not loop or waste quota/context.

**Acceptance criteria:**

- [ ] The scorer considers user goal, current result signals, visited tools, remaining_calls, remaining_context_tokens, entitlement, estimated quota cost, cache state and expected output size.
- [ ] At most two actions are returned with tool, args, priority, rationale/condition, estimated_calls, estimated_tokens and premium flag.
- [ ] Actions already visited or impossible under entitlement/budget are suppressed unless explicitly justified as a retry.
- [ ] Scenario tests cover research, peer comparison, news event, quota-low, context-low and premium-unavailable states.

**Dependencies:** [US07.03.02](./E07-bounded-response-and-token-contract.md#id-us07-03-02), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01)

**Labels:** `next-actions` `routing` `context` `P1`

**Source findings:**

- next_actions is a static cycle and should consider result signals, goal, visited tools, context, quota, cache and entitlement.
- Recommendations should be limited and disclose cost/tokens/premium.

**Subtasks:**

<a id="id-st08-01-01-01"></a>
- [ ] **ST08.01.01.01 — Define recommendation feature vector and scoring policy**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Explainable next-action scoring specification.
  - Status: Not Started
<a id="id-st08-01-01-02"></a>
- [ ] **ST08.01.01.02 — Implement context-aware action scorer**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Shared recommendation service with bounded actions.
  - Status: Not Started
<a id="id-st08-01-01-03"></a>
- [ ] **ST08.01.01.03 — Build scenario evaluation suite**
  - Type: evaluation
  - Estimate: 12 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Golden recommendation scenarios and ranking metrics.
  - Status: Not Started

<a id="id-us08-01-02"></a>
### US08.01.02 — Guarantee executable next-action schemas

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.01](#id-f08-01) | P0 | 3 | 19 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** AI research agent

**User story:** As an AI agent, I want every recommended action to validate against the target schema so that executing it never fails because of malformed arguments.

**Acceptance criteria:**

- [ ] args is a structured JSON object, not a stringified dictionary, and validates against the currently enabled target tool schema.
- [ ] The exchange-symbols recommendation no longer calls search-symbol with only exchange and supplies a valid query or recommends the correct tool.
- [ ] A global contract test enumerates every emitted action from every tool/view and validates target existence, enablement and required arguments.
- [ ] Invalid candidate actions are logged as internal defects and omitted from user responses.

**Dependencies:** [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01)

**Labels:** `next-actions` `schema` `bug` `P0`

**Source findings:**

- get-exchange-symbols emits a search-symbol action containing exchange but missing required query.
- There is no global schema-validation test for next_actions.

**Subtasks:**

<a id="id-st08-01-02-01"></a>
- [ ] **ST08.01.02.01 — Represent action args as typed JSON**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Structured next-action contract.
  - Status: Not Started
<a id="id-st08-01-02-02"></a>
- [ ] **ST08.01.02.02 — Repair invalid exchange-symbol action**
  - Type: bugfix
  - Estimate: 3 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Executable replacement action and regression test.
  - Status: Not Started
<a id="id-st08-01-02-03"></a>
- [ ] **ST08.01.02.03 — Generate global action-schema test**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: All-emitter/target schema validation suite.
  - Status: Not Started

<a id="id-us08-02-01"></a>
### US08.02.01 — Build a labeled tool-discovery benchmark

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.02](#id-f08-02) | P1 | 6 | 44 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Maintainer

**User story:** As a maintainer, I want a versioned intent corpus and metrics so that search-tools changes can be evaluated objectively.

**Acceptance criteria:**

- [ ] The initial corpus contains 100-300 labeled intents spanning positive, negative, ambiguous, multi-tool and paraphrased cases for all enabled tools.
- [ ] Evaluation reports tool@1, Recall@3, MRR, nDCG, zero-match quality and top confusion pairs against a fixed baseline.
- [ ] CI blocks statistically meaningful regression at agreed thresholds and publishes a machine-readable report artifact.
- [ ] At least one held-out set is maintained separately from tuning examples to reduce overfitting.

**Dependencies:** [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01)

**Labels:** `bm25` `evals` `search-tools` `P1`

**Source findings:**

- Only two real ranking acceptance cases exist; search quality needs a 100-300 intent corpus and retrieval metrics.

**Subtasks:**

<a id="id-st08-02-01-01"></a>
- [ ] **ST08.02.01.01 — Author labeled intent corpus**
  - Type: data
  - Estimate: 24 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Versioned 100-300-case train/held-out corpus.
  - Status: Not Started
<a id="id-st08-02-01-02"></a>
- [ ] **ST08.02.01.02 — Implement retrieval metric harness**
  - Type: evaluation
  - Estimate: 14 hours
  - Suggested owner role: Search engineer
  - Deliverable/evidence: Tool@1, Recall@3, MRR, nDCG and confusion reports.
  - Status: Not Started
<a id="id-st08-02-01-03"></a>
- [ ] **ST08.02.01.03 — Add CI baseline gate**
  - Type: devops
  - Estimate: 6 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Regression thresholds and published report artifact.
  - Status: Not Started

<a id="id-us08-02-02"></a>
### US08.02.02 — Improve multilingual and fuzzy intent matching

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.02](#id-f08-02) | P1 | 7 | 48 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** International MCP user

**User story:** As an international user, I want discovery to handle natural variants, typos and non-English intents so that ASCII keywords are not the only reliable path.

**Acceptance criteria:**

- [ ] Normalization supports Unicode, case folding and safe punctuation while preserving financial tokens such as P/E, 10-K and tickers.
- [ ] A curated synonym/concept layer covers common finance terms and light typo tolerance without turning raw BM25 score into a probability.
- [ ] The benchmark includes Chinese and at least one additional language plus transliterated, acronym and misspelled intents.
- [ ] Search output labels relevance scores as relative ranking scores and exposes matched concepts for debugging without exposing private feedback.

**Dependencies:** [US08.02.01](#id-us08-02-01)

**Labels:** `bm25` `multilingual` `fuzzy-search` `P1`

**Source findings:**

- The index is ASCII-oriented and lacks stemming/synonyms/typo/CJK handling; raw scores are uncalibrated.
- Example intents do not cover enough intent variation or multilingual phrasing.

**Subtasks:**

<a id="id-st08-02-02-01"></a>
- [ ] **ST08.02.02.01 — Implement Unicode finance-aware normalization**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Search engineer
  - Deliverable/evidence: Tokenizer/normalizer preserving financial syntax.
  - Status: Not Started
<a id="id-st08-02-02-02"></a>
- [ ] **ST08.02.02.02 — Add synonyms and controlled typo tolerance**
  - Type: implementation
  - Estimate: 18 hours
  - Suggested owner role: Search engineer
  - Deliverable/evidence: Curated concept expansion and fuzzy-match layer.
  - Status: Not Started
<a id="id-st08-02-02-03"></a>
- [ ] **ST08.02.02.03 — Expand multilingual benchmark**
  - Type: data
  - Estimate: 14 hours
  - Suggested owner role: Localization QA engineer
  - Deliverable/evidence: Chinese, additional-language and typo evaluation cases.
  - Status: Not Started

<a id="id-us08-02-03"></a>
### US08.02.03 — Control index rebuild and feedback learning

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.02](#id-f08-02) | P1 | 5 | 38 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Maintainer

**User story:** As a maintainer, I want reproducible rebuilds and privacy-safe feedback so that discovery evolves without poisoning production ranking.

**Acceptance criteria:**

- [ ] The immutable BM25 snapshot rebuilds at startup/catalogue version change or explicit administrative reload, not on an arbitrary timer for a static catalogue.
- [ ] Rebuilds are atomic, versioned and retain the prior snapshot on validation failure.
- [ ] Feedback is explicit opt-in, stores no raw sensitive intent by default and feeds an offline reviewed evaluation/tuning pipeline rather than online self-learning.
- [ ] Poisoning, privacy retention and minimum-sample controls are documented and covered by administrative tests.

**Dependencies:** [US08.02.01](#id-us08-02-01), [US10.03.02](./E10-service-operations-resources-and-extensibility.md#id-us10-03-02)

**Labels:** `bm25` `feedback` `privacy` `P1`

**Source findings:**

- Periodic rebuild is unnecessary for the current static 12-tool catalogue; rebuild should follow catalogue change.
- User feedback can help offline but online learning creates privacy and poisoning risks.

**Subtasks:**

<a id="id-st08-02-03-01"></a>
- [ ] **ST08.02.03.01 — Implement atomic versioned index snapshots**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Search engineer
  - Deliverable/evidence: Catalogue-triggered rebuild and rollback path.
  - Status: Not Started
<a id="id-st08-02-03-02"></a>
- [ ] **ST08.02.03.02 — Design privacy-safe feedback event**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: Privacy engineer
  - Deliverable/evidence: Opt-in, minimized feedback schema and retention policy.
  - Status: Not Started
<a id="id-st08-02-03-03"></a>
- [ ] **ST08.02.03.03 — Build reviewed offline tuning workflow**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: Search engineer
  - Deliverable/evidence: Feedback-to-corpus review pipeline with poisoning controls.
  - Status: Not Started

<a id="id-us08-03-01"></a>
### US08.03.01 — Set concise tool-description budgets

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.03](#id-f08-03) | P1 | 4 | 30 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** LLM application developer

**User story:** As an application developer, I want compact, consistent descriptions so that tool schemas do not consume avoidable context.

**Acceptance criteria:**

- [ ] Each tool description follows Use when, Do not use when, key inputs, output/freshness format and targets 50-120 tokens unless an approved exception exists.
- [ ] Detailed field definitions, examples and premium notes move to capabilities/API resources rather than repeating across tools.
- [ ] CI records tools/list character and token estimates, schema order and hashes and blocks budget regressions.
- [ ] Description changes are linked to discovery/task evaluation results rather than subjective length alone.

**Dependencies:** [US08.02.01](#id-us08-02-01)

**Labels:** `tool-descriptions` `context-budget` `P1`

**Source findings:**

- The twelve descriptions total roughly 16,833 characters before schemas and should be shortened to a consistent 50-120 token contract.

**Subtasks:**

<a id="id-st08-03-01-01"></a>
- [ ] **ST08.03.01.01 — Rewrite descriptions to common template**
  - Type: content
  - Estimate: 16 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Concise descriptions for all enabled tools.
  - Status: Not Started
<a id="id-st08-03-01-02"></a>
- [ ] **ST08.03.01.02 — Move verbose field help into resources**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Capabilities/API resource content and cross-links.
  - Status: Not Started
<a id="id-st08-03-01-03"></a>
- [ ] **ST08.03.01.03 — Add tools/list budget guard**
  - Type: test
  - Estimate: 6 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Schema token, order and hash CI report.
  - Status: Not Started

<a id="id-us08-03-02"></a>
### US08.03.02 — Run cross-model tool-use evaluations

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.03](#id-f08-03) | P1 | 8 | 56 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Maintainer

**User story:** As a maintainer, I want the same task suite run across major model families so that routing changes are supported by evidence.

**Acceptance criteria:**

- [ ] A canonical neutral suite runs against representative Claude, GPT and Gemini configurations with pinned model/version metadata.
- [ ] Reports include tool@1/top3, invalid-argument rate, calls/task, quota/task, schema and response tokens, latency, unsupported-claim rate and end-to-end task success.
- [ ] Baseline and candidate prompts/descriptions are A/B compared on the same cases with confidence intervals or repeated-run variance.
- [ ] No model-specific production variant is introduced unless it materially improves an agreed metric without unacceptable regressions.

**Dependencies:** [US08.02.01](#id-us08-02-01), [US08.03.01](#id-us08-03-01)

**Labels:** `evals` `llm` `ab-test` `P1`

**Source findings:**

- There is no A/B test data for tool descriptions.
- Descriptions should be tested for Claude, GPT and Gemini, with neutral defaults and model-specific tuning only when proven.

**Subtasks:**

<a id="id-st08-03-02-01"></a>
- [ ] **ST08.03.02.01 — Define cross-model task suite and metrics**
  - Type: design
  - Estimate: 14 hours
  - Suggested owner role: Evaluation engineer
  - Deliverable/evidence: Pinned evaluation protocol and neutral baseline.
  - Status: Not Started
<a id="id-st08-03-02-02"></a>
- [ ] **ST08.03.02.02 — Implement model-adapter evaluation runner**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Evaluation engineer
  - Deliverable/evidence: Repeatable Claude/GPT/Gemini runner with cost controls.
  - Status: Not Started
<a id="id-st08-03-02-03"></a>
- [ ] **ST08.03.02.03 — Publish baseline and A/B report**
  - Type: evaluation
  - Estimate: 18 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Routing, argument, cost, latency and task-success comparison.
  - Status: Not Started

<a id="id-us08-03-03"></a>
### US08.03.03 — Add targeted contrastive few-shot guidance

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.03](#id-f08-03) | P2 | 3 | 18 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** AI research agent

**User story:** As an AI agent, I want a small number of contrastive examples for ambiguous choices so that I distinguish similar tools without bloated schemas.

**Acceptance criteria:**

- [ ] Few-shot examples are limited to empirically confused pairs and the search-tools/workflow resources, not duplicated in every description.
- [ ] Examples include both choose and do-not-choose cases with valid arguments and bounded outputs.
- [ ] The cross-model suite demonstrates improvement over the no-example baseline before release.
- [ ] Examples are versioned with the canonical intent corpus and removed when no longer beneficial.

**Dependencies:** [US08.03.02](#id-us08-03-02)

**Labels:** `few-shot` `context-engineering` `P2`

**Source findings:**

- Few-shot examples can help ambiguous tools but should be sparse and evaluation-backed rather than added everywhere.

**Subtasks:**

<a id="id-st08-03-03-01"></a>
- [ ] **ST08.03.03.01 — Select empirically confused tool pairs**
  - Type: analysis
  - Estimate: 6 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Confusion-driven few-shot candidate list.
  - Status: Not Started
<a id="id-st08-03-03-02"></a>
- [ ] **ST08.03.03.02 — Author and evaluate contrastive examples**
  - Type: content
  - Estimate: 12 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Versioned examples with before/after metrics.
  - Status: Not Started

<a id="id-us08-04-01"></a>
### US08.04.01 — Correct built-in prompt workflows

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.04](#id-f08-04) | P0 | 3 | 24 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP prompt user

**User story:** As a prompt user, I want workflows to request available evidence and use correct financial terminology so that their conclusions are supportable.

**Acceptance criteria:**

- [ ] research-ticker invokes company profile before asking what the company does and uses only metrics actually returned by its selected tools.
- [ ] compare-peers either retrieves statement metrics for margin/growth comparisons or removes those unsupported requests.
- [ ] news-pulse distinguishes article-volume trend from sentiment trend and does not label count delta as sentiment.
- [ ] All prompt symbol/date validation uses the same shared validators as downstream tools and prompt integration tests validate every generated call.

**Dependencies:** [US08.01.02](#id-us08-01-02)

**Labels:** `prompts` `correctness` `P0`

**Source findings:**

- research-ticker asks what the company does without fetching profile.
- compare-peers asks for margins/growth absent from the current ten metrics.
- news-pulse treats article-count delta as sentiment trend.
- Prompt and downstream symbol validation differ.

**Subtasks:**

<a id="id-st08-04-01-01"></a>
- [ ] **ST08.04.01.01 — Correct three built-in prompt plans**
  - Type: bugfix
  - Estimate: 12 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Evidence-aligned research, peer and news prompts.
  - Status: Not Started
<a id="id-st08-04-01-02"></a>
- [ ] **ST08.04.01.02 — Unify prompt and tool validators**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Shared symbol/date validation service.
  - Status: Not Started
<a id="id-st08-04-01-03"></a>
- [ ] **ST08.04.01.03 — Add generated-call prompt tests**
  - Type: test
  - Estimate: 6 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Prompt-to-tool schema integration suite.
  - Status: Not Started

<a id="id-us08-04-02"></a>
### US08.04.02 — Support bounded workflow templates

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.04](#id-f08-04) | P1 | 7 | 46 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Research workflow author

**User story:** As a workflow author, I want versioned templates with configurable depth and budget so that I can reuse safe research plans.

**Acceptance criteria:**

- [ ] Built-in workflows accept validated optional depth, period, max_peers, include sections, max_calls and max_tokens parameters.
- [ ] Templates use a versioned, trusted JSON or YAML schema with an allowlist of tools and no arbitrary code or prompt-file execution.
- [ ] A dry-run mode returns planned calls, dependencies and estimated quota/tokens before execution.
- [ ] Template compatibility and migration tests cover version changes and disabled/premium tools.

**Dependencies:** [US08.01.01](#id-us08-01-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01), [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01)

**Labels:** `prompts` `workflow` `templates` `P1`

**Source findings:**

- Prompts should support custom workflow templates and parameters such as depth, period, peers, max calls and max tokens.

**Subtasks:**

<a id="id-st08-04-02-01"></a>
- [ ] **ST08.04.02.01 — Define safe workflow template schema**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Versioned JSON/YAML schema and security constraints.
  - Status: Not Started
<a id="id-st08-04-02-02"></a>
- [ ] **ST08.04.02.02 — Implement planner and dry-run mode**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Bounded workflow renderer with call/token estimates.
  - Status: Not Started
<a id="id-st08-04-02-03"></a>
- [ ] **ST08.04.02.03 — Add template migration/conformance tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Version, enablement and entitlement workflow suite.
  - Status: Not Started

<a id="id-us08-05-01"></a>
### US08.05.01 — Score and reuse symbol-search candidates

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F08.05](#id-f08-05) | P0 | 5 | 36 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** AI research agent

**User story:** As an AI research agent, I want deterministic symbol confidence and exact-match signals so that names and security identifiers resolve correctly and follow-up tools reuse the result.

**Acceptance criteria:**

- [ ] Candidate scoring uses normalized exact symbol, company name, ISIN and CUSIP evidence plus exchange/type context and returns a documented relative confidence and IsExactMatch.
- [ ] The resolver accepts the first search result set instead of issuing a second Finnhub lookup under a different cache key; provider candidates are cached separately from request metadata.
- [ ] Requested limit is applied and deterministic tie-breaking prevents all-zero scores from blocking company-name, ISIN or CUSIP next actions.
- [ ] Ticker validation supports documented share-class forms such as BRK.A without accidentally accepting arbitrary provider syntax, and optional exchange input uses an allowlist.
- [ ] Tests cover exact ticker, company-name ambiguity, ISIN, CUSIP, share class, exchange disambiguation and cache-hit metadata freshness.

**Dependencies:** [US07.01.02](./E07-bounded-response-and-token-contract.md#id-us07-01-02), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01)

**Labels:** `symbol-search` `resolver` `bug` `P0`

**Source findings:**

- SymbolResult confidence remains zero and IsExactMatch is not populated, so ambiguous resolver thresholds prevent name/ISIN/CUSIP next actions.
- Search and resolver can make two Finnhub calls with different cache keys.
- BRK.A is knowingly misparsed and exchange/share-class validation needs explicit rules.

**Subtasks:**

<a id="id-st08-05-01-01"></a>
- [ ] **ST08.05.01.01 — Design evidence-based candidate scoring**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: Search engineer
  - Deliverable/evidence: Symbol/name/identifier/exchange scoring and tie-break specification.
  - Status: Not Started
<a id="id-st08-05-01-02"></a>
- [ ] **ST08.05.01.02 — Refactor search/resolver to share candidates**
  - Type: bugfix
  - Estimate: 18 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Single-lookup scored resolution with request-local metadata.
  - Status: Not Started
<a id="id-st08-05-01-03"></a>
- [ ] **ST08.05.01.03 — Add identifier, share-class and cache tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Resolution quality and upstream-call-count regression suite.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST08.01.01.01](#id-st08-01-01-01) | [US08.01.01](#id-us08-01-01) | P1 | Define recommendation feature vector and scoring policy | design | 12 | Context engineer | Explainable next-action scoring specification. | Not Started |
| [ST08.01.01.02](#id-st08-01-01-02) | [US08.01.01](#id-us08-01-01) | P1 | Implement context-aware action scorer | implementation | 24 | Backend engineer | Shared recommendation service with bounded actions. | Not Started |
| [ST08.01.01.03](#id-st08-01-01-03) | [US08.01.01](#id-us08-01-01) | P1 | Build scenario evaluation suite | evaluation | 12 | Context engineer | Golden recommendation scenarios and ranking metrics. | Not Started |
| [ST08.01.02.01](#id-st08-01-02-01) | [US08.01.02](#id-us08-01-02) | P0 | Represent action args as typed JSON | implementation | 8 | Backend engineer | Structured next-action contract. | Not Started |
| [ST08.01.02.02](#id-st08-01-02-02) | [US08.01.02](#id-us08-01-02) | P0 | Repair invalid exchange-symbol action | bugfix | 3 | Backend engineer | Executable replacement action and regression test. | Not Started |
| [ST08.01.02.03](#id-st08-01-02-03) | [US08.01.02](#id-us08-01-02) | P0 | Generate global action-schema test | test | 8 | QA automation engineer | All-emitter/target schema validation suite. | Not Started |
| [ST08.02.01.01](#id-st08-02-01-01) | [US08.02.01](#id-us08-02-01) | P1 | Author labeled intent corpus | data | 24 | Context engineer | Versioned 100-300-case train/held-out corpus. | Not Started |
| [ST08.02.01.02](#id-st08-02-01-02) | [US08.02.01](#id-us08-02-01) | P1 | Implement retrieval metric harness | evaluation | 14 | Search engineer | Tool@1, Recall@3, MRR, nDCG and confusion reports. | Not Started |
| [ST08.02.01.03](#id-st08-02-01-03) | [US08.02.01](#id-us08-02-01) | P1 | Add CI baseline gate | devops | 6 | DevOps engineer | Regression thresholds and published report artifact. | Not Started |
| [ST08.02.02.01](#id-st08-02-02-01) | [US08.02.02](#id-us08-02-02) | P1 | Implement Unicode finance-aware normalization | implementation | 16 | Search engineer | Tokenizer/normalizer preserving financial syntax. | Not Started |
| [ST08.02.02.02](#id-st08-02-02-02) | [US08.02.02](#id-us08-02-02) | P1 | Add synonyms and controlled typo tolerance | implementation | 18 | Search engineer | Curated concept expansion and fuzzy-match layer. | Not Started |
| [ST08.02.02.03](#id-st08-02-02-03) | [US08.02.02](#id-us08-02-02) | P1 | Expand multilingual benchmark | data | 14 | Localization QA engineer | Chinese, additional-language and typo evaluation cases. | Not Started |
| [ST08.02.03.01](#id-st08-02-03-01) | [US08.02.03](#id-us08-02-03) | P1 | Implement atomic versioned index snapshots | implementation | 16 | Search engineer | Catalogue-triggered rebuild and rollback path. | Not Started |
| [ST08.02.03.02](#id-st08-02-03-02) | [US08.02.03](#id-us08-02-03) | P1 | Design privacy-safe feedback event | design | 10 | Privacy engineer | Opt-in, minimized feedback schema and retention policy. | Not Started |
| [ST08.02.03.03](#id-st08-02-03-03) | [US08.02.03](#id-us08-02-03) | P1 | Build reviewed offline tuning workflow | implementation | 12 | Search engineer | Feedback-to-corpus review pipeline with poisoning controls. | Not Started |
| [ST08.03.01.01](#id-st08-03-01-01) | [US08.03.01](#id-us08-03-01) | P1 | Rewrite descriptions to common template | content | 16 | Context engineer | Concise descriptions for all enabled tools. | Not Started |
| [ST08.03.01.02](#id-st08-03-01-02) | [US08.03.01](#id-us08-03-01) | P1 | Move verbose field help into resources | implementation | 8 | Technical writer | Capabilities/API resource content and cross-links. | Not Started |
| [ST08.03.01.03](#id-st08-03-01-03) | [US08.03.01](#id-us08-03-01) | P1 | Add tools/list budget guard | test | 6 | QA automation engineer | Schema token, order and hash CI report. | Not Started |
| [ST08.03.02.01](#id-st08-03-02-01) | [US08.03.02](#id-us08-03-02) | P1 | Define cross-model task suite and metrics | design | 14 | Evaluation engineer | Pinned evaluation protocol and neutral baseline. | Not Started |
| [ST08.03.02.02](#id-st08-03-02-02) | [US08.03.02](#id-us08-03-02) | P1 | Implement model-adapter evaluation runner | implementation | 24 | Evaluation engineer | Repeatable Claude/GPT/Gemini runner with cost controls. | Not Started |
| [ST08.03.02.03](#id-st08-03-02-03) | [US08.03.02](#id-us08-03-02) | P1 | Publish baseline and A/B report | evaluation | 18 | Context engineer | Routing, argument, cost, latency and task-success comparison. | Not Started |
| [ST08.03.03.01](#id-st08-03-03-01) | [US08.03.03](#id-us08-03-03) | P2 | Select empirically confused tool pairs | analysis | 6 | Context engineer | Confusion-driven few-shot candidate list. | Not Started |
| [ST08.03.03.02](#id-st08-03-03-02) | [US08.03.03](#id-us08-03-03) | P2 | Author and evaluate contrastive examples | content | 12 | Context engineer | Versioned examples with before/after metrics. | Not Started |
| [ST08.04.01.01](#id-st08-04-01-01) | [US08.04.01](#id-us08-04-01) | P0 | Correct three built-in prompt plans | bugfix | 12 | Context engineer | Evidence-aligned research, peer and news prompts. | Not Started |
| [ST08.04.01.02](#id-st08-04-01-02) | [US08.04.01](#id-us08-04-01) | P0 | Unify prompt and tool validators | implementation | 6 | Backend engineer | Shared symbol/date validation service. | Not Started |
| [ST08.04.01.03](#id-st08-04-01-03) | [US08.04.01](#id-us08-04-01) | P0 | Add generated-call prompt tests | test | 6 | QA automation engineer | Prompt-to-tool schema integration suite. | Not Started |
| [ST08.04.02.01](#id-st08-04-02-01) | [US08.04.02](#id-us08-04-02) | P1 | Define safe workflow template schema | design | 12 | API architect | Versioned JSON/YAML schema and security constraints. | Not Started |
| [ST08.04.02.02](#id-st08-04-02-02) | [US08.04.02](#id-us08-04-02) | P1 | Implement planner and dry-run mode | implementation | 24 | Backend engineer | Bounded workflow renderer with call/token estimates. | Not Started |
| [ST08.04.02.03](#id-st08-04-02-03) | [US08.04.02](#id-us08-04-02) | P1 | Add template migration/conformance tests | test | 10 | QA automation engineer | Version, enablement and entitlement workflow suite. | Not Started |
| [ST08.05.01.01](#id-st08-05-01-01) | [US08.05.01](#id-us08-05-01) | P0 | Design evidence-based candidate scoring | design | 8 | Search engineer | Symbol/name/identifier/exchange scoring and tie-break specification. | Not Started |
| [ST08.05.01.02](#id-st08-05-01-02) | [US08.05.01](#id-us08-05-01) | P0 | Refactor search/resolver to share candidates | bugfix | 18 | Backend engineer | Single-lookup scored resolution with request-local metadata. | Not Started |
| [ST08.05.01.03](#id-st08-05-01-03) | [US08.05.01](#id-us08-05-01) | P0 | Add identifier, share-class and cache tests | test | 10 | QA automation engineer | Resolution quality and upstream-call-count regression suite. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E08 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| A-03 | A. Feature Enhancements | Make next_actions context-aware, bounded, schema-valid, quota-aware, entitlement-aware, and context-window-aware. | [US08.01.01](#id-us08-01-01), [US08.01.02](#id-us08-01-02) | [E08](#id-e08) | P0 | Covered | Explicit review question A3. |
| A-04 | A. Feature Enhancements | Define BM25 index rebuild rules and an opt-in, offline user-feedback learning loop with poisoning/privacy controls. | [US08.02.03](#id-us08-02-03), [US13.03.02](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-03-02) | [E08](#id-e08) | P1 | Covered | Explicit review question A4. |
| G-01 | G. Context Engineering | Shorten and clarify tool descriptions, establish a canonical cross-model tool-selection evaluation, and collect A/B evidence before model-specific variants. | [US08.03.01](#id-us08-03-01), [US08.03.02](#id-us08-03-02) | [E08](#id-e08) | P1 | Covered | Explicit review question G1. |
| G-02 | G. Context Engineering | Expand search-tools intent coverage with positive, negative, ambiguous, typo, alias, and multilingual examples plus ranking metrics. | [US08.02.01](#id-us08-02-01), [US08.02.02](#id-us08-02-02) | [E08](#id-e08) | P1 | Covered | Explicit review question G2. |
| G-03 | G. Context Engineering | Make next_actions respect context-window and output-token budgets, visited tools, quota cost, cache state, and likely information gain. | [US08.01.01](#id-us08-01-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01) | [E08](#id-e08) | P1 | Covered | Explicit review question G3. |
| G-04 | G. Context Engineering | Make prompts flexible through validated parameters and versioned trusted workflow templates. | [US08.04.01](#id-us08-04-01), [US08.04.02](#id-us08-04-02) | [E08](#id-e08) | P0 | Covered | Explicit review question G4. |
| G-05 | G. Context Engineering | Evaluate Claude, GPT, and Gemini using one neutral schema/description baseline; introduce model-specific optimization only when measured. | [US08.03.02](#id-us08-03-02) | [E08](#id-e08) | P1 | Covered | Explicit review question G5. |
| G-06 | G. Context Engineering | Add few-shot examples selectively for ambiguous routing and workflows without bloating every tool schema. | [US08.03.03](#id-us08-03-03) | [E08](#id-e08) | P2 | Covered | Explicit review question G6. |
| R-05 | Repository finding | Calculate deterministic confidence and exact-match signals so name/ISIN/CUSIP ambiguity resolution and next_actions work. | [US05.02.01](./E05-financial-and-symbol-data-correctness.md#id-us05-02-01), [US05.02.03](./E05-financial-and-symbol-data-correctness.md#id-us05-02-03), [US08.05.01](#id-us08-05-01) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Code-level core-functionality finding. |
| R-12 | Repository finding | Align prompts with actual tool capabilities: add profile to research, avoid unavailable margin/growth promises, and distinguish volume from sentiment. | [US08.04.01](#id-us08-04-01) | [E08](#id-e08) | P0 | Covered | Code-level prompt finding. |
| R-14 | Repository finding | Validate every next_action against target tool schemas and fix get-exchange-symbols suggestions that omit search-symbol.query. | [US08.01.02](#id-us08-01-02) | [E08](#id-e08) | P0 | Covered | Code-level agent UX finding. |
| R-15 | Repository finding | Address ASCII-only BM25 tokenization, missing synonyms/stemming/typos/CJK, uncalibrated scores, and sparse ranking acceptance tests. | [US08.02.01](#id-us08-02-01), [US08.02.02](#id-us08-02-02) | [E08](#id-e08) | P1 | Covered | Code-level search finding. |
| RF-114 | Code-review detail | A3/G3 - next_actions should be context, context-window, quota, cache and entitlement aware | [US08.01.01](#id-us08-01-01), [US08.01.02](#id-us08-01-02) | [E08](#id-e08) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-115 | Code-review detail | A4/G2 - BM25 rebuild, intent coverage, multilingual matching and feedback learning | [US08.02.01](#id-us08-02-01), [US08.02.02](#id-us08-02-02), [US08.02.03](#id-us08-02-03) | [E08](#id-e08) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-127 | Code-review detail | G1/G5 - tool-description clarity, token size and model-specific optimization require A/B evaluation | [US08.03.01](#id-us08-03-01), [US08.03.02](#id-us08-03-02) | [E08](#id-e08) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-128 | Code-review detail | G4 - prompts need correctness fixes, flexible parameters and custom workflow templates | [US08.04.01](#id-us08-04-01), [US08.04.02](#id-us08-04-02) | [E08](#id-e08) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-129 | Code-review detail | G6 - few-shot examples should target empirically ambiguous choices | [US08.03.03](#id-us08-03-03) | [E08](#id-e08) | P2 | Covered | Detailed finding retained from the repository review. |
| RF-132 | Code-review detail | Observed bug - request-specific search metadata is cached and duplicate symbol-resolution paths waste calls | [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01), [US08.05.01](#id-us08-05-01) | [E11](./E11-user-experience-performance-and-quota-control.md#id-e11) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-135 | Code-review detail | Observed financial defect - news windows overlap and article-volume change can be mislabeled as sentiment | [US06.09.03](./E06-financial-data-coverage-and-semantics.md#id-us06-09-03), [US08.04.01](#id-us08-04-01) | [E06](./E06-financial-data-coverage-and-semantics.md#id-e06) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-136 | Code-review detail | Observed search defect - confidence/exact-match are unset, identifier/name resolution stalls and share classes are misparsed | [US08.05.01](#id-us08-05-01) | [E08](#id-e08) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-139 | Code-review detail | Observed prompt defects - profile omission, unsupported margin/growth and article volume mislabeled as sentiment | [US08.04.01](#id-us08-04-01) | [E08](#id-e08) | P0 | Covered | Detailed finding retained from the repository review. |

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
| E08 | Epic | — | P1 | Intelligent Discovery and Context Engineering | See [E08](#id-e08) | See [E08](#id-e08) | — | finnhub-mcp; epic | — | Not Started |
| F08.01 | Feature | [E08](#id-e08) | P1 | Context-aware next actions | See [F08.01](#id-f08-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e08 | — | Not Started |
| F08.02 | Feature | [E08](#id-e08) | P1 | Measured BM25 discovery | See [F08.02](#id-f08-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e08 | — | Not Started |
| F08.03 | Feature | [E08](#id-e08) | P1 | Tool-description and model evaluation | See [F08.03](#id-f08-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e08 | — | Not Started |
| F08.04 | Feature | [E08](#id-e08) | P1 | Correct, configurable prompt workflows | See [F08.04](#id-f08-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e08 | — | Not Started |
| F08.05 | Feature | [E08](#id-e08) | P0 | Reliable symbol resolution | See [F08.05](#id-f08-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e08 | — | Not Started |
| US08.01.01 | Story | [F08.01](#id-f08-01) | P1 | Rank context-aware next actions | See [US08.01.01](#id-us08-01-01) | See [US08.01.01](#id-us08-01-01) | 6d | next-actions; routing; context; P1 | [US07.03.02](./E07-bounded-response-and-token-contract.md#id-us07-03-02), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01) | Not Started |
| US08.01.02 | Story | [F08.01](#id-f08-01) | P0 | Guarantee executable next-action schemas | See [US08.01.02](#id-us08-01-02) | See [US08.01.02](#id-us08-01-02) | 3d | next-actions; schema; bug; P0 | [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01) | Not Started |
| US08.02.01 | Story | [F08.02](#id-f08-02) | P1 | Build a labeled tool-discovery benchmark | See [US08.02.01](#id-us08-02-01) | See [US08.02.01](#id-us08-02-01) | 6d | bm25; evals; search-tools; P1 | [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01) | Not Started |
| US08.02.02 | Story | [F08.02](#id-f08-02) | P1 | Improve multilingual and fuzzy intent matching | See [US08.02.02](#id-us08-02-02) | See [US08.02.02](#id-us08-02-02) | 7d | bm25; multilingual; fuzzy-search; P1 | [US08.02.01](#id-us08-02-01) | Not Started |
| US08.02.03 | Story | [F08.02](#id-f08-02) | P1 | Control index rebuild and feedback learning | See [US08.02.03](#id-us08-02-03) | See [US08.02.03](#id-us08-02-03) | 5d | bm25; feedback; privacy; P1 | [US08.02.01](#id-us08-02-01), [US10.03.02](./E10-service-operations-resources-and-extensibility.md#id-us10-03-02) | Not Started |
| US08.03.01 | Story | [F08.03](#id-f08-03) | P1 | Set concise tool-description budgets | See [US08.03.01](#id-us08-03-01) | See [US08.03.01](#id-us08-03-01) | 4d | tool-descriptions; context-budget; P1 | [US08.02.01](#id-us08-02-01) | Not Started |
| US08.03.02 | Story | [F08.03](#id-f08-03) | P1 | Run cross-model tool-use evaluations | See [US08.03.02](#id-us08-03-02) | See [US08.03.02](#id-us08-03-02) | 8d | evals; llm; ab-test; P1 | [US08.02.01](#id-us08-02-01), [US08.03.01](#id-us08-03-01) | Not Started |
| US08.03.03 | Story | [F08.03](#id-f08-03) | P2 | Add targeted contrastive few-shot guidance | See [US08.03.03](#id-us08-03-03) | See [US08.03.03](#id-us08-03-03) | 3d | few-shot; context-engineering; P2 | [US08.03.02](#id-us08-03-02) | Not Started |
| US08.04.01 | Story | [F08.04](#id-f08-04) | P0 | Correct built-in prompt workflows | See [US08.04.01](#id-us08-04-01) | See [US08.04.01](#id-us08-04-01) | 3d | prompts; correctness; P0 | [US08.01.02](#id-us08-01-02) | Not Started |
| US08.04.02 | Story | [F08.04](#id-f08-04) | P1 | Support bounded workflow templates | See [US08.04.02](#id-us08-04-02) | See [US08.04.02](#id-us08-04-02) | 7d | prompts; workflow; templates; P1 | [US08.01.01](#id-us08-01-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01), [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01) | Not Started |
| US08.05.01 | Story | [F08.05](#id-f08-05) | P0 | Score and reuse symbol-search candidates | See [US08.05.01](#id-us08-05-01) | See [US08.05.01](#id-us08-05-01) | 5d | symbol-search; resolver; bug; P0 | [US07.01.02](./E07-bounded-response-and-token-contract.md#id-us07-01-02), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01) | Not Started |
| ST08.01.01.01 | Sub-task | [US08.01.01](#id-us08-01-01) | P1 | Define recommendation feature vector and scoring policy | See [ST08.01.01.01](#id-st08-01-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST08.01.01.02 | Sub-task | [US08.01.01](#id-us08-01-01) | P1 | Implement context-aware action scorer | See [ST08.01.01.02](#id-st08-01-01-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST08.01.01.03 | Sub-task | [US08.01.01](#id-us08-01-01) | P1 | Build scenario evaluation suite | See [ST08.01.01.03](#id-st08-01-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; evaluation | — | Not Started |
| ST08.01.02.01 | Sub-task | [US08.01.02](#id-us08-01-02) | P0 | Represent action args as typed JSON | See [ST08.01.02.01](#id-st08-01-02-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST08.01.02.02 | Sub-task | [US08.01.02](#id-us08-01-02) | P0 | Repair invalid exchange-symbol action | See [ST08.01.02.02](#id-st08-01-02-02) | Not applicable; see detail or parent section | 3h | finnhub-mcp; bugfix | — | Not Started |
| ST08.01.02.03 | Sub-task | [US08.01.02](#id-us08-01-02) | P0 | Generate global action-schema test | See [ST08.01.02.03](#id-st08-01-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST08.02.01.01 | Sub-task | [US08.02.01](#id-us08-02-01) | P1 | Author labeled intent corpus | See [ST08.02.01.01](#id-st08-02-01-01) | Not applicable; see detail or parent section | 24h | finnhub-mcp; data | — | Not Started |
| ST08.02.01.02 | Sub-task | [US08.02.01](#id-us08-02-01) | P1 | Implement retrieval metric harness | See [ST08.02.01.02](#id-st08-02-01-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; evaluation | — | Not Started |
| ST08.02.01.03 | Sub-task | [US08.02.01](#id-us08-02-01) | P1 | Add CI baseline gate | See [ST08.02.01.03](#id-st08-02-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; devops | — | Not Started |
| ST08.02.02.01 | Sub-task | [US08.02.02](#id-us08-02-02) | P1 | Implement Unicode finance-aware normalization | See [ST08.02.02.01](#id-st08-02-02-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST08.02.02.02 | Sub-task | [US08.02.02](#id-us08-02-02) | P1 | Add synonyms and controlled typo tolerance | See [ST08.02.02.02](#id-st08-02-02-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; implementation | — | Not Started |
| ST08.02.02.03 | Sub-task | [US08.02.02](#id-us08-02-02) | P1 | Expand multilingual benchmark | See [ST08.02.02.03](#id-st08-02-02-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; data | — | Not Started |
| ST08.02.03.01 | Sub-task | [US08.02.03](#id-us08-02-03) | P1 | Implement atomic versioned index snapshots | See [ST08.02.03.01](#id-st08-02-03-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST08.02.03.02 | Sub-task | [US08.02.03](#id-us08-02-03) | P1 | Design privacy-safe feedback event | See [ST08.02.03.02](#id-st08-02-03-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST08.02.03.03 | Sub-task | [US08.02.03](#id-us08-02-03) | P1 | Build reviewed offline tuning workflow | See [ST08.02.03.03](#id-st08-02-03-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST08.03.01.01 | Sub-task | [US08.03.01](#id-us08-03-01) | P1 | Rewrite descriptions to common template | See [ST08.03.01.01](#id-st08-03-01-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; content | — | Not Started |
| ST08.03.01.02 | Sub-task | [US08.03.01](#id-us08-03-01) | P1 | Move verbose field help into resources | See [ST08.03.01.02](#id-st08-03-01-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST08.03.01.03 | Sub-task | [US08.03.01](#id-us08-03-01) | P1 | Add tools/list budget guard | See [ST08.03.01.03](#id-st08-03-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; test | — | Not Started |
| ST08.03.02.01 | Sub-task | [US08.03.02](#id-us08-03-02) | P1 | Define cross-model task suite and metrics | See [ST08.03.02.01](#id-st08-03-02-01) | Not applicable; see detail or parent section | 14h | finnhub-mcp; design | — | Not Started |
| ST08.03.02.02 | Sub-task | [US08.03.02](#id-us08-03-02) | P1 | Implement model-adapter evaluation runner | See [ST08.03.02.02](#id-st08-03-02-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST08.03.02.03 | Sub-task | [US08.03.02](#id-us08-03-02) | P1 | Publish baseline and A/B report | See [ST08.03.02.03](#id-st08-03-02-03) | Not applicable; see detail or parent section | 18h | finnhub-mcp; evaluation | — | Not Started |
| ST08.03.03.01 | Sub-task | [US08.03.03](#id-us08-03-03) | P2 | Select empirically confused tool pairs | See [ST08.03.03.01](#id-st08-03-03-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; analysis | — | Not Started |
| ST08.03.03.02 | Sub-task | [US08.03.03](#id-us08-03-03) | P2 | Author and evaluate contrastive examples | See [ST08.03.03.02](#id-st08-03-03-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; content | — | Not Started |
| ST08.04.01.01 | Sub-task | [US08.04.01](#id-us08-04-01) | P0 | Correct three built-in prompt plans | See [ST08.04.01.01](#id-st08-04-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; bugfix | — | Not Started |
| ST08.04.01.02 | Sub-task | [US08.04.01](#id-us08-04-01) | P0 | Unify prompt and tool validators | See [ST08.04.01.02](#id-st08-04-01-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST08.04.01.03 | Sub-task | [US08.04.01](#id-us08-04-01) | P0 | Add generated-call prompt tests | See [ST08.04.01.03](#id-st08-04-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; test | — | Not Started |
| ST08.04.02.01 | Sub-task | [US08.04.02](#id-us08-04-02) | P1 | Define safe workflow template schema | See [ST08.04.02.01](#id-st08-04-02-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST08.04.02.02 | Sub-task | [US08.04.02](#id-us08-04-02) | P1 | Implement planner and dry-run mode | See [ST08.04.02.02](#id-st08-04-02-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST08.04.02.03 | Sub-task | [US08.04.02](#id-us08-04-02) | P1 | Add template migration/conformance tests | See [ST08.04.02.03](#id-st08-04-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST08.05.01.01 | Sub-task | [US08.05.01](#id-us08-05-01) | P0 | Design evidence-based candidate scoring | See [ST08.05.01.01](#id-st08-05-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST08.05.01.02 | Sub-task | [US08.05.01](#id-us08-05-01) | P0 | Refactor search/resolver to share candidates | See [ST08.05.01.02](#id-st08-05-01-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; bugfix | — | Not Started |
| ST08.05.01.03 | Sub-task | [US08.05.01](#id-us08-05-01) | P0 | Add identifier, share-class and cache tests | See [ST08.05.01.03](#id-st08-05-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |

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

- [ ] Create E08 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E08.
- [ ] Create all 11 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 32 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 21 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 49 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

