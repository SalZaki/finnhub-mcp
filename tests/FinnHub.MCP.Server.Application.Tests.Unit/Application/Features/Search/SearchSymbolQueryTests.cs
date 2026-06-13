// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Search;

public sealed class SearchSymbolQueryTests
{
    [Fact]
    public void Create_SetsRequiredFields_AndDefaultsLimit()
    {
        var q = SearchSymbolQuery.Create("qid-1", "AAPL");

        Assert.Equal("qid-1", q.QueryId);
        Assert.Equal("AAPL", q.Query);
        Assert.Equal(10, q.Limit);
        Assert.Null(q.Exchange);
    }

    [Fact]
    public void Create_HonoursExplicitLimit()
    {
        var q = SearchSymbolQuery.Create("qid-1", "AAPL", limit: 25);

        Assert.Equal(25, q.Limit);
    }

    [Fact]
    public void Exchange_CanBeInitialised()
    {
        var q = new SearchSymbolQuery { QueryId = "qid-1", Query = "AAPL", Limit = 15, Exchange = "NASDAQ" };

        Assert.Equal("NASDAQ", q.Exchange);
        Assert.Equal(15, q.Limit);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validate_LimitOutOfRange_Throws(int limit)
    {
        var q = SearchSymbolQuery.Create("qid", "AAPL", limit: limit);

        Assert.Throws<ArgumentOutOfRangeException>(() => q.Validate());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_BlankQuery_Throws(string? query)
    {
        var q = new SearchSymbolQuery { QueryId = "qid", Query = query!, Limit = 10 };

        Assert.Throws<ArgumentException>(() => q.Validate());
    }

    [Fact]
    public void Validate_QueryTooLong_Throws()
    {
        var q = SearchSymbolQuery.Create("qid", new string('a', 501));

        Assert.Throws<ArgumentException>(() => q.Validate());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]
    public void Validate_ValidInput_DoesNotThrow(int limit)
    {
        var q = SearchSymbolQuery.Create("qid", "A", limit: limit);

        q.Validate();
    }
}
