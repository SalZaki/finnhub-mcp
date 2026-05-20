// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Caching;

public sealed class CacheKeyTests
{
    [Fact]
    public void Build_ProducesExpectedFormat()
    {
        var key = CacheKey.Build("shared", CacheTier.News, "search-symbol:q=aapl:ex=*:lim=10");

        Assert.Equal("finnhub:tenant=shared:tier=news:search-symbol:q=aapl:ex=*:lim=10", key);
    }

    [Theory]
    [InlineData(CacheTier.Quote, "quote")]
    [InlineData(CacheTier.News, "news")]
    [InlineData(CacheTier.Financials, "financials")]
    [InlineData(CacheTier.Profile, "profile")]
    [InlineData(CacheTier.Exchanges, "exchanges")]
    public void Build_LowercasesTierName(CacheTier tier, string expectedSegment)
    {
        var key = CacheKey.Build("shared", tier, "x");

        Assert.Contains($":tier={expectedSegment}:", key);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("SHARED")]
    [InlineData("user@host")]
    [InlineData("has space")]
    public void Build_RejectsInvalidTenant(string tenant)
    {
        Assert.Throws<ArgumentException>(() =>
            CacheKey.Build(tenant, CacheTier.News, "x"));
    }

    [Fact]
    public void Build_RejectsTenantOver64Chars()
    {
        var tenant = new string('a', 65);

        Assert.Throws<ArgumentException>(() =>
            CacheKey.Build(tenant, CacheTier.News, "x"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Build_RejectsEmptyLogicalKey(string logicalKey)
    {
        Assert.Throws<ArgumentException>(() =>
            CacheKey.Build("shared", CacheTier.News, logicalKey));
    }

    [Fact]
    public void Build_RejectsOversizedLogicalKey()
    {
        var logicalKey = new string('x', 257);

        Assert.Throws<ArgumentException>(() =>
            CacheKey.Build("shared", CacheTier.News, logicalKey));
    }
}
