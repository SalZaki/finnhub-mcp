// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Models;

public sealed class EnvelopeSerializationTests
{
    private static readonly Dictionary<string, string> s_symbolArgs =
        new(StringComparer.Ordinal) { ["symbol"] = "AAPL" };

    [Fact]
    public void Envelope_SerializesAsSnakeCase()
    {
        var envelope = EnvelopeFactory.Success(
            new SearchSymbolResponse { Symbols = [] },
            ToolView.Summary,
            nextActions: [new NextAction("get-news-pulse", s_symbolArgs, "sentiment")],
            explanation: "found");

        var json = JsonSerializer.Serialize(
            envelope,
            FinnHubJsonContext.Default.ToolResponseEnvelopeSearchSymbolResponse);

        Assert.Contains("\"is_success\"", json);
        Assert.Contains("\"next_actions\"", json);
        Assert.Contains("\"approx_tokens\"", json);
        Assert.DoesNotContain("\"IsSuccess\"", json);
        Assert.DoesNotContain("\"NextActions\"", json);
    }

    [Fact]
    public void Envelope_OmitsNullPropertiesPerContextPolicy()
    {
        var envelope = EnvelopeFactory.Success(
            new SearchSymbolResponse { Symbols = [] },
            ToolView.Summary);

        var json = JsonSerializer.Serialize(
            envelope,
            FinnHubJsonContext.Default.ToolResponseEnvelopeSearchSymbolResponse);

        Assert.DoesNotContain("\"error_message\"", json);
        Assert.DoesNotContain("\"error_type\"", json);
        Assert.DoesNotContain("\"explanation\"", json);
        Assert.DoesNotContain("\"rate_limit\"", json);
        Assert.DoesNotContain("\"sentiment_source\"", json);
    }

    [Fact]
    public void ToolView_SerializesAsLowercaseString()
    {
        var json = JsonSerializer.Serialize(ToolView.Summary, FinnHubJsonContext.Default.ToolView);

        Assert.Equal("\"summary\"", json);
    }

    [Fact]
    public void NextAction_RoundTripsThroughJson()
    {
        var original = new NextAction("get-price-summary", s_symbolArgs, "stats");

        var json = JsonSerializer.Serialize(original, FinnHubJsonContext.Default.NextAction);
        var roundTrip = JsonSerializer.Deserialize(json, FinnHubJsonContext.Default.NextAction);

        Assert.NotNull(roundTrip);
        Assert.Equal(original.Tool, roundTrip.Tool);
        Assert.Equal(original.Why, roundTrip.Why);
        Assert.Equal(original.Args["symbol"], roundTrip.Args["symbol"]);
    }
}
