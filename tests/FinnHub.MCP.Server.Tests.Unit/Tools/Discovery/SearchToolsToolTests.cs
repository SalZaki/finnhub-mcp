// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Discovery;
using FinnHub.MCP.Server.Tools.Discovery;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Discovery;

public sealed class SearchToolsToolTests
{
    private readonly IToolRegistry _registry = Substitute.For<IToolRegistry>();
    private readonly SearchToolsTool _sut;

    public SearchToolsToolTests() =>
        this._sut = new SearchToolsTool(this._registry, NullLogger<SearchToolsTool>.Instance);

    private static ToolDescriptor Descriptor(string name) => new()
    {
        Name = name,
        Title = name,
        Description = $"description for {name}",
        Category = "Pricing"
    };

    [Fact]
    public void SearchTools_ValidIntent_ReturnsRankedMatchesInEnvelope()
    {
        this._registry.Search("price now", Arg.Any<int>()).Returns(
        [
            new ToolMatch(Descriptor("get-price-summary"), 3.2d),
            new ToolMatch(Descriptor("get-quote"), 1.1d)
        ]);

        var envelope = this._sut.SearchTools("price now");

        Assert.True(envelope.IsSuccess);
        Assert.NotNull(envelope.Data);
        Assert.Equal("price now", envelope.Data!.Intent);
        Assert.Equal(2, envelope.Data.TotalMatches);
        Assert.Equal("get-price-summary", envelope.Data.Matches[0].Name);
        Assert.Equal("get-quote", envelope.Data.Matches[1].Name);
    }

    [Fact]
    public void SearchTools_SummaryView_OmitsDescription()
    {
        this._registry.Search(Arg.Any<string>(), Arg.Any<int>()).Returns([new ToolMatch(Descriptor("get-quote"), 2d)]);

        var envelope = this._sut.SearchTools("price", view: "summary");

        Assert.Null(envelope.Data!.Matches[0].Description);
    }

    [Fact]
    public void SearchTools_StandardView_IncludesDescription()
    {
        this._registry.Search(Arg.Any<string>(), Arg.Any<int>()).Returns([new ToolMatch(Descriptor("get-quote"), 2d)]);

        var envelope = this._sut.SearchTools("price", view: "standard");

        Assert.Equal("description for get-quote", envelope.Data!.Matches[0].Description);
    }

    [Fact]
    public void SearchTools_NoMatches_ReturnsEmptyMatchesSuccess()
    {
        this._registry.Search(Arg.Any<string>(), Arg.Any<int>()).Returns([]);

        var envelope = this._sut.SearchTools("nonsense");

        Assert.True(envelope.IsSuccess);
        Assert.Empty(envelope.Data!.Matches);
    }

    [Fact]
    public void SearchTools_IntentOver200Chars_Throws()
    {
        Assert.Throws<ArgumentException>(() => this._sut.SearchTools(new string('a', 201)));
    }

    [Fact]
    public void SearchTools_EmptyIntent_Throws()
    {
        Assert.Throws<ArgumentException>(() => this._sut.SearchTools(""));
    }
}
