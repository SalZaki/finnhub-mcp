// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;

/// <summary>
/// Represents a paginated response that contains one or more stock exchange records.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ExchangesResponse
{
    /// <summary>
    /// Gets the list of stock exchanges returned by the API.
    /// </summary>
    [JsonPropertyName("exchanges")]
    public IReadOnlyList<Exchange> Exchanges { get; init; } = [];

    /// <summary>
    /// Gets the number of exchanges in the response.
    /// </summary>
    [JsonPropertyName("total_count")]
    public int TotalCount => this.Exchanges.Count;

    /// <summary>
    /// Gets whether there are any exchanges returned.
    /// </summary>
    [JsonPropertyName("has_results")]
    public bool HasResults => this.TotalCount > 0;
}
