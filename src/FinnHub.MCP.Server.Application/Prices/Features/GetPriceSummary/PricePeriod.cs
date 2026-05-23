// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;

/// <summary>
/// Lookback windows the <c>get-price-summary</c> tool supports. Each value resolves to
/// a Finnhub <c>resolution</c> and a from/to epoch pair under the hood.
/// </summary>
public enum PricePeriod
{
    /// <summary>7 calendar days at daily resolution.</summary>
    SevenDays,

    /// <summary>30 calendar days at daily resolution (default).</summary>
    ThirtyDays,

    /// <summary>90 calendar days at daily resolution.</summary>
    NinetyDays,

    /// <summary>365 calendar days at weekly resolution.</summary>
    OneYear
}
