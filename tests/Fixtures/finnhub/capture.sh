#!/usr/bin/env bash
# Re-capture all Finnhub fixtures. Run from repo root with FINNHUB_API_KEY exported
# (or a .env at the repo root containing it).
#
#   ./tests/Fixtures/finnhub/capture.sh
#
# Fixtures are committed — only regenerate when Finnhub changes a response shape
# or when adding a new endpoint.
set -euo pipefail

if [ -z "${FINNHUB_API_KEY:-}" ]; then
  if [ -f .env ]; then
    set -a; . ./.env; set +a
  fi
fi
test -n "${FINNHUB_API_KEY:-}" || { echo "FINNHUB_API_KEY not set" >&2; exit 1; }

TO=$(date -u +%Y-%m-%d)
FROM=$(date -u -v-7d +%Y-%m-%d 2>/dev/null || date -u -d '7 days ago' +%Y-%m-%d)
NOW=$(date -u +%s)
WEEK_AGO=$((NOW - 86400 * 7))

OUT_DIR="tests/Fixtures/finnhub"
mkdir -p "$OUT_DIR"

fetch() {
  local name=$1 path=$2 out="$OUT_DIR/${name}.json" status size
  status=$(curl -s -o "$out" -w "%{http_code}" \
    -H "X-Finnhub-Token: $FINNHUB_API_KEY" \
    "https://finnhub.io/api/v1/$path")
  size=$(wc -c < "$out" | tr -d ' ')
  printf "  %-25s [%s, %sb]\n" "$name" "$status" "$size"
}

fetch search-apple             "search?q=apple"
fetch peers-AAPL-industry      "stock/peers?symbol=AAPL&grouping=industry"
fetch metric-AAPL              "stock/metric?symbol=AAPL&metric=all"
fetch quote-AAPL               "quote?symbol=AAPL"
fetch quote-unknown            "quote?symbol=ZZZNOTASYMBOL"
fetch profile-AAPL             "stock/profile2?symbol=AAPL"
fetch candle-AAPL              "stock/candle?symbol=AAPL&resolution=D&from=$WEEK_AGO&to=$NOW"
fetch company-news-AAPL        "company-news?symbol=AAPL&from=$FROM&to=$TO"
fetch news-sentiment-AAPL      "news-sentiment?symbol=AAPL"
# Calendar (parameter-dispatched tool: kind=earnings|ipo|economic).
# Earnings fixture: AAPL-scoped, current quarter window.
CAL_FROM=$(date -u +%Y-%m-01)
CAL_TO=$(date -u -v+3m +%Y-%m-%d 2>/dev/null || date -u -d '+3 months' +%Y-%m-%d)
fetch calendar-earnings-aapl   "calendar/earnings?from=$CAL_FROM&to=$CAL_TO&symbol=AAPL"
# IPO fixture: trailing 6-month window from origin captures known SPAC + traditional IPOs
# in mixed states (priced, withdrawn) so the parser exercises the nullable-field branches.
fetch calendar-ipo-2026        "calendar/ipo?from=2025-01-01&to=2025-06-01"
# Economic fixture: a 30-day window captures ~1300 events across ~115 countries with
# mixed impact tiers (low/medium/high) and many entries lacking actual/estimate/prev.
fetch calendar-economic-2026   "calendar/economic?from=2026-06-01&to=2026-06-30"
# Insider transactions fixture: trailing 30 days for AAPL — exercises sells / gifts
# / nullable transaction price and zero-price grant branches.
INS_FROM=$(date -u -v-30d +%Y-%m-%d 2>/dev/null || date -u -d '30 days ago' +%Y-%m-%d)
INS_TO=$(date -u +%Y-%m-%d)
fetch insider-transactions-AAPL "stock/insider-transactions?symbol=AAPL&from=$INS_FROM&to=$INS_TO"
# Recommendations fixture: AAPL ships multiple monthly snapshots so change_vs_prev
# is exercised end-to-end.
fetch recommendation-AAPL      "stock/recommendation?symbol=AAPL"
