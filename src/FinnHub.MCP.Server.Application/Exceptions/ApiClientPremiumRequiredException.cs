// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Thrown when the upstream Finnhub endpoint requires a premium plan that the
/// configured API key does not have access to (HTTP 403 Forbidden).
/// </summary>
/// <remarks>
/// Distinct from <see cref="ApiClientHttpException"/> so the resilience pipeline
/// and the service layer can treat premium-locked endpoints as a permanent,
/// non-retryable failure. Polly retry and circuit breaker policies are configured
/// to bypass 403; the service layer maps this exception to
/// <c>ResultErrorType.PremiumRequired</c> and surfaces <c>premium = true</c> on
/// the response envelope.
/// </remarks>
/// <param name="endpoint">The Finnhub endpoint path that returned 403, e.g. <c>"/search"</c>.</param>
/// <param name="responseContent">Optional raw response body for diagnostics.</param>
/// <param name="innerException">The underlying exception, if any.</param>
public sealed class ApiClientPremiumRequiredException(
    string endpoint,
    string? responseContent = null,
    Exception? innerException = null)
    : ApiClientException(
        $"FinnHub endpoint '{endpoint}' requires a premium plan (HTTP 403).",
        innerException)
{
    /// <summary>
    /// Gets the upstream endpoint path that returned 403.
    /// </summary>
    public string Endpoint { get; } = endpoint;

    /// <summary>
    /// Gets the raw response content from the 403, when available.
    /// </summary>
    public string? ResponseContent { get; } = responseContent;

    /// <inheritdoc />
    public override string ErrorCode => "API_CLIENT_PREMIUM_REQUIRED";
}
