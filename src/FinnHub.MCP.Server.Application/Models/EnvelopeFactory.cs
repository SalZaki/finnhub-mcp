// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Constructs <see cref="ToolResponseEnvelope{T}"/> instances and bridges
/// service-layer <see cref="Result{T}"/> values into the wire envelope shape.
/// </summary>
/// <remarks>
/// Single creation surface for envelopes. Lives on a non-generic static class
/// so the factory members do not violate CA1000 and so tools have one obvious
/// place to look.
/// </remarks>
public static class EnvelopeFactory
{
    /// <summary>
    /// Builds a success envelope wrapping the supplied payload.
    /// </summary>
    public static ToolResponseEnvelope<T> Success<T>(
        T data,
        ToolView view = ToolView.Summary,
        IReadOnlyList<NextAction>? nextActions = null,
        string? explanation = null,
        string? sentimentSource = null,
        bool premium = false) => new()
        {
            IsSuccess = true,
            Data = data,
            View = view,
            NextActions = nextActions ?? [],
            Explanation = explanation,
            SentimentSource = sentimentSource,
            Premium = premium
        };

    /// <summary>
    /// Builds a failure envelope. <see cref="ToolResponseEnvelope{T}.Data"/> stays <c>null</c>.
    /// </summary>
    public static ToolResponseEnvelope<T> Failure<T>(
        string errorMessage,
        ResultErrorType errorType = ResultErrorType.Unknown,
        ToolView view = ToolView.Summary,
        int approxTokens = 0,
        bool premium = false) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorType = errorType.ToString(),
            View = view,
            ApproxTokens = approxTokens,
            Premium = premium
        };

    /// <summary>
    /// Maps a service-layer <see cref="Result{T}"/> into a wire envelope.
    /// </summary>
    /// <typeparam name="T">The payload type carried by both the result and the envelope.</typeparam>
    /// <param name="result">The service-layer result.</param>
    /// <param name="view">The view the tool was asked for.</param>
    /// <param name="nextActions">Server-suggested follow-ups for the success branch.</param>
    /// <param name="explanation">Short summary of the payload for the consuming model.</param>
    /// <param name="sentimentSource">Source label for sentiment values, when applicable.</param>
    /// <param name="premium">Whether the underlying endpoint required a premium key. Auto-derived from a <c>PremiumRequired</c> failure when not explicitly set.</param>
    public static ToolResponseEnvelope<T> FromResult<T>(
        Result<T> result,
        ToolView view = ToolView.Summary,
        IReadOnlyList<NextAction>? nextActions = null,
        string? explanation = null,
        string? sentimentSource = null,
        bool premium = false)
    {
        ArgumentNullException.ThrowIfNull(result);

        var parsedError = ParseErrorType(result.ErrorType);
        var effectivePremium = premium || parsedError == ResultErrorType.PremiumRequired;

        return result.IsSuccess && result.Data is not null
            ? Success(result.Data, view, nextActions, explanation, sentimentSource, effectivePremium)
            : Failure<T>(
                result.ErrorMessage ?? "Operation failed.",
                parsedError,
                view,
                premium: effectivePremium);
    }

    private static ResultErrorType ParseErrorType(string? errorType) =>
        Enum.TryParse<ResultErrorType>(errorType, ignoreCase: false, out var parsed)
            ? parsed
            : ResultErrorType.Unknown;
}
