// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Exception thrown when an HTTP request to an external API fails.
/// </summary>
/// <param name="message">The message describing the error.</param>
/// <param name="statusCode">The HTTP status code returned by the API.</param>
/// <param name="responseContent">Optional content of the failed response.</param>
/// <param name="requestUri">The URI that was requested.</param>
/// <param name="innerException">The underlying exception, if any.</param>
public sealed class ApiClientHttpException(
    string message,
    HttpStatusCode statusCode,
    string? responseContent = null,
    string? requestUri = null,
    Exception? innerException = null)
    : ApiClientException(message, innerException)
{
    /// <summary>
    /// The HTTP status code returned by the external API.
    /// </summary>
    public HttpStatusCode StatusCode { get; } = statusCode;

    /// <summary>
    /// The raw content of the HTTP response (if available).
    /// </summary>
    public string? ResponseContent { get; } = responseContent;

    /// <summary>
    /// The URI that caused the exception.
    /// </summary>
    public string? RequestUri { get; } = requestUri;

    /// <inheritdoc />
    public override string ErrorCode => "API_CLIENT_HTTP";
}
