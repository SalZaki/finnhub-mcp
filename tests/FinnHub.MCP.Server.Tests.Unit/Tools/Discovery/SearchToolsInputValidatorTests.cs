// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Discovery;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Discovery;

public sealed class SearchToolsInputValidatorTests
{
    [Theory]
    [InlineData("is apple stock up this week", "is apple stock up this week")]
    [InlineData("  upcoming earnings  ", "upcoming earnings")]
    public void ValidateIntent_Valid_Normalises(string input, string expected) =>
        Assert.Equal(expected, SearchToolsInputValidator.ValidateIntent(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateIntent_EmptyOrWhitespace_Throws(string? input) =>
        Assert.Throws<ArgumentException>(() => SearchToolsInputValidator.ValidateIntent(input));

    [Fact]
    public void ValidateIntent_Over200Chars_Throws()
    {
        var tooLong = new string('a', 201);

        Assert.Throws<ArgumentException>(() => SearchToolsInputValidator.ValidateIntent(tooLong));
    }

    [Fact]
    public void ValidateIntent_Exactly200Chars_Allowed()
    {
        var atLimit = new string('a', 200);

        Assert.Equal(atLimit, SearchToolsInputValidator.ValidateIntent(atLimit));
    }

    [Theory]
    [InlineData("price <script>")]
    [InlineData("earnings;DROP TABLE")]
    [InlineData("emoji 🚀 intent")]
    public void ValidateIntent_InvalidCharacters_Throws(string input) =>
        Assert.Throws<ArgumentException>(() => SearchToolsInputValidator.ValidateIntent(input));

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("standard", ToolView.Standard)]
    [InlineData("FULL", ToolView.Full)]
    public void ValidateView_KnownValues_MapToEnum(string? input, ToolView expected) =>
        Assert.Equal(expected, SearchToolsInputValidator.ValidateView(input));

    [Fact]
    public void ValidateView_Unknown_Throws() =>
        Assert.Throws<ArgumentException>(() => SearchToolsInputValidator.ValidateView("brief"));
}
