// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Nodes;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Middleware;
using FinnHub.MCP.Server.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Middleware;

public sealed class ToolInvocationMiddlewareTests
{
    private readonly ILogger<ToolInvocationMiddleware> _logger =
        Substitute.For<ILogger<ToolInvocationMiddleware>>();

    [Fact]
    public async Task InvokeAsync_DelegatesToInnerTool()
    {
        var inner = MakeInnerTool(SmallEnvelopeJson());
        var middleware = new ToolInvocationMiddleware(inner, new StubTokenEstimator(10), this._logger);

        await middleware.InvokeAsync(MakeRequest(), CancellationToken.None);

        Assert.Equal(1, inner.InvocationCount);
    }

    [Fact]
    public async Task InvokeAsync_UnderBudget_PatchesApproxTokens()
    {
        var inner = MakeInnerTool(SmallEnvelopeJson(approxTokens: 0));
        var middleware = new ToolInvocationMiddleware(inner, new StubTokenEstimator(123), this._logger);

        var result = await middleware.InvokeAsync(MakeRequest(), CancellationToken.None);

        var node = ReadEnvelope(result);
        Assert.Equal(123, (int?)node!["approx_tokens"]);
        Assert.True((bool?)node["is_success"]);
    }

    [Fact]
    public async Task InvokeAsync_OverSummaryBudget_DowngradesToFailureEnvelope()
    {
        var inner = MakeInnerTool(SmallEnvelopeJson());
        var middleware = new ToolInvocationMiddleware(inner, new StubTokenEstimator(501), this._logger);

        var result = await middleware.InvokeAsync(MakeRequest("summary"), CancellationToken.None);

        Assert.True(result.IsError);
        var node = ReadEnvelope(result);
        Assert.False((bool?)node!["is_success"]);
        Assert.Equal(nameof(ResultErrorType.BudgetExceeded), (string?)node["error_type"]);
        Assert.Equal("response exceeded token budget for declared view", (string?)node["error_message"]);
        Assert.Null(node["data"]?.GetValue<object?>());
        Assert.Equal(501, (int?)node["approx_tokens"]);
    }

    [Fact]
    public async Task InvokeAsync_FullView_NoCeilingEnforced()
    {
        var inner = MakeInnerTool(SmallEnvelopeJson());
        var middleware = new ToolInvocationMiddleware(inner, new StubTokenEstimator(100_000), this._logger);

        var result = await middleware.InvokeAsync(MakeRequest("full"), CancellationToken.None);

        Assert.False(result.IsError);
        var node = ReadEnvelope(result);
        Assert.True((bool?)node!["is_success"]);
        Assert.Equal(100_000, (int?)node["approx_tokens"]);
    }

    [Fact]
    public async Task InvokeAsync_StandardView_HonoursStandardCeiling()
    {
        var inner = MakeInnerTool(SmallEnvelopeJson());
        var underStandard = new ToolInvocationMiddleware(inner, new StubTokenEstimator(1_500), this._logger);
        var overStandard = new ToolInvocationMiddleware(inner, new StubTokenEstimator(2_001), this._logger);

        var under = await underStandard.InvokeAsync(MakeRequest("standard"), CancellationToken.None);
        var over = await overStandard.InvokeAsync(MakeRequest("standard"), CancellationToken.None);

        Assert.False(under.IsError);
        Assert.True(over.IsError);
    }

    [Fact]
    public async Task InvokeAsync_EmitsStructuredLog()
    {
        var inner = MakeInnerTool(SmallEnvelopeJson());
        var middleware = new ToolInvocationMiddleware(inner, new StubTokenEstimator(42), this._logger);

        await middleware.InvokeAsync(MakeRequest(), CancellationToken.None);

        this._logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static FakeMcpServerTool MakeInnerTool(string envelopeJson)
    {
        var element = JsonSerializer.Deserialize<JsonElement>(envelopeJson);
        return new FakeMcpServerTool("test-tool", (_, _) =>
            ValueTask.FromResult(new CallToolResult
            {
                StructuredContent = element,
                Content = [new TextContentBlock { Text = envelopeJson }],
                IsError = false
            }));
    }

    private static RequestContext<CallToolRequestParams> MakeRequest(string? view = null)
    {
        var args = view is null
            ? null
            : new Dictionary<string, JsonElement>(StringComparer.Ordinal)
            {
                ["view"] = JsonSerializer.Deserialize<JsonElement>($"\"{view}\"")
            };

        var server = Substitute.For<McpServer>();
        var rpcRequest = new JsonRpcRequest { Method = "tools/call" };
        var parameters = new CallToolRequestParams { Name = "test-tool", Arguments = args };

        return new RequestContext<CallToolRequestParams>(server, rpcRequest, parameters);
    }

    private static JsonObject? ReadEnvelope(CallToolResult result)
    {
        var element = result.StructuredContent!.Value;
        return JsonNode.Parse(element.GetRawText())?.AsObject();
    }

    private static string SmallEnvelopeJson(int approxTokens = 0) =>
        $$"""
        {
          "is_success": true,
          "data": {"symbols": []},
          "view": "summary",
          "next_actions": [],
          "approx_tokens": {{approxTokens}},
          "premium": false
        }
        """;
}
