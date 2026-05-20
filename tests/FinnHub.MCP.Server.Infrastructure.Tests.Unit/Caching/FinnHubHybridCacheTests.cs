// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Caching;

public sealed class FinnHubHybridCacheTests
{
    private readonly HybridCache _hybridCache;
    private readonly ILogger<FinnHubHybridCache> _logger;
    private readonly FinnHubHybridCache _sut;

    public FinnHubHybridCacheTests()
    {
        var services = new ServiceCollection();
        services.AddHybridCache();
        this._hybridCache = services.BuildServiceProvider().GetRequiredService<HybridCache>();
        this._logger = Substitute.For<ILogger<FinnHubHybridCache>>();
        this._sut = new FinnHubHybridCache(
            this._hybridCache,
            Microsoft.Extensions.Options.Options.Create(new CacheOptions()),
            this._logger);
    }

    [Fact]
    public async Task GetOrCreateAsync_FirstCall_InvokesFactory()
    {
        var calls = 0;

        var result = await this._sut.GetOrCreateAsync(
            $"first-call:{Guid.NewGuid()}",
            CacheTier.News,
            _ =>
            {
                calls++;
                return ValueTask.FromResult("payload");
            });

        Assert.Equal(1, calls);
        Assert.Equal("payload", result);
    }

    [Fact]
    public async Task GetOrCreateAsync_SecondCallWithSameKey_DoesNotInvokeFactory()
    {
        var key = $"same-key:{Guid.NewGuid()}";
        var calls = 0;

        async ValueTask<string> Factory(CancellationToken _)
        {
            calls++;
            return await ValueTask.FromResult("payload");
        }

        await this._sut.GetOrCreateAsync(key, CacheTier.News, Factory);
        var second = await this._sut.GetOrCreateAsync(key, CacheTier.News, Factory);

        Assert.Equal(1, calls);
        Assert.Equal("payload", second);
    }

    [Fact]
    public async Task GetOrCreateAsync_DifferentKeys_InvokeFactoryPerKey()
    {
        var calls = 0;

        async ValueTask<string> Factory(CancellationToken _)
        {
            calls++;
            return await ValueTask.FromResult($"call-{calls}");
        }

        await this._sut.GetOrCreateAsync($"k-a:{Guid.NewGuid()}", CacheTier.News, Factory);
        await this._sut.GetOrCreateAsync($"k-b:{Guid.NewGuid()}", CacheTier.News, Factory);

        Assert.Equal(2, calls);
    }

    [Fact]
    public async Task GetOrCreateAsync_FactoryThrows_DoesNotPoisonCache()
    {
        var key = $"throws:{Guid.NewGuid()}";
        var calls = 0;

        async ValueTask<string> Factory(CancellationToken _)
        {
            calls++;
            if (calls == 1)
            {
                throw new InvalidOperationException("first call boom");
            }

            return await ValueTask.FromResult("recovered");
        }

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._sut.GetOrCreateAsync(key, CacheTier.News, Factory));

        var second = await this._sut.GetOrCreateAsync(key, CacheTier.News, Factory);

        Assert.Equal(2, calls);
        Assert.Equal("recovered", second);
    }

    [Fact]
    public async Task GetOrCreateAsync_EmitsMissThenHitLogs()
    {
        var key = $"log-shape:{Guid.NewGuid()}";

        await this._sut.GetOrCreateAsync(key, CacheTier.News, _ => ValueTask.FromResult("v"));
        await this._sut.GetOrCreateAsync(key, CacheTier.News, _ => ValueTask.FromResult("v"));

        this._logger.Received(2).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
