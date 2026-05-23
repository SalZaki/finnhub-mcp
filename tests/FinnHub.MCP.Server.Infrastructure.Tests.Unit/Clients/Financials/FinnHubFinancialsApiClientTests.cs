// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Financials;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Financials;

public sealed class FinnHubFinancialsApiClientTests : IDisposable
{
    private const string SamplePayload = """
                                         {
                                           "symbol": "AAPL",
                                           "metricType": "all",
                                           "metric": {
                                             "marketCapitalization": 3000000.0,
                                             "peTTM": 28.4,
                                             "pbAnnual": 45.2,
                                             "epsTTM": 6.13,
                                             "dividendYieldIndicatedAnnual": 0.5,
                                             "52WeekHigh": 199.6,
                                             "52WeekLow": 164.1,
                                             "52WeekPriceReturnDaily": 12.1,
                                             "beta": 1.28,
                                             "revenuePerShareTTM": 24.7,
                                             "extraField": 99.0
                                           }
                                         }
                                         """;

    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubFinancialsApiClient _sut;

    public FinnHubFinancialsApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints = [new FinnHubEndPoint { Name = "financials-metric", Url = "stock/metric", IsActive = true }]
        });

        this._sut = new FinnHubFinancialsApiClient(
            this._httpClient,
            options,
            NullLogger<FinnHubFinancialsApiClient>.Instance);
    }

    [Fact]
    public async Task GetSnapshotAsync_ProjectsCuratedKpis()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetSnapshotAsync(
            new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.Equal("AAPL", result.Symbol);
        Assert.Equal(3000000.0, result.MarketCap);
        Assert.Equal(28.4, result.PeTtm);
        Assert.Equal(199.6, result.Week52High);
        Assert.Equal(1.28, result.Beta);
        Assert.Null(result.Raw);
    }

    [Fact]
    public async Task GetSnapshotAsync_IncludeRaw_PopulatesRawDictionary()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetSnapshotAsync(
            new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "AAPL", IncludeRaw = true },
            CancellationToken.None);

        Assert.NotNull(result.Raw);
        Assert.True(result.Raw!.ContainsKey("extraField"));
    }

    [Fact]
    public async Task GetSnapshotAsync_MissingMetric_ReturnsNullKpis()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"symbol\":\"UNK\",\"metricType\":\"all\",\"metric\":null}");

        var result = await this._sut.GetSnapshotAsync(
            new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "UNK" },
            CancellationToken.None);

        Assert.Null(result.MarketCap);
        Assert.Null(result.Beta);
    }

    [Fact]
    public async Task GetSnapshotAsync_MixedNumericAndStringValues_ParsesNumericKpisAndSkipsStrings()
    {
        // Real Finnhub /stock/metric?metric=all responses interleave numeric KPIs with
        // string-formatted dates like 52WeekHighDate. Confirm we don't throw on the
        // string values and that the raw dictionary only carries the numeric subset.
        const string MixedPayload = """
                                    {
                                      "symbol": "NVDA",
                                      "metricType": "all",
                                      "metric": {
                                        "marketCapitalization": 3000000.0,
                                        "peTTM": 50.4,
                                        "52WeekHigh": 199.6,
                                        "52WeekHighDate": "2024-12-26",
                                        "52WeekLow": 164.1,
                                        "52WeekLowDate": "2024-04-19",
                                        "beta": 1.8
                                      }
                                    }
                                    """;
        this._handler.SetResponse(HttpStatusCode.OK, MixedPayload);

        var result = await this._sut.GetSnapshotAsync(
            new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "NVDA", IncludeRaw = true },
            CancellationToken.None);

        Assert.Equal(3000000.0, result.MarketCap);
        Assert.Equal(50.4, result.PeTtm);
        Assert.Equal(199.6, result.Week52High);
        Assert.Equal(1.8, result.Beta);

        // raw carries only the numeric keys; the string-typed date fields are filtered out.
        Assert.NotNull(result.Raw);
        Assert.True(result.Raw!.ContainsKey("marketCapitalization"));
        Assert.True(result.Raw.ContainsKey("52WeekHigh"));
        Assert.False(result.Raw.ContainsKey("52WeekHighDate"));
        Assert.False(result.Raw.ContainsKey("52WeekLowDate"));
    }

    [Fact]
    public async Task GetSnapshotAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetSnapshotAsync(
            new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
