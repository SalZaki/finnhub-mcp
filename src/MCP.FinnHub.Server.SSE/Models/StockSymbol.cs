// --------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of the MCP FinnHub project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace MCP.FinnHub.Server.SSE.Models;

/// <summary>
/// Represents a stock symbol as returned by the FinnHub API.
/// This includes metadata such as exchange codes, security type, and identifiers.
/// </summary>
public sealed class StockSymbol
{
    /// <summary>
    /// The currency in which the stock is traded (e.g., "USD").
    /// </summary>
    public required string Currency { get; init; }

    /// <summary>
    /// A human-readable name or description of the company or security.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The symbol displayed to users or traders, which may differ slightly from the raw symbol.
    /// </summary>
    public required string DisplaySymbol { get; init; }

    /// <summary>
    /// The FIGI (Financial Instrument Global Identifier) for the security.
    /// </summary>
    public required string Figi { get; init; }

    /// <summary>
    /// The MIC (Market Identifier Code) where the stock is traded (e.g., "XNYS" for NYSE).
    /// </summary>
    public required string Mic { get; init; }

    /// <summary>
    /// The raw symbol code used to identify the stock on the exchange.
    /// </summary>
    public required string Symbol { get; init; }

    /// <summary>
    /// The type of security (e.g., "Common Stock").
    /// </summary>
    public required string Type { get; init; }
}
