using System.ComponentModel.DataAnnotations;

namespace MCP.FinnHub.Server.SSE.Options;

public sealed class FinnHubOptions
{
    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    [Range(1, 60)]
    public int TimeoutSeconds { get; set; } = 10;

    public List<FinnHubEndpoint> Endpoints { get; set; } = [];
}
