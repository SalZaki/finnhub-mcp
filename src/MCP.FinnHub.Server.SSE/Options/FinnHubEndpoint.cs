using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MCP.FinnHub.Server.SSE.Options;

[ExcludeFromCodeCoverage]
public sealed class FinnHubEndpoint
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string Description { get; set; } = string.Empty;
}
