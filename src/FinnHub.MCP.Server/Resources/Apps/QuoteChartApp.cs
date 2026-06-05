// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace FinnHub.MCP.Server.Resources.Apps;

/// <summary>
/// SPIKE: self-contained HTML for the <c>get-quote</c> MCP App, served as the
/// <c>ui://finnhub/quote-chart.html</c> resource and rendered by the host in a sandboxed iframe
/// (MCP Apps / SEP-1865).
/// </summary>
/// <remarks>
/// <c>get-quote</c> hits Finnhub's free-tier <c>/quote</c> endpoint, so it returns real data on a
/// free key (unlike <c>get-price-summary</c>, whose <c>/stock/candle</c> source is premium-gated).
/// Dependency-free (inline CSS/JS/SVG, no CDN, no build step). Renders sample data immediately,
/// then best-effort adopts a real <c>get-quote</c> result pushed over the MCP Apps postMessage
/// channel. Full binding uses <c>@modelcontextprotocol/ext-apps</c>' <c>App.ontoolresult</c>;
/// first-class C# support is tracked against csharp-sdk PR #1484.
/// </remarks>
[ExcludeFromCodeCoverage]
internal static class QuoteChartApp
{
    public const string Html =
        """
        <!doctype html>
        <html lang="en">
        <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>Finnhub Quote</title>
        <style>
          :root { color-scheme: light dark; }
          body { font-family: -apple-system, system-ui, "Segoe UI", sans-serif; margin: 0; padding: 16px; }
          .card { border: 1px solid color-mix(in srgb, currentColor 15%, transparent); border-radius: 12px; padding: 16px 18px; max-width: 640px; }
          h1 { font-size: 15px; margin: 0 0 2px; display: flex; align-items: center; gap: 8px; }
          .sym { font-weight: 700; }
          .badge { font-size: 11px; padding: 2px 7px; border-radius: 6px; background: color-mix(in srgb, currentColor 12%, transparent); }
          .sub { font-size: 12px; opacity: .6; margin: 0 0 10px; }
          .big { font-size: 34px; font-weight: 700; letter-spacing: -.5px; margin: 6px 0 2px; }
          .range { position: relative; height: 10px; border-radius: 6px; background: linear-gradient(90deg, #ef4444, #eab308, #22c55e); margin: 34px 4px 6px; }
          .ends { display: flex; justify-content: space-between; font-size: 10px; opacity: .55; margin: 0 2px 4px; }
          .marker { position: absolute; top: -5px; width: 2px; height: 20px; background: currentColor; }
          .marker span { position: absolute; top: -15px; left: 50%; transform: translateX(-50%); font-size: 10px; white-space: nowrap; opacity: .85; }
          .stats { display: grid; grid-template-columns: repeat(3, 1fr); gap: 10px; margin-top: 22px; }
          .stat { padding: 10px 12px; border-radius: 8px; background: color-mix(in srgb, currentColor 6%, transparent); }
          .stat .k { font-size: 10px; opacity: .6; text-transform: uppercase; letter-spacing: .04em; }
          .stat .v { font-size: 17px; font-weight: 600; margin-top: 3px; }
          .pos { color: #16a34a; } .neg { color: #dc2626; }
        </style>
        </head>
        <body>
        <div class="card">
          <h1>Quote <span class="sym" id="sym">AAPL</span> <span class="badge" id="chg">-1.25%</span></h1>
          <p class="sub" id="status">MCP App spike — sample data until the host pushes a get-quote result.</p>
          <div class="big" id="current">307.34</div>
          <div class="range">
            <div class="marker" id="m-pc" style="left:50%"><span>prev</span></div>
            <div class="marker" id="m-open" style="left:60%"><span>open</span></div>
            <div class="marker" id="m-cur" style="left:40%"><span>now</span></div>
          </div>
          <div class="ends"><span id="lo">low</span><span id="hi">high</span></div>
          <div class="stats" id="stats"></div>
        </div>
        <script>
        (function () {
          var d = { symbol: "AAPL", current: 307.34, change: -3.89, percent_change: -1.2499, high: 315.17, low: 307.15, open: 312.86, prev_close: 311.23 };
          function fmt(n) { return (typeof n === "number") ? n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : "—"; }
          function pct(n) { return (typeof n === "number") ? (n >= 0 ? "+" : "") + n.toFixed(2) + "%" : "—"; }
          function render() {
            var sign = (d.change >= 0) ? "pos" : "neg";
            document.getElementById("sym").textContent = d.symbol || "";
            document.getElementById("current").textContent = fmt(d.current);
            var badge = document.getElementById("chg");
            badge.textContent = pct(d.percent_change);
            badge.className = "badge " + sign;
            document.getElementById("lo").textContent = "low " + fmt(d.low);
            document.getElementById("hi").textContent = "high " + fmt(d.high);
            var lo = d.low, hi = d.high, span = (hi - lo) || 1;
            var at = function (v) { return Math.max(0, Math.min(100, ((v - lo) / span) * 100)); };
            document.getElementById("m-pc").style.left = at(d.prev_close) + "%";
            document.getElementById("m-open").style.left = at(d.open) + "%";
            document.getElementById("m-cur").style.left = at(d.current) + "%";
            var tiles = [
              ["Current", fmt(d.current)],
              ["Change", "<span class='" + sign + "'>" + fmt(d.change) + "</span>"],
              ["% Change", "<span class='" + sign + "'>" + pct(d.percent_change) + "</span>"],
              ["Open", fmt(d.open)],
              ["Prev Close", fmt(d.prev_close)],
              ["Day Range", fmt(d.low) + " – " + fmt(d.high)]
            ];
            document.getElementById("stats").innerHTML = tiles.map(function (t) {
              return "<div class='stat'><div class='k'>" + t[0] + "</div><div class='v'>" + t[1] + "</div></div>";
            }).join("");
          }
          render();
          function adopt(sc) {
            if (!sc || typeof sc !== "object") { return false; }
            var x = sc.data || sc;
            if (x && typeof x.current === "number") {
              d = Object.assign({}, d, x);
              document.getElementById("status").textContent = "Live quote" + (x.symbol ? " for " + x.symbol : "") + " from get-quote.";
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
