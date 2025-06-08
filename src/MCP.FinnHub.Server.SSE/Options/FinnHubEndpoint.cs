using System.ComponentModel.DataAnnotations;

namespace MCP.FinnHub.Server.SSE.Options;

public sealed class FinnHubEndpoint
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string Description { get; set; } = string.Empty;
}
