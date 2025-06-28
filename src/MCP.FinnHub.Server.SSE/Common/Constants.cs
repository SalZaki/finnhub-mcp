// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace MCP.FinnHub.Server.SSE.Common;

public static class Constants
{
    public static class Server
    {
        public const string ProtocolVersion = "2024-11-05";

        public const string Instructions = "If no programming language is specified, assume C#. Keep your responses brief and professional.";
    }

    public static class Tools
    {
        public static class SearchSymbols
        {
            public const string Name = "search-symbol";
            public const string Title = "Search Symbol";

            public static class Parameters
            {
                public const string QueryName = "query";
                public const string QueryDescription = "Query string, e.g., ticker, company name, ISIN, or CUSIP.";
                public const string LimitName = "limit";
                public const string LimitDescription = "Maximum number of results to return (default: 10, max: 100).";
                public const string ExchangeName = "exchange";
                public const string ExchangeDescription = "Optional exchange, e.g., US.";
            }

            public const string Description =
                @"Search for best-matching symbols based on your query. You can input anything from a stock ticker, security name, ISIN, or CUSIP.

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
";
        }
    }
}
