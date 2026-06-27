#!/usr/bin/env node
"use strict";

// Thin launcher for the finnhub-mcp native server.
//
// Resolves the platform-specific self-contained binary — shipped as an optional
// dependency (@salzaki/finnhub-mcp-<os>-<arch>), with a postinstall download
// fallback for the pruned-optional-dependency case — and execs it, forwarding
// argv, stdio, and the full environment (incl. FINNHUB_API_KEY). So
// `npx -y finnhub-mcp --stdio` (or a `command: "npx", args: ["-y","finnhub-mcp"]`
// MCP client config) runs the real MCP server over stdio.

const { spawnSync } = require("node:child_process");
const path = require("node:path");
const fs = require("node:fs");

const SCOPE = "@salzaki";
const PKG = "finnhub-mcp";

function platformKey() {
  // e.g. darwin-arm64, darwin-x64, linux-x64, linux-arm64, win32-x64, win32-arm64
  return `${process.platform}-${process.arch}`;
}

function exeName() {
  return process.platform === "win32" ? "finnhub-mcp.exe" : "finnhub-mcp";
}

function fail(message) {
  process.stderr.write(`[finnhub-mcp] ${message}\n`);
  process.exit(1);
}

function resolveBinary() {
  // 1) Explicit override — local builds / debugging.
  const override = process.env.FINNHUB_MCP_BINARY;
  if (override) {
    if (fs.existsSync(override)) return override;
    fail(`FINNHUB_MCP_BINARY is set but does not exist: ${override}`);
  }

  // 2) Matching platform package (optionalDependencies — the normal path).
  const pkgName = `${SCOPE}/${PKG}-${platformKey()}`;
  try {
    const manifest = require.resolve(`${pkgName}/package.json`);
    const candidate = path.join(path.dirname(manifest), "bin", exeName());
    if (fs.existsSync(candidate)) return candidate;
  } catch {
    // Not installed: pruned optional dep, --no-optional, or a node_modules
    // copied across architectures. Fall through to the vendored fallback.
  }

  // 3) Postinstall download fallback, vendored next to this launcher.
  const vendored = path.join(__dirname, "..", "vendor", exeName());
  if (fs.existsSync(vendored)) return vendored;

  return null;
}

const binary = resolveBinary();
if (!binary) {
  fail(
    `no native binary found for ${platformKey()}.\n` +
      `  Try one of:\n` +
      `    - reinstall WITHOUT --ignore-scripts (so the platform package or the download fallback runs)\n` +
      `    - set FINNHUB_MCP_BINARY to a finnhub-mcp executable\n` +
      `  Supported platforms: darwin-(arm64|x64), linux-(x64|arm64), win32-(x64|arm64).`
  );
}

const result = spawnSync(binary, process.argv.slice(2), {
  stdio: "inherit",
  env: process.env,
  windowsHide: true,
});

if (result.error) {
  fail(`failed to launch ${binary}: ${result.error.message}`);
}

// Terminated by a signal -> non-zero exit; otherwise mirror the child's code.
process.exit(result.signal ? 1 : result.status === null ? 1 : result.status);
