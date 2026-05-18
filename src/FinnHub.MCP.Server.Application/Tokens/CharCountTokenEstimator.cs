// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Tokens;

/// <summary>
/// Heuristic <see cref="ITokenEstimator"/> that approximates the cl100k token count
/// as <c>ceil(length / 4)</c>.
/// </summary>
/// <remarks>
/// Trades accuracy for zero dependencies and AOT-cleanliness. Empirically drifts ±30%
/// on financial-data JSON (lots of numbers and ISO timestamps that don't tokenize 1:4).
/// Benchmarks against a real cl100k tokenizer may motivate swapping to
/// <c>Microsoft.ML.Tokenizers</c>; the <see cref="ITokenEstimator"/> boundary makes
/// the swap a one-file change.
/// </remarks>
public sealed class CharCountTokenEstimator : ITokenEstimator
{
    /// <inheritdoc />
    public int EstimateTokens(ReadOnlySpan<char> text) =>
        text.IsEmpty ? 0 : (text.Length + 3) / 4;

    /// <inheritdoc />
    public int EstimateTokens(string? text) =>
        string.IsNullOrEmpty(text) ? 0 : this.EstimateTokens(text.AsSpan());
}
