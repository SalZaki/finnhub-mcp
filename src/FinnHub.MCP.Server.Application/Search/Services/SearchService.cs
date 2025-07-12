// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Search.Services;

/// <summary>
/// Provides a high-level service for searching financial symbols via the configured search API client.
/// This service handles validation, exception translation, logging, and standardized result formatting.
/// </summary>
public sealed class SearchService(
    ISearchApiClient searchApiClient,
    ILogger<SearchService> logger)
    : ISearchService
{
    /// <summary>
    /// Performs a symbol search using the provided query and returns a standardized result object.
    /// Handles API-specific exceptions and maps them to application-level result error types.
    /// </summary>
    /// <param name="query">The search query containing symbol criteria such as text and exchange.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing a <see cref="SearchSymbolResponse"/> if the operation succeeds,
    /// or a failure result with an appropriate <see cref="ResultErrorType"/> on failure.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="query"/> is null.</exception>
    /// <exception cref="Exception">Rethrows unexpected non-handled exceptions for upstream handling.</exception>
    public async Task<Result<SearchSymbolResponse>> SearchSymbolAsync(
        SearchSymbolQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var response = await searchApiClient.SearchSymbolAsync(query, cancellationToken);

            logger.Log(LogLevel.Information, "Retrieved {ResponseTotalCount} symbols for query: {Query}",
                response.TotalCount, query);

            return response.HasResults
                ? new Result<SearchSymbolResponse>().Success(response)
                : new Result<SearchSymbolResponse>().Failure("No search symbol(s) found.", ResultErrorType.NotFound);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error from FinnHub API for query: {Query} (Status: {StatusCode})", query.Query, ex.StatusCode.ToString());
            return new Result<SearchSymbolResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.Log(LogLevel.Warning, ex, "Request to FinnHub Api timed out for query: {Query}", query.Query);
            return new Result<SearchSymbolResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.Log(LogLevel.Error, ex, "Failed to deserialize response from FinnHub Api for query: {Query}", query.Query);
            return new Result<SearchSymbolResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected symbol search failure for query: {Query}", query.Query);
            return new Result<SearchSymbolResponse>().Failure("Symbol search failed unexpectedly");
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, ex, "Unexpected error occurred while searching symbols for query: {Query}", query.Query);
            throw;
        }
    }
}
