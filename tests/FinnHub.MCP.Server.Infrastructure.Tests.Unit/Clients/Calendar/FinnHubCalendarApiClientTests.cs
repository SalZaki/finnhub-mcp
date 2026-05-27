// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Calendar;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Calendar;

public sealed class FinnHubCalendarApiClientTests : IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubCalendarApiClient _sut;

    public FinnHubCalendarApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints =
            [
                new FinnHubEndPoint { Name = "calendar-earnings", Url = "calendar/earnings", IsActive = true },
                new FinnHubEndPoint { Name = "calendar-ipo", Url = "calendar/ipo", IsActive = true }
            ]
        });

        this._sut = new FinnHubCalendarApiClient(this._httpClient, options, NullLogger<FinnHubCalendarApiClient>.Instance);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_RealAaplResponse_MapsAllFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("calendar-earnings-aapl"));

        var events = await this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 8, 1), "AAPL", CancellationToken.None);

        var only = Assert.Single(events);
        Assert.Equal("AAPL", only.Symbol);
        Assert.Equal(new DateOnly(2026, 7, 29), only.Date);
        Assert.Equal("amc", only.Hour);
        Assert.Equal(3, only.Quarter);
        Assert.Equal(2026, only.Year);
        Assert.Null(only.EpsActual);
        Assert.Equal(1.9316, only.EpsEstimate);
        Assert.Null(only.RevenueActual);
        Assert.Equal(110771401872d, only.RevenueEstimate);
    }

    /// <summary>
    /// Regression: catches future URL-construction drift for the symbol-filtered path.
    /// See FinnHubProfilesApiClientTests for the bug class this protects against.
    /// </summary>
    [Fact]
    public async Task GetEarningsCalendarAsync_WithSymbol_HitsApiV1CalendarEarningsEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        await this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Equal(
            "https://finnhub.io/api/v1/calendar/earnings?from=2026-05-01&to=2026-06-01&symbol=AAPL",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_WithoutSymbol_OmitsSymbolParameter()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        await this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), null, CancellationToken.None);

        Assert.Equal(
            "https://finnhub.io/api/v1/calendar/earnings?from=2026-05-01&to=2026-06-01",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_EmptyEnvelope_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"earningsCalendar\":[]}");

        var events = await this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None);

        Assert.Empty(events);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_EntryWithMalformedDate_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"earningsCalendar\":[{\"symbol\":\"AAPL\",\"date\":\"not-a-date\"},{\"symbol\":\"MSFT\",\"date\":\"2026-06-15\"}]}");

        var events = await this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 8, 1), null, CancellationToken.None);

        Assert.Single(events);
        Assert.Equal("MSFT", events[0].Symbol);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_EntryWithMissingSymbol_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"earningsCalendar\":[{\"date\":\"2026-06-15\"},{\"symbol\":\"AAPL\",\"date\":\"2026-07-29\"}]}");

        var events = await this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 8, 1), null, CancellationToken.None);

        Assert.Single(events);
        Assert.Equal("AAPL", events[0].Symbol);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_HttpRequestException_PreservesInnerExceptionAndUsesServiceUnavailable()
    {
        var network = new HttpRequestException("DNS resolution failed");
        this._handler.SetException(network);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None));

        Assert.Same(network, ex.InnerException);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.StatusCode);
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", cts.Token));
    }

    [Fact]
    public async Task GetEarningsCalendarAsync_NoConfiguredEndpoint_Throws()
    {
        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1/",
            ApiKey = "test-key",
            EndPoints = []
        });

        var client = new FinnHubCalendarApiClient(this._httpClient, options, NullLogger<FinnHubCalendarApiClient>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetEarningsCalendarAsync(
            new DateOnly(2026, 5, 1), new DateOnly(2026, 6, 1), "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetIpoCalendarAsync_RealFixture_MapsPricedAndWithdrawnEntries()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("calendar-ipo-2026"));

        var events = await this._sut.GetIpoCalendarAsync(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 6, 1), CancellationToken.None);

        Assert.NotEmpty(events);

        var priced = Assert.Single(events, e =>
            string.Equals(e.Status, "priced", StringComparison.Ordinal) &&
            string.Equals(e.Symbol, "PTNM", StringComparison.Ordinal));
        Assert.Equal("Pitanium Ltd", priced.Name);
        Assert.Equal("NASDAQ Capital", priced.Exchange);
        Assert.Equal(4.0, priced.Price);
        Assert.Equal(1750000, priced.NumberOfShares);
        Assert.Equal(7000000d, priced.TotalSharesValue);

        var withdrawn = events.First(e => string.Equals(e.Status, "withdrawn", StringComparison.Ordinal));
        Assert.Null(withdrawn.Symbol);
        Assert.Null(withdrawn.Exchange);
        Assert.Null(withdrawn.Price);
    }

    /// <summary>
    /// Regression: catches URL-construction drift for the IPO path. The endpoint
    /// must not append a symbol segment regardless of caller intent — the upstream
    /// ignores the parameter and returning a "filtered" envelope would be misleading.
    /// </summary>
    [Fact]
    public async Task GetIpoCalendarAsync_HitsApiV1CalendarIpoEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        await this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None);

        Assert.Equal(
            "https://finnhub.io/api/v1/calendar/ipo?from=2026-06-01&to=2026-12-31",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetIpoCalendarAsync_EmptyEnvelope_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"ipoCalendar\":[]}");

        var events = await this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None);

        Assert.Empty(events);
    }

    [Fact]
    public async Task GetIpoCalendarAsync_EntryWithMalformedDate_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"ipoCalendar\":[{\"name\":\"Bad\",\"date\":\"not-a-date\"},{\"name\":\"Good\",\"date\":\"2025-05-30\",\"symbol\":\"X\"}]}");

        var events = await this._sut.GetIpoCalendarAsync(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 6, 1), CancellationToken.None);

        Assert.Single(events);
        Assert.Equal("Good", events[0].Name);
    }

    [Fact]
    public async Task GetIpoCalendarAsync_UnparseablePrice_DegradesToNull()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"ipoCalendar\":[{\"name\":\"Acme\",\"date\":\"2025-05-30\",\"price\":\"TBD\"}]}");

        var events = await this._sut.GetIpoCalendarAsync(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 6, 1), CancellationToken.None);

        var only = Assert.Single(events);
        Assert.Null(only.Price);
    }

    [Fact]
    public async Task GetIpoCalendarAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None));
    }

    [Fact]
    public async Task GetIpoCalendarAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None));
    }

    [Fact]
    public async Task GetIpoCalendarAsync_HttpRequestException_PreservesInnerExceptionAndUsesServiceUnavailable()
    {
        var network = new HttpRequestException("DNS resolution failed");
        this._handler.SetException(network);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None));

        Assert.Same(network, ex.InnerException);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.StatusCode);
    }

    [Fact]
    public async Task GetIpoCalendarAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None));
    }

    [Fact]
    public async Task GetIpoCalendarAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), cts.Token));
    }

    [Fact]
    public async Task GetIpoCalendarAsync_NoConfiguredEndpoint_Throws()
    {
        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1/",
            ApiKey = "test-key",
            EndPoints = []
        });

        var client = new FinnHubCalendarApiClient(this._httpClient, options, NullLogger<FinnHubCalendarApiClient>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetIpoCalendarAsync(
            new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31), CancellationToken.None));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
