// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Discovery;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Discovery;

/// <summary>
/// The curated set of <see cref="ToolDescriptor"/>s the <see cref="ToolRegistry"/> ranks over.
/// Names, titles and descriptions come from <see cref="Constants.Tools"/> (the single source of
/// truth shared with each tool's <c>[McpServerTool]</c> registration); the <c>Examples</c> are
/// curated natural-language intents that align ranking with how users actually phrase requests.
/// </summary>
/// <remarks>
/// Lives in the Server layer because <see cref="Constants"/> does — the Application
/// <see cref="ToolRegistry"/> stays host-agnostic and receives these descriptors via DI. Every
/// tool registered with <c>WithWrappedTools&lt;&gt;</c> in <c>Program.cs</c> must have an entry
/// here; the drift test enforces that parity. <c>Premium</c> stays <c>false</c> — the runtime
/// envelope's <c>premium</c> flag is the authoritative per-call signal.
/// </remarks>
internal static class ToolCatalog
{
    public static IReadOnlyList<ToolDescriptor> Descriptors { get; } =
    [
        new ToolDescriptor
        {
            Name = Constants.Tools.SearchTools.Name,
            Title = Constants.Tools.SearchTools.Title,
            Description = Constants.Tools.SearchTools.Description,
            Category = "Discovery",
            Searchable = false,
            Examples =
            [
                "find the right tool for a task",
                "which tool should i use",
                "discover tools by intent"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.SearchSymbols.Name,
            Title = Constants.Tools.SearchSymbols.Title,
            Description = Constants.Tools.SearchSymbols.Description,
            Category = "Discovery",
            Examples =
            [
                "find a ticker by company name",
                "look for a ticker by symbol",
                "resolve a company by name, ISIN or CUSIP"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.Quote.Name,
            Title = Constants.Tools.Quote.Title,
            Description = Constants.Tools.Quote.Description,
            Category = "Pricing",
            Examples =
            [
                "current stock price right now",
                "latest real-time quote",
                "what is a stock trading at now"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.CompanyProfile.Name,
            Title = Constants.Tools.CompanyProfile.Title,
            Description = Constants.Tools.CompanyProfile.Description,
            Category = "Company",
            Examples =
            [
                "company profile and what it does",
                "industry, country and market cap",
                "company headquarters and IPO date"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.Peers.Name,
            Title = Constants.Tools.Peers.Title,
            Description = Constants.Tools.Peers.Description,
            Category = "Comparison",
            Examples =
            [
                "industry peers and competitors",
                "companies in the same sector",
                "comparable companies to compare against"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.FinancialsSnapshot.Name,
            Title = Constants.Tools.FinancialsSnapshot.Title,
            Description = Constants.Tools.FinancialsSnapshot.Description,
            Category = "Fundamentals",
            Examples =
            [
                "valuation ratios and fundamentals",
                "price-to-earnings ratio and market cap",
                "fundamental financial metrics and margins"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.PriceSummary.Name,
            Title = Constants.Tools.PriceSummary.Title,
            Description = Constants.Tools.PriceSummary.Description,
            Category = "Pricing",
            Examples =
            [
                "is the stock up this week",
                "stock price up or down this week",
                "price movement and return over the week",
                "how the price has performed recently",
                "price range and volatility"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.NewsPulse.Name,
            Title = Constants.Tools.NewsPulse.Title,
            Description = Constants.Tools.NewsPulse.Description,
            Category = "News",
            Examples =
            [
                "latest news this week",
                "news headlines and market sentiment",
                "week over week news and sentiment",
                "what is moving a stock on news this week",
                "what is the news saying"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.Calendar.Name,
            Title = Constants.Tools.Calendar.Title,
            Description = Constants.Tools.Calendar.Description,
            Category = "Events",
            Examples =
            [
                "upcoming earnings dates",
                "upcoming earnings this season",
                "upcoming ipo listings",
                "economic release schedule",
                "when a company reports earnings"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.InsiderSignal.Name,
            Title = Constants.Tools.InsiderSignal.Title,
            Description = Constants.Tools.InsiderSignal.Description,
            Category = "Insider",
            Examples =
            [
                "are insiders buying or selling",
                "insider transactions activity",
                "insider trading signal"
            ]
        },
        new ToolDescriptor
        {
            Name = Constants.Tools.Recommendations.Name,
            Title = Constants.Tools.Recommendations.Title,
            Description = Constants.Tools.Recommendations.Description,
            Category = "Analyst",
            Examples =
            [
                "what analysts recommend",
                "analyst consensus and ratings",
                "is analyst sentiment turning bullish or bearish"
            ]
        },
    ];
}
