// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Exception thrown when an API request is cancelled by a caller.
/// </summary>
/// <param name="message">The cancellation reason.</param>
/// <param name="innerException">The underlying cancellation exception, if any.</param>
public sealed class ApiClientCancelledException(string message, Exception? innerException = null)
    : ApiClientException(message, innerException)
{
    /// <summary>
    /// Gets the stable identifier for this error category, used by clients and log
    /// pipelines to correlate caller-initiated cancellations across the API surface.
    /// Always returns <c>"API_CLIENT_CANCELLED"</c>.
    /// </summary>
    public override string ErrorCode => "API_CLIENT_CANCELLED";
}
