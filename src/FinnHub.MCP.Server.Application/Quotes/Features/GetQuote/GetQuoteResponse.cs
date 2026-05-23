// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;

/// <summary>
/// Wire response for <c>get-quote</c>. Real-time price snapshot.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetQuoteResponse
{
    /// <summary>Ticker symbol.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Current price.</summary>
    [JsonPropertyName("current")]
    public double Current { get; init; }

    /// <summary>Absolute price change vs previous close.</summary>
    [JsonPropertyName("change")]
    public double Change { get; init; }

    /// <summary>Percentage price change vs previous close.</summary>
    [JsonPropertyName("percent_change")]
    public double PercentChange { get; init; }

    /// <summary>Session high.</summary>
    [JsonPropertyName("high")]
    public double High { get; init; }

    /// <summary>Session low.</summary>
    [JsonPropertyName("low")]
    public double Low { get; init; }

    /// <summary>Session open.</summary>
    [JsonPropertyName("open")]
    public double Open { get; init; }

    /// <summary>Previous close.</summary>
    [JsonPropertyName("prev_close")]
    public double PrevClose { get; init; }

    /// <summary>UTC timestamp of the snapshot.</summary>
    [JsonPropertyName("timestamp_utc")]
    public DateTimeOffset TimestampUtc { get; init; }
}
