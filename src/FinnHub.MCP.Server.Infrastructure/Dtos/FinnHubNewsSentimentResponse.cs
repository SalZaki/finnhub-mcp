// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>Wire DTO for the Finnhub <c>/news-sentiment</c> endpoint (premium).</summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubNewsSentimentResponse
{
    /// <summary>Symbol echoed by Finnhub.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Composite company news sentiment score.</summary>
    [JsonPropertyName("companyNewsScore")]
    public double? CompanyNewsScore { get; init; }

    /// <summary>Nested sentiment breakdown.</summary>
    [JsonPropertyName("sentiment")]
    public FinnHubNewsSentimentBreakdown? Sentiment { get; init; }
}

/// <summary>Bullish/bearish breakdown nested under <c>sentiment</c>.</summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubNewsSentimentBreakdown
{
    /// <summary>Share of news scored as bullish (0..1).</summary>
    [JsonPropertyName("bullishPercent")]
    public double? BullishPercent { get; init; }

    /// <summary>Share of news scored as bearish (0..1).</summary>
    [JsonPropertyName("bearishPercent")]
    public double? BearishPercent { get; init; }
}
