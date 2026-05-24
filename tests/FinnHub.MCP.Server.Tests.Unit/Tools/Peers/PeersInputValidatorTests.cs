// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Tools.Peers;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Peers;

public sealed class PeersInputValidatorTests
{
    [Theory]
    [InlineData("AAPL", "AAPL")]
    [InlineData("aapl", "AAPL")]
    [InlineData(" AAPL ", "AAPL")]
    public void ValidateSymbol_Valid_Normalises(string input, string expected) =>
        Assert.Equal(expected, PeersInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("!!!")]
    [InlineData("1AAPL")]
    public void ValidateSymbol_Invalid_Throws(string? input) =>
        Assert.Throws<ArgumentException>(() => PeersInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null, PeersGrouping.Industry)]
    [InlineData("", PeersGrouping.Industry)]
    [InlineData("industry", PeersGrouping.Industry)]
    [InlineData("INDUSTRY", PeersGrouping.Industry)]
    [InlineData("subindustry", PeersGrouping.SubIndustry)]
    [InlineData("sector", PeersGrouping.Sector)]
    public void ValidateGrouping_KnownValues_MapToEnum(string? input, PeersGrouping expected) =>
        Assert.Equal(expected, PeersInputValidator.ValidateGrouping(input));

    [Fact]
    public void ValidateGrouping_Unknown_Throws() =>
        Assert.Throws<ArgumentException>(() => PeersInputValidator.ValidateGrouping("nonsense"));

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("standard", ToolView.Standard)]
    [InlineData("full", ToolView.Full)]
    public void ValidateView_KnownValues_MapToEnum(string? input, ToolView expected) =>
        Assert.Equal(expected, PeersInputValidator.ValidateView(input));

    [Fact]
    public void ValidateView_Unknown_Throws() =>
        Assert.Throws<ArgumentException>(() => PeersInputValidator.ValidateView("nonsense"));
}
