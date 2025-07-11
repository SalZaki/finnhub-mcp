// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Search.Services;

public sealed class SearchService(
    ISearchClient searchClient,
    ILogger<SearchService> logger)
    : ISearchService
{
    public async Task<Result<SearchSymbolResponse>> SearchSymbolAsync(
        SearchSymbolQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var response = await searchClient.SearchSymbolAsync(query, cancellationToken);

            logger.Log(LogLevel.Information, "Retrieved {ResponseTotalCount} symbols for query: {Query}",
                response.TotalCount, query);

            return response.HasResults
                ? new Result<SearchSymbolResponse>().Success(response)
                : new Result<SearchSymbolResponse>().Failure("No search symbol(s) found.", ResultErrorType.NotFound);
        }
        catch (SearchSymbolHttpException ex)
        {
            logger.LogError(ex, "HTTP error from FinnHub API for query: {Query} (Status: {StatusCode})", query.Query, ex.StatusCode.ToString());
            return new Result<SearchSymbolResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (SearchSymbolTimeoutException ex)
        {
            logger.Log(LogLevel.Warning, ex, "Request to FinnHub Api timed out for query: {Query}", query.Query);
            return new Result<SearchSymbolResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (SearchSymbolDeserializationException ex)
        {
            logger.Log(LogLevel.Error, ex, "Failed to deserialize response from FinnHub Api for query: {Query}", query.Query);
            return new Result<SearchSymbolResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (SearchSymbolException ex)
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
