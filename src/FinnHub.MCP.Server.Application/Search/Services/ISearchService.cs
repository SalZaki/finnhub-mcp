// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

namespace FinnHub.MCP.Server.Application.Search.Services;

/// <summary>
/// Application-level contract for performing financial-symbol searches.
/// Implementations encapsulate provider-specific concerns (HTTP transport,
/// deserialization, retry, exception translation) and expose a stable
/// <see cref="Result{T}"/>-based shape to the rest of the application so
/// that callers never have to handle raw infrastructure exceptions.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Executes a symbol-search request and returns a categorized result.
    /// </summary>
    /// <param name="query">
    /// The validated search criteria. Must not be <c>null</c>; implementations
    /// may perform additional validation specific to the underlying provider.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to observe while waiting for the search to complete. When cancelled,
    /// the returned task transitions to a faulted/canceled state.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> wrapping a <see cref="SearchSymbolResponse"/> on
    /// success, or an error result whose <see cref="Result{T}.ErrorType"/> describes
    /// the failure category (<c>NotFound</c>, <c>Timeout</c>, <c>ServiceUnavailable</c>,
    /// <c>InvalidResponse</c>, etc.). Implementations should not surface raw
    /// provider exceptions to callers except for unexpected/unclassified failures.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown by implementations when <paramref name="query"/> is <c>null</c>.
    /// </exception>
    Task<Result<SearchSymbolResponse>> SearchSymbolAsync(
        SearchSymbolQuery query,
        CancellationToken cancellationToken = default);
}
