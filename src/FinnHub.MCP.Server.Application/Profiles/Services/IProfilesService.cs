// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;

namespace FinnHub.MCP.Server.Application.Profiles.Services;

/// <summary>Application-level entry point for company profile lookups.</summary>
public interface IProfilesService
{
    /// <summary>Executes a profile lookup and returns a categorised result.</summary>
    Task<Result<GetCompanyProfileResponse>> GetProfileAsync(GetCompanyProfileQuery query, CancellationToken cancellationToken = default);
}
