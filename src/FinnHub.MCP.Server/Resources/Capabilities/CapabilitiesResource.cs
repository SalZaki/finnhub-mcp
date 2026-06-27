// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Net.Mime;
using FinnHub.MCP.Server.Application.Discovery;

namespace FinnHub.MCP.Server.Resources.Capabilities;

/// <summary>
/// MCP resource that exposes the full machine-readable catalog of registered tools.
/// </summary>
/// <remarks>
/// Reads from the same <see cref="IToolRegistry"/> the <c>search-tools</c> meta-tool ranks over,
/// so the catalog and the ranker can never disagree. A client can enumerate the whole tool surface
/// — name, title, description, category, premium flag and example intents — from one read instead
/// of issuing repeated <c>search-tools</c> calls.
/// </remarks>
[McpServerResourceType]
public sealed class CapabilitiesResource(IToolRegistry registry)
{
    /// <summary>
    /// Returns the tool catalog serialized as JSON for the
    /// <c>finnhub://resources/capabilities</c> resource.
    /// </summary>
    /// <remarks>
    /// The MCP SDK accepts only a fixed set of return types for resource handlers
    /// (<c>ResourceContents</c>, <c>string</c>, <c>IEnumerable&lt;...&gt;</c>); a
    /// <see cref="string"/> is wrapped in a <c>TextResourceContents</c> using the
    /// declared <see cref="MediaTypeNames.Application.Json"/> mime type.
    /// </remarks>
    [McpServerResource(
        UriTemplate = "finnhub://resources/capabilities",
        Name = "get-capabilities",
        Title = "Capabilities",
        MimeType = MediaTypeNames.Application.Json)]
    [Description("Lists every Finnhub MCP tool with its title, description, category, premium flag, and example intents.")]
    public string GetCapabilities()
    {
        var payload = new CapabilitiesPayload
        {
            Tools =
            [
                .. registry.Descriptors.Select(descriptor => new CapabilityEntry
                {
                    Name = descriptor.Name,
                    Title = descriptor.Title,
                    Description = descriptor.Description,
                    Category = descriptor.Category,
                    Examples = descriptor.Examples,
                    Premium = descriptor.Premium
                })
            ]
        };

        return JsonSerializer.Serialize(payload, ResourceJsonContext.Default.CapabilitiesPayload);
    }
}
