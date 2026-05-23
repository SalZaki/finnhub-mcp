# Contributing to finnhub-mcp

## Setup after cloning

```bash
git clone https://github.com/SalZaki/finnhub-mcp.git
cd finnhub-mcp

# Restore .NET tools (currently: Husky.Net for git hooks)
dotnet tool restore

# Install local git hooks (commit-msg validator + future hooks)
dotnet husky install

# Optional: pull dependencies
dotnet restore
```

The `dotnet tool restore` + `dotnet husky install` steps install a `commit-msg`
git hook that validates every commit message against the Conventional Commits
spec — required because `release-please` reads these prefixes to compute
version bumps and CHANGELOG entries.

## Commit message format

Every commit must follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<optional scope>)<optional !>: <subject>
```

**Allowed types** (mirror `.release-please-config.json`):

| Type       | Section in CHANGELOG     | Version bump |
|------------|--------------------------|--------------|
| `feat`     | ✨ Features              | minor        |
| `fix`      | 🐛 Bug Fixes             | patch        |
| `perf`     | ⚡ Performance           | patch        |
| `refactor` | ♻️ Refactoring           | patch        |
| `revert`   | ⏪ Reverts                | patch        |
| `style`    | 💄 Style                 | patch        |
| `build`    | 🔧 Build System          | patch        |
| `ci`       | 👷 CI/CD                 | patch        |
| `chore`    | 🧹 Chores (hidden)       | none         |
| `docs`     | 📚 Documentation (hidden)| none         |
| `test`     | 🧪 Tests (hidden)        | none         |

Append `!` after the type or scope to mark a **BREAKING CHANGE** — release-please
treats this as a major bump (or minor pre-1.0).

**Examples:**

```text
feat(p6c): add get-calendar tool with parameter dispatch
fix(p6a): tolerate mixed types in /stock/metric response
ci: validate before release-please so failed tests do not create orphan releases
refactor!: drop net8 target
chore: install Husky.Net and conventional-commit validator
```

The first line must be 1–72 characters total (after the `:` and space).

## Bypassing the hook (don't, except in genuine emergencies)

`git commit --no-verify` skips the local hook. The server-side
`PR Title` action will still block the PR — that one cannot be bypassed.

## Release flow

Releases are gated through the `release` branch. The path is documented in
`.planning/specs/01-product-surface.md` and the workflow files
`.github/workflows/dotnet.yml` + `release.yml`. Short version:

1. Feature/fix PRs merge to `main` as normal
2. Open a promotion PR from `main` → `release` when ready to ship
3. Merging it triggers `validate` (format + tests), then `release-please`
   opens its own PR against `release`
4. Merging the release-please PR tags the release and triggers the
   6-platform build matrix

## Project conventions

See `CLAUDE.md` for the broader project conventions (project layout,
source-generated JSON, NSubstitute for mocks, no third-party HTTP clients,
etc.).
