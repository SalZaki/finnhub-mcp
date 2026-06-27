# finnhub-mcp

A [Model Context Protocol](https://modelcontextprotocol.io) (MCP) server for [Finnhub](https://finnhub.io) financial data — quotes, company profiles, financials, news, peers, recommendations, insider activity, and more, as token-efficient aggregation-first tools.

This npm package is a thin launcher around the native, self-contained `finnhub-mcp` server. **No .NET SDK required** — the matching prebuilt binary for your platform is installed automatically.

## Use with an MCP client

Add it to your MCP client config. The server reads your Finnhub API key from the `FINNHUB_API_KEY` environment variable.

**Claude Desktop** (`claude_desktop_config.json`) / **Claude Code** / **Cursor**:

```json
{
  "mcpServers": {
    "finnhub": {
      "command": "npx",
      "args": ["-y", "finnhub-mcp", "--stdio"],
      "env": { "FINNHUB_API_KEY": "your_finnhub_api_key" }
    }
  }
}
```

Or run it directly:

```bash
export FINNHUB_API_KEY=your_finnhub_api_key
npx -y finnhub-mcp --stdio          # stdio transport (for MCP clients)
npx -y finnhub-mcp                  # HTTP transport on :8080
```

Get a free API key at [finnhub.io/register](https://finnhub.io/register).

## Supported platforms

macOS (arm64, x64), Linux (x64, arm64), Windows (x64, arm64).

The correct native binary is delivered as a platform-specific optional dependency
(`@salzaki/finnhub-mcp-<os>-<arch>`); installs pull only the one that matches your
machine. If optional dependencies are unavailable (e.g. `--no-optional`), a
postinstall step downloads and SHA-256-verifies the binary from the matching
GitHub release. To point at a local build, set `FINNHUB_MCP_BINARY`.

## Links

- Source, docs, and issues: <https://github.com/SalZaki/finnhub-mcp>
- License: MIT
