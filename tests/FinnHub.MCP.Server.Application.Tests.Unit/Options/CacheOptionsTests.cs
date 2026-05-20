// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Options;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Options;

public sealed class CacheOptionsTests
{
    [Fact]
    public void Defaults_MatchSpecValues()
    {
        var options = new CacheOptions();

        Assert.Equal(TimeSpan.FromSeconds(10), options.QuoteTtl);
        Assert.Equal(TimeSpan.FromSeconds(60), options.NewsTtl);
        Assert.Equal(TimeSpan.FromHours(1), options.FinancialsTtl);
        Assert.Equal(TimeSpan.FromHours(24), options.ProfileTtl);
        Assert.Equal(TimeSpan.FromDays(7), options.ExchangesTtl);
    }

    [Theory]
    [InlineData(CacheTier.Quote, 10)]
    [InlineData(CacheTier.News, 60)]
    [InlineData(CacheTier.Financials, 3600)]
    [InlineData(CacheTier.Profile, 86_400)]
    [InlineData(CacheTier.Exchanges, 604_800)]
    public void GetTtl_ReturnsConfiguredValuePerTier(CacheTier tier, int expectedSeconds)
    {
        var options = new CacheOptions();

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), options.GetTtl(tier));
    }

    [Fact]
    public void GetTtl_UnknownTier_Throws()
    {
        var options = new CacheOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.GetTtl((CacheTier)999));
    }

    [Fact]
    public void Validation_ZeroQuoteTtl_FailsDataAnnotations()
    {
        // The data-annotation [Range] is what ValidateDataAnnotations() / ValidateOnStart()
        // consults at startup. Asserting it directly avoids wiring IOptions in tests.
        var options = new CacheOptions { QuoteTtl = TimeSpan.Zero };

        Assert.False(TryValidate(options, out var results));
        Assert.Contains(results, r =>
            r.MemberNames.Contains(nameof(CacheOptions.QuoteTtl)));
    }

    [Fact]
    public void Validation_DefaultInstance_PassesDataAnnotations()
    {
        var options = new CacheOptions();

        Assert.True(TryValidate(options, out _));
    }

    [Theory]
    [InlineData(nameof(CacheOptions.QuoteTtl))]
    [InlineData(nameof(CacheOptions.NewsTtl))]
    [InlineData(nameof(CacheOptions.FinancialsTtl))]
    [InlineData(nameof(CacheOptions.ProfileTtl))]
    [InlineData(nameof(CacheOptions.ExchangesTtl))]
    public void Validation_NegativeTtl_FailsForEveryTier(string memberName)
    {
        var options = memberName switch
        {
            nameof(CacheOptions.QuoteTtl) => new CacheOptions { QuoteTtl = TimeSpan.FromSeconds(-1) },
            nameof(CacheOptions.NewsTtl) => new CacheOptions { NewsTtl = TimeSpan.FromSeconds(-1) },
            nameof(CacheOptions.FinancialsTtl) => new CacheOptions { FinancialsTtl = TimeSpan.FromSeconds(-1) },
            nameof(CacheOptions.ProfileTtl) => new CacheOptions { ProfileTtl = TimeSpan.FromSeconds(-1) },
            nameof(CacheOptions.ExchangesTtl) => new CacheOptions { ExchangesTtl = TimeSpan.FromSeconds(-1) },
            _ => throw new ArgumentOutOfRangeException(nameof(memberName))
        };

        Assert.False(TryValidate(options, out var results));
        Assert.Contains(results, r => r.MemberNames.Contains(memberName));
    }

    private static bool TryValidate(CacheOptions options, out List<ValidationResult> results)
    {
        results = [];
        return Validator.TryValidateObject(
            options,
            new ValidationContext(options),
            results,
            validateAllProperties: true);
    }
}
