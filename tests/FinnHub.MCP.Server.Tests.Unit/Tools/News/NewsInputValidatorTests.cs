// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.News;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.News;

public sealed class NewsInputValidatorTests
{
    [Theory]
    [InlineData("AAPL", "AAPL")]
    [InlineData("aapl", "AAPL")]
    public void ValidateSymbol_Valid_Normalises(string input, string expected) =>
        Assert.Equal(expected, NewsInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("!!!")]
    public void ValidateSymbol_Invalid_Throws(string? input) =>
        Assert.Throws<ArgumentException>(() => NewsInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("standard", ToolView.Standard)]
    [InlineData("full", ToolView.Full)]
    public void ValidateView_KnownValues_MapToEnum(string? input, ToolView expected) =>
        Assert.Equal(expected, NewsInputValidator.ValidateView(input));

    [Fact]
    public void ValidateView_Unknown_Throws() =>
        Assert.Throws<ArgumentException>(() => NewsInputValidator.ValidateView("nonsense"));
}
