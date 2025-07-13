// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.SSE.Common;
using Json.Schema;

namespace FinnHub.MCP.Server.SSE.Tools.Search;
/// <summary>
/// Tool implementation for searching financial symbols via the MCP server's search service.
/// </summary>
/// <remarks>
/// This tool validates user input, constructs a <see cref="SearchSymbolQuery"/>, invokes the search service,
/// and returns the results in a format compatible with SSE tooling.
/// </remarks>
public sealed class SearchSymbolTool(
    ISearchService searchService,
    ILogger<SearchSymbolTool> logger)
    : BaseSearchTool
{
    /// <summary>
    /// Lazily initialized serialized JSON schema used for tool input validation.
    /// </summary>
    private static readonly Lazy<JsonElement> s_serializedSchema = new(() =>
        JsonSerializer.SerializeToElement(s_toolSchema));

    /// <summary>
    /// Defines the JSON schema for tool inputs, including query, exchange, and result limit.
    /// </summary>
    private static readonly JsonSchema s_toolSchema = new JsonSchemaBuilder()
        .Properties(
            (Constants.Tools.SearchSymbols.Parameters.QueryName, new JsonSchemaBuilder()
                .Type(SchemaValueType.String)
                .Required()
                .Description(Constants.Tools.SearchSymbols.Parameters.QueryDescription)),
            (Constants.Tools.SearchSymbols.Parameters.ExchangeName, new JsonSchemaBuilder()
                .Type(SchemaValueType.String)
                .Description(Constants.Tools.SearchSymbols.Parameters.ExchangeDescription)
            ),
            (Constants.Tools.SearchSymbols.Parameters.LimitName, new JsonSchemaBuilder()
                .Type(SchemaValueType.Integer)
                .Description(Constants.Tools.SearchSymbols.Parameters.LimitDescription)
                .Minimum(1)
                .Maximum(100)
            ))
        .Type(SchemaValueType.Object)
        .AdditionalProperties(false)
        .Build();

    /// <summary>
    /// Gets metadata about the tool, including its name, description, input schema, and annotations.
    /// </summary>
    public override Tool ProtocolTool => new()
    {
        Name = Constants.Tools.SearchSymbols.Name,
        Description = Constants.Tools.SearchSymbols.Description,
        InputSchema = s_serializedSchema.Value,
        Annotations = new ToolAnnotations
        {
            Title = Constants.Tools.SearchSymbols.Title,
            ReadOnlyHint = true,
            DestructiveHint = false,
            IdempotentHint = true
        }
    };

    /// <summary>
    /// Executes the tool's logic by validating parameters, running the symbol search,
    /// and generating a structured success or error response.
    /// </summary>
    /// <param name="request">The request context, including tool arguments and user metadata.</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the tool's response.</returns>
    public override async ValueTask<CallToolResponse> InvokeAsync(
        RequestContext<CallToolRequestParams> request,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", this.ProtocolTool.Name);

            var args = request.Params?.Arguments;

            var query = ValidateAndGetQuery(args);
            var limit = ValidateAndGetLimit(args);
            var exchange = ValidateAndGetExchange(args);

            logger.LogDebug("Executing search with query: '{Query}', exchange: '{Exchange}', limit: {Limit}", query, exchange, limit);

            var symbolSearchQuery = new SearchSymbolQueryBuilder()
                .WithQuery(query)
                .WithExchange(exchange)
                .WithLimit(limit)
                .Build();

            var results = await searchService.SearchSymbolAsync(symbolSearchQuery, cancellationToken);

            logger.Log(LogLevel.Information, "Search completed successfully. Found {Count} results in {ElapsedMs}ms",
                results.Data?.TotalCount, stopwatch.ElapsedMilliseconds);

            return this.CreateSuccessResponse(results);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            logger.Log(LogLevel.Error, ex, "Parameter out of range in '{Tool}': {Message}", this.ProtocolTool.Name, ex.Message);
            return this.CreateValidationErrorResponse(ex.ParamName ?? "unknown", ex.Message);
        }
        catch (ArgumentException ex)
        {
            logger.Log(LogLevel.Error, ex, "Validation error in '{Tool}': {Message}", this.ProtocolTool.Name, ex.Message);
            return this.CreateValidationErrorResponse(ex.ParamName ?? "unknown", ex.Message);
        }
        catch (OperationCanceledException ex)
        {
            logger.Log(LogLevel.Error, ex, "Search operation was canceled for '{Tool}'.", this.ProtocolTool.Name);
            return this.CreateOperationErrorResponse("search", "Search operation was cancelled.");
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, ex, "An exception occurred running '{Tool}'.", this.ProtocolTool.Name);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogTrace("Finished executing '{Tool}' in {ElapsedMs}ms.", this.ProtocolTool.Name, stopwatch.ElapsedMilliseconds);
        }
    }
}
