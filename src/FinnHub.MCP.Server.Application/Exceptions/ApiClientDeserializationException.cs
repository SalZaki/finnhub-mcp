// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exceptions;

/// <summary>
/// Exception thrown when deserialization of the API response fails.
/// </summary>
/// <param name="message">A message describing the error.</param>
/// <param name="responseContent">The raw response content, if available.</param>
/// <param name="innerException">The inner exception, if any.</param>
public sealed class ApiClientDeserializationException(
    string message,
    string? responseContent = null,
    Exception? innerException = null)
    : ApiClientException(message, innerException)
{
    public string? ResponseContent { get; } = responseContent;

    public override string ErrorCode => "API_CLIENT_DESERIALIZATION";
}
