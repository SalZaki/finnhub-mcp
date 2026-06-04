// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Discovery;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Discovery;

public sealed class ToolRegistryTests
{
    private static ToolDescriptor Descriptor(string name, string description, params string[] examples) => new()
    {
        Name = name,
        Title = name,
        Description = description,
        Examples = examples
    };

    private static ToolRegistry BuildRegistry() => new(
    [
        Descriptor("price-tool", "current price movement and percent return over a period", "is the stock up", "price performance"),
        Descriptor("news-tool", "latest news headlines and sentiment for a company", "company news", "what is the sentiment"),
        Descriptor("earnings-tool", "upcoming earnings calendar and scheduled report dates", "upcoming earnings", "earnings season"),
    ]);

    [Fact]
    public void Search_RanksDocumentContainingQueryTermFirst()
    {
        var registry = BuildRegistry();

        var results = registry.Search("price return movement");

        Assert.NotEmpty(results);
        Assert.Equal("price-tool", results[0].Tool.Name);
    }

    [Fact]
    public void Search_DistinctIntent_SelectsTheRightTool()
    {
        var registry = BuildRegistry();

        Assert.Equal("news-tool", registry.Search("news headlines sentiment")[0].Tool.Name);
        Assert.Equal("earnings-tool", registry.Search("upcoming earnings calendar")[0].Tool.Name);
    }

    [Fact]
    public void Search_OrdersResultsByDescendingScore()
    {
        var registry = BuildRegistry();

        var results = registry.Search("upcoming earnings");

        for (var i = 1; i < results.Count; i++)
        {
            Assert.True(results[i - 1].Score >= results[i].Score);
        }
    }

    [Fact]
    public void Search_RespectsTopN()
    {
        var registry = BuildRegistry();

        // A term shared by every document so all three match, then cap at 2.
        var results = registry.Search("company price news earnings", topN: 2);

        Assert.True(results.Count <= 2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Search_BlankIntent_ReturnsEmpty(string intent)
    {
        Assert.Empty(BuildRegistry().Search(intent));
    }

    [Fact]
    public void Search_IntentMatchingNothing_ReturnsEmpty()
    {
        Assert.Empty(BuildRegistry().Search("cryptocurrency mining rig"));
    }

    [Fact]
    public void Search_NonPositiveTopN_ReturnsEmpty()
    {
        Assert.Empty(BuildRegistry().Search("price", topN: 0));
    }

    [Fact]
    public void Descriptors_PreservesConstructionOrder()
    {
        var registry = BuildRegistry();

        Assert.Equal(
            ["price-tool", "news-tool", "earnings-tool"],
            registry.Descriptors.Select(d => d.Name));
    }

    [Fact]
    public void Search_OnlyMatchingScoresAreReturned()
    {
        var registry = BuildRegistry();

        var results = registry.Search("sentiment");

        Assert.All(results, m => Assert.True(m.Score > 0d));
        Assert.Equal("news-tool", Assert.Single(results).Tool.Name);
    }

    [Fact]
    public void Search_ExcludesNonSearchableDescriptors_ButDescriptorsStillListsThem()
    {
        var registry = new ToolRegistry(
        [
            new ToolDescriptor
            {
                Name = "meta-tool",
                Title = "meta",
                Description = "discover price news earnings tools by intent",
                Searchable = false
            },
            new ToolDescriptor
            {
                Name = "price-tool",
                Title = "price",
                Description = "price movement and return"
            },
        ]);

        var results = registry.Search("price");

        Assert.DoesNotContain(results, m => m.Tool.Name == "meta-tool");
        Assert.Equal("price-tool", Assert.Single(results).Tool.Name);
        Assert.Contains(registry.Descriptors, d => d.Name == "meta-tool");
    }
}
