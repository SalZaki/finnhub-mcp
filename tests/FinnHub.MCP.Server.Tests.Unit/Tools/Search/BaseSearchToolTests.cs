// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using FinnHub.MCP.Server.SSE.Tools.Search;
using Json.Schema;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Search;

public class BaseSearchToolTests
{
    private static Dictionary<string, JsonElement> BuildArgs(string key, string value) =>
        new()
        {
            [key] = JsonSerializer.SerializeToElement(value)
        };

    [Theory]
    [InlineData("AAPL", false)]
    [InlineData("X", false)]
    [InlineData("X-NYSE_2024", false)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("<tag>", true)]
    [InlineData("<script>", true)]
    [InlineData(null, true)]
    public void ValidateAndGetQuery_HandlesVariousInputs(string? input, bool shouldThrow)
    {
        var args = BuildArgs("query", input ?? "");

        if (shouldThrow)
        {
            Assert.Throws<ArgumentException>(() => BaseSearchToolTestImpl.ValidateAndGetQuery(args));
        }
        else
        {
            var result = BaseSearchToolTestImpl.ValidateAndGetQuery(args);
            Assert.Equal(input?.Trim(), result);
        }
    }

    [Theory]
    [InlineData("NASDAQ", false)]
    [InlineData("X-NYSE", false)]
    [InlineData("ABC_DEF", false)]
    [InlineData("", false)]
    [InlineData("nasdaq", false)]
    [InlineData("X#INVALID", true)]
    [InlineData("TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG",
        true)]
    public void ValidateAndGetExchange_HandlesVariousInputs(string? input, bool shouldThrow)
    {
        var args = BuildArgs("exchange", input ?? string.Empty);

        if (shouldThrow)
        {
            Assert.Throws<ArgumentException>(() => BaseSearchToolTestImpl.ValidateAndGetExchange(args));
        }
        else
        {
            var result = BaseSearchToolTestImpl.ValidateAndGetExchange(args);
            if (string.IsNullOrWhiteSpace(input))
            {
                Assert.Null(result);
            }
            else
            {
                Assert.Equal(input.Trim().ToUpperInvariant(), result);
            }
        }
    }

    private static EvaluationOptions SchemaOptions => new()
    {
        OutputFormat = OutputFormat.List,
        RequireFormatValidation = true
    };

    [Theory]
    [InlineData("AAPL", "NASDAQ", true)]
    [InlineData("  XOM  ", "X-NYSE", true)]
    [InlineData("", "NYSE", false)]
    [InlineData("<script>", "NYSE", false)]
    [InlineData("AAPL", "nasdaq", false)]
    [InlineData("AAPL", "NYSE!", false)]
    public void ToolSchema_Validation_AlignsWithRuntime(string query, string exchange, bool expectedValid)
    {
        // Arrange: build input object as JsonElement
        var input = JsonSerializer.SerializeToElement(new
        {
            query,
            exchange
        });

        // Act
        var schema = SearchSymbolTool.ToolSchema;
        var result = schema.Evaluate(input, SchemaOptions);

        // Assert
        Assert.Equal(expectedValid, result.IsValid);
    }
}

public sealed class BaseSearchToolTestImpl : BaseSearchTool
{
    public static new string ValidateAndGetQuery(IReadOnlyDictionary<string, JsonElement>? args,
        string paramName = "query", int min = 1, int max = 500) =>
        BaseSearchTool.ValidateAndGetQuery(args, paramName, min, max);

    public static new string? ValidateAndGetExchange(IReadOnlyDictionary<string, JsonElement>? args,
        string paramName = "exchange") =>
        BaseSearchTool.ValidateAndGetExchange(args, paramName);

    public override ValueTask<CallToolResponse> InvokeAsync(RequestContext<CallToolRequestParams> request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public override Tool ProtocolTool => new();
}
