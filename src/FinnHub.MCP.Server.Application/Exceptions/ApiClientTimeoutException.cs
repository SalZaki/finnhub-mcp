// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Exception thrown when an API request exceeds the configured timeout duration.
/// </summary>
/// <param name="message">A message describing the timeout condition.</param>
/// <param name="innerException">The underlying timeout exception, if any.</param>
public sealed class ApiClientTimeoutException(string message, Exception? innerException = null)
    : ApiClientException(message, innerException)
{
    public override string ErrorCode => "API_CLIENT_TIMEOUT";
}
