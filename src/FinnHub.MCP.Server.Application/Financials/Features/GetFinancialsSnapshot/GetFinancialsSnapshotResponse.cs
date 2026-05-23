// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;

/// <summary>
/// Wire response for <c>get-financials-snapshot</c>. Curated 10-KPI projection
/// over the Finnhub <c>/stock/metric</c> payload. <see cref="Raw"/> is populated
/// only when the tool was invoked with <c>view = "full"</c>.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetFinancialsSnapshotResponse
{
    /// <summary>Ticker symbol the snapshot applies to.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Market capitalisation in USD millions.</summary>
    [JsonPropertyName("market_cap")]
    public double? MarketCap { get; init; }

    /// <summary>Trailing twelve-month price/earnings ratio.</summary>
    [JsonPropertyName("pe_ttm")]
    public double? PeTtm { get; init; }

    /// <summary>Annual price/book ratio.</summary>
    [JsonPropertyName("pb_annual")]
    public double? PbAnnual { get; init; }

    /// <summary>Trailing twelve-month earnings per share.</summary>
    [JsonPropertyName("eps_ttm")]
    public double? EpsTtm { get; init; }

    /// <summary>Indicated annual dividend yield.</summary>
    [JsonPropertyName("dividend_yield")]
    public double? DividendYield { get; init; }

    /// <summary>52-week high price.</summary>
    [JsonPropertyName("week52_high")]
    public double? Week52High { get; init; }

    /// <summary>52-week low price.</summary>
    [JsonPropertyName("week52_low")]
    public double? Week52Low { get; init; }

    /// <summary>52-week price return (percent).</summary>
    [JsonPropertyName("week52_price_return_pct")]
    public double? Week52PriceReturnPct { get; init; }

    /// <summary>Beta vs market.</summary>
    [JsonPropertyName("beta")]
    public double? Beta { get; init; }

    /// <summary>Trailing twelve-month revenue per share.</summary>
    [JsonPropertyName("revenue_per_share_ttm")]
    public double? RevenuePerShareTtm { get; init; }

    /// <summary>Raw upstream metric dictionary. Populated only when the tool was invoked with <c>view = "full"</c>.</summary>
    [JsonPropertyName("raw")]
    public IReadOnlyDictionary<string, double?>? Raw { get; init; }
}
