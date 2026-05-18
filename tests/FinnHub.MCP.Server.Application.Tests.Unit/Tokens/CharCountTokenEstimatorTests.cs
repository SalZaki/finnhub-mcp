// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Tokens;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Tokens;

public sealed class CharCountTokenEstimatorTests
{
    private readonly CharCountTokenEstimator _estimator = new();

    [Theory]
    [InlineData("", 0)]
    [InlineData("a", 1)]
    [InlineData("ab", 1)]
    [InlineData("abc", 1)]
    [InlineData("abcd", 1)]
    [InlineData("abcde", 2)]
    [InlineData("abcdefgh", 2)]
    public void EstimateTokens_ShortStrings_UsesCeilingDivision(string input, int expected)
    {
        Assert.Equal(expected, this._estimator.EstimateTokens(input));
    }

    [Fact]
    public void EstimateTokens_NullString_ReturnsZero()
    {
        Assert.Equal(0, this._estimator.EstimateTokens((string?)null));
    }

    [Fact]
    public void EstimateTokens_VeryLargeInput_DoesNotOverflow()
    {
        var large = new string('x', 1_048_576);

        var tokens = this._estimator.EstimateTokens(large);

        Assert.Equal(262_144, tokens);
        Assert.True(tokens > 0 && tokens < int.MaxValue);
    }

    [Fact]
    public void EstimateTokens_SpanAndStringOverloads_AgreeOnSameInput()
    {
        const string Payload = "the quick brown fox jumps over the lazy dog";

        Assert.Equal(
            this._estimator.EstimateTokens(Payload),
            this._estimator.EstimateTokens(Payload.AsSpan()));
    }
}
