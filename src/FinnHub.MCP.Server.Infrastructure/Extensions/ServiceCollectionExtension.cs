// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Calendar.Clients;
using FinnHub.MCP.Server.Application.Calendar.Services;
using FinnHub.MCP.Server.Application.Exchanges.Clients;
using FinnHub.MCP.Server.Application.Exchanges.Services;
using FinnHub.MCP.Server.Application.Financials.Clients;
using FinnHub.MCP.Server.Application.Financials.Services;
using FinnHub.MCP.Server.Application.Insiders.Clients;
using FinnHub.MCP.Server.Application.Insiders.Services;
using FinnHub.MCP.Server.Application.News.Clients;
using FinnHub.MCP.Server.Application.News.Services;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Peers.Clients;
using FinnHub.MCP.Server.Application.Peers.Services;
using FinnHub.MCP.Server.Application.Prices.Clients;
using FinnHub.MCP.Server.Application.Prices.Services;
using FinnHub.MCP.Server.Application.Profiles.Clients;
using FinnHub.MCP.Server.Application.Profiles.Services;
using FinnHub.MCP.Server.Application.Quotes.Clients;
using FinnHub.MCP.Server.Application.Quotes.Services;
using FinnHub.MCP.Server.Application.RateLimiting;
using FinnHub.MCP.Server.Application.Recommendations.Clients;
using FinnHub.MCP.Server.Application.Recommendations.Services;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Infrastructure.Caching;
using FinnHub.MCP.Server.Infrastructure.Clients.Calendar;
using FinnHub.MCP.Server.Infrastructure.Clients.Exchanges;
using FinnHub.MCP.Server.Infrastructure.Clients.Financials;
using FinnHub.MCP.Server.Infrastructure.Clients.Insiders;
using FinnHub.MCP.Server.Infrastructure.Clients.News;
using FinnHub.MCP.Server.Infrastructure.Clients.Peers;
using FinnHub.MCP.Server.Infrastructure.Clients.Prices;
using FinnHub.MCP.Server.Infrastructure.Clients.Profiles;
using FinnHub.MCP.Server.Infrastructure.Clients.Quotes;
using FinnHub.MCP.Server.Infrastructure.Clients.Recommendations;
using FinnHub.MCP.Server.Infrastructure.Clients.Search;
using FinnHub.MCP.Server.Infrastructure.RateLimiting;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace FinnHub.MCP.Server.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for registering infrastructure services such as typed HTTP clients
/// and resilient policies using Polly for retry and circuit breaker behavior.
/// </summary>
/// <remarks>
/// Excluded from coverage measurement for the same reason as <c>Program.cs</c> (see
/// <c>coverlet.runsettings</c>): the only meaningful test for DI wiring is an end-to-end
/// host bootstrap, which is exercised by the live smoke step in CLAUDE.md's
/// "Adding a new MCP tool" recipe — not by unit tests. Instrumenting it would force a
/// <c>WebApplicationFactory</c> harness that hits lines without validating behaviour.
/// The load-bearing trailing-slash normalization in <see cref="ConfigureFinnHubClient"/>
/// is covered by per-client URL-assertion tests; see PR #169.
/// </remarks>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers infrastructure dependencies into the service collection, including
    /// a typed HTTP client for the FinnHub API with connection pooling, headers, and resilience policies.
    /// </summary>
    /// <param name="services">The service collection to which services are added.</param>
    public static void RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1 * 1024 * 1024;
            options.MaximumKeyLength = 1024;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(1),
                LocalCacheExpiration = TimeSpan.FromMinutes(1)
            };
        });

        services.AddSingleton<IFinnHubCache, FinnHubHybridCache>();

        services.AddSingleton<IRateLimitTracker, RateLimitTracker>();
        services.AddTransient<RateLimitHeaderHandler>();

        services.AddHttpClient<ISearchApiClient, FinnHubSearchApiClient>("FinnHub-Search-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            // Rate-limit header observation runs outermost so it sees the post-retry response
            // Polly ultimately surfaces to the caller, not the intermediate failed attempts.
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubSearchApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubSearchApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IPeersService, PeersService>();
        services.AddHttpClient<IPeersApiClient, FinnHubPeersApiClient>("FinnHub-Peers-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubPeersApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubPeersApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IFinancialsService, FinancialsService>();
        services.AddHttpClient<IFinancialsApiClient, FinnHubFinancialsApiClient>("FinnHub-Financials-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubFinancialsApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubFinancialsApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IPricesService, PricesService>();
        services.AddHttpClient<IPricesApiClient, FinnHubPricesApiClient>("FinnHub-Prices-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubPricesApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubPricesApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<INewsService, NewsService>();
        services.AddHttpClient<INewsApiClient, FinnHubNewsApiClient>("FinnHub-News-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubNewsApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubNewsApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IQuotesService, QuotesService>();
        services.AddHttpClient<IQuotesApiClient, FinnHubQuotesApiClient>("FinnHub-Quotes-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubQuotesApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubQuotesApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IProfilesService, ProfilesService>();
        services.AddHttpClient<IProfilesApiClient, FinnHubProfilesApiClient>("FinnHub-Profiles-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubProfilesApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubProfilesApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<ICalendarService, CalendarService>();
        services.AddHttpClient<ICalendarApiClient, FinnHubCalendarApiClient>("FinnHub-Calendar-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubCalendarApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubCalendarApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IInsidersService, InsidersService>();
        services.AddHttpClient<IInsidersApiClient, FinnHubInsidersApiClient>("FinnHub-Insiders-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubInsidersApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubInsidersApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IRecommendationsService, RecommendationsService>();
        services.AddHttpClient<IRecommendationsApiClient, FinnHubRecommendationsApiClient>("FinnHub-Recommendations-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubRecommendationsApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubRecommendationsApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton<IExchangeSymbolsService, ExchangeSymbolsService>();
        services.AddHttpClient<IExchangesApiClient, FinnHubExchangesApiClient>("FinnHub-Exchanges-Client", ConfigureFinnHubClient)
            .ConfigurePrimaryHttpMessageHandler(BuildPrimaryHandler)
            .AddHttpMessageHandler<RateLimitHeaderHandler>()
            .AddPolicyHandler((provider, _) => GetRetryPolicy(provider.GetRequiredService<ILogger<FinnHubExchangesApiClient>>()))
            .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy(provider.GetRequiredService<ILogger<FinnHubExchangesApiClient>>()))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// Shared <see cref="HttpClient"/> configuration for every Finnhub-typed client.
    /// </summary>
    /// <remarks>
    /// The trailing-slash normalization is load-bearing. Per RFC 3986, if <see cref="HttpClient.BaseAddress"/>
    /// lacks a trailing slash, the last path segment is **replaced** when combined with a relative request URI:
    /// <c>https://finnhub.io/api/v1</c> + <c>stock/profile2</c> → <c>https://finnhub.io/api/stock/profile2</c>
    /// (the <c>/v1</c> is silently lost). Finnhub responds to that wrong URL with a 302 to its landing
    /// page, the HttpClient follows the redirect, and our deserializer fails on the resulting HTML —
    /// surfacing as <c>InvalidResponse</c> rather than a clean <c>ServiceUnavailable</c>.
    /// We append the slash defensively so a configuration regression cannot reintroduce the bug.
    /// </remarks>
    internal static void ConfigureFinnHubClient(IServiceProvider provider, HttpClient client)
    {
        var options = provider.GetRequiredService<IOptions<FinnHubOptions>>().Value;

        var baseUrl = options.BaseUrl.EndsWith('/') ? options.BaseUrl : options.BaseUrl + "/";
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("FinnHub-MCP-Server", "1.0"));
        client.Timeout = TimeSpan.FromSeconds(30);

        if (!string.IsNullOrWhiteSpace(options.ApiKey))
        {
            client.DefaultRequestHeaders.Add("X-Finnhub-Token", options.ApiKey);
        }
    }

    private static HttpMessageHandler BuildPrimaryHandler() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        MaxConnectionsPerServer = 10,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    };

    /// <summary>
    /// Builds a retry policy using Polly with exponential backoff and logs each retry attempt.
    /// </summary>
    /// <param name="logger">The logger to record retry attempts and reasons.</param>
    /// <returns>An asynchronous retry policy for HTTP responses.</returns>
    /// <remarks>
    /// HTTP 403 and 401 are excluded — Finnhub uses 403 to signal premium-locked endpoints and
    /// 401 for premium-gated exchanges on <c>/stock/symbol</c>; both are permanent failures for the
    /// current API key. Retrying either wastes quota and delays the typed
    /// <c>ApiClientPremiumRequiredException</c> the caller needs.
    /// </remarks>
    internal static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
    {
        return Policy<HttpResponseMessage>
            .HandleResult(res => !res.IsSuccessStatusCode && res.StatusCode is not (HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized))
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    context["Policy"] = "RetryPolicy";
                    context["RequestUri"] = outcome.Result?.RequestMessage?.RequestUri?.ToString();

                    var message = outcome.Exception?.Message
                                  ?? outcome.Result?.StatusCode.ToString()
                                  ?? "Unknown failure reason";

                    logger.Log(LogLevel.Warning, "Retry {RetryAttempt} after {Delay}s due to: {Reason}", retryAttempt, timespan.TotalSeconds, message);
                });
    }

    /// <summary>
    /// Builds a circuit breaker policy using Polly that breaks after 3 failures for 30 seconds,
    /// and logs state changes.
    /// </summary>
    /// <param name="logger">The logger to record circuit breaker events.</param>
    /// <returns>An asynchronous circuit breaker policy for HTTP responses.</returns>
    /// <remarks>
    /// HTTP 403 and 401 are excluded for the same reason as the retry policy: premium-locked
    /// endpoints (403) and premium-gated exchanges (401) are expected, per-key permanent failure
    /// modes and must not trip the breaker for the broader client.
    /// </remarks>
    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ILogger logger)
    {
        return Policy<HttpResponseMessage>
            .HandleResult(r => !r.IsSuccessStatusCode && r.StatusCode is not (HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized))
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay) =>
                {
                    var statusCode = outcome.Result?.StatusCode;
                    var message = outcome.Exception?.Message ?? statusCode?.ToString() ?? "Unknown error";

                    logger.Log(LogLevel.Warning, "Circuit broken for {BreakDelay}s due to: {Reason}", breakDelay.TotalSeconds, message);
                },
                onReset: () =>
                {
                    logger.Log(LogLevel.Information, "Circuit reset.");
                });
    }
}
