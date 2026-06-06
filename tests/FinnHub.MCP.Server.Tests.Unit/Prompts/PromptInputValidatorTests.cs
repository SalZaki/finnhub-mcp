// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Prompts;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Prompts;

public sealed class PromptInputValidatorTests
{
    [Theory]
    [InlineData("aapl", "AAPL")]
    [InlineData("  msft  ", "MSFT")]
    [InlineData("BRK.A", "BRK.A")]
    [InlineData("RDS-A", "RDS-A")]
    public void ValidateSymbol_Valid_NormalisesToUppercase(string input, string expected)
    {
        Assert.Equal(expected, PromptInputValidator.ValidateSymbol(input));
    }

    [Fact]
    public void ValidateSymbol_MaxLength32_IsAccepted()
    {
        var symbol = new string('A', 32);

        Assert.Equal(symbol, PromptInputValidator.ValidateSymbol(symbol));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateSymbol_Empty_Throws(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => PromptInputValidator.ValidateSymbol(symbol));
    }

    [Theory]
    [InlineData("AAPL MSFT")] // whitespace
    [InlineData("A/B")]       // slash
    [InlineData("A!B")]       // punctuation
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567")] // 33 chars — exceeds the 32-char bound
    public void ValidateSymbol_Invalid_Throws(string symbol)
    {
        Assert.Throws<ArgumentException>(() => PromptInputValidator.ValidateSymbol(symbol));
    }
}
