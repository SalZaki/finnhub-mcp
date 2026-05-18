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
/// Upstream rate-limit snapshot attached to a response envelope.
/// </summary>
/// <param name="Remaining">Requests remaining in the current quota window, when reported by the upstream.</param>
/// <param name="ResetAt">When the current quota window resets, when reported by the upstream.</param>
/// <remarks>
/// Both properties are nullable. P1 ships the shape with values always <c>null</c>;
/// the rate-limit tracker populates them in a later phase.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed record RateLimitInfo(
    [property: JsonPropertyName("remaining")] int? Remaining,
    [property: JsonPropertyName("reset_at")] DateTimeOffset? ResetAt);
