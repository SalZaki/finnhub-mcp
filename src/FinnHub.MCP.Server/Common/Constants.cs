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
    }
}
