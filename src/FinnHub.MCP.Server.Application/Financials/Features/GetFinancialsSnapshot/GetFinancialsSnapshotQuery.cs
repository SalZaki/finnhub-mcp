// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;

/// <summary>
/// Query parameters for the <c>get-financials-snapshot</c> tool — a Finnhub
/// <c>/stock/metric</c> lookup that returns a curated 10-KPI snapshot for a ticker.
/// </summary>
public sealed class GetFinancialsSnapshotQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the snapshot is requested for.</summary>
    public required string Symbol { get; init; }

    /// <summary>Whether the response should carry the raw upstream metric dictionary in addition to the curated KPIs.</summary>
    public bool IncludeRaw { get; init; }
}
