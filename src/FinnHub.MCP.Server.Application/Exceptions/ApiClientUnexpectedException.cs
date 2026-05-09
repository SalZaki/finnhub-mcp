// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Exception thrown when an unexpected error occurs during an API client operation.
/// This typically wraps unknown or unclassified exceptions that are not caught by more specific handlers.
/// </summary>
/// <param name="message">A human-readable message describing the error.</param>
/// <param name="innerException">The underlying exception that triggered this one.</param>
public sealed class ApiClientUnexpectedException(string message, Exception? innerException = null)
    : ApiClientException(message, innerException)
{
    /// <summary>
    /// Gets the stable identifier for this error category, indicating an
    /// unclassified failure that did not match any of the more specific
    /// API client exception types. Always returns <c>"API_CLIENT_UNEXPECTED"</c>.
    /// Treat occurrences of this code as a signal to investigate the inner
    /// exception and, where possible, reclassify into a more specific type.
    /// </summary>
    public override string ErrorCode => "API_CLIENT_UNEXPECTED";
}
