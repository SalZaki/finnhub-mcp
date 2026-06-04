// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Discovery;
using FinnHub.MCP.Server.Resources.Capabilities;
using FinnHub.MCP.Server.Tools.Discovery;
using ModelContextProtocol.Server;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Resources;

public sealed class CapabilitiesResourceTests
{
    private static CapabilitiesResource RealResource() => new(new ToolRegistry(ToolCatalog.Descriptors));

    private static HashSet<string> RegisteredToolNames() =>
        typeof(CapabilitiesResource).Assembly
            .GetTypes()
            .Where(type => type.GetCustomAttribute<McpServerToolTypeAttribute>() is not null)
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            .Select(method => method.GetCustomAttribute<McpServerToolAttribute>())
            .Where(attribute => !string.IsNullOrEmpty(attribute?.Name))
            .Select(attribute => attribute!.Name!)
            .ToHashSet(StringComparer.Ordinal);

    [Fact]
    public void GetCapabilities_EnumeratesEveryRegisteredTool()
    {
        var json = RealResource().GetCapabilities();

        using var document = JsonDocument.Parse(json);
        var names = document.RootElement
            .GetProperty("tools")
            .EnumerateArray()
            .Select(tool => tool.GetProperty("name").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        var missing = RegisteredToolNames().Except(names).Order().ToList();

        Assert.True(
            missing.Count == 0,
            $"Tools registered with [McpServerTool] but missing from the capabilities catalog: {string.Join(", ", missing)}");
    }

    [Fact]
    public void GetCapabilities_TotalCountMatchesToolArrayLength()
    {
        var json = RealResource().GetCapabilities();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal(
            root.GetProperty("tools").GetArrayLength(),
            root.GetProperty("total_count").GetInt32());
    }

    [Fact]
    public void GetCapabilities_ProjectsAllDescriptorFields()
    {
        var registry = Substitute.For<IToolRegistry>();
        IReadOnlyList<ToolDescriptor> descriptors =
        [
            new ToolDescriptor
            {
                Name = "get-quote",
                Title = "Get Quote",
                Description = "real-time price snapshot",
                Category = "Pricing",
                Examples = ["current price right now"],
                Premium = false
            }
        ];
        registry.Descriptors.Returns(descriptors);

        var json = new CapabilitiesResource(registry).GetCapabilities();

        using var document = JsonDocument.Parse(json);
        var tool = document.RootElement.GetProperty("tools")[0];

        Assert.Equal("get-quote", tool.GetProperty("name").GetString());
        Assert.Equal("Get Quote", tool.GetProperty("title").GetString());
        Assert.Equal("real-time price snapshot", tool.GetProperty("description").GetString());
        Assert.Equal("Pricing", tool.GetProperty("category").GetString());
        Assert.False(tool.GetProperty("premium").GetBoolean());
        Assert.Equal("current price right now", tool.GetProperty("examples")[0].GetString());
        Assert.Equal(1, document.RootElement.GetProperty("total_count").GetInt32());
    }
}
