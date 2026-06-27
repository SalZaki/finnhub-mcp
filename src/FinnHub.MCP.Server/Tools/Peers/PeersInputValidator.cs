// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;

namespace FinnHub.MCP.Server.Tools.Peers;

/// <summary>
/// Validation helpers for <c>get-peers</c> tool parameters.
/// </summary>
internal static class PeersInputValidator
{
    public static PeersGrouping ValidateGrouping(string? grouping)
    {
        if (string.IsNullOrWhiteSpace(grouping))
        {
            return PeersGrouping.Industry;
        }

        return grouping.Trim().ToLowerInvariant() switch
        {
            "industry" => PeersGrouping.Industry,
            "subindustry" => PeersGrouping.SubIndustry,
            "sector" => PeersGrouping.Sector,
            _ => throw new ArgumentException(
                "Grouping must be one of: industry, subindustry, sector.",
                nameof(grouping))
        };
    }
}
