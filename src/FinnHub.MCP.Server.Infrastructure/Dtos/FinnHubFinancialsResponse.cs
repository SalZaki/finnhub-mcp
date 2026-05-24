// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Wire DTO for the Finnhub <c>/stock/metric</c> endpoint. The upstream payload
/// nests a flat metric dictionary under a <c>metric</c> property; the dictionary
/// contains a mix of numeric KPIs and string-formatted dates
/// (e.g. <c>52WeekHighDate: "2024-12-26"</c>), so values are held as
/// <see cref="JsonElement"/> and coerced to <c>double?</c> by the client.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubFinancialsResponse
{
    /// <summary>Symbol echoed by Finnhub.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Metric type requested (e.g. <c>all</c>).</summary>
    [JsonPropertyName("metricType")]
    public string? MetricType { get; init; }

    /// <summary>Flat dictionary of metric names to raw JSON values (numbers, strings, or null).</summary>
    [JsonPropertyName("metric")]
    public Dictionary<string, JsonElement>? Metric { get; init; }
}
