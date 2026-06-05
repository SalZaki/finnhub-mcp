// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FinnHub.MCP.Server.Resources.Apps;

/// <summary>
/// SPIKE: serves the <c>get-price-summary</c> MCP App UI at <c>ui://finnhub/price-summary-chart.html</c>.
/// </summary>
/// <remarks>
/// The <c>get-price-summary</c> tool links to this resource via <c>_meta.ui.resourceUri</c> (set in
/// <c>Program.cs</c>); a host that supports MCP Apps (SEP-1865) fetches this resource and renders the
/// returned HTML in a sandboxed iframe. The MIME type <c>text/html;profile=mcp-app</c> is the
/// MCP Apps marker the host keys off. This is a proof-of-concept to validate the C# wiring end-to-end;
/// the ergonomic layer is tracked against csharp-sdk PR #1484.
/// </remarks>
[McpServerResourceType]
[ExcludeFromCodeCoverage]
public sealed class PriceSummaryChartResource
{
    /// <summary>
    /// Returns the self-contained price-summary chart HTML for the MCP Apps host to render.
    /// </summary>
    [McpServerResource(
        UriTemplate = "ui://finnhub/price-summary-chart.html",
        Name = "price-summary-chart",
        Title = "Price Summary Chart",
        MimeType = "text/html;profile=mcp-app")]
    [Description("Interactive price-summary chart (MCP App) rendered by the host for get-price-summary results.")]
    public string GetPriceSummaryChart() => PriceSummaryChartApp.Html;
}
