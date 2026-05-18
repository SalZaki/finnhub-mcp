// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Tokens;

/// <summary>
/// Estimates the token cost of a serialized response payload.
/// </summary>
/// <remarks>
/// Single-method boundary so the implementation is trivially swappable. The default
/// implementation (<see cref="CharCountTokenEstimator"/>) is a fast heuristic; a future
/// swap to a BPE-correct tokenizer such as <c>Microsoft.ML.Tokenizers</c> changes one
/// registration without touching call sites.
/// </remarks>
public interface ITokenEstimator
{
    /// <summary>
    /// Returns the estimated token count for the supplied text.
    /// </summary>
    /// <param name="text">The serialized payload to measure.</param>
    /// <returns>A non-negative token count.</returns>
    int EstimateTokens(ReadOnlySpan<char> text);

    /// <summary>
    /// Convenience overload over <see cref="EstimateTokens(ReadOnlySpan{char})"/>.
    /// </summary>
    /// <param name="text">The serialized payload to measure. <c>null</c> is treated as the empty string.</param>
    int EstimateTokens(string? text);
}
