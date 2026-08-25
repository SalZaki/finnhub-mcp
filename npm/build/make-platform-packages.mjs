// Generates the six platform packages and stamps the wrapper, from the
// self-contained single-file binaries that release.yml already builds.
//
// Single source of truth for the npm platform matrix. Run by the publish-npm CI
// job (see .github/workflows/release.yml) before `npm publish`.
//
// Usage:
//   node npm/build/make-platform-packages.mjs \
//     --version 1.21.0 \
//     --artifacts <dir with <rid>/finnhub-mcp[.exe] + <rid>/appsettings.json> \
//     --out <output dir for platform packages>
//
// Effects:
//   - writes <out>/finnhub-mcp-<os>-<arch>/{package.json, bin/<exe>, bin/appsettings.json}
//   - stamps npm/finnhub-mcp/package.json version + optionalDependencies to <version>
//   - writes npm/finnhub-mcp/scripts/checksums.json { "<os>-<arch>": "<sha256>" }
//   - prints, on stdout, the publish order (platform packages first, then wrapper)

import fs from "node:fs";
import path from "node:path";
import crypto from "node:crypto";
import url from "node:url";

const SCOPE = "@salzaki";
const PKG = "finnhub-mcp";

// .NET RID -> npm { os, cpu }. RID is how release.yml names its artifacts.
const MATRIX = [
  { rid: "osx-arm64", os: "darwin", cpu: "arm64" },
  { rid: "osx-x64", os: "darwin", cpu: "x64" },
  { rid: "linux-x64", os: "linux", cpu: "x64" },
  { rid: "linux-arm64", os: "linux", cpu: "arm64" },
  { rid: "win-x64", os: "win32", cpu: "x64" },
  { rid: "win-arm64", os: "win32", cpu: "arm64" },
];

function arg(name) {
  const i = process.argv.indexOf(`--${name}`);
  if (i === -1 || i === process.argv.length - 1) return undefined;
  return process.argv[i + 1];
}

function die(msg) {
  process.stderr.write(`make-platform-packages: ${msg}\n`);
  process.exit(1);
}

const version = arg("version");
const artifactsDir = arg("artifacts");
const outDir = arg("out");
if (!version) die("--version is required");
if (!artifactsDir) die("--artifacts is required");
if (!outDir) die("--out is required");

const here = path.dirname(url.fileURLToPath(import.meta.url));
const wrapperDir = path.resolve(here, "..", "finnhub-mcp");

const exeFor = (os) => (os === "win32" ? "finnhub-mcp.exe" : "finnhub-mcp");

// npm renders this on the package page. Without it the six generated packages
// display "ERROR: No README data found!" where the description belongs, and npm
// publishes are immutable, so every release without one ships six more such
// pages (#531). npm always includes README.md in the tarball regardless of the
// `files` field below, so the manifest needs no change to carry it.
function readmeFor(key, os, cpu, exe) {
  return [
    `# ${SCOPE}/${PKG}-${key}`,
    "",
    `Platform binary for [\`${PKG}\`](https://www.npmjs.com/package/${PKG}), a`,
    "[Model Context Protocol](https://modelcontextprotocol.io) server exposing",
    "Finnhub's financial-data APIs to MCP-compatible clients.",
    "",
    "## Do not install this package directly",
    "",
    `npm selects it automatically as an optional dependency of \`${PKG}\` when`,
    `installing on ${os}/${cpu}. Install the wrapper instead:`,
    "",
    "```sh",
    `npx ${PKG}`,
    "```",
    "",
    "## Contents",
    "",
    `- \`bin/${exe}\` - self-contained .NET binary for ${key}`,
    "- `bin/appsettings.json` - default configuration",
    "",
    "## Links",
    "",
    "- Source and documentation: <https://github.com/SalZaki/finnhub-mcp>",
    `- Wrapper package: <https://www.npmjs.com/package/${PKG}>`,
    "",
    "Released under the MIT licence.",
    "",
  ].join("\n");
}
const sha256 = (buf) => crypto.createHash("sha256").update(buf).digest("hex");

const checksums = {};
const platformPackages = [];

for (const { rid, os, cpu } of MATRIX) {
  const key = `${os}-${cpu}`;
  const exe = exeFor(os);

  const srcDir = path.join(artifactsDir, rid);
  const srcBin = fs.existsSync(path.join(srcDir, exe))
    ? path.join(srcDir, exe)
    : path.join(srcDir, "finnhub-mcp"); // tolerate win artifact without .exe
  const srcAppsettings = path.join(srcDir, "appsettings.json");
  if (!fs.existsSync(srcBin)) die(`missing binary for ${rid}: ${srcBin}`);
  if (!fs.existsSync(srcAppsettings)) die(`missing appsettings.json for ${rid}: ${srcAppsettings}`);

  const pkgDir = path.join(outDir, `${PKG}-${key}`);
  const binDir = path.join(pkgDir, "bin");
  fs.mkdirSync(binDir, { recursive: true });

  const binBuf = fs.readFileSync(srcBin);
  const destBin = path.join(binDir, exe);
  fs.writeFileSync(destBin, binBuf);
  fs.chmodSync(destBin, 0o755); // exec bit is stripped by upload/download-artifact
  fs.copyFileSync(srcAppsettings, path.join(binDir, "appsettings.json"));

  checksums[key] = sha256(binBuf);

  const manifest = {
    name: `${SCOPE}/${PKG}-${key}`,
    version,
    description: `finnhub-mcp native binary for ${key}.`,
    os: [os],
    cpu: [cpu],
    files: ["bin/"],
    publishConfig: { access: "public", provenance: true },
    license: "MIT",
    repository: { type: "git", url: "git+https://github.com/SalZaki/finnhub-mcp.git" },
    homepage: "https://github.com/SalZaki/finnhub-mcp#readme",
  };
  fs.writeFileSync(path.join(pkgDir, "package.json"), JSON.stringify(manifest, null, 2) + "\n");
  fs.writeFileSync(path.join(pkgDir, "README.md"), readmeFor(key, os, cpu, exe));
  platformPackages.push(pkgDir);
}

// Stamp the wrapper: version + every optionalDependency pinned to this version.
const wrapperManifestPath = path.join(wrapperDir, "package.json");
const wrapper = JSON.parse(fs.readFileSync(wrapperManifestPath, "utf8"));
wrapper.version = version;
wrapper.optionalDependencies = Object.fromEntries(
  MATRIX.map(({ os, cpu }) => [`${SCOPE}/${PKG}-${os}-${cpu}`, version]),
);
fs.writeFileSync(wrapperManifestPath, JSON.stringify(wrapper, null, 2) + "\n");

// Ship the checksum manifest used by the postinstall download fallback.
fs.writeFileSync(
  path.join(wrapperDir, "scripts", "checksums.json"),
  JSON.stringify(checksums, null, 2) + "\n",
);

// Publish order: platform packages first (the wrapper's optionalDependencies
// must already exist), then the wrapper.
process.stdout.write([...platformPackages, wrapperDir].join("\n") + "\n");
process.stderr.write(`make-platform-packages: stamped ${version}, ${platformPackages.length} platform packages.\n`);
