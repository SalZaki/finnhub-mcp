// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Discovery;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Discovery;

/// <summary>
/// MCP meta-tool that discovers the right Finnhub tool from a natural-language intent, so a client
/// keeps full tool schemas off the wire until a tool is actually needed.
/// </summary>
[McpServerToolType]
public sealed class SearchToolsTool(
    IToolRegistry registry,
    ILogger<SearchToolsTool> logger)
{
    private const int MaxResults = 5;

    /// <summary>
    /// Ranks the registered tools against <paramref name="intent"/> and returns the best matches.
    /// </summary>
    /// <remarks>
    /// Synchronous and in-process: the registry is an in-memory BM25 index, so there is no I/O.
    /// Validation failures throw <see cref="ArgumentException"/>, which the MCP runtime surfaces
    /// as a tool error. <c>next_actions</c> is intentionally empty — the ranked <c>matches</c>
    /// payload is itself the set of suggested follow-up tools.
    /// </remarks>
    /// <param name="intent">Natural-language description of the task (max 200 chars).</param>
    /// <param name="view">Response detail level. <c>summary</c> omits per-tool descriptions.</param>
    /// <returns>A <see cref="ToolResponseEnvelope{T}"/> wrapping the ranked <see cref="SearchToolsResponse"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="intent"/> or <paramref name="view"/> fails validation.</exception>
    [McpServerTool(
        Name = Constants.Tools.SearchTools.Name,
        Title = Constants.Tools.SearchTools.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = false)]
    [Description(Constants.Tools.SearchTools.Description)]
    public ToolResponseEnvelope<SearchToolsResponse> SearchTools(
        [Description(Constants.Tools.SearchTools.Parameters.IntentDescription)]
        string intent,
        [Description(Constants.Tools.SearchTools.Parameters.ViewDescription)]
        string? view = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var toolName = Constants.Tools.SearchTools.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", toolName);

            var validatedIntent = SearchToolsInputValidator.ValidateIntent(intent);
            var validatedView = SearchToolsInputValidator.ValidateView(view);

            var matches = registry.Search(validatedIntent, MaxResults);

            var response = new SearchToolsResponse
            {
                Intent = validatedIntent,
                Matches = [.. matches.Select(match => ProjectMatch(match, validatedView))]
            };

            logger.LogInformation(
                "search-tools matched {Count} tool(s) in {ElapsedMs}ms",
                matches.Count, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.Success(
                response,
                validatedView,
                explanation: BuildExplanation(validatedIntent, matches));
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Validation error in '{Tool}': {Message}", toolName, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred running '{Tool}'.", toolName);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogTrace("Finished executing '{Tool}' in {ElapsedMs}ms.", toolName, stopwatch.ElapsedMilliseconds);
        }
    }

    private static ToolMatchView ProjectMatch(ToolMatch match, ToolView view) => new()
    {
        Name = match.Tool.Name,
        Title = match.Tool.Title,
        Score = Math.Round(match.Score, 3, MidpointRounding.AwayFromZero),
        Category = match.Tool.Category,
        Premium = match.Tool.Premium,
        Description = view == ToolView.Summary ? null : match.Tool.Description
    };

    private static string BuildExplanation(string intent, IReadOnlyList<ToolMatch> matches)
    {
        if (matches.Count == 0)
        {
            return string.Create(
                CultureInfo.InvariantCulture,
                $"No tools matched '{intent}'. Try rephrasing the intent.");
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Top {matches.Count} tool(s) for '{intent}': {string.Join(", ", matches.Select(m => m.Tool.Name))}.");
    }
}
