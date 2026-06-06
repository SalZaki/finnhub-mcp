// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;

/// <summary>
/// A single symbol listed on an exchange — the slim projection returned in the
/// <c>get-exchange-symbols</c> sample and the element type the upstream rows map to.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ExchangeSymbol
{
    /// <summary>The ticker symbol, e.g. <c>"AAPL"</c>.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>The display symbol as shown by the exchange.</summary>
    [JsonPropertyName("display_symbol")]
    public string? DisplaySymbol { get; init; }

    /// <summary>Human-readable security description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Security type, e.g. <c>"Common Stock"</c> or <c>"ETP"</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
