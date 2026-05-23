// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;

/// <summary>
/// Query parameters for the <c>get-price-summary</c> tool — a Finnhub
/// <c>/stock/candle</c> lookup that aggregates the candle range into curated stats.
/// </summary>
public sealed class GetPriceSummaryQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the summary is requested for.</summary>
    public required string Symbol { get; init; }

    /// <summary>Lookback window (default 30 days).</summary>
    public PricePeriod Period { get; init; } = PricePeriod.ThirtyDays;

    /// <summary>Whether the response should carry the raw OHLCV arrays in addition to the summary stats.</summary>
    public bool IncludeCandles { get; init; }
}
