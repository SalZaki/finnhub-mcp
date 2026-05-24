// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;
using FinnHub.MCP.Server.Middleware;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Middleware;

public sealed class ToolBudgetTests
{
    [Theory]
    [InlineData(ToolView.Summary, Constants.Envelope.SummaryTokenCeiling)]
    [InlineData(ToolView.Standard, Constants.Envelope.StandardTokenCeiling)]
    public void CeilingFor_KnownViews_ReturnsConstants(ToolView view, int expected) =>
        Assert.Equal(expected, ToolBudget.CeilingFor(view));

    [Fact]
    public void CeilingFor_FullView_ReturnsIntMaxValue() =>
        Assert.Equal(int.MaxValue, ToolBudget.CeilingFor(ToolView.Full));

    [Fact]
    public void CeilingFor_UnknownEnumValue_FallsBackToSummary() =>
        // Defends the `_ =>` default arm against future ToolView additions where the
        // enum was extended but ToolBudget wasn't updated to match.
        Assert.Equal(Constants.Envelope.SummaryTokenCeiling, ToolBudget.CeilingFor((ToolView)999));
}
