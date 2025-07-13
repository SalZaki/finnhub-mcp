// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Infrastructure.Clients.Search;
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
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers infrastructure dependencies into the service collection, including
    /// a typed HTTP client for the FinnHub API with connection pooling, headers, and resilience policies.
    /// </summary>
    /// <param name="services">The service collection to which services are added.</param>
    public static void RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<ISearchApiClient, FinnHubSearchApiClient>("FinnHub", (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<FinnHubOptions>>().Value;

                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("FinnHub-MCP-Server", "1.0"));
                client.Timeout = TimeSpan.FromSeconds(30);

                if (!string.IsNullOrWhiteSpace(options.ApiKey))
                {
                    client.DefaultRequestHeaders.Add("X-Finnhub-Token", options.ApiKey);
                }
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 10,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            .AddPolicyHandler((provider, _) =>
            {
                var logger = provider.GetRequiredService<ILogger<FinnHubSearchApiClient>>();
                return GetRetryPolicy(logger);
            })
            .AddPolicyHandler((provider, _) =>
            {
                var logger = provider.GetRequiredService<ILogger<FinnHubSearchApiClient>>();
                return GetCircuitBreakerPolicy(logger);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// Builds a retry policy using Polly with exponential backoff and logs each retry attempt.
    /// </summary>
    /// <param name="logger">The logger to record retry attempts and reasons.</param>
    /// <returns>An asynchronous retry policy for HTTP responses.</returns>
    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
    {
        return Policy<HttpResponseMessage>
            .HandleResult(res => !res.IsSuccessStatusCode)
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
    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ILogger logger)
    {
        return Policy<HttpResponseMessage>
            .HandleResult(r => !r.IsSuccessStatusCode)
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
