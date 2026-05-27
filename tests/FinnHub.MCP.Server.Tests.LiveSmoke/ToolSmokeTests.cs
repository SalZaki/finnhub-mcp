// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Calendar.Services;
using FinnHub.MCP.Server.Application.Financials.Services;
using FinnHub.MCP.Server.Application.Insiders.Services;
using FinnHub.MCP.Server.Application.News.Services;
using FinnHub.MCP.Server.Application.Peers.Services;
using FinnHub.MCP.Server.Application.Prices.Services;
using FinnHub.MCP.Server.Application.Profiles.Services;
using FinnHub.MCP.Server.Application.Quotes.Services;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.Application.Symbols;
using FinnHub.MCP.Server.Tools.Calendar;
using FinnHub.MCP.Server.Tools.Financials;
using FinnHub.MCP.Server.Tools.Insiders;
using FinnHub.MCP.Server.Tools.News;
using FinnHub.MCP.Server.Tools.Peers;
using FinnHub.MCP.Server.Tools.Prices;
using FinnHub.MCP.Server.Tools.Profiles;
using FinnHub.MCP.Server.Tools.Quotes;
using FinnHub.MCP.Server.Tools.Search;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.LiveSmoke;

/// <summary>
/// Scheduled live-smoke run that hits real Finnhub through the full server pipeline.
/// </summary>
/// <remarks>
/// <para>
/// Each test resolves the production tool class from the host's DI container and
/// invokes it as a real MCP client would, then asserts the envelope reports
/// <c>IsSuccess=true</c> (or a tolerated typed failure like <c>PremiumRequired</c>).
/// </para>
/// <para>
/// What this catches that mocked unit tests can't:
/// <list type="bullet">
///   <item>Finnhub payload shape drift — DTO deserialization fails (PR #166 class)</item>
///   <item>URL-resolution regressions — `/api/...` vs `/api/v1/...` (PR #169 class)</item>
///   <item>Auth / API-key expiry — 401 from upstream</item>
///   <item>Endpoints becoming premium-gated overnight — 403</item>
///   <item>Host boot failures from DI / options misconfiguration</item>
/// </list>
/// </para>
/// <para>
/// Excluded from <c>dotnet test</c> by default via the <c>LiveSmoke</c> trait;
/// runs only when explicitly filtered (the live-smoke workflow does
/// <c>--filter Category=LiveSmoke</c>) so PR CI never burns the Finnhub quota.
/// </para>
/// </remarks>
[Trait("Category", "LiveSmoke")]
public sealed class ToolSmokeTests : IClassFixture<LiveSmokeFactory>
{
    private const string Symbol = "AAPL";

    private readonly IServiceProvider _services;

    public ToolSmokeTests(LiveSmokeFactory factory)
    {
        RequireApiKey();

        // Force the factory to construct the host so DI is ready.
        _ = factory.Services;
        this._services = factory.Services;
    }

