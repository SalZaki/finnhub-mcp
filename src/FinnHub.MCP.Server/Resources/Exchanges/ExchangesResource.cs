// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Net.Mime;
using FinnHub.MCP.Server.Application.Exchanges;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;
using FinnHub.MCP.Server.Application.Models;
using ModelContextProtocol.Server;

namespace FinnHub.MCP.Server.Resources.Exchanges;

/// <summary>
/// MCP resource that provides stock exchange listings from Finnhub.
/// </summary>
[McpServerResourceType]
public sealed class ExchangesResource
{
    /// <summary>
    /// Returns the catalog of stock exchanges available through the Finnhub provider,
    /// wrapped in an application-level <see cref="Result{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the read handler for the <c>finnhub://resources/exchanges</c> MCP
    /// resource. Each entry exposes the exchange code, full name, country,
    /// MIC code, time zone, trading hours, and a public information URL — enough
    /// for clients to populate dropdowns or filter symbol searches by venue.
    /// </para>
    /// <para>
    /// The current implementation returns a static stub (London Stock Exchange only)
    /// pending wiring to the Finnhub <c>/stock/exchange</c> endpoint.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A successful <see cref="Result{T}"/> containing an <see cref="ExchangesResponse"/>
    /// with the available exchanges. The result is never a failure under the current
    /// stubbed implementation, but consumers should still check <c>IsSuccess</c>
    /// once the live provider call is wired up.
    /// </returns>
    [McpServerResource(
        UriTemplate = "finnhub://resources/exchanges",
        Name = "get-exchanges",
        Title = "Exchanges",
        MimeType = MediaTypeNames.Application.Json)]
    [Description("Gets all the exchanges listed on Finnhub.")]
    public Result<ExchangesResponse> GetExchanges()
    {
        // TODO: Replace stub data with real Finnhub API call logic.
        var responsePayload = new ExchangesResponse
        {
            Exchanges =
            [
                new Exchange
                {
                    ExchangeCode = "L",
                    ExchangeName = "London Stock Exchange",
                    CountryCode = "GB",
                    CountryName = "United Kingdom",
                    MicCode = "XLON",
                    TimeZone = "Europe/London",
                    TradingHours = "08:00-16:30",
                    Url = "https://www.tradinghours.com/exchanges/lse",
                    CloseDate = string.Empty
                }
            ]
        };

        return new Result<ExchangesResponse>().Success(responsePayload);
    }
}
