// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json.Serialization;
using FinnHub.MCP.Server.Application.Exchanges;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Resources.Exchanges;

/// <summary>
/// MCP resource that provides stock exchange listings from Finnhub.
/// Uses <see cref="BaseResource.CreateResponse{T}"/> to serialize JSON data for clients.
/// </summary>
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
        var responsePayload = new ExchangesResponse
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
            new Result<ExchangesResponse>().Success(responsePayload),
            uri,
            ResourceJsonContext.Default.ResultExchangesResponse);

        return ValueTask.FromResult(result);
    }
}

[ExcludeFromCodeCoverage]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseUpper,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(Result<ExchangesResponse>))]
public partial class ResourceJsonContext : JsonSerializerContext;
