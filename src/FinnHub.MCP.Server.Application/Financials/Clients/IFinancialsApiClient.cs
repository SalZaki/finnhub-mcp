// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;

namespace FinnHub.MCP.Server.Application.Financials.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/metric</c> endpoint.
/// </summary>
public interface IFinancialsApiClient
{
    /// <summary>
    /// Fetches the basic-financials metric snapshot for a symbol. The returned
    /// response always carries the 10 curated KPIs; the raw dictionary is populated
    /// only when <paramref name="query"/> has <c>IncludeRaw = true</c>.
    /// </summary>
    Task<GetFinancialsSnapshotResponse> GetSnapshotAsync(GetFinancialsSnapshotQuery query, CancellationToken cancellationToken);
}
