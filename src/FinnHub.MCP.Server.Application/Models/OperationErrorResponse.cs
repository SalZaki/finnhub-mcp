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
/// Represents a response for a failed operation that wasn't caused by input validation.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class OperationErrorResponse : ErrorResponse
{
    /// <inheritdoc/>
    public override string Title => "OperationError";

    /// <summary>
    /// Gets the name of the failed operation.
    /// </summary>
    [JsonPropertyName("operation")]
    public required string Operation { get; init; }
}
