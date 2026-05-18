// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace FinnHub.MCP.Server.Tests.Unit.TestDoubles;

/// <summary>
/// Minimal hand-rolled <see cref="McpServerTool"/> double — the SDK type is abstract
/// with protocol-level construction concerns NSubstitute can't trivially handle.
/// </summary>
internal sealed class FakeMcpServerTool(
    string name,
    Func<RequestContext<CallToolRequestParams>, CancellationToken, ValueTask<CallToolResult>> body)
    : McpServerTool
{
    public int InvocationCount { get; private set; }

    public override Tool ProtocolTool { get; } = new() { Name = name };

    public override IReadOnlyList<object> Metadata { get; } = [];

    public override ValueTask<CallToolResult> InvokeAsync(
        RequestContext<CallToolRequestParams> request,
        CancellationToken cancellationToken = default)
    {
        this.InvocationCount++;
        return body(request, cancellationToken);
    }
}
