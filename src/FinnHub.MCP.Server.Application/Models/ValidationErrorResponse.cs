// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Represents a response for a validation failure, typically due to invalid input.
/// </summary>
public sealed class ValidationErrorResponse : ErrorResponse
{
    /// <inheritdoc/>
    public override string Title => "ValidationError";

    /// <summary>
    /// Gets the name of the input parameter that failed validation.
    /// </summary>
    [JsonPropertyName("parameter")]
    public required string Parameter { get; init; }
}
