// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Base exception for all API client-related failures.
/// </summary>
/// <param name="message">A message describing the error.</param>
/// <param name="inner">The inner exception, if any.</param>
public abstract class ApiClientException(string message, Exception? inner = null)
    : FinnHubMcpServerException(message, inner)
{
    /// <summary>
    /// Gets the unique error code representing this exception type.
    /// </summary>
    public virtual string ErrorCode => "API_CLIENT_ERROR";

    /// <summary>
    /// Optional correlation ID for tracking the request.
    /// </summary>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// The external service this exception is associated with.
    /// </summary>
    public string? SourceService { get; init; }

    /// <summary>
    /// The time at which this exception was created (UTC).
    /// </summary>
    public DateTimeOffset OccurredAtUtc { get; } = DateTimeOffset.UtcNow;
}
