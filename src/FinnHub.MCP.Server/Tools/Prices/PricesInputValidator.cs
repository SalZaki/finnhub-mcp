// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;

namespace FinnHub.MCP.Server.Tools.Prices;

/// <summary>Validation helpers for the <c>get-price-summary</c> tool parameters.</summary>
internal static class PricesInputValidator
{
    public static PricePeriod ValidatePeriod(string? period)
    {
        if (string.IsNullOrWhiteSpace(period))
        {
            return PricePeriod.ThirtyDays;
        }

        return period.Trim().ToLowerInvariant() switch
        {
            "7d" => PricePeriod.SevenDays,
            "30d" => PricePeriod.ThirtyDays,
            "90d" => PricePeriod.NinetyDays,
            "1y" => PricePeriod.OneYear,
            _ => throw new ArgumentException(
                "Period must be one of: 7d, 30d, 90d, 1y.",
                nameof(period))
        };
    }
}
