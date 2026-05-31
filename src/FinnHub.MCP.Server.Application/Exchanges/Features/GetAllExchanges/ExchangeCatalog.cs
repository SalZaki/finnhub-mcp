// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;

/// <summary>
/// In-memory catalog of the stock exchanges Finnhub supports.
/// </summary>
/// <remarks>
/// Finnhub exposes no <c>/stock/exchange</c> API endpoint — the supported-exchange list
/// is published only as a reference document, so the catalog ships in-process rather than
/// behind an HTTP client and cache. The backing data lives in the <c>ExchangeCatalog.Data.cs</c>
/// partial and is a verbatim capture of Finnhub's public "Supported Exchanges" sheet; see that
/// file for provenance and refresh instructions.
/// </remarks>
public sealed partial class ExchangeCatalog : IExchangeCatalog
{
    /// <inheritdoc />
    public IReadOnlyList<Exchange> Exchanges => s_all;
}
