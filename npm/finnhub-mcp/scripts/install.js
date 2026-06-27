#!/usr/bin/env node
"use strict";

// Postinstall FALLBACK only. The normal install path ships the native binary as
// a platform-specific optional dependency (@salzaki/finnhub-mcp-<os>-<arch>) and
// this script does nothing. It engages only when that optional dependency was
// pruned (npm/cli #4828/#8320, --no-optional, cross-arch node_modules): it
// downloads the version-pinned self-contained binary from the matching GitHub
// Release, verifies its SHA-256 against the shipped manifest, and vendors it
// next to the launcher so bin/launcher.js can find it.
//
// It never hard-fails the install: on any problem it warns and exits 0, leaving
// bin/launcher.js to print an actionable error at runtime. (A failed postinstall
// would break `npm install` entirely.)

const fs = require("node:fs");
const path = require("node:path");
const crypto = require("node:crypto");

const SCOPE = "@salzaki";
const PKG = "finnhub-mcp";
const REPO = "SalZaki/finnhub-mcp";

// npm platform key -> .NET RID used in the GitHub Release asset names.
const RID_BY_PLATFORM = {
  "darwin-arm64": "osx-arm64",
  "darwin-x64": "osx-x64",
  "linux-x64": "linux-x64",
  "linux-arm64": "linux-arm64",
  "win32-x64": "win-x64",
  "win32-arm64": "win-arm64",
};

function warn(msg) {
  process.stderr.write(`[finnhub-mcp:install] ${msg}\n`);
}

function exeName() {
  return process.platform === "win32" ? "finnhub-mcp.exe" : "finnhub-mcp";
}

function platformKey() {
  return `${process.platform}-${process.arch}`;
}

// Already have the platform package? Then there's nothing to do.
function platformPackageInstalled() {
  try {
    const manifest = require.resolve(`${SCOPE}/${PKG}-${platformKey()}/package.json`);
    return fs.existsSync(path.join(path.dirname(manifest), "bin", exeName()));
  } catch {
    return false;
  }
}

async function download(url) {
  const res = await fetch(url, {
    redirect: "follow",
    headers: { "User-Agent": `${PKG}-installer` },
  });
  if (!res.ok) throw new Error(`HTTP ${res.status} for ${url}`);
  return Buffer.from(await res.arrayBuffer());
}

async function main() {
  if (platformPackageInstalled()) return; // normal path — nothing to do.

  const key = platformKey();
  const rid = RID_BY_PLATFORM[key];
  if (!rid) {
    warn(`unsupported platform ${key}; no prebuilt binary available.`);
    return;
  }

  const pkg = require(path.join(__dirname, "..", "package.json"));
  const version = pkg.version;
  if (!version || version.startsWith("0.0.0")) {
    warn(`dev/placeholder version (${version}); skipping download fallback. ` +
      `Set FINNHUB_MCP_BINARY or install a published release.`);
    return;
  }

  // Checksums are stamped by CI at publish time (scripts/checksums.json).
  let checksums = {};
  try {
    checksums = require(path.join(__dirname, "checksums.json"));
  } catch {
    /* no manifest shipped */
  }
  const expectedSha = checksums[key];
  if (!expectedSha) {
    warn(`no checksum for ${key} in manifest; refusing unverified download.`);
    return;
  }

  const base = `https://github.com/${REPO}/releases/download/v${version}`;
  const vendorDir = path.join(__dirname, "..", "vendor");

  try {
    warn(`platform package missing; downloading finnhub-mcp ${version} for ${key}...`);
    const binBuf = await download(`${base}/finnhub-mcp-${rid}`);

    const actualSha = crypto.createHash("sha256").update(binBuf).digest("hex");
    if (actualSha !== expectedSha) {
      warn(`checksum mismatch for ${key}: expected ${expectedSha}, got ${actualSha}. Aborting.`);
      return;
    }

    fs.mkdirSync(vendorDir, { recursive: true });
    const binPath = path.join(vendorDir, exeName());
    fs.writeFileSync(binPath, binBuf);
    fs.chmodSync(binPath, 0o755);

    // appsettings.json is a non-optional startup dependency; co-locate it.
    const appsettings = await download(`${base}/appsettings.json`);
    fs.writeFileSync(path.join(vendorDir, "appsettings.json"), appsettings);

    // macOS: strip the quarantine xattr so Gatekeeper does not block the
    // unsigned CLI binary (see spec G9). Best-effort.
    if (process.platform === "darwin") {
      try {
        require("node:child_process").execFileSync("xattr", ["-d", "com.apple.quarantine", binPath], {
          stdio: "ignore",
        });
      } catch {
        /* xattr absent or attribute not set — fine */
      }
    }

    warn(`downloaded and verified finnhub-mcp ${version} for ${key}.`);
  } catch (err) {
    warn(`download fallback failed: ${err.message}. ` +
      `Reinstall without --ignore-scripts, or set FINNHUB_MCP_BINARY.`);
  }
}

main().catch((err) => {
  warn(`unexpected error: ${err && err.message}`);
  // never break the install.
});
