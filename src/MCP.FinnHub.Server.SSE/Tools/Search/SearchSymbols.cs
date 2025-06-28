// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Json.Schema;
using MCP.FinnHub.Server.SSE.Application.Features.Search.Queries;
using MCP.FinnHub.Server.SSE.Application.Features.Search.Services;
using MCP.FinnHub.Server.SSE.Common;

namespace MCP.FinnHub.Server.SSE.Tools.Search;

public sealed class SearchSymbolsTool(
    ISearchService searchService,
    ILogger<SearchSymbolsTool> logger) :
    BaseSearchTool
{
    private static readonly Lazy<JsonElement> s_serializedSchema = new(() =>
        JsonSerializer.SerializeToElement(s_toolSchema));

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

    public override async ValueTask<CallToolResponse> InvokeAsync(
        RequestContext<CallToolRequestParams> request,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", this.ProtocolTool.Name);

            var args = request.Params?.Arguments;

            var query = ValidateAndGetQuery(args);
            var limit = ValidateAndGetLimit(args);
            var exchange = GetStringParameter(args, Constants.Tools.SearchSymbols.Parameters.ExchangeName);

            logger.LogDebug("Executing search with query: '{Query}', exchange: '{Exchange}', limit: {Limit}", query, exchange, limit);

            var symbolSearchQuery = new SymbolSearchQueryBuilder()
                .WithQuery(query)
                .WithExchange(exchange)
                .WithLimit(limit)
                .Build();

            var results = await searchService.SearchSymbolsAsync(symbolSearchQuery, cancellationToken);

            logger.LogInformation("Search completed successfully. Found {Count} results in {ElapsedMs}ms", results.Data?.Count, stopwatch.ElapsedMilliseconds);

            return this.CreateSuccessResponse(results);
        }

        catch (ArgumentOutOfRangeException ex)
        {
            logger.LogWarning(ex, "Parameter out of range in '{Tool}': {Message}", this.ProtocolTool.Name, ex.Message);
            return this.CreateValidationErrorResponse(ex.ParamName ?? "unknown", ex.Message);
        }

        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error in '{Tool}': {Message}", this.ProtocolTool.Name, ex.Message);
            return this.CreateValidationErrorResponse(ex.ParamName ?? "unknown", ex.Message);
        }

        catch (OperationCanceledException)
        {
            logger.LogInformation("Search operation was cancelled for '{Tool}'.", this.ProtocolTool.Name);
            return this.CreateOperationErrorResponse("search", "Search operation was cancelled.");
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred running '{Tool}'.", this.ProtocolTool.Name);
            throw;
        }

        finally
        {
            stopwatch.Stop();
            logger.LogTrace("Finished executing '{Tool}' in {ElapsedMs}ms.", this.ProtocolTool.Name,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
