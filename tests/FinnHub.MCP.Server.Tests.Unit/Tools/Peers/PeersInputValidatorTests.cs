// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Tools.Peers;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Peers;

// Symbol/view validation moved to CommonInputValidatorsTests; this file keeps the Peers-specific grouping rule.
public sealed class PeersInputValidatorTests
{
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
}
