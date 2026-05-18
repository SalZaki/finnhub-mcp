// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Default <see cref="IToolResponseEnvelope{T}"/> implementation.
/// </summary>
/// <typeparam name="T">The domain payload type.</typeparam>
/// <remarks>
/// Construct via <see cref="EnvelopeFactory"/>. Tools must leave
/// <see cref="ApproxTokens"/> at <c>0</c> and <see cref="RateLimit"/> at <c>null</c>;
/// the middleware populates both before the response leaves the server.
/// </remarks>
public sealed record ToolResponseEnvelope<T> : IToolResponseEnvelope<T>
{
    /// <inheritdoc />
    [JsonPropertyName("is_success")]
    public bool IsSuccess { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("data")]
    public T? Data { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("error_type")]
    public string? ErrorType { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("view")]
    public ToolView View { get; init; } = ToolView.Summary;

    /// <inheritdoc />
    [JsonPropertyName("next_actions")]
    public IReadOnlyList<NextAction> NextActions { get; init; } = [];

    /// <inheritdoc />
    [JsonPropertyName("explanation")]
    public string? Explanation { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("approx_tokens")]
    public int ApproxTokens { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("rate_limit")]
    public RateLimitInfo? RateLimit { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("sentiment_source")]
    public string? SentimentSource { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("premium")]
    public bool Premium { get; init; }
}
