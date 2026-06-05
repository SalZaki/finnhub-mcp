// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace FinnHub.MCP.Server.Resources.Apps;

/// <summary>
/// SPIKE: self-contained HTML for the <c>get-price-summary</c> MCP App, served as the
/// <c>ui://finnhub/price-summary-chart.html</c> resource and rendered by the host in a
/// sandboxed iframe (MCP Apps / SEP-1865).
/// </summary>
/// <remarks>
/// Deliberately dependency-free (inline CSS/JS/SVG, no CDN, no build step) so it ships as a plain
/// string resource and survives Native AOT. It renders sample data immediately — proving the host
/// renders the resource end-to-end — and makes a best-effort attempt to adopt a real
/// <c>get-price-summary</c> result pushed over the MCP Apps postMessage channel. Full data binding
/// uses <c>@modelcontextprotocol/ext-apps</c>' <c>App.ontoolresult</c>; first-class C# support is
/// tracked against csharp-sdk PR #1484.
/// </remarks>
[ExcludeFromCodeCoverage]
internal static class PriceSummaryChartApp
{
    public const string Html =
        """
        <!doctype html>
        <html lang="en">
        <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>Finnhub Price Summary</title>
        <style>
          :root { color-scheme: light dark; }
          body { font-family: -apple-system, system-ui, "Segoe UI", sans-serif; margin: 0; padding: 16px; }
          .card { border: 1px solid color-mix(in srgb, currentColor 15%, transparent); border-radius: 12px; padding: 16px 18px; max-width: 640px; }
          h1 { font-size: 15px; margin: 0 0 2px; display: flex; align-items: center; gap: 8px; }
          .badge { font-size: 10px; text-transform: uppercase; letter-spacing: .05em; padding: 2px 6px; border-radius: 6px; background: color-mix(in srgb, currentColor 12%, transparent); }
          .sub { font-size: 12px; opacity: .6; margin: 0 0 8px; }
          .range { position: relative; height: 10px; border-radius: 6px; background: linear-gradient(90deg, #3b82f6, #22c55e); margin: 30px 4px 8px; }
          .marker { position: absolute; top: -5px; width: 2px; height: 20px; background: currentColor; }
          .marker span { position: absolute; top: -15px; left: 50%; transform: translateX(-50%); font-size: 10px; white-space: nowrap; opacity: .8; }
          .stats { display: grid; grid-template-columns: repeat(3, 1fr); gap: 10px; margin-top: 22px; }
          .stat { padding: 10px 12px; border-radius: 8px; background: color-mix(in srgb, currentColor 6%, transparent); }
          .stat .k { font-size: 10px; opacity: .6; text-transform: uppercase; letter-spacing: .04em; }
          .stat .v { font-size: 17px; font-weight: 600; margin-top: 3px; }
          .pos { color: #16a34a; } .neg { color: #dc2626; }
        </style>
        </head>
        <body>
        <div class="card">
          <h1>Price Summary <span class="badge" id="period">30d</span></h1>
          <p class="sub" id="status">MCP App spike — sample data until the host pushes a get-price-summary result.</p>
          <div class="range">
            <div class="marker" style="left:0%"><span>min</span></div>
            <div class="marker" id="m-mean" style="left:50%"><span>mean</span></div>
            <div class="marker" id="m-latest" style="left:70%"><span>latest</span></div>
            <div class="marker" style="left:100%"><span>max</span></div>
          </div>
          <div class="stats" id="stats"></div>
        </div>
        <script>
        (function () {
          var data = { symbol: "AAPL", period: "30d", min: 188.2, max: 214.7, mean: 201.4, latest: 209.8, return_pct: 6.3, vol: 0.21 };
          function fmt(n) { return (typeof n === "number") ? n.toLocaleString(undefined, { maximumFractionDigits: 2 }) : "—"; }
          function pct(n) { return (typeof n === "number") ? (n >= 0 ? "+" : "") + n.toFixed(2) + "%" : "—"; }
          function render() {
            document.getElementById("period").textContent = data.period || "";
            var lo = data.min, hi = data.max, span = (hi - lo) || 1;
            var at = function (v) { return Math.max(0, Math.min(100, ((v - lo) / span) * 100)); };
            document.getElementById("m-mean").style.left = at(data.mean) + "%";
            document.getElementById("m-latest").style.left = at(data.latest) + "%";
            var rc = (data.return_pct >= 0) ? "pos" : "neg";
            var tiles = [
              ["Latest", fmt(data.latest)],
              ["Return", "<span class='" + rc + "'>" + pct(data.return_pct) + "</span>"],
              ["Volatility", fmt(data.vol)],
              ["Min", fmt(data.min)],
              ["Mean", fmt(data.mean)],
              ["Max", fmt(data.max)]
            ];
            document.getElementById("stats").innerHTML = tiles.map(function (t) {
              return "<div class='stat'><div class='k'>" + t[0] + "</div><div class='v'>" + t[1] + "</div></div>";
            }).join("");
          }
          render();
          // Best-effort: adopt a get-price-summary result pushed by the MCP Apps host.
          function adopt(sc) {
            if (!sc || typeof sc !== "object") { return false; }
            var d = sc.data || sc;
            if (d && (typeof d.latest === "number" || typeof d.mean === "number")) {
              data = Object.assign({}, data, d);
              document.getElementById("status").textContent = "Live data" + (d.symbol ? " for " + d.symbol : "") + " from get-price-summary.";
              render();
              return true;
            }
            return false;
          }
          window.addEventListener("message", function (ev) {
            try {
              var m = ev.data; if (!m) { return; }
              var r = m.result || m.params || m;
              if (!adopt(r.structuredContent)) { adopt(r); }
            } catch (e) { /* ignore non-conforming messages */ }
          });
          try { parent.postMessage({ jsonrpc: "2.0", method: "ui/initialize", params: {} }, "*"); } catch (e) { }
        })();
        </script>
        </body>
        </html>
        """;
}
