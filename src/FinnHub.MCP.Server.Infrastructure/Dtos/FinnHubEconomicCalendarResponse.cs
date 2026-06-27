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
/// Wire DTO for the Finnhub <c>/calendar/economic</c> endpoint envelope.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubEconomicCalendarResponse
{
    /// <summary>The list of economic entries; may be empty when no events match the window.</summary>
    [JsonPropertyName("economicCalendar")]
    public IReadOnlyList<FinnHubEconomicEntry>? EconomicCalendar { get; init; }
}
