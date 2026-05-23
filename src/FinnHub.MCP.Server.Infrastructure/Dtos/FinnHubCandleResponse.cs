// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Wire DTO for the Finnhub <c>/stock/candle</c> endpoint. Status field <c>s</c>
/// is <c>"ok"</c> on success or <c>"no_data"</c> when the range yielded no candles.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubCandleResponse
{
    /// <summary>Open prices.</summary>
    [JsonPropertyName("o")]
    public double[]? Open { get; init; }

    /// <summary>High prices.</summary>
    [JsonPropertyName("h")]
    public double[]? High { get; init; }

    /// <summary>Low prices.</summary>
    [JsonPropertyName("l")]
    public double[]? Low { get; init; }

    /// <summary>Close prices.</summary>
    [JsonPropertyName("c")]
    public double[]? Close { get; init; }

    /// <summary>Volume.</summary>
    [JsonPropertyName("v")]
    public long[]? Volume { get; init; }

    /// <summary>Unix-epoch (seconds) timestamps.</summary>
    [JsonPropertyName("t")]
    public long[]? Timestamps { get; init; }

    /// <summary>Status code: <c>ok</c> or <c>no_data</c>.</summary>
    [JsonPropertyName("s")]
    public string? Status { get; init; }
}
