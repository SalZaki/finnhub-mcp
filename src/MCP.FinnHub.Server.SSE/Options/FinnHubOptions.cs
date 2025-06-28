// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MCP.FinnHub.Server.SSE.Options;

[ExcludeFromCodeCoverage]
public sealed class FinnHubOptions
{
    [Required]
    public string ApiKey { get; init; } = string.Empty;

    [Required]
    public string BaseUrl { get; init; } = string.Empty;

    [Range(1, 60)]
    public int TimeoutSeconds { get; init; } = 10;

    public List<FinnHubEndPoint> EndPoints { get; init; } = [];
}
