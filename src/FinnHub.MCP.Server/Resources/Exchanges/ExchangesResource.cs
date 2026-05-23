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

namespace FinnHub.MCP.Server.Resources.Exchanges;

/// <summary>
/// MCP resource that provides stock exchange listings from Finnhub.
/// </summary>
[McpServerResourceType]
public sealed class ExchangesResource
{
    /// <summary>
    /// Returns the catalog of stock exchanges serialized as JSON for the
    /// <c>finnhub://resources/exchanges</c> resource.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each entry exposes the exchange code, full name, country, MIC code, time
    /// zone, trading hours, and a public information URL — enough for clients to
    /// populate dropdowns or filter symbol searches by venue.
    /// </para>
    /// <para>
    /// The current implementation returns a static stub (London Stock Exchange only)
    /// pending wiring to the Finnhub <c>/stock/exchange</c> endpoint.
    /// </para>
    /// <para>
    /// Returns a JSON <see cref="string"/> rather than the typed response because
    /// the MCP SDK only marshals a fixed set of resource handler return types
    /// (<c>ResourceContents</c>, <c>string</c>, <c>IEnumerable&lt;...&gt;</c>);
    /// the SDK wraps the string in a <c>TextResourceContents</c> using the
    /// declared <see cref="MediaTypeNames.Application.Json"/> mime type.
    /// </para>
    /// </remarks>
    [McpServerResource(
        UriTemplate = "finnhub://resources/exchanges",
        Name = "get-exchanges",
        Title = "Exchanges",
        MimeType = MediaTypeNames.Application.Json)]
    [Description("Gets all the exchanges listed on Finnhub.")]
    public string GetExchanges()
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

        return JsonSerializer.Serialize(responsePayload, ResourceJsonContext.Default.ExchangesResponse);
    }
}
