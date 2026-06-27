// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools;

/// <summary>
/// The shared exception-propagation contract for the canonical async tools: a cancellation and an
/// unexpected exception both propagate (the tool does not swallow them), and a failure
/// <c>Result</c> yields no next_actions. Each tool supplies the three seams below; subclasses keep
/// their own SUT and service mock, so only the propagation triple is shared.
/// </summary>
/// <typeparam name="TResponse">The tool's response payload type.</typeparam>
public abstract class ToolExceptionPropagationTests<TResponse>
    where TResponse : class
{
    /// <summary>Arranges the service mock to throw <paramref name="ex"/> from the tool's service call.</summary>
    protected abstract void SetupServiceThrows(Exception ex);

    /// <summary>Arranges the service mock to return a failure <c>Result</c>.</summary>
    protected abstract void SetupServiceFailureResult();

    /// <summary>Invokes the tool's primary method with a valid input.</summary>
    protected abstract Task<ToolResponseEnvelope<TResponse>> ActAsync();

    [Fact]
    public async Task Cancelled_PropagatesOperationCanceled()
    {
        this.SetupServiceThrows(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() => this.ActAsync());
    }

    [Fact]
    public virtual async Task UnexpectedFailure_PropagatesException()
    {
        this.SetupServiceThrows(new InvalidOperationException("downstream broke"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => this.ActAsync());
    }

    [Fact]
    public async Task FailureResult_ReturnsEmptyNextActions()
    {
        this.SetupServiceFailureResult();

        var envelope = await this.ActAsync();

        Assert.Empty(envelope.NextActions);
    }
}
