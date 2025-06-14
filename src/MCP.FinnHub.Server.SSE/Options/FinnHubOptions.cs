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

    public List<FinnHubEndpoint> Endpoints { get; init; } = [];
}
