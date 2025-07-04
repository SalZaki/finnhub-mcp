﻿// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.SSE.Common;
using Json.Schema;

namespace FinnHub.MCP.Server.SSE.Tools.Search;

public sealed class SearchSymbolTool(
    ISearchService searchService,
    ILogger<SearchSymbolTool> logger)
    : BaseSearchTool
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

            var symbolSearchQuery = new SearchSymbolQueryBuilder()
                .WithQuery(query)
                .WithExchange(exchange)
                .WithLimit(limit)
                .Build();

            var results = await searchService.SearchSymbolAsync(symbolSearchQuery, cancellationToken);

            logger.LogInformation("Search completed successfully. Found {Count} results in {ElapsedMs}ms", results.Data?.TotalCount, stopwatch.ElapsedMilliseconds);

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
            logger.LogInformation("Search operation was canceled for '{Tool}'.", this.ProtocolTool.Name);
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
            logger.LogTrace("Finished executing '{Tool}' in {ElapsedMs}ms.", this.ProtocolTool.Name, stopwatch.ElapsedMilliseconds);
        }
    }
}
