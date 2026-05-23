// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Profiles.Clients;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Profiles;

/// <summary>HTTP client for the Finnhub <c>/stock/profile2</c> endpoint.</summary>
public sealed class FinnHubProfilesApiClient : IProfilesApiClient
{
    private const string EndpointName = "profile";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubProfilesApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubProfilesApiClient"/>.</summary>
    public FinnHubProfilesApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubProfilesApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetCompanyProfileResponse> GetProfileAsync(
        GetCompanyProfileQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Profile endpoint is not configured or inactive");

        var requestUri = $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(query.Symbol)}";

        this._logger.LogInformation("Requesting profile from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub profile failed: {Uri}", requestUri);
            throw new ApiClientHttpException($"HTTP request to FinnHub profile failed: {requestUri}", HttpStatusCode.InternalServerError);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Profile request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Profile request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Profile request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Profile request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await this.HandleErrorAsync(response, contentStream, cancellationToken).ConfigureAwait(false);
        }

        FinnHubProfileResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubProfileResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize profile response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize profile response: {requestUri}",
                innerException: ex);
        }

        return new GetCompanyProfileResponse
        {
            Ticker = dto?.Ticker ?? query.Symbol,
            Name = dto?.Name,
            Country = dto?.Country,
            Currency = dto?.Currency,
            Exchange = dto?.Exchange,
            Ipo = dto?.Ipo,
            MarketCap = dto?.MarketCapitalization,
            ShareOutstanding = dto?.ShareOutstanding,
            Industry = dto?.FinnhubIndustry,
            Logo = query.IncludeCosmeticFields ? dto?.Logo : null,
            Phone = query.IncludeCosmeticFields ? dto?.Phone : null,
            WebUrl = query.IncludeCosmeticFields ? dto?.WebUrl : null
        };
    }

    private async Task HandleErrorAsync(HttpResponseMessage response, Stream contentStream, CancellationToken ct)
    {
        var statusCode = response.StatusCode;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        if (statusCode == HttpStatusCode.Forbidden)
        {
            var endpoint = response.RequestMessage?.RequestUri?.AbsolutePath ?? "(unknown)";
            this._logger.LogWarning("Premium-locked profile endpoint: {Endpoint} - {Content}", endpoint, errorBody);
            throw new ApiClientPremiumRequiredException(endpoint, errorBody);
        }

        this._logger.Log(
            (int)statusCode >= 500 ? LogLevel.Error : LogLevel.Warning,
            "Profile API error: {StatusCode} - {Content}", statusCode, errorBody);

        throw new ApiClientHttpException($"FinnHub profile returned {statusCode}.", statusCode, errorBody);
    }
}
