// Smoke-test the npm packages BEFORE publishing: pack each and assert the
// critical files are actually inside the tarball, and that versions are in
// lockstep. Catches the class of bug where a file is silently excluded (e.g. a
// .gitignore `bin/` rule swallowing bin/launcher.js) or a wrapper/platform
// version skew — both of which would otherwise only surface as a broken install.
//
// Usage: node npm/build/smoke-pack.mjs <wrapperDir> [packagesDir]
//   wrapperDir  : the wrapper package dir (npm/finnhub-mcp)
//   packagesDir : dir of generated platform packages (CI: dist/packages) — optional

import { execFileSync } from "node:child_process";
import fs from "node:fs";
import path from "node:path";

let failed = false;
const fail = (m) => { console.error(`✗ ${m}`); failed = true; };
const ok = (m) => console.log(`✓ ${m}`);

function packedFiles(dir) {
  const out = execFileSync("npm", ["pack", "--dry-run", "--json"], { cwd: dir, encoding: "utf8" });
  const meta = JSON.parse(out);
  // npm <= 11 emits an array of package objects; npm >= 12 emits an object keyed
  // by package name. Normalise both. npm 12.0.0 (2026-07-08) made this change and
  // the release job installs `npm@latest`, so v1.21.2 packed fine but every file
  // read as missing — `meta[0]` is undefined on an object. Don't drop either branch.
  const entries = Array.isArray(meta) ? meta : Object.values(meta);
  const entry = entries[0];
  if (!entry?.files) {
    // Fail loudly rather than returning [] — an unparsed shape previously looked
    // identical to "the tarball is empty", which sent the last investigation at
    // the packages instead of at the parser.
    const v = execFileSync("npm", ["--version"], { encoding: "utf8" }).trim();
    throw new Error(
      `cannot read the packed file list from \`npm pack --json\` in ${dir}: ` +
      `npm ${v} returned an unrecognised shape: ${out.slice(0, 200)}`,
    );
  }
  return entry.files.map((f) => f.path);
}

function assertFiles(label, dir, required) {
  const files = packedFiles(dir);
  const missing = required.filter((r) => !files.includes(r));
  if (missing.length) {
    fail(`${label}: tarball missing ${missing.join(", ")}`);
    console.error(`  packed: ${files.join(", ") || "(none)"}`);
  } else {
    ok(`${label}: ${required.join(", ")} present`);
  }
}

const wrapperDir = process.argv[2] ?? path.resolve("npm/finnhub-mcp");
const packagesDir = process.argv[3];

// 1) Wrapper must ship the launcher + postinstall + checksums (the gitignore-bug class).
assertFiles("wrapper finnhub-mcp", wrapperDir, [
  "package.json",
  "bin/launcher.js",
  "scripts/install.js",
  "scripts/checksums.json",
]);

// 2) Wrapper version must match every optionalDependency version (skew = broken install).
const wrapper = JSON.parse(fs.readFileSync(path.join(wrapperDir, "package.json"), "utf8"));
const skew = Object.entries(wrapper.optionalDependencies ?? {})
  .filter(([, v]) => v !== wrapper.version)
  .map(([n, v]) => `${n}@${v}`);
if (skew.length) fail(`version skew: wrapper@${wrapper.version} but ${skew.join(", ")}`);
else ok(`version lockstep: optionalDependencies all == ${wrapper.version}`);

// 3) Each generated platform package must ship its binary + appsettings.json.
if (packagesDir && fs.existsSync(packagesDir)) {
  for (const name of fs.readdirSync(packagesDir)) {
    const dir = path.join(packagesDir, name);
    if (!fs.statSync(dir).isDirectory()) continue;
    const exe = name.includes("win32-") ? "bin/finnhub-mcp.exe" : "bin/finnhub-mcp";
    // README.md included so a platform package can never regress to npm's
    // "ERROR: No README data found!" page (#531).
    assertFiles(`platform ${name}`, dir, ["package.json", "README.md", exe, "bin/appsettings.json"]);
  }
} else {
  console.log("• no packages dir given — skipping platform-package checks");
}

if (failed) { console.error("\nsmoke-pack FAILED"); process.exit(1); }
console.log("\nsmoke-pack OK");
