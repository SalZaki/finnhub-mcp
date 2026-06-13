// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Http;

/// <summary>
/// Translates a non-success FinnHub HTTP response into the appropriate typed <c>ApiClient*</c>
/// exception. This is the single shared implementation behind every client's error path — it
/// replaces the per-client <c>HandleErrorAsync</c> methods that had drifted into eleven near-identical copies.
/// </summary>
internal static class FinnHubResponseErrors
{
    /// <summary>
    /// Reads the error body and throws the exception that matches the response status. The method
    /// always throws; the <see cref="Task"/> return type preserves the callers' <c>await …;</c> shape.
    /// </summary>
    /// <param name="response">The non-success response.</param>
    /// <param name="contentStream">The already-opened response content stream.</param>
    /// <param name="logger">The calling client's logger.</param>
    /// <param name="resource">The per-client noun (e.g. "quote") woven into the thrown message and the logs.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="treatUnauthorizedAsPremium">
    /// When <c>true</c>, 401 Unauthorized is mapped to <see cref="ApiClientPremiumRequiredException"/> alongside 403.
    /// Only the exchange-symbols client sets this: Finnhub gates its premium <c>/stock/symbol</c> exchanges behind 401.
    /// </param>
    /// <exception cref="ApiClientPremiumRequiredException">403 (or 401 when <paramref name="treatUnauthorizedAsPremium"/>).</exception>
    /// <exception cref="ApiClientHttpException">Any other non-success status.</exception>
    internal static async Task ThrowForStatusAsync(
        HttpResponseMessage response,
        Stream contentStream,
        ILogger logger,
        string resource,
        CancellationToken cancellationToken,
        bool treatUnauthorizedAsPremium = false)
    {
        var statusCode = response.StatusCode;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);

        var premium = statusCode == HttpStatusCode.Forbidden
            || (treatUnauthorizedAsPremium && statusCode == HttpStatusCode.Unauthorized);

        if (premium)
        {
            var endpoint = response.RequestMessage?.RequestUri?.AbsolutePath ?? "(unknown)";
            logger.LogWarning("Premium-locked {Resource} endpoint: {Endpoint} - {Content}", resource, endpoint, errorBody);
            throw new ApiClientPremiumRequiredException(endpoint, errorBody);
        }

        logger.Log(
            (int)statusCode >= 500 ? LogLevel.Error : LogLevel.Warning,
            "{Resource} API error: {StatusCode} - {Content}",
            resource,
            statusCode,
            errorBody);

        throw new ApiClientHttpException($"FinnHub {resource} returned {statusCode}.", statusCode, errorBody);
    }
}
