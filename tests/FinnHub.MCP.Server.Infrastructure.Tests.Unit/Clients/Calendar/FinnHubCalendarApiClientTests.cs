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
            EndPoints = [new FinnHubEndPoint { Name = "calendar-earnings", Url = "calendar/earnings", IsActive = true }]
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

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