    [Fact]
    public async Task SearchSymbol_AAPL_ReturnsMatches()
    {
        var tool = new SearchSymbolTool(
            this._services.GetRequiredService<ISearchService>(),
            this._services.GetRequiredService<ISymbolResolver>(),
            NullLogger<SearchSymbolTool>.Instance);

        var envelope = await tool.SearchSymbolAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetQuote_AAPL_ReturnsCurrentPrice()
    {
        var tool = new GetQuoteTool(
            this._services.GetRequiredService<IQuotesService>(),
            NullLogger<GetQuoteTool>.Instance);

        var envelope = await tool.GetQuoteAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetCompanyProfile_AAPL_ReturnsProfile()
    {
        var tool = new GetCompanyProfileTool(
            this._services.GetRequiredService<IProfilesService>(),
            NullLogger<GetCompanyProfileTool>.Instance);

        var envelope = await tool.GetCompanyProfileAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetFinancialsSnapshot_AAPL_ReturnsMetrics()
    {
        var tool = new GetFinancialsSnapshotTool(
            this._services.GetRequiredService<IFinancialsService>(),
            NullLogger<GetFinancialsSnapshotTool>.Instance);

        var envelope = await tool.GetFinancialsSnapshotAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetPriceSummary_AAPL_ReturnsCandles()
    {
        var tool = new GetPriceSummaryTool(
            this._services.GetRequiredService<IPricesService>(),
            NullLogger<GetPriceSummaryTool>.Instance);

        var envelope = await tool.GetPriceSummaryAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetNewsPulse_AAPL_ReturnsHeadlines()
    {
        var tool = new GetNewsPulseTool(
            this._services.GetRequiredService<INewsService>(),
            NullLogger<GetNewsPulseTool>.Instance);

        var envelope = await tool.GetNewsPulseAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetPeers_AAPL_ReturnsPeerList()
    {
        var tool = new GetPeersTool(
            this._services.GetRequiredService<IPeersService>(),
            NullLogger<GetPeersTool>.Instance);

        var envelope = await tool.GetPeersAsync(Symbol);

        AssertSuccessOrPremium(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetCalendar_EarningsAapl_ReturnsScheduleOrDegrades()
    {
        // AAPL has scheduled earnings most quarters but the next event can be
        // outside a 90-day window depending on when this runs; tolerate NotFound
        // alongside Success/PremiumRequired so the smoke doesn't flap.
        var tool = new GetCalendarTool(
            this._services.GetRequiredService<ICalendarService>(),
            NullLogger<GetCalendarTool>.Instance);

        var envelope = await tool.GetCalendarAsync("earnings", symbol: Symbol);

        AssertGracefulDegradation(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetCalendar_IpoForward12Months_ReturnsListingsOrDegrades()
    {
        // IPO calendars are sparse for free-tier keys and can return zero entries
        // for a forward window; tolerate NotFound alongside Success/PremiumRequired.
        var tool = new GetCalendarTool(
            this._services.GetRequiredService<ICalendarService>(),
            NullLogger<GetCalendarTool>.Instance);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var envelope = await tool.GetCalendarAsync(
            "ipo",
            from: today.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
            to: today.AddDays(365).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));

        AssertGracefulDegradation(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetInsiderSignal_AAPL_ReturnsSignalOrDegrades()
    {
        // AAPL insider activity can lull for stretches; tolerate NotFound alongside
        // Success / PremiumRequired so the smoke doesn't flap on quiet weeks.
        var tool = new GetInsiderSignalTool(
            this._services.GetRequiredService<IInsidersService>(),
            NullLogger<GetInsiderSignalTool>.Instance);

        var envelope = await tool.GetInsiderSignalAsync(Symbol);

        AssertGracefulDegradation(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetCalendar_EconomicUsForward30Days_ReturnsScheduleOrDegrades()
    {
        // The /calendar/economic feed is dense globally but the US-filtered slice can
        // be sparse over a short window depending on FOMC / payrolls timing; tolerate
        // NotFound alongside Success/PremiumRequired so the smoke doesn't flap.
        var tool = new GetCalendarTool(
            this._services.GetRequiredService<ICalendarService>(),
            NullLogger<GetCalendarTool>.Instance);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var envelope = await tool.GetCalendarAsync(
            "economic",
            country: "US",
            from: today.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
            to: today.AddDays(30).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));

        AssertGracefulDegradation(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    [Fact]
    public async Task GetQuote_UnknownSymbol_DegradesGracefully()
    {
        // Finnhub returns {"c":0,"d":null,"dp":null,...} for unknown tickers.
        // PR #167 made the DTOs tolerate the nulls so deserialization doesn't
        // crash; the service layer is free to either pass that through as
        // IsSuccess=true with degraded data, OR detect the all-zeros shape
        // and return a typed NotFound envelope. Both are "graceful degradation"
        // — the regression we're guarding against is an unhandled exception
        // or a ServiceUnavailable bubbling up from a deserialization failure.
        var tool = new GetQuoteTool(
            this._services.GetRequiredService<IQuotesService>(),
            NullLogger<GetQuoteTool>.Instance);

        var envelope = await tool.GetQuoteAsync("ZZZZ");

        AssertGracefulDegradation(envelope.IsSuccess, envelope.ErrorType, envelope.ErrorMessage);
    }

    private static void AssertSuccessOrPremium(bool isSuccess, string? errorType, string? errorMessage)
    {
        if (isSuccess)
        {
            return;
        }

        // PremiumRequired is a legitimate Finnhub state — the endpoint exists
        // but the smoke key tier can't reach it. Treat as a smoke pass.
        if (string.Equals(errorType, "PremiumRequired", StringComparison.Ordinal))
        {
            return;
        }

        Assert.Fail($"Tool failed against real Finnhub. error_type={errorType ?? "<null>"} error_message={errorMessage ?? "<null>"}");
    }

    private static void AssertGracefulDegradation(bool isSuccess, string? errorType, string? errorMessage)
    {
        // Tolerated outcomes for an unknown / nonsense input:
        //   - IsSuccess=true  → service passed through degraded data (zeros / nulls)
        //   - NotFound        → service detected the bogus symbol and returned a typed not-found
        //   - PremiumRequired → endpoint is premium-gated (unlikely for /quote but kept for parity)
        // Anything else (ServiceUnavailable, an uncaught exception type, etc.) means
        // the failure path itself broke — that's the regression we want to catch.
        if (isSuccess
            || string.Equals(errorType, "NotFound", StringComparison.Ordinal)
            || string.Equals(errorType, "PremiumRequired", StringComparison.Ordinal))
        {
            return;
        }

        Assert.Fail($"Unknown-symbol path failed unexpectedly. error_type={errorType ?? "<null>"} error_message={errorMessage ?? "<null>"}");
    }

    private static void RequireApiKey()
    {
        // The live-smoke workflow gates on this secret being set; the call here
        // is a defensive failure for local runs that forgot to source the .env.
        // Failing loudly is better than silently passing — a no-op smoke is the
        // exact failure mode this whole project is meant to prevent.
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FINNHUB_API_KEY")))
        {
            throw new InvalidOperationException(
                "FINNHUB_API_KEY is not set. Live-smoke needs a real Finnhub key. " +
                "Set the env var locally or run via the live-smoke.yml workflow.");
        }
    }
}
