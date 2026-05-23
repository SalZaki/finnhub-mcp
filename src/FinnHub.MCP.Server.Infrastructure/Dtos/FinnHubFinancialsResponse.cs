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
/// Wire DTO for the Finnhub <c>/stock/metric</c> endpoint. The upstream payload
/// nests a flat metric dictionary under a <c>metric</c> property; the dictionary
/// has dozens of keys, all numeric or string-formatted dates. We project only
/// the numeric subset and let the client map specific keys to typed KPIs.
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

    /// <summary>Flat dictionary of metric names to numeric values.</summary>
    [JsonPropertyName("metric")]
    public Dictionary<string, double?>? Metric { get; init; }
}
