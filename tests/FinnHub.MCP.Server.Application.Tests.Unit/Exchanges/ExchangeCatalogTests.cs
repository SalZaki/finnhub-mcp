// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Exchanges;

public sealed class ExchangeCatalogTests
{
    [Fact]
    public void Exchanges_ContainsFullMultiVenueCatalog_NotTheSingleStub()
    {
        var catalog = new ExchangeCatalog().Exchanges;

        Assert.True(
            catalog.Count >= 70,
            $"Expected the full Finnhub venue catalog (70+ exchanges), but got {catalog.Count}.");

        var codes = catalog.Select(e => e.ExchangeCode).ToHashSet();
        Assert.Contains("US", codes);
        Assert.Contains("L", codes);
        Assert.Contains("T", codes);
        Assert.Contains("HK", codes);
    }

    [Fact]
    public void Exchanges_HaveUniqueCodes()
    {
        var codes = new ExchangeCatalog().Exchanges.Select(e => e.ExchangeCode).ToList();

        Assert.Equal(codes.Count, codes.Distinct().Count());
    }

    [Fact]
    public void Exchanges_EveryEntry_HasRequiredFieldsPopulated()
    {
        var catalog = new ExchangeCatalog().Exchanges;

        Assert.All(catalog, exchange =>
        {
            Assert.False(string.IsNullOrWhiteSpace(exchange.ExchangeCode));
            Assert.False(string.IsNullOrWhiteSpace(exchange.ExchangeName));
            Assert.False(string.IsNullOrWhiteSpace(exchange.CountryCode));
            Assert.False(string.IsNullOrWhiteSpace(exchange.CountryName));
        });
    }

    [Fact]
    public void LondonStockExchange_IsPresent_AmongManyVenues()
    {
        var catalog = new ExchangeCatalog().Exchanges;

        var london = catalog.Single(e => e.ExchangeCode == "L");

        Assert.Equal("XLON", london.MicCode);
        Assert.Equal("GB", london.CountryCode);
        Assert.True(catalog.Count > 1, "Catalog must no longer be the single-venue stub.");
    }
}
