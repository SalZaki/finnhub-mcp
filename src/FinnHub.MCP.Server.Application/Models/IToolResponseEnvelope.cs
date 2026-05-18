// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Contract for the response envelope returned by every MCP tool.
/// </summary>
/// <typeparam name="T">The domain payload type carried by <see cref="Data"/>.</typeparam>
/// <remarks>
/// Tools return an envelope directly; the tool invocation middleware reads
/// <see cref="ApproxTokens"/> and may rebuild the envelope as a budget-exceeded
/// failure when the serialized response exceeds the per-view ceiling.
/// </remarks>
public interface IToolResponseEnvelope<out T>
{
    /// <summary>
    /// Whether the underlying operation succeeded.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// The domain payload when <see cref="IsSuccess"/> is <c>true</c>; otherwise <c>null</c>.
    /// </summary>
    T? Data { get; }

    /// <summary>
    /// Human-readable failure reason when <see cref="IsSuccess"/> is <c>false</c>.
    /// </summary>
    string? ErrorMessage { get; }

    /// <summary>
    /// Categorised error type when <see cref="IsSuccess"/> is <c>false</c>.
    /// String form of <see cref="ResultErrorType"/>.
    /// </summary>
    string? ErrorType { get; }

    /// <summary>
    /// Echoes the response detail level the tool was asked for.
    /// </summary>
    ToolView View { get; }

    /// <summary>
    /// Server-suggested follow-up tool calls, in priority order. Empty when no follow-ups apply.
    /// </summary>
    IReadOnlyList<NextAction> NextActions { get; }

    /// <summary>
    /// One- or two-line natural-language summary of the payload, for the consuming model.
    /// </summary>
    string? Explanation { get; }

    /// <summary>
    /// Estimated token cost of the serialized response. Populated by the middleware.
    /// </summary>
    int ApproxTokens { get; }

    /// <summary>
    /// Upstream rate-limit snapshot. Populated by the rate-limit tracker; <c>null</c> in P1.
    /// </summary>
    RateLimitInfo? RateLimit { get; }

    /// <summary>
    /// Source of any sentiment value carried in <see cref="Data"/>.
    /// <c>null</c> for tools that do not surface sentiment.
    /// </summary>
    string? SentimentSource { get; }

    /// <summary>
    /// <c>true</c> when the underlying upstream endpoint required a premium key.
    /// </summary>
    bool Premium { get; }
}
