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
  return (meta[0]?.files ?? []).map((f) => f.path);
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
    assertFiles(`platform ${name}`, dir, ["package.json", exe, "bin/appsettings.json"]);
  }
} else {
  console.log("• no packages dir given — skipping platform-package checks");
}

if (failed) { console.error("\nsmoke-pack FAILED"); process.exit(1); }
console.log("\nsmoke-pack OK");
