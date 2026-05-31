// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Net.Mime;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;

namespace FinnHub.MCP.Server.Resources.Exchanges;

/// <summary>
/// MCP resource that provides the catalog of stock exchanges Finnhub supports.
/// </summary>
[McpServerResourceType]
public sealed class ExchangesResource(IExchangeCatalog catalog)
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
    /// The catalog is sourced from <see cref="IExchangeCatalog"/>. Finnhub has no
    /// <c>/stock/exchange</c> endpoint, so the list ships as in-process reference
    /// data rather than a live upstream call.
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
    [Description("Gets the catalog of stock exchanges Finnhub supports.")]
    public string GetExchanges()
    {
        var responsePayload = new ExchangesResponse
        {
            Exchanges = catalog.Exchanges
        };

        return JsonSerializer.Serialize(responsePayload, ResourceJsonContext.Default.ExchangesResponse);
    }
}
