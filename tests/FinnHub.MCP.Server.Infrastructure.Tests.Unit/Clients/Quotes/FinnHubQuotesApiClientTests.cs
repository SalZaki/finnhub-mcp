// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using FinnHub.MCP.Server.Infrastructure.Clients.Quotes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Quotes;

public sealed class FinnHubQuotesApiClientTests : IDisposable
{
    private const string SamplePayload = """
                                         {
                                           "c": 261.74,
                                           "d": 0.4,
                                           "dp": 0.1531,
                                           "h": 263.31,
                                           "l": 260.68,
                                           "o": 261.07,
                                           "pc": 261.34,
                                           "t": 1700000000
                                         }
                                         """;

    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubQuotesApiClient _sut;

    public FinnHubQuotesApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints = [new FinnHubEndPoint { Name = "quote", Url = "quote", IsActive = true }]
        });

        this._sut = new FinnHubQuotesApiClient(this._httpClient, options, NullLogger<FinnHubQuotesApiClient>.Instance);
    }

    [Fact]
    public async Task GetQuoteAsync_MapsAllFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.Equal("AAPL", result.Symbol);
        Assert.Equal(261.74, result.Current);
        Assert.Equal(0.4, result.Change);
        Assert.Equal(0.1531, result.PercentChange);
        Assert.Equal(263.31, result.High);
        Assert.Equal(261.34, result.PrevClose);
    }

    [Fact]
    public async Task GetQuoteAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetQuoteAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
