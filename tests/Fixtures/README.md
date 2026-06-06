# Test fixtures

Real Finnhub HTTP responses captured from `https://finnhub.io/api/v1/*` and frozen
for use in unit tests. Lets the client tests prove the parsers handle the **actual**
upstream wire shape rather than synthetic payloads we invented ourselves.

## Why

We previously had a class of bug where unit tests passed (synthetic JSON matched
the DTO's assumed shape) but the live tool failed on real data — most recently the
`get-financials-snapshot` `Dictionary<string, double?>` vs string-typed date fields
issue (PR #166), and the `get-quote` non-nullable `change`/`percent_change` issue
exposed by the `quote-ZZZNOTASYMBOL` capture. Both would have been caught at test
time with these fixtures.

## Refreshing fixtures

If Finnhub changes a response shape (or we add a new endpoint), regenerate fixtures
via `tests/Fixtures/finnhub/capture.sh` with `FINNHUB_API_KEY` exported. The captured
files are intentionally committed so tests are deterministic and don't depend on the
live API.

## File naming

`tests/Fixtures/finnhub/<endpoint-slug>[-<scenario>].json`

Examples:
- `quote-AAPL.json` — happy path, US stock
- `quote-unknown.json` — `ZZZNOTASYMBOL`; exercises the all-zero / null-field edge
- `metric-AAPL.json` — full `/stock/metric?metric=all` (~234 kB) with mixed numeric + date string values
- `candle-AAPL.json` — 403 premium response (candles are premium on the free key)
- `news-sentiment-AAPL.json` — 403 premium response
- `stock-symbol-US.json` — `/stock/symbol?exchange=US`, **truncated to the first 25 of ~30,538 real rows** (the live payload is ~7.2 MB and 302-redirects to a signed CDN file); keeps the real wire shape while staying committable. Re-capture with `capture.sh`, which follows the redirect (`-L`) and re-truncates.
