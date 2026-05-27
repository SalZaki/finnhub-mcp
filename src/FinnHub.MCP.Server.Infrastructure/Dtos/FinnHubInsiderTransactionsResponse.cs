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
/// Wire DTO for the Finnhub <c>/stock/insider-transactions</c> endpoint envelope.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubInsiderTransactionsResponse
{
    /// <summary>The list of insider transactions; may be empty when no filings match the window.</summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<FinnHubInsiderTransactionEntry>? Data { get; init; }

    /// <summary>Echo of the queried symbol.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }
}
