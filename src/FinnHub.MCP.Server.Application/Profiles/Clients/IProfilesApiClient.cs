// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;

namespace FinnHub.MCP.Server.Application.Profiles.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/profile2</c> endpoint.
/// </summary>
public interface IProfilesApiClient
{
    /// <summary>Fetches the company profile snapshot for a symbol.</summary>
    Task<GetCompanyProfileResponse> GetProfileAsync(GetCompanyProfileQuery query, CancellationToken cancellationToken);
}
