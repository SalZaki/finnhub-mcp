// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;

/// <summary>
/// Wire response for <c>get-news-pulse</c>. Aggregates the company-news headline
/// volume for the current and prior 7-day windows and (when available) the
/// upstream sentiment scores.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetNewsPulseResponse
{
    /// <summary>Ticker symbol the pulse applies to.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Lookback window — always <c>7d</c> in v1.</summary>
    [JsonPropertyName("period")]
    public string Period { get; init; } = "7d";

    /// <summary>Composite sentiment score from Finnhub's <c>news-sentiment</c> endpoint, or <c>null</c> when the endpoint is premium-locked.</summary>
    [JsonPropertyName("sentiment_score")]
    public double? SentimentScore { get; init; }

    /// <summary>Bullish percent from upstream sentiment, or <c>null</c> when unavailable.</summary>
    [JsonPropertyName("bullish_percent")]
    public double? BullishPercent { get; init; }

    /// <summary>Bearish percent from upstream sentiment, or <c>null</c> when unavailable.</summary>
    [JsonPropertyName("bearish_percent")]
    public double? BearishPercent { get; init; }

    /// <summary>Where the sentiment numbers came from: <c>finnhub</c> when present, <c>null</c> when the endpoint was unavailable.</summary>
    [JsonPropertyName("sentiment_source")]
    public string? SentimentSource { get; init; }

    /// <summary>Top headlines from the current 7-day window (default 5; full list when <c>view = "full"</c>), ordered newest first.</summary>
    [JsonPropertyName("top_headlines")]
    public IReadOnlyList<NewsHeadline> TopHeadlines { get; init; } = [];

    /// <summary>Article count over the current 7-day window.</summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }

    /// <summary>Article-count difference vs the prior 7-day window (positive = more articles this week).</summary>
    [JsonPropertyName("delta_vs_prev_week")]
    public int DeltaVsPrevWeek { get; init; }
}

/// <summary>Curated wire shape for an individual news headline.</summary>
/// <param name="Headline">Article headline.</param>
/// <param name="Url">Public URL of the article.</param>
/// <param name="Source">Publisher name.</param>
/// <param name="DatetimeUtc">UTC publication timestamp.</param>
[ExcludeFromCodeCoverage]
public sealed record NewsHeadline(
    [property: JsonPropertyName("headline")] string Headline,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("datetime_utc")] DateTimeOffset DatetimeUtc);
