// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Calendar.Clients;
using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Calendar.Services;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Calendar.Services;

public sealed class CalendarServiceTests
{
    private readonly ICalendarApiClient _apiClient = Substitute.For<ICalendarApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly CalendarService _sut;

    public CalendarServiceTests()
    {
        this._sut = new CalendarService(this._apiClient, this._cache, NullLogger<CalendarService>.Instance);
    }

    private static GetCalendarQuery EarningsQuery(string? symbol = "AAPL") => new()
    {
        QueryId = "q1",
        Kind = CalendarKind.Earnings,
        From = new DateOnly(2026, 5, 1),
        To = new DateOnly(2026, 6, 1),
        Symbol = symbol
    };

    private static EarningsEvent Event(string symbol, int dayOffset) => new()
    {
        Symbol = symbol,
        Date = new DateOnly(2026, 5, 1).AddDays(dayOffset),
        EpsEstimate = 1.0
    };

    [Fact]
    public async Task GetCalendarAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetCalendarAsync(null!));
    }

    [Fact]
    public async Task GetCalendarAsync_EarningsWithEvents_ReturnsSuccess()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns([Event("AAPL", 1)]);

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.True(result.IsSuccess);
        Assert.Equal("earnings", result.Data!.Kind);
        Assert.Equal(1, result.Data.TotalCount);
        Assert.Single(result.Data.EarningsEvents!);
    }

    [Fact]
    public async Task GetCalendarAsync_EarningsEmpty_ReturnsNotFound()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_OrdersEventsByDateThenSymbol()
    {
        EarningsEvent[] unordered =
        [
            Event("AAPL", 5),
            Event("MSFT", 1),
            Event("GOOG", 1)
        ];
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(unordered);

        var result = await this._sut.GetCalendarAsync(EarningsQuery(symbol: null));

        Assert.True(result.IsSuccess);
        var events = result.Data!.EarningsEvents!;
        Assert.Equal("GOOG", events[0].Symbol);
        Assert.Equal("MSFT", events[1].Symbol);
        Assert.Equal("AAPL", events[2].Symbol);
    }

    [Fact]
    public async Task GetCalendarAsync_TwoIdenticalCalls_HitsApiClientExactlyOnce()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns([Event("AAPL", 1)]);

        var query = EarningsQuery();
        await this._sut.GetCalendarAsync(query);
        await this._sut.GetCalendarAsync(query);

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetEarningsCalendarAsync(
            Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCalendarAsync_HttpError_MapsToServiceUnavailable()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_Premium_MapsToPremiumRequired()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/calendar/earnings", "premium"));

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_Timeout_MapsToTimeout()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientTimeoutException("slow"));

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("Timeout", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_InvalidResponse_MapsToInvalidResponse()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientDeserializationException("bad json"));

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("InvalidResponse", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_Cancelled_RethrowsTypedException()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientCancelledException("cancelled"));

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetCalendarAsync(EarningsQuery()));
    }

    [Fact]
    public async Task GetCalendarAsync_UnknownApiClientException_MapsToUnknown()
    {
        this._apiClient
            .GetEarningsCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientUnexpectedException("weird"));

        var result = await this._sut.GetCalendarAsync(EarningsQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("Unknown", result.ErrorType);
    }

    private static GetCalendarQuery IpoQuery() => new()
    {
        QueryId = "q1",
        Kind = CalendarKind.Ipo,
        From = new DateOnly(2026, 6, 1),
        To = new DateOnly(2026, 12, 31),
        Symbol = null
    };

    private static IpoEvent Ipo(string name, int dayOffset, string? symbol = "NEWCO") => new()
    {
        Name = name,
        Date = new DateOnly(2026, 6, 1).AddDays(dayOffset),
        Symbol = symbol,
        Exchange = "NASDAQ",
        Status = "priced"
    };

    [Fact]
    public async Task GetCalendarAsync_IpoWithEvents_ReturnsSuccess()
    {
        this._apiClient
            .GetIpoCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns([Ipo("Acme Co", 0)]);

        var result = await this._sut.GetCalendarAsync(IpoQuery());

        Assert.True(result.IsSuccess);
        Assert.Equal("ipo", result.Data!.Kind);
        Assert.Single(result.Data.IpoEvents!);
        Assert.Null(result.Data.EarningsEvents);
        Assert.Null(result.Data.Symbol);
    }

    [Fact]
    public async Task GetCalendarAsync_IpoEmpty_ReturnsNotFound()
    {
        this._apiClient
            .GetIpoCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await this._sut.GetCalendarAsync(IpoQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_IpoOrdersMostRecentFirstThenByName()
    {
        IpoEvent[] unordered =
        [
            Ipo("Earlier Co", 0),
            Ipo("Later Co B", 10),
            Ipo("Later Co A", 10)
        ];
        this._apiClient
            .GetIpoCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(unordered);

        var result = await this._sut.GetCalendarAsync(IpoQuery());

        Assert.True(result.IsSuccess);
        var events = result.Data!.IpoEvents!;
        Assert.Equal("Later Co A", events[0].Name);
        Assert.Equal("Later Co B", events[1].Name);
        Assert.Equal("Earlier Co", events[2].Name);
    }

    [Fact]
    public async Task GetCalendarAsync_IpoTwoIdenticalCalls_HitsApiClientExactlyOnce()
    {
        this._apiClient
            .GetIpoCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns([Ipo("Acme", 0)]);

        var query = IpoQuery();
        await this._sut.GetCalendarAsync(query);
        await this._sut.GetCalendarAsync(query);

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetIpoCalendarAsync(
            Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCalendarAsync_IpoHttpError_MapsToServiceUnavailable()
    {
        this._apiClient
            .GetIpoCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetCalendarAsync(IpoQuery());

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }

    private static GetCalendarQuery EconomicQuery(string? country = "US") => new()
    {
        QueryId = "q1",
        Kind = CalendarKind.Economic,
        From = new DateOnly(2026, 6, 1),
        To = new DateOnly(2026, 6, 30),
        Symbol = null,
        Country = country
    };

    private static EconomicEvent Macro(string country, int dayOffset, string name = "CPI YoY") => new()
    {
        Country = country,
        EventName = name,
        Time = new DateTime(2026, 6, 1, 13, 30, 0, DateTimeKind.Utc).AddDays(dayOffset),
        Impact = "medium"
    };

    [Fact]
    public async Task GetCalendarAsync_EconomicWithCountry_FiltersToCountryAndReturnsSuccess()
    {
        EconomicEvent[] upstream =
        [
            Macro("US", 0),
            Macro("GB", 1),
            Macro("US", 2),
            Macro("DE", 3)
        ];
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetCalendarAsync(EconomicQuery("US"));

        Assert.True(result.IsSuccess);
        Assert.Equal("economic", result.Data!.Kind);
        Assert.Equal("US", result.Data.Country);
        Assert.Equal(2, result.Data.TotalCount);
        Assert.All(result.Data.EconomicEvents!, e => Assert.Equal("US", e.Country));
    }

    [Fact]
    public async Task GetCalendarAsync_EconomicWithoutCountry_ReturnsAllCountries()
    {
        EconomicEvent[] upstream =
        [
            Macro("US", 0),
            Macro("GB", 1),
            Macro("DE", 2)
        ];
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetCalendarAsync(EconomicQuery(country: null));

        Assert.True(result.IsSuccess);
        Assert.Null(result.Data!.Country);
        Assert.Equal(3, result.Data.TotalCount);
    }

    [Fact]
    public async Task GetCalendarAsync_EconomicCountryFilterMatchesNothing_ReturnsNotFound()
    {
        EconomicEvent[] upstream =
        [
            Macro("GB", 1),
            Macro("DE", 2)
        ];
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetCalendarAsync(EconomicQuery("US"));

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
        Assert.Contains("US", result.ErrorMessage, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetCalendarAsync_EconomicEmpty_ReturnsNotFound()
    {
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await this._sut.GetCalendarAsync(EconomicQuery(country: null));

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetCalendarAsync_EconomicOrdersByTimeAscending()
    {
        EconomicEvent[] unordered =
        [
            Macro("US", 5),
            Macro("US", 0, name: "PMI"),
            Macro("US", 0, name: "CPI YoY")
        ];
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(unordered);

        var result = await this._sut.GetCalendarAsync(EconomicQuery("US"));

        Assert.True(result.IsSuccess);
        var events = result.Data!.EconomicEvents!;
        Assert.Equal("CPI YoY", events[0].EventName);
        Assert.Equal("PMI", events[1].EventName);
        Assert.Equal(5, (events[2].Time - events[0].Time).Days);
    }

    [Fact]
    public async Task GetCalendarAsync_EconomicTwoCallsDifferentCountries_HitsApiClientExactlyOnce()
    {
        // Country filter is applied after cache — one upstream fetch serves both calls.
        EconomicEvent[] upstream = [Macro("US", 0), Macro("GB", 0)];
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        await this._sut.GetCalendarAsync(EconomicQuery("US"));
        await this._sut.GetCalendarAsync(EconomicQuery("GB"));

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetEconomicCalendarAsync(
            Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCalendarAsync_EconomicHttpError_MapsToServiceUnavailable()
    {
        this._apiClient
            .GetEconomicCalendarAsync(Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetCalendarAsync(EconomicQuery("US"));

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }
}
