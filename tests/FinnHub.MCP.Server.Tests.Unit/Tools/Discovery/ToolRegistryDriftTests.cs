// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using FinnHub.MCP.Server.Tools.Discovery;
using ModelContextProtocol.Server;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Discovery;

/// <summary>
/// Guards parity between the tools actually registered on the server (every
/// <c>[McpServerTool]</c> method on a <c>[McpServerToolType]</c> class in the Server assembly)
/// and the <see cref="ToolCatalog"/> the discovery index ranks over. A tool added to the server
/// without a catalog descriptor would be undiscoverable via <c>search-tools</c> — this fails first.
/// </summary>
public sealed class ToolRegistryDriftTests
{
    private static HashSet<string> RegisteredToolNames() =>
        typeof(SearchToolsTool).Assembly
            .GetTypes()
            .Where(type => type.GetCustomAttribute<McpServerToolTypeAttribute>() is not null)
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            .Select(method => method.GetCustomAttribute<McpServerToolAttribute>())
            .Where(attribute => !string.IsNullOrEmpty(attribute?.Name))
            .Select(attribute => attribute!.Name!)
            .ToHashSet(StringComparer.Ordinal);

    [Fact]
    public void EveryRegisteredTool_HasACatalogDescriptor()
    {
        var registered = RegisteredToolNames();
        Assert.NotEmpty(registered);

        var catalog = ToolCatalog.Descriptors.Select(d => d.Name).ToHashSet(StringComparer.Ordinal);

        var missing = registered.Except(catalog).Order().ToList();

        Assert.True(
            missing.Count == 0,
            $"Tools registered with [McpServerTool] but missing a ToolCatalog descriptor: {string.Join(", ", missing)}");
    }

    [Fact]
    public void EveryCatalogDescriptor_MapsToARegisteredTool()
    {
        var registered = RegisteredToolNames();

        var orphans = ToolCatalog.Descriptors
            .Select(d => d.Name)
            .Where(name => !registered.Contains(name))
            .Order()
            .ToList();

        Assert.True(
            orphans.Count == 0,
            $"ToolCatalog descriptors with no registered tool: {string.Join(", ", orphans)}");
    }
}
