// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Profiles;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Profiles;

public sealed class ProfilesInputValidatorTests
{
    [Theory]
    [InlineData("AAPL", "AAPL")]
    [InlineData("aapl", "AAPL")]
    [InlineData(" AAPL ", "AAPL")]
    [InlineData("BRK.B", "BRK.B")]
    public void ValidateSymbol_Valid_NormalisesToUppercase(string input, string expected) =>
        Assert.Equal(expected, ProfilesInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("!!!")]
    [InlineData("1AAPL")]
    [InlineData("TOOLONGSYMBOLNAME1234567890")]
    public void ValidateSymbol_Invalid_Throws(string? input) =>
        Assert.Throws<ArgumentException>(() => ProfilesInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("", ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("standard", ToolView.Standard)]
    [InlineData("full", ToolView.Full)]
    [InlineData("FULL", ToolView.Full)]
    public void ValidateView_KnownValues_MapToEnum(string? input, ToolView expected) =>
        Assert.Equal(expected, ProfilesInputValidator.ValidateView(input));

    [Fact]
    public void ValidateView_UnknownValue_Throws() =>
        Assert.Throws<ArgumentException>(() => ProfilesInputValidator.ValidateView("nonsense"));
}
