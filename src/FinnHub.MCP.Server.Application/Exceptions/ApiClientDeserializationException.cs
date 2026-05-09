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
    /// <summary>
    /// Gets the raw HTTP response body that the deserializer failed to parse, if it
    /// was captured before the exception was thrown. Useful for diagnostics, log
    /// correlation, and reproducing parsing failures against a frozen payload.
    /// May be <c>null</c> when the body could not be read or was deliberately not
    /// captured to avoid logging sensitive data.
    /// </summary>
    public string? ResponseContent { get; } = responseContent;

    /// <summary>
    /// Gets the stable identifier for this error category, indicating that the
    /// upstream response was received but could not be deserialized into the
    /// expected schema. Always returns <c>"API_CLIENT_DESERIALIZATION"</c>.
    /// </summary>
    public override string ErrorCode => "API_CLIENT_DESERIALIZATION";
}
