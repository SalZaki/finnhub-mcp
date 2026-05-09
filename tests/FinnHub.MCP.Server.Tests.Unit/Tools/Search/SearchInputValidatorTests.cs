// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Tools.Search;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Search;

public class SearchInputValidatorTests
{
    [Theory]
    [InlineData("AAPL", false)]
    [InlineData("X", false)]
    [InlineData("X-NYSE_2024", false)]
    [InlineData("  XOM  ", false)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("<tag>", true)]
    [InlineData("<script>", true)]
    [InlineData(null, true)]
    public void ValidateQuery_HandlesVariousInputs(string? input, bool shouldThrow)
    {
        if (shouldThrow)
        {
            Assert.Throws<ArgumentException>(() => SearchInputValidator.ValidateQuery(input));
        }
        else
        {
            var result = SearchInputValidator.ValidateQuery(input);
            Assert.Equal(input?.Trim(), result);
        }
    }

    [Theory]
    [InlineData("NASDAQ", false)]
    [InlineData("X-NYSE", false)]
    [InlineData("ABC_DEF", false)]
    [InlineData("", false)]
    [InlineData("nasdaq", false)]
    [InlineData("X#INVALID", true)]
    [InlineData("TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG_TOOLONG", true)]
    public void ValidateExchange_HandlesVariousInputs(string? input, bool shouldThrow)
    {
        if (shouldThrow)
        {
            Assert.Throws<ArgumentException>(() => SearchInputValidator.ValidateExchange(input));
        }
        else
        {
            var result = SearchInputValidator.ValidateExchange(input);
            if (string.IsNullOrWhiteSpace(input))
            {
                Assert.Null(result);
            }
            else
            {
                Assert.Equal(input.Trim().ToUpperInvariant(), result);
            }
        }
    }

    [Theory]
    [InlineData(null, 10, false)]
    [InlineData(1, 1, false)]
    [InlineData(50, 50, false)]
    [InlineData(100, 100, false)]
    [InlineData(0, 0, true)]
    [InlineData(101, 0, true)]
    [InlineData(-1, 0, true)]
    public void ValidateLimit_HandlesVariousInputs(int? input, int expected, bool shouldThrow)
    {
        if (shouldThrow)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SearchInputValidator.ValidateLimit(input));
        }
        else
        {
            Assert.Equal(expected, SearchInputValidator.ValidateLimit(input));
        }
    }
}
