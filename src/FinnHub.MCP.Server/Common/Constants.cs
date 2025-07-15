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
        /// Guides the model to assume C# as the default programming language and maintain
        /// brief, professional responses.
        /// </summary>
        public const string Instructions = "If no programming language is specified, assume C#. Keep your responses brief and professional.";
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

                """;
        }
    }
}
