// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Nodes;
using FinnHub.MCP.Server.Application.RateLimiting;
using FinnHub.MCP.Server.Application.Tokens;

namespace FinnHub.MCP.Server.Middleware;

/// <summary>
/// Registers MCP tools wrapped by <see cref="ToolInvocationMiddleware"/>.
/// </summary>
/// <remarks>
/// Replaces the SDK's <c>WithTools&lt;T&gt;</c> for tool types that should
/// participate in the envelope-budget contract. See
/// <see cref="WithWrappedTools{TToolType}(IMcpServerBuilder, JsonObject)"/> for details.
/// </remarks>
public static class WrappedToolsExtensions
{
    private const BindingFlags ToolMethodFlags =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    /// <summary>
    /// Registers every <see cref="McpServerToolAttribute"/>-annotated method on
    /// <typeparamref name="TToolType"/> as a budgeted MCP tool wrapped by
    /// <see cref="ToolInvocationMiddleware"/>.
    /// </summary>
    /// <typeparam name="TToolType">The tool class to scan for tool methods.</typeparam>
    /// <param name="builder">The MCP server builder.</param>
    /// <remarks>
    /// Each wrapped tool is registered as a singleton <see cref="McpServerTool"/>
    /// service; the SDK collects every <see cref="McpServerTool"/> registration from
    /// DI into its <c>ToolCollection</c> at startup. Per-invocation target instantiation
    /// is delegated to <see cref="ActivatorUtilities"/> against the request-scoped
    /// service provider exposed on the <c>RequestContext</c>.
    /// </remarks>
    /// <param name="toolMeta">
    /// Optional <c>_meta</c> to attach to every tool on <typeparamref name="TToolType"/>
    /// (seeds <c>Tool.Meta</c>). Used to declare MCP Apps UI metadata such as
    /// <c>{ "ui": { "resourceUri": "ui://..." } }</c>. Cloned per tool so a single
    /// object is not parented to multiple tools.
    /// </param>
    public static IMcpServerBuilder WithWrappedTools<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
    TToolType>(this IMcpServerBuilder builder, JsonObject? toolMeta = null)
        where TToolType : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        var methods = typeof(TToolType)
            .GetMethods(ToolMethodFlags)
            .Where(m => m.GetCustomAttribute<McpServerToolAttribute>() is not null)
            .ToArray();

        foreach (var method in methods)
        {
            var capturedMethod = method;
            var capturedMeta = toolMeta;

            builder.Services.AddSingleton<McpServerTool>(sp =>
            {
                var estimator = sp.GetRequiredService<ITokenEstimator>();
                var rateLimitTracker = sp.GetRequiredService<IRateLimitTracker>();
                var logger = sp.GetRequiredService<ILogger<ToolInvocationMiddleware>>();

                var options = capturedMeta is null
                    ? null
                    : new McpServerToolCreateOptions { Meta = (JsonObject)capturedMeta.DeepClone() };

                var inner = McpServerTool.Create(
                    capturedMethod,
                    static ctx => ActivatorUtilities.CreateInstance(ctx.Services!, typeof(TToolType)),
                    options);

                return new ToolInvocationMiddleware(inner, estimator, rateLimitTracker, logger);
            });
        }

        return builder;
    }
}
