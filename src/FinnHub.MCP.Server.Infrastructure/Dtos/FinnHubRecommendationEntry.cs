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
/// Wire DTO for a single entry in Finnhub's <c>/stock/recommendation</c> response.
/// </summary>
/// <remarks>
/// The endpoint returns a JSON array (no envelope) ordered most-recent first.
/// <c>period</c> is the first calendar day of the covered month as ISO
/// <c>yyyy-MM-dd</c>. Property names are camelCase, overriding the snake_case default
/// in <see cref="Serialization.FinnHubJsonContext"/>.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubRecommendationEntry
{
    /// <summary>Echo of the queried symbol.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Period covered by the snapshot as ISO <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("period")]
    public string? Period { get; init; }

    /// <summary>Number of analysts at <c>Strong Buy</c>.</summary>
    [JsonPropertyName("strongBuy")]
    public int? StrongBuy { get; init; }

    /// <summary>Number of analysts at <c>Buy</c>.</summary>
    [JsonPropertyName("buy")]
    public int? Buy { get; init; }

    /// <summary>Number of analysts at <c>Hold</c>.</summary>
    [JsonPropertyName("hold")]
    public int? Hold { get; init; }

    /// <summary>Number of analysts at <c>Sell</c>.</summary>
    [JsonPropertyName("sell")]
    public int? Sell { get; init; }

    /// <summary>Number of analysts at <c>Strong Sell</c>.</summary>
    [JsonPropertyName("strongSell")]
    public int? StrongSell { get; init; }
}
