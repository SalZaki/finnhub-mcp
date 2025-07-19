// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Represents a standardized base structure for error responses returned by the MCP Server.
/// Derived classes should provide a specific <see cref="Title"/> to indicate the error category.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class ErrorResponse
{
    /// <summary>
    /// Gets the title or category of the error.
    /// This is intended to distinguish between types of errors (e.g., "ValidationError", "OperationError").
    /// </summary>
    [JsonPropertyName("title")]
    public abstract string Title { get; }

    /// <summary>
    /// Gets or sets a human-readable message describing the error.
    /// This message is intended to be shown directly to users or logged for diagnostics.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    /// <summary>
    /// Gets or sets the original exception message, if available.
    /// Useful for debugging or exposing internal error details in trusted environments.
    /// </summary>
    [JsonPropertyName("exception_message")]
    public string? ExceptionMessage { get; init; }

    /// <summary>
    /// Gets or sets a trace identifier that can be used to correlate logs and diagnostics.
    /// Typically set using <c>Activity.Current?.TraceId</c> or <c>HttpContext.TraceIdentifier</c>.
    /// </summary>
    [JsonPropertyName("trace_id")]
    public string? TraceId { get; init; }

    /// <summary>
    /// Gets or sets the timestamp (in UTC) when the error occurred.
    /// This is useful for logging and correlating with system activity.
    /// </summary>
    [JsonPropertyName("timestamp_utc")]
    public required DateTimeOffset TimestampUtc { get; init; }
}
