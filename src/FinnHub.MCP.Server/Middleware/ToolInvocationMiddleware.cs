// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Text.Json.Nodes;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Tokens;

namespace FinnHub.MCP.Server.Middleware;

/// <summary>
/// Wraps every MCP tool invocation to populate <c>approx_tokens</c> on the response
/// envelope and to enforce per-view token ceilings.
/// </summary>
/// <remarks>
/// <para>
/// Implemented as a <see cref="DelegatingMcpServerTool"/> per the planning decision
/// for <c>R1</c> / <c>R2</c>: the SDK's decorator base class delegates to an inner
/// tool, sees the fully-serialized <see cref="CallToolResult"/>, and lets us patch
/// the structured content via <see cref="JsonNode"/> without re-running the typed
/// envelope through any reflection-based serializer.
/// </para>
/// <para>
/// Tools must leave <c>approx_tokens</c> at <c>0</c>; this middleware overwrites it.
/// Responses that exceed the declared view's ceiling are rebuilt as a
/// <see cref="ResultErrorType.BudgetExceeded"/> failure envelope so the consuming
/// model receives a structured error instead of an oversized payload.
/// </para>
/// </remarks>
public sealed class ToolInvocationMiddleware(
    McpServerTool innerTool,
    ITokenEstimator estimator,
    ILogger<ToolInvocationMiddleware> logger) : DelegatingMcpServerTool(innerTool)
{
    private const string ApproxTokensKey = "approx_tokens";

    /// <inheritdoc />
    public override async ValueTask<CallToolResult> InvokeAsync(
        RequestContext<CallToolRequestParams> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var toolName = this.ProtocolTool.Name;
        var declaredView = ExtractView(request);
        var stopwatch = Stopwatch.StartNew();

        var result = await base.InvokeAsync(request, cancellationToken).ConfigureAwait(false);

        stopwatch.Stop();

        var serialized = SerializeStructuredContent(result);
        var approxTokens = estimator.EstimateTokens(serialized);
        var ceiling = ToolBudget.CeilingFor(declaredView);

        if (approxTokens > ceiling)
        {
            logger.LogWarning(
                "tool {ToolName} view={View} tokens={ApproxTokens} budget={Budget} budget_exceeded=true duration_ms={ElapsedMs}",
                toolName, declaredView, approxTokens, ceiling, stopwatch.ElapsedMilliseconds);

            return BuildBudgetExceededResult(approxTokens, ceiling);
        }

        PatchApproxTokens(result, approxTokens);

        logger.LogInformation(
            "tool {ToolName} view={View} tokens={ApproxTokens} budget_exceeded=false duration_ms={ElapsedMs}",
            toolName, declaredView, approxTokens, stopwatch.ElapsedMilliseconds);

        return result;
    }

    /// <summary>
    /// Reads the declared <c>view</c> argument from the incoming request, defaulting
    /// to <see cref="ToolView.Summary"/> when absent or unparseable.
    /// </summary>
    /// <remarks>
    /// The middleware is lenient on parse failure — the tool's own validator (when present)
    /// produces the authoritative validation error. The middleware only needs the view to
    /// pick the right token ceiling.
    /// </remarks>
    private static ToolView ExtractView(RequestContext<CallToolRequestParams> request)
    {
        if (request.Params?.Arguments is not { } args)
        {
            return ToolView.Summary;
        }

        if (!args.TryGetValue("view", out var raw))
        {
            return ToolView.Summary;
        }

        var text = raw.ValueKind == JsonValueKind.String ? raw.GetString() : null;

        return text?.Trim().ToLowerInvariant() switch
        {
            "summary" => ToolView.Summary,
            "standard" => ToolView.Standard,
            "full" => ToolView.Full,
            _ => ToolView.Summary
        };
    }

    private static string SerializeStructuredContent(CallToolResult result)
    {
        if (result.StructuredContent is { } element)
        {
            return element.GetRawText();
        }

        if (result.Content.Count > 0 && result.Content[0] is TextContentBlock text)
        {
            return text.Text;
        }

        return string.Empty;
    }

    private static void PatchApproxTokens(CallToolResult result, int approxTokens)
    {
        if (result.StructuredContent is not { } element)
        {
            return;
        }

        if (JsonNode.Parse(element.GetRawText()) is not JsonObject obj)
        {
            return;
        }

        obj[ApproxTokensKey] = approxTokens;

        var patched = obj.ToJsonString();
        result.StructuredContent = JsonSerializer.Deserialize<JsonElement>(patched);

        if (result.Content.Count > 0 && result.Content[0] is TextContentBlock text)
        {
            text.Text = patched;
        }
    }

    private static CallToolResult BuildBudgetExceededResult(int approxTokens, int ceiling)
    {
        var envelope = new JsonObject
        {
            ["is_success"] = false,
            ["data"] = null,
            ["error_message"] = "response exceeded token budget for declared view",
            ["error_type"] = nameof(ResultErrorType.BudgetExceeded),
            ["view"] = "summary",
            ["next_actions"] = new JsonArray(),
            ["explanation"] =
                $"Response would be ~{approxTokens} tokens against a {ceiling}-token ceiling. " +
                "Retry with view=standard, view=full, or a sparser fields projection.",
            ["approx_tokens"] = approxTokens,
            ["rate_limit"] = null,
            ["sentiment_source"] = null,
            ["premium"] = false
        };

        var json = envelope.ToJsonString();

        return new CallToolResult
        {
            StructuredContent = JsonSerializer.Deserialize<JsonElement>(json),
            Content = [new TextContentBlock { Text = json }],
            IsError = true
        };
    }
}
