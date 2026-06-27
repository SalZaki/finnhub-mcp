// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;

/// <summary>
/// Query parameters for the <c>get-insider-signal</c> tool — a Finnhub
/// <c>/stock/insider-transactions</c> lookup that aggregates the trailing-30-day
/// insider net buy/sell signal and surfaces the most active named insiders.
/// </summary>
public sealed class GetInsiderSignalQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the signal is requested for.</summary>
    public required string Symbol { get; init; }

    /// <summary>
    /// Inclusive start of the lookup window (UTC calendar day). Defaults to today − 30 days
    /// at the tool boundary; the service does not infer it.
    /// </summary>
    public required DateOnly From { get; init; }

    /// <summary>Inclusive end of the lookup window (UTC calendar day). Defaults to today.</summary>
    public required DateOnly To { get; init; }
}
