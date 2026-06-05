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
/// SPIKE: serves the <c>get-quote</c> MCP App UI at <c>ui://finnhub/quote-chart.html</c>.
/// </summary>
/// <remarks>
/// The <c>get-quote</c> tool links to this resource via <c>_meta.ui.resourceUri</c> (set in
/// <c>Program.cs</c>); a host that supports MCP Apps (SEP-1865) fetches it and renders the HTML in
/// a sandboxed iframe. <c>get-quote</c> uses Finnhub's free-tier <c>/quote</c> endpoint, so the
/// chart binds real data on a free key. MIME <c>text/html;profile=mcp-app</c> is the MCP Apps
/// marker. Proof-of-concept; the ergonomic layer is tracked against csharp-sdk PR #1484.
/// </remarks>
[McpServerResourceType]
[ExcludeFromCodeCoverage]
public sealed class QuoteChartResource
{
    /// <summary>
    /// Returns the self-contained quote chart HTML for the MCP Apps host to render.
    /// </summary>
    [McpServerResource(
        UriTemplate = "ui://finnhub/quote-chart.html",
        Name = "quote-chart",
        Title = "Quote Chart",
        MimeType = "text/html;profile=mcp-app")]
    [Description("Interactive quote chart (MCP App) rendered by the host for get-quote results.")]
    public string GetQuoteChart() => QuoteChartApp.Html;
}
