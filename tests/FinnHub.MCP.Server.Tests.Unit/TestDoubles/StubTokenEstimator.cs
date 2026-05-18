// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Tokens;

namespace FinnHub.MCP.Server.Tests.Unit.TestDoubles;

/// <summary>
/// Returns a fixed token count regardless of input so middleware tests can
/// assert budget behaviour without depending on the char/4 heuristic.
/// </summary>
internal sealed class StubTokenEstimator(int returnValue) : ITokenEstimator
{
    public int EstimateTokens(ReadOnlySpan<char> text) => returnValue;

    public int EstimateTokens(string? text) => returnValue;
}
