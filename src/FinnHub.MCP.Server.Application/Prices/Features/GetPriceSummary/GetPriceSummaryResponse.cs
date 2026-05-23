// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;

/// <summary>
/// Wire response for <c>get-price-summary</c>. Server-side aggregation over the
/// upstream candle range. <see cref="Candles"/> is populated only when the tool
/// was invoked with <c>view = "full"</c>.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetPriceSummaryResponse
{
    /// <summary>Ticker symbol the summary applies to.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Lookback window the summary covers, e.g. <c>30d</c>, <c>1y</c>.</summary>
    [JsonPropertyName("period")]
    public required string Period { get; init; }

    /// <summary>Finnhub resolution code, e.g. <c>D</c>, <c>W</c>.</summary>
    [JsonPropertyName("resolution")]
    public required string Resolution { get; init; }

    /// <summary>Minimum low across the range, or <c>null</c> if no candles were returned.</summary>
    [JsonPropertyName("min")]
    public double? Min { get; init; }

    /// <summary>Maximum high across the range, or <c>null</c> if no candles were returned.</summary>
    [JsonPropertyName("max")]
    public double? Max { get; init; }

    /// <summary>Arithmetic mean of closing prices, or <c>null</c> if no candles were returned.</summary>
    [JsonPropertyName("mean")]
    public double? Mean { get; init; }

    /// <summary>Total return (percent) from the first close to the last close.</summary>
    [JsonPropertyName("return_pct")]
    public double? ReturnPct { get; init; }

    /// <summary>Population standard deviation of closing prices (volatility proxy).</summary>
    [JsonPropertyName("vol")]
    public double? Vol { get; init; }

    /// <summary>Most-recent close and its UTC timestamp.</summary>
    [JsonPropertyName("latest")]
    public PriceLatest? Latest { get; init; }

    /// <summary>Number of candles aggregated.</summary>
    [JsonPropertyName("candle_count")]
    public int CandleCount { get; init; }

    /// <summary>Raw OHLCV arrays. Populated only when the tool was invoked with <c>view = "full"</c>.</summary>
    [JsonPropertyName("candles")]
    public PriceCandles? Candles { get; init; }
}

/// <summary>Most-recent candle's close and timestamp.</summary>
/// <param name="Close">Closing price.</param>
/// <param name="TimestampUtc">UTC timestamp of the candle.</param>
[ExcludeFromCodeCoverage]
public sealed record PriceLatest(
    [property: JsonPropertyName("close")] double Close,
    [property: JsonPropertyName("timestamp_utc")] DateTimeOffset TimestampUtc);

/// <summary>Raw OHLCV arrays, returned only for <c>view = "full"</c>.</summary>
[ExcludeFromCodeCoverage]
public sealed class PriceCandles
{
    /// <summary>Open prices.</summary>
    [JsonPropertyName("o")]
    public required IReadOnlyList<double> Open { get; init; }

    /// <summary>High prices.</summary>
    [JsonPropertyName("h")]
    public required IReadOnlyList<double> High { get; init; }

    /// <summary>Low prices.</summary>
    [JsonPropertyName("l")]
    public required IReadOnlyList<double> Low { get; init; }

    /// <summary>Close prices.</summary>
    [JsonPropertyName("c")]
    public required IReadOnlyList<double> Close { get; init; }

    /// <summary>Volume.</summary>
    [JsonPropertyName("v")]
    public required IReadOnlyList<long> Volume { get; init; }

    /// <summary>Unix-epoch (seconds) timestamps.</summary>
    [JsonPropertyName("t")]
    public required IReadOnlyList<long> Timestamps { get; init; }
}
