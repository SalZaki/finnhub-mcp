// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Wire DTO for the Finnhub <c>/calendar/ipo</c> endpoint envelope.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubIpoCalendarResponse
{
    /// <summary>The list of IPO entries; may be empty when no listings match the window.</summary>
    [JsonPropertyName("ipoCalendar")]
    public IReadOnlyList<FinnHubIpoEntry>? IpoCalendar { get; init; }
}
