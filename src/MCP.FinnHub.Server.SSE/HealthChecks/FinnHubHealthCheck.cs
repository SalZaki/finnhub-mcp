using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MCP.FinnHub.Server.SSE.Options;

namespace MCP.FinnHub.Server.SSE.HealthChecks;

public sealed class FinnHubHealthCheck(IHttpClientFactory httpClientFactory, IOptions<FinnHubOptions> options)
    : IHealthCheck
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("finnhub");
    private readonly FinnHubOptions _options = options.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{this._options.BaseUrl}/quote?symbol=IBM");
            request.Headers.Add("X-Finnhub-Token", this._options.ApiKey);
            var response = await this._httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("FinnHub API is reachable.")
                : HealthCheckResult.Unhealthy($"FinnHub API returned status code {response.StatusCode}.");
        }
        catch (HttpRequestException ex)
        {
            return HealthCheckResult.Unhealthy("Exception occurred while accessing FinnHub API.", ex);
        }
        catch (TaskCanceledException ex)
        {
            return HealthCheckResult.Degraded("FinnHub API request timed out.", ex);
        }
    }
}
