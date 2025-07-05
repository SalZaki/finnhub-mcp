// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using FinnHub.MCP.Server.Application.Common;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Clients;
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

            logger.Log(LogLevel.Information, $"Retrieved {response.TotalCount} symbols from FinnHub Api");

            return response.HasResults
                ? new Result<SearchSymbolResponse>().Success(response)
                : new Result<SearchSymbolResponse>().Failure("No search symbol(s) found.", ResultErrorType.NotFound);
        }
        catch (HttpRequestException ex)
        {
            logger.Log(LogLevel.Error, ex, "HTTP request to FinnHub Api failed for query: {Query}", query.Query);

            return new Result<SearchSymbolResponse>()
                .Failure("Service temporarily unavailable", ResultErrorType.ServiceUnavailable);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            logger.Log(LogLevel.Warning, ex, "Request to FinnHub Api timed out for query: {Query}", query.Query);

            return new Result<SearchSymbolResponse>()
                .Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (JsonException ex)
        {
            logger.Log(LogLevel.Error, ex, "Failed to deserialize response from FinnHub Api for query: {Query}", query.Query);

            return new Result<SearchSymbolResponse>()
                .Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, ex, "Unexpected error occurred while searching symbols for query: {Query}", query.Query);

            throw;
        }
    }
}
