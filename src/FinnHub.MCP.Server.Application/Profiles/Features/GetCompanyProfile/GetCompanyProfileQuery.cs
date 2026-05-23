// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;

/// <summary>
/// Query parameters for the <c>get-company-profile</c> tool — a Finnhub
/// <c>/stock/profile2</c> lookup that returns the company snapshot for a ticker.
/// </summary>
public sealed class GetCompanyProfileQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the profile is requested for.</summary>
    public required string Symbol { get; init; }

    /// <summary>Whether to include the optional cosmetic fields (logo, phone, weburl). False on the default summary view.</summary>
    public bool IncludeCosmeticFields { get; init; }
}
