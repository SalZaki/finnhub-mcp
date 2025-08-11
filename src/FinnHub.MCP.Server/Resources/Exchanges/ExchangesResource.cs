// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json.Serialization;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Resources.Exchanges;

/// <summary>
/// MCP resource that provides stock exchange listings from Finnhub.
/// Uses <see cref="BaseResource.CreateResponse{T}"/> to serialize JSON data for clients.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ExchangesResource : BaseResource
{
    private static readonly ResourceTemplate s_template = new()
    {
        UriTemplate = "finnhub://resources/exchanges",
        Name = "get‑exchanges",
        Description = "Gets all the exchanges listed on Finnhub.",
        MimeType = MediaTypeNames.Application.Json,
        Annotations = new Annotations
        {
            Audience = [Role.Assistant, Role.User]
        }
    };

    public override ResourceTemplate ProtocolResourceTemplate => s_template;

    public override Resource? ProtocolResource => s_template.AsResource();

    /// <summary>
    /// Handles a resource read request by returning a static list of exchanges.
    /// </summary>
    /// <param name="request">
    /// The <see cref="RequestContext{ReadResourceRequestParams}"/> containing
    /// request metadata including the requested URI.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{ReadResourceResult}"/> containing JSON-serialized
    /// exchange data via <see cref="ReadResourceResult"/> and
    /// <see cref="TextResourceContents"/>, or <c>null</c> in case of no response required.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="request"/>, its <c>Params</c>, or <c>Params.Uri</c> is <c>null</c>.
    /// </exception>
    public override ValueTask<ReadResourceResult?> ReadAsync(
        RequestContext<ReadResourceRequestParams> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Params);
        ArgumentNullException.ThrowIfNull(request.Params.Uri);
        cancellationToken.ThrowIfCancellationRequested();

        var uri = request.Params.Uri;

        // TODO: Replace stub data with real Finnhub API call logic.
        var responsePayload = new ExchangeResponse
        {
            Exchanges =
            [
                new Exchange
                {
                    ExchangeCode = "L",
                    ExchangeName = "London Stock Exchange",
                    CountryCode = "GB",
                    CountryName = "United Kingdom",
                    MicCode = "XLON",
                    TimeZone = "Europe/London",
                    TradingHours = "08:00‑16:30",
                    Url = "https://www.tradinghours.com/exchanges/lse",
                    CloseDate = string.Empty
                }
            ]
        };

        var result = CreateResponse(
            new Result<ExchangeResponse>().Success(responsePayload),
            uri,
            ResourceJsonContext.Default.ResultExchangeResponse);

        return ValueTask.FromResult(result);
    }
}

[ExcludeFromCodeCoverage]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseUpper,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(Result<ExchangeResponse>))]
public partial class ResourceJsonContext : JsonSerializerContext;

/// <summary>
/// Represents a paginated response that contains one or more stock exchange records.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ExchangeResponse
{
    /// <summary>
    /// Gets the list of stock exchanges returned by the API.
    /// </summary>
    [JsonPropertyName("exchanges")]
    public IReadOnlyList<Exchange> Exchanges { get; init; } = [];

    /// <summary>
    /// Gets the number of exchanges in the response.
    /// </summary>
    [JsonPropertyName("total_count")]
    public int TotalCount => this.Exchanges.Count;

    /// <summary>
    /// Gets whether there are any exchanges returned.
    /// </summary>
    [JsonPropertyName("has_results")]
    public bool HasResults => this.TotalCount > 0;
}

/// <summary>
/// Represents detailed information about a single stock exchange.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class Exchange
{
    /// <summary>
    /// Gets the exchange code or symbol (e.g. "NYSE", "AB").
    /// </summary>
    [JsonPropertyName("code")]
    public required string ExchangeCode { get; init; }

    /// <summary>
    /// Gets the full human-readable name of the exchange.
    /// </summary>
    [JsonPropertyName("name")]
    public required string ExchangeName { get; init; }

    /// <summary>
    /// Gets the Market Identifier Code (MIC), per ISO 10383.
    /// </summary>
    [JsonPropertyName("mic")]
    public string? MicCode { get; init; }

    /// <summary>
    /// Gets the IANA time zone identifier for the exchange's location.
    /// </summary>
    [JsonPropertyName("time_zone")]
    public string? TimeZone { get; init; }

    /// <summary>
    /// Gets the scheduled start time of the pre‑market session.
    /// </summary>
    [JsonPropertyName("pre_market_hours")]
    public string? PreMarketHours { get; init; }

    /// <summary>
    /// Gets the regular market trading hours.
    /// </summary>
    [JsonPropertyName("trading_hours")]
    public string? TradingHours { get; init; }

    /// <summary>
    /// Gets the scheduled hours of the post‑market session.
    /// </summary>
    [JsonPropertyName("post_market_hours")]
    public string? PostMarketHours { get; init; }

    /// <summary>
    /// Gets the date or time when the exchange is closed (if applicable).
    /// </summary>
    [JsonPropertyName("close_date")]
    public string? CloseDate { get; init; }

    /// <summary>
    /// Gets the ISO 2‑letter country code where the exchange is located.
    /// </summary>
    [JsonPropertyName("country_code")]
    public required string CountryCode { get; init; }

    /// <summary>
    /// Gets the full country name where the exchange is located.
    /// </summary>
    [JsonPropertyName("country_name")]
    public required string CountryName { get; init; }

    /// <summary>
    /// Gets the URL for more information about the exchange.
    /// </summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}
