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
    public override string ErrorCode => "API_CLIENT_UNEXPECTED";
}
