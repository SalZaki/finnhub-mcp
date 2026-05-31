// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;

/// <summary>
/// Provides the catalog of stock exchanges Finnhub supports.
/// </summary>
public interface IExchangeCatalog
{
    /// <summary>
    /// Gets the full list of supported stock exchanges.
    /// </summary>
    IReadOnlyList<Exchange> Exchanges { get; }
}
