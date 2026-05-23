// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Extensions;

/// <summary>
/// Regression tests for <see cref="ServiceCollectionExtension.ConfigureFinnHubClient"/>.
/// Covers the URI-resolution failure mode that surfaced as <c>InvalidResponse</c>
/// when <c>BaseUrl</c> was configured without a trailing slash — relative request URIs
/// were resolved against a base that replaced the <c>/v1</c> segment, sending
/// requests to <c>/api/...</c> instead of <c>/api/v1/...</c>.
/// </summary>
public sealed class ConfigureFinnHubClientTests
{
    [Theory]
    [InlineData("https://finnhub.io/api/v1")]   // no trailing slash — must self-heal
    [InlineData("https://finnhub.io/api/v1/")]  // trailing slash already present — must stay correct
    public void BaseAddress_AlwaysEndsWithTrailingSlash_SoRelativeUrisResolveAgainstApiV1(string configuredBaseUrl)
    {
        using var client = new HttpClient();
        var provider = BuildProvider(configuredBaseUrl);

        ServiceCollectionExtension.ConfigureFinnHubClient(provider, client);

        Assert.NotNull(client.BaseAddress);
        Assert.EndsWith("/", client.BaseAddress!.AbsoluteUri, StringComparison.Ordinal);
        Assert.Equal("https://finnhub.io/api/v1/", client.BaseAddress.AbsoluteUri);

        // The load-bearing check: a relative URI like "stock/profile2" must combine
        // to the v1 namespace, NOT replace it. If this asserts the wrong URL,
        // the production server is silently hitting the Finnhub landing page and
        // failing with InvalidResponse on the resulting HTML.
        var resolved = new Uri(client.BaseAddress, "stock/profile2?symbol=AAPL");
        Assert.Equal("https://finnhub.io/api/v1/stock/profile2?symbol=AAPL", resolved.AbsoluteUri);
    }

    [Fact]
    public void ApiKey_WhenSet_IsAttachedAsXFinnhubTokenHeader()
    {
        using var client = new HttpClient();
        var provider = BuildProvider("https://finnhub.io/api/v1/", "test-key-123");

        ServiceCollectionExtension.ConfigureFinnHubClient(provider, client);

        Assert.True(client.DefaultRequestHeaders.TryGetValues("X-Finnhub-Token", out var values));
        Assert.Equal("test-key-123", values!.Single());
    }

    [Fact]
    public void ApiKey_WhenBlank_NoTokenHeaderAdded()
    {
        using var client = new HttpClient();
        var provider = BuildProvider("https://finnhub.io/api/v1/", apiKey: "");

        ServiceCollectionExtension.ConfigureFinnHubClient(provider, client);

        Assert.False(client.DefaultRequestHeaders.Contains("X-Finnhub-Token"));
    }

    private static ServiceProvider BuildProvider(string baseUrl, string apiKey = "any-key")
    {
        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            EndPoints = []
        });

        var services = new ServiceCollection();
        services.AddSingleton(options);
        return services.BuildServiceProvider();
    }
}
