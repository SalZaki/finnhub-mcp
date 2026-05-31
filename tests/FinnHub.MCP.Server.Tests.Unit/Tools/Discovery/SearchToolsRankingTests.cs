// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Discovery;
using FinnHub.MCP.Server.Tools.Discovery;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Discovery;

/// <summary>
/// Acceptance-criteria rankings (spec §3 P7 / issue #219) exercised against the real
/// <see cref="ToolCatalog"/> — the descriptors and curated examples actually shipped.
/// </summary>
public sealed class SearchToolsRankingTests
{
    private static ToolRegistry RealRegistry() => new(ToolCatalog.Descriptors);

    [Fact]
    public void UpcomingEarningsTechSector_RanksCalendarOrFinancialsInTopThree()
    {
        var top3 = RealRegistry()
            .Search("upcoming earnings tech sector", topN: 3)
            .Select(m => m.Tool.Name)
            .ToList();

        Assert.True(
            top3.Contains("get-calendar") || top3.Contains("get-financials-snapshot"),
            $"Expected get-calendar or get-financials-snapshot in top 3, got: {string.Join(", ", top3)}");
    }

    [Fact]
    public void IsAppleStockUpThisWeek_RanksPriceSummaryFirstNewsPulseSecond()
    {
        var ranked = RealRegistry()
            .Search("is apple stock up this week", topN: 5)
            .Select(m => m.Tool.Name)
            .ToList();

        Assert.True(ranked.Count >= 2, $"Expected at least 2 matches, got: {string.Join(", ", ranked)}");
        Assert.Equal("get-price-summary", ranked[0]);
        Assert.Equal("get-news-pulse", ranked[1]);
    }
}
