// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Common;

/// <summary>
/// Provides application-wide constants for the FinnHub MCP Server including server configuration,
/// tool definitions, parameter specifications, and API documentation.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Contains server-level configuration constants.
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// The MCP (Model Context Protocol) version supported by this server.
        /// This version determines the protocol compatibility and feature set available.
        /// </summary>
        public const string ProtocolVersion = "2024-11-05";

        /// <summary>
        /// Default instructions provided to AI models when interacting with this server.
        /// Orients the model toward aggregation-first tools that default to token-efficient
        /// summaries and toward intent-based tool discovery rather than full schema enumeration.
        /// </summary>
        public const string Instructions =
            "This server exposes Finnhub financial data through aggregation-first tools that default to " +
            "token-efficient summaries. When tool discovery is needed, use search-tools first to find the " +
            "right tool by intent rather than enumerating the full schema set. Every tool accepts " +
            "view: summary|standard|full to control response detail and fields for sparse projections.";
    }

    /// <summary>
    /// Constants for the response envelope contract every tool participates in.
    /// </summary>
    public static class Envelope
    {
        /// <summary>
        /// Hard ceiling on the estimated token count of a <c>summary</c>-view response.
        /// Responses that exceed this are rebuilt by the tool invocation middleware
        /// as a <c>BudgetExceeded</c> failure envelope.
        /// </summary>
        public const int SummaryTokenCeiling = 500;

        /// <summary>
        /// Hard ceiling on the estimated token count of a <c>standard</c>-view response.
        /// </summary>
        public const int StandardTokenCeiling = 2000;

        /// <summary>
        /// Description applied to every tool's <c>view</c> parameter so the consuming
        /// model picks a tier deliberately.
        /// </summary>
        public const string ViewParameterDescription =
            "Response detail level: summary (curated, ~500 tokens), standard (curated, ~2000 tokens), " +
            "full (raw payload, no ceiling). Defaults to summary.";

        /// <summary>
        /// Description applied to every tool's <c>fields</c> parameter for sparse projection.
        /// </summary>
        public const string FieldsParameterDescription =
            "Optional sparse projection. When set, only the named fields plus the envelope are returned. " +
            "Unknown field names are rejected as a validation error.";
    }

    /// <summary>
    /// Contains tool-specific constants and configurations for all available MCP tools.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Constants and configuration for the symbol search tool, which enables searching
        /// for financial symbols based on various identifiers including tickers, company names,
        /// ISIN, and CUSIP codes.
        /// </summary>
        public static class SearchSymbols
        {
            /// <summary>
            /// The unique identifier name for the search symbol tool as registered with the MCP server.
            /// </summary>
            public const string Name = "search-symbol";

            /// <summary>
            /// The human-readable title for the search symbol tool displayed in user interfaces.
            /// </summary>
            public const string Title = "Search Symbol";

            /// <summary>
            /// Parameter definitions for the search symbol tool, including names and descriptions
            /// for all supported input parameters.
            /// </summary>
            public static class Parameters
            {
                /// <summary>
                /// The parameter name for the search query input.
                /// </summary>
                public const string QueryName = "query";

                /// <summary>
                /// Human-readable description of the query parameter explaining supported input formats.
                /// </summary>
                public const string QueryDescription = "Query string, e.g., ticker, company name, ISIN, or CUSIP.";

                /// <summary>
                /// The parameter name for limiting the number of search results returned.
                /// </summary>
                public const string LimitName = "limit";

                /// <summary>
                /// Human-readable description of the limit parameter including default and maximum values.
                /// </summary>
                public const string LimitDescription = "Maximum number of results to return (default: 10, max: 100).";

                /// <summary>
                /// The parameter name for filtering results by exchange.
                /// </summary>
                public const string ExchangeName = "exchange";

                /// <summary>
                /// Human-readable description of the exchange parameter with example values.
                /// </summary>
                public const string ExchangeDescription = "Optional exchange, e.g., US.";

                /// <summary>
                /// The parameter name for the response detail level.
                /// </summary>
                public const string ViewName = "view";

                /// <summary>
                /// Human-readable description of the view parameter.
                /// </summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;

                /// <summary>
                /// The parameter name for sparse field projection.
                /// </summary>
                public const string FieldsName = "fields";

                /// <summary>
                /// Human-readable description of the fields parameter.
                /// </summary>
                public const string FieldsDescription = Envelope.FieldsParameterDescription;
            }

            /// <summary>
            /// Comprehensive documentation for the search symbol tool including usage examples,
            /// parameter specifications, response field descriptions, and implementation notes.
            /// This documentation is used for tool registration and developer guidance.
            ///
            /// The tool supports fuzzy matching using AI heuristics and provides confidence scores
            /// for match quality assessment. It can search across multiple exchanges and supports
            /// various financial identifier formats including stock tickers, company names,
            /// ISIN (International Securities Identification Number), and CUSIP
            /// (Committee on Uniform Securities Identification Procedures) codes.
            /// </summary>
            public const string Description =
                """
                Search for best-matching symbols based on your query. You can input anything from a stock ticker, security name, ISIN, or CUSIP.

                ## Example Queries:
                - query='apple', exchange='US'
                - query='US5949181045'
                - query='AAPL'

                ## Request Parameters:
                - query (string, required): Symbol, Company Name, ISIN, or CUSIP
                - exchange (string, optional): Exchange code (e.g., 'US', 'TO', etc.)

                ## Response Fields:
                - count (int): Number of matching results
                - result (array):
                  - symbol (string): Unique symbol used in other endpoints
                  - display_symbol (string): Formatted version of the symbol
                  - description (string): Full company name or asset description
                  - type (string): Type of security (e.g., 'Common Stock', 'ETF')
                  - confidence_score (float, 0.0 to 1.0): AI-inferred match quality
                  - is_exact_match (bool): Whether the symbol matches query exactly

                ## Notes:
                - Results are fuzzy-ranked using AI heuristics and string similarity
                - Financial Instrument Global Identifier
                - Supports symbol lookup across exchanges
                - Intended for use in financial tools, ai agents, and UIs

                Approx tokens: summary ~200, standard ~2000, full ~8000.
                """;
        }

        /// <summary>
        /// Constants for the <c>search-tools</c> meta-tool — intent-based tool discovery
        /// that keeps full tool schemas off the wire until a tool is actually needed.
        /// </summary>
        public static class SearchTools
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "search-tools";

            /// <summary>The human-readable title.</summary>
            public const string Title = "Search Tools";

            /// <summary>Parameter definitions for the search-tools meta-tool.</summary>
            public static class Parameters
            {
                /// <summary>The parameter name for the natural-language intent.</summary>
                public const string IntentName = "intent";

                /// <summary>Human-readable description of the intent parameter.</summary>
                public const string IntentDescription =
                    "Natural-language description of what you want to do (max 200 characters), e.g. " +
                    "'is apple stock up this week' or 'upcoming earnings'. Returns the most relevant tools, ranked.";

                /// <summary>The parameter name for the response detail level.</summary>
                public const string ViewName = "view";

                /// <summary>Human-readable description of the view parameter.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>
            /// Documentation for the <c>search-tools</c> meta-tool used at registration time.
            /// </summary>
            public const string Description =
                """
                Discover the right Finnhub tool by intent, without loading every tool schema up front.
                Describe what you want in plain language and get back the best-matching tools, ranked by relevance.

                ## Example Queries:
                - intent='find the right tool for a question'
                - intent='which tool should I use'
                - intent='list tools for a topic'

                ## Request Parameters:
                - intent (string, required): natural-language intent, max 200 characters

                ## Response Fields:
                - intent (string): the echoed intent
                - matches (array, ranked most-relevant first):
                  - name (string): the tool name to invoke next
                  - title (string): human-readable title
                  - score (float): relevance score
                  - category (string, nullable): coarse grouping
                  - premium (bool): whether the tool's upstream endpoint may require a premium key
                  - description (string, nullable): full tool description (standard and full views only)
                - total_matches (int)

                ## Notes:
                - Ranking is a BM25 keyword index over each tool's name, title, description, and example intents.
                - summary view omits per-tool descriptions to stay token-light; standard and full include them.

                Approx tokens: summary ~150, standard ~700, full ~700.
                """;
        }

        /// <summary>
        /// Constants for the <c>get-peers</c> tool — peer ticker lookup for a symbol.
        /// </summary>
        public static class Peers
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-peers";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Peers";

            /// <summary>
            /// Parameter names and descriptions for <c>get-peers</c>.
            /// </summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>Grouping parameter name.</summary>
                public const string GroupingName = "grouping";

                /// <summary>Grouping parameter description.</summary>
                public const string GroupingDescription =
                    "Peer grouping strategy: industry (default), subindustry, or sector.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>
            /// Tool description registered with the MCP server.
            /// </summary>
            public const string Description =
                """
                Get the peer ticker list for a symbol — companies in the same industry, sub-industry, or sector.

                ## Example:
                - symbol='AAPL', grouping='industry'

                ## Response Fields:
                - peers (array of strings): peer ticker symbols
                - grouping (string): echo of the grouping strategy
                - total_count (int)
                - has_results (bool)

                ## Notes:
                - summary view returns the top 10 peers; standard returns up to 25; full returns the complete array.

                Approx tokens: summary ~80, standard ~200, full ~400.
                """;
        }

        /// <summary>
        /// Constants for the <c>get-financials-snapshot</c> tool — curated 10-KPI snapshot.
        /// </summary>
        public static class FinancialsSnapshot
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-financials-snapshot";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Financials Snapshot";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get a curated 10-KPI financial snapshot for a symbol — market cap, P/E, P/B, EPS, dividend yield,
                52-week high/low, 52-week price return, beta, and revenue per share.

                ## Example:
                - symbol='AAPL'

                ## Response Fields (10 curated KPIs, all nullable):
                - symbol (string)
                - market_cap (double): market capitalisation in USD millions
                - pe_ttm (double): trailing twelve-month P/E
                - pb_annual (double): annual price/book
                - eps_ttm (double): trailing twelve-month EPS
                - dividend_yield (double): indicated annual dividend yield
                - week52_high, week52_low (double)
                - week52_price_return_pct (double): 52-week price return
                - beta (double)
                - revenue_per_share_ttm (double)
                - raw (object, optional): full upstream metric dictionary, populated only when view = "full"

                Approx tokens: summary ~200, standard ~200, full ~3000.
                """;
        }

        /// <summary>Constants for the <c>get-price-summary</c> tool — aggregated price stats over a candle range.</summary>
        public static class PriceSummary
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-price-summary";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Price Summary";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>Period parameter name.</summary>
                public const string PeriodName = "period";

                /// <summary>Period parameter description.</summary>
                public const string PeriodDescription =
                    "Lookback window: 7d, 30d (default), 90d, or 1y.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Aggregate the price candle range for a symbol into curated summary stats —
                min, max, mean, total return %, volatility (population stdev of closes), and the most-recent close.

                ## Example:
                - symbol='AAPL', period='30d'

                ## Response Fields:
                - symbol (string)
                - period (string): echo of the requested window
                - resolution (string): Finnhub resolution code (D|W)
                - min, max, mean (double): from low/high/close arrays
                - return_pct (double): (last_close - first_close) / first_close * 100
                - vol (double): population stdev of closing prices
                - latest (object): { close, timestamp_utc }
                - candle_count (int)
                - candles (object, optional): full OHLCV arrays, populated only when view = "full"

                ## Notes:
                - 1y period uses weekly resolution to stay within token budget; 7d/30d/90d use daily.

                Approx tokens: summary ~120, standard ~120, full varies with period (~600 at 30d, ~2000 at 1y).
                """;
        }

        /// <summary>Constants for the <c>get-news-pulse</c> tool — aggregated news sentiment and headlines.</summary>
        public static class NewsPulse
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-news-pulse";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get News Pulse";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get an aggregated news pulse for a symbol over the past 7 days — sentiment scores
                (when the upstream sentiment endpoint is available), top 5 headlines, total article
                count, and the article-count delta vs the prior 7-day window.

                ## Example:
                - symbol='AAPL'

                ## Response Fields:
                - symbol (string)
                - period (string): always "7d" in v1
                - sentiment_score, bullish_percent, bearish_percent (double, nullable):
                  populated only when the upstream /news-sentiment endpoint is reachable.
                  Falls back to null gracefully on premium-locked keys.
                - sentiment_source (string, nullable): "finnhub" when sentiment is present, null otherwise.
                - top_headlines (array of objects): top 5 most-recent (or all if view="full"); each
                  { headline, url, source, datetime_utc }.
                - count (int): article count over the current 7-day window.
                - delta_vs_prev_week (int): current count minus prior-week count.

                Approx tokens: summary ~400, standard ~400, full varies with article count.
                """;
        }

        /// <summary>Constants for the <c>get-quote</c> tool — real-time price snapshot.</summary>
        public static class Quote
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-quote";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Quote";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get the real-time price snapshot for a symbol — current, change, percent change,
                session high/low/open, previous close, and snapshot timestamp.

                ## Example:
                - symbol='AAPL'

                ## Response Fields:
                - symbol (string)
                - current (double): latest trade price
                - change (double): absolute change vs previous close
                - percent_change (double)
                - high, low, open, prev_close (double)
                - timestamp_utc (ISO 8601)

                ## Notes:
                - Cached at the 10s Quote tier; bypassed cache for very short staleness windows.
                - The response is already curated — no view variation beyond default.

                Approx tokens: summary ~80, standard ~80, full ~80.
                """;
        }

        /// <summary>Constants for the <c>get-calendar</c> tool — parameter-dispatched calendar lookup.</summary>
        public static class Calendar
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-calendar";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Calendar";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Kind parameter name.</summary>
                public const string KindName = "kind";

                /// <summary>Kind parameter description.</summary>
                public const string KindDescription =
                    "Calendar feed to dispatch to: 'earnings', 'ipo', or 'economic'.";

                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription =
                    "Optional uppercase ticker filter, e.g. 'AAPL'. Only valid with kind='earnings'; omit for kind='ipo' and kind='economic' (those upstreams do not filter by ticker).";

                /// <summary>Country parameter name.</summary>
                public const string CountryName = "country";

                /// <summary>Country parameter description.</summary>
                public const string CountryDescription =
                    "Optional ISO 3166-1 alpha-2 country code (e.g. 'US', 'GB', 'DE') or Finnhub pseudo-code ('EU', 'WW' for global). Only valid with kind='economic'. When omitted, the full global feed is returned (subject to view caps).";

                /// <summary>From-date parameter name.</summary>
                public const string FromName = "from";

                /// <summary>From-date parameter description.</summary>
                public const string FromDescription =
                    "Inclusive start of the date window as ISO yyyy-MM-dd, e.g. '2026-05-01'. Defaults to today (UTC).";

                /// <summary>To-date parameter name.</summary>
                public const string ToName = "to";

                /// <summary>To-date parameter description.</summary>
                public const string ToDescription =
                    "Inclusive end of the date window as ISO yyyy-MM-dd. Defaults to from + the kind's max window. Maximum window: 90 days for earnings and economic, 365 days for IPO.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get the dispatched calendar for a date window — corporate earnings, IPO listings, or macro releases.

                ## Examples:
                - kind='earnings', symbol='AAPL', from='2026-05-01', to='2026-08-01'
                - kind='earnings', from='2026-05-26', to='2026-06-02'   (full week, all symbols)
                - kind='ipo', from='2026-06-01', to='2026-12-31'        (next 6 months of listings)
                - kind='economic', country='US', from='2026-06-01', to='2026-06-30'  (US macro releases for the month)

                ## Request Parameters:
                - kind (string, required): 'earnings', 'ipo', or 'economic'
                - symbol (string, optional): uppercase ticker filter — only valid with kind='earnings'
                - country (string, optional): ISO 3166-1 alpha-2 code — only valid with kind='economic'
                - from (string, optional, ISO yyyy-MM-dd): inclusive start; defaults to today (UTC)
                - to (string, optional, ISO yyyy-MM-dd): inclusive end; defaults to from + the kind's max window. Max: 90 days for earnings and economic, 365 days for IPO.

                ## Response Fields:
                - kind (string): echo of the dispatched feed
                - from, to (ISO date): echo of the window
                - symbol (string, nullable): echo of the symbol filter (always null for kind='ipo'/'economic')
                - country (string, nullable): echo of the country filter (only populated for kind='economic')
                - total_count (int)
                - earnings_events (array, populated when kind='earnings'):
                  - symbol, date (ISO), hour (bmo|amc|dmh|null), quarter, year
                  - eps_actual, eps_estimate, revenue_actual, revenue_estimate (all nullable doubles)
                - ipo_events (array, populated when kind='ipo', sorted most-recent first):
                  - symbol (nullable; null on withdrawn/unpriced), name, date (ISO), exchange
                  - price (nullable double, parsed from upstream string), number_of_shares, total_shares_value
                  - status (e.g. 'priced', 'filed', 'withdrawn', 'expected')
                - economic_events (array, populated when kind='economic', sorted earliest-first):
                  - country, event_name, time_utc (ISO 8601)
                  - impact (low|medium|high|null)
                  - actual, estimate, prev (all nullable doubles), unit (e.g. '%', '$')

                ## Notes:
                - summary view caps at 10 events; standard at 25; full returns the complete window.
                - Cached at the News tier — calendars revise as analysts update estimates and SEC filings land.
                - kind='earnings' suggests get-financials-snapshot + get-news-pulse for the queried symbol.
                - kind='ipo' suggests get-company-profile for the most recent IPO with a tradable ticker.
                - kind='economic' returns no next_actions — macro releases are not symbol-scoped.

                Approx tokens: summary ~250, standard varies with event count (~1000 at 25 events), full no ceiling.
                """;
        }

        /// <summary>Constants for the <c>get-recommendations</c> tool — analyst consensus snapshot + change-vs-previous-period.</summary>
        public static class Recommendations
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-recommendations";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Recommendations";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get the analyst-consensus snapshot for a symbol plus the change vs the prior period —
                shows whether sentiment is shifting without comparing two periods by hand.

                ## Example:
                - symbol='AAPL'

                ## Response Fields:
                - symbol (string)
                - period (ISO date): first day of the latest covered month
                - consensus (string): 'Strong Buy' | 'Buy' | 'Hold' | 'Sell' | 'Strong Sell' — derived from
                  the weighted mean rating (Strong Buy = +2, Buy = +1, Hold = 0, Sell = −1, Strong Sell = −2).
                - strong_buy, buy, hold, sell, strong_sell (int): analyst counts in the latest period
                - total (int): sum of all rating buckets
                - change_vs_prev (object, nullable): per-bucket delta vs the prior period and a
                  single-label `consensus_shift` ('more bullish' | 'more bearish' | 'no change').
                  Null when Finnhub returns only one period.
                - snapshots (array, optional): full history of period snapshots (most-recent first);
                  populated only when view='full'.

                ## Notes:
                - Cached at the Profile tier — analyst consensus revises monthly.
                - One upstream call returns multiple periods; change_vs_prev needs no extra fetch.
                - Suggests get-financials-snapshot and get-peers as next actions on success.

                Approx tokens: summary ~200, standard ~200, full varies with history depth (~80 tokens per snapshot).
                """;
        }

        /// <summary>Constants for the <c>get-exchange-symbols</c> tool — symbols listed on an exchange.</summary>
        public static class ExchangeSymbols
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-exchange-symbols";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Exchange Symbols";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Exchange parameter name.</summary>
                public const string ExchangeName = "exchange";

                /// <summary>Exchange parameter description.</summary>
                public const string ExchangeDescription =
                    "Exchange code, e.g. 'US' (US exchanges), 'L' (London), 'T' (Tokyo). 1-8 letters.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                List the symbols traded on a specific exchange as an aggregated, token-conscious summary —
                a total count, a breakdown by security type, and a capped sample. This is NOT the full symbol
                list (a major exchange lists tens of thousands of symbols); use search-symbol to resolve a
                specific ticker.

                ## Example:
                - exchange='US'

                ## Request Parameters:
                - exchange (string, required): exchange code, e.g. 'US', 'L', 'T'. 1-8 letters.

                ## Response Fields:
                - exchange (string): echo of the requested code
                - total_count (int): total number of symbols listed on the exchange
                - type_breakdown (object): count per security type, e.g. { "Common Stock": 4500, "ETP": 3100 }
                - symbols (array, optional): a capped sample of { symbol, display_symbol, description, type } —
                  omitted in summary, 25 rows in standard, up to 100 in full
                - has_results (bool)

                ## Notes:
                - summary returns count + type_breakdown only; standard adds a 25-row sample; full adds up to 100.
                - Cached at the Exchanges tier (~7 days) — exchange membership is near-static.
                - Free Finnhub plans only support exchange='US'; other exchanges require a paid plan (returned as premium).
                - To find a specific symbol on an exchange, use search-symbol.

                Approx tokens: summary ~250, standard ~700, full ~2200.
                """;
        }

        /// <summary>Constants for the <c>get-insider-signal</c> tool — aggregated insider net buy/sell signal.</summary>
        public static class InsiderSignal
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-insider-signal";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Insider Signal";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>From-date parameter name.</summary>
                public const string FromName = "from";

                /// <summary>From-date parameter description.</summary>
                public const string FromDescription =
                    "Inclusive start of the lookup window as ISO yyyy-MM-dd, e.g. '2026-05-01'. Defaults to today − 30 days (UTC).";

                /// <summary>To-date parameter name.</summary>
                public const string ToName = "to";

                /// <summary>To-date parameter description.</summary>
                public const string ToDescription =
                    "Inclusive end of the lookup window as ISO yyyy-MM-dd. Defaults to today (UTC). Maximum window: 90 days.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get an aggregated insider-transaction signal for a symbol — net buy/sell volume over the trailing 30 days,
                the most active named insiders, and the latest filed transaction.

                ## Example:
                - symbol='AAPL'
                - symbol='AAPL', from='2026-04-27', to='2026-05-27'

                ## Request Parameters:
                - symbol (string, required): uppercase ticker
                - from (string, optional, ISO yyyy-MM-dd): inclusive start; defaults to today − 30 days (UTC)
                - to (string, optional, ISO yyyy-MM-dd): inclusive end; defaults to today (UTC). Max window: 90 days.

                ## Response Fields:
                - symbol (string)
                - from, to (ISO date): echo of the window
                - net_buy_sell_30d (long): sum of signed share changes across the window — positive when insiders
                  are net acquirers, negative when net sellers.
                - notable_names (array of strings): top 5 unique insiders ranked by absolute trade volume (most active first)
                - total_count (int)
                - latest (object, nullable): most-recent transaction in the window — { name, change, share, transaction_date,
                  filing_date, transaction_price, transaction_code, is_derivative, currency }
                - transactions (array, optional): full transaction list post-DTO mapping, populated only when view='full'

                ## Notes:
                - Cached at the News tier — insider filings revise as supplemental Form 4s land.
                - Suggests get-company-profile and get-quote for the queried symbol on successful response.

                Approx tokens: summary ~250, standard ~250, full varies with transaction count (~80 tokens per transaction).
                """;
        }

        /// <summary>Constants for the <c>get-company-profile</c> tool — company snapshot.</summary>
        public static class CompanyProfile
        {
            /// <summary>The unique tool identifier.</summary>
            public const string Name = "get-company-profile";

            /// <summary>The human-readable tool title.</summary>
            public const string Title = "Get Company Profile";

            /// <summary>Parameter names and descriptions.</summary>
            public static class Parameters
            {
                /// <summary>Symbol parameter name.</summary>
                public const string SymbolName = "symbol";

                /// <summary>Symbol parameter description.</summary>
                public const string SymbolDescription = "Uppercase ticker symbol, e.g. 'AAPL'.";

                /// <summary>View parameter name.</summary>
                public const string ViewName = "view";

                /// <summary>View parameter description.</summary>
                public const string ViewDescription = Envelope.ViewParameterDescription;
            }

            /// <summary>Tool description registered with the MCP server.</summary>
            public const string Description =
                """
                Get the company profile for a symbol — name, ticker, country, currency, exchange,
                IPO date, market cap, shares outstanding, industry, and (on standard/full views)
                logo, phone, and website URL.

                ## Example:
                - symbol='AAPL'

                ## Response Fields (all nullable):
                - ticker, name, country, currency, exchange, ipo, industry (string)
                - market_cap (double, USD millions)
                - share_outstanding (double, millions)
                - logo, phone, weburl (string) — populated only on view='standard' or 'full'

                Approx tokens: summary ~100, standard ~150, full ~150.
                """;
        }
    }
}
