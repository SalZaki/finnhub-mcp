// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.Insiders.Services;

/// <summary>Application-level entry point for insider-transaction signal lookups.</summary>
public interface IInsidersService
{
    /// <summary>Aggregates insider transactions into a buy/sell signal envelope.</summary>
    Task<Result<GetInsiderSignalResponse>> GetInsiderSignalAsync(
        GetInsiderSignalQuery query,
        CancellationToken cancellationToken = default);
}
