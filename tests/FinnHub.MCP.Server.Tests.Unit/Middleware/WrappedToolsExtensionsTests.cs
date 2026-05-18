// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.Application.Tokens;
using FinnHub.MCP.Server.Middleware;
using FinnHub.MCP.Server.Tools.Search;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Middleware;

public class WrappedToolsExtensionsTests
{
    [Fact]
    public void WithWrappedTools_RegistersOneMiddlewareWrapperPerToolMethod()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<ITokenEstimator, CharCountTokenEstimator>();
        services.AddSingleton(Substitute.For<ISearchService>());

        services.AddMcpServer().WithWrappedTools<SearchSymbolTool>();

        using var provider = services.BuildServiceProvider();
        var tools = provider.GetServices<McpServerTool>().ToArray();

        Assert.NotEmpty(tools);
        Assert.All(tools, t => Assert.IsType<ToolInvocationMiddleware>(t));
        Assert.Contains(tools, t => t.ProtocolTool.Name == "search-symbol");
    }
}
