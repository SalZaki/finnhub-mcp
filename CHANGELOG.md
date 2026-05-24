# Changelog

## [1.20.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.19.0...v1.20.0) (2026-05-24)


### ✨ Features

* **p6b:** add get-quote and get-company-profile (P6 Wave B) ([a6cdeba](https://github.com/SalZaki/finnhub-mcp/commit/a6cdebaada632a447543712bda4917a91a5a1649))
* **p6b:** add get-quote and get-company-profile (P6 Wave B) ([d4b0789](https://github.com/SalZaki/finnhub-mcp/commit/d4b0789f4e14252118386b9eb8186a1ca0952999))


### 🐛 Bug Fixes

* **p6a:** tolerate mixed types in /stock/metric response ([2e7214e](https://github.com/SalZaki/finnhub-mcp/commit/2e7214eb742be5c148a82a0f0e1c69e17589c1bc))
* **p6a:** tolerate mixed types in /stock/metric response ([a6bf456](https://github.com/SalZaki/finnhub-mcp/commit/a6bf4563ee2349d5eb40048f9e78cba68693e526))
* **p6:** BaseUrl trailing-slash so relative request URIs hit /api/v1/ ([bf79257](https://github.com/SalZaki/finnhub-mcp/commit/bf792570c8c40cbb3eecffe0818835dd775fffac))
* **p6:** BaseUrl trailing-slash so relative request URIs hit /api/v1/ ([205f1e3](https://github.com/SalZaki/finnhub-mcp/commit/205f1e3bb49a07335202e57d3a3dd78393f5efde))
* **p6:** nullable defensive DTOs + real Finnhub fixtures ([039373a](https://github.com/SalZaki/finnhub-mcp/commit/039373a01e6cc7293ef66c3aea03f2e0d47b9b69))
* **p6:** nullable defensive DTOs + real Finnhub fixtures for client tests ([99f0b5a](https://github.com/SalZaki/finnhub-mcp/commit/99f0b5a23841ccd8dc90c851af279dea744feeb4))


### 👷 CI/CD

* add PR title validator + CONTRIBUTING.md for commitizen flow ([2d5d541](https://github.com/SalZaki/finnhub-mcp/commit/2d5d5414b8b9a3622f917e2f40bd6e7e57ead31f))
* enforce conventional commits via Husky.Net + PR title validator ([b739281](https://github.com/SalZaki/finnhub-mcp/commit/b739281e419c391cc1dd4d05faaa17bd4d79ff23))

## [1.19.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.18.0...v1.19.0) (2026-05-23)


### ✨ Features

* **p6a:** cross-link next_actions across Wave A tools + README ([5538cae](https://github.com/SalZaki/finnhub-mcp/commit/5538caefdd49faa5de9477be18c6e6fc2bc79769))
* **p6a:** cross-link next_actions across Wave A tools and document in README ([dedb7dd](https://github.com/SalZaki/finnhub-mcp/commit/dedb7dd93c4c0ee6b8ccdcb9ceb6aac278c9f290))


### 👷 CI/CD

* gate releases through release branch ([a95f026](https://github.com/SalZaki/finnhub-mcp/commit/a95f026f973727d5d3bbbab3da5aff964293ce28))
* remove dorny/test-reporter — the real source of PR CI flakes ([c630a10](https://github.com/SalZaki/finnhub-mcp/commit/c630a10b4c48a458b2691714d4be944929eaeac6))
* target release branch from release-please ([9fdcc46](https://github.com/SalZaki/finnhub-mcp/commit/9fdcc46b6cd8d8fc5d916967c46296add27898ab))
* target release branch from release-please ([9db7de8](https://github.com/SalZaki/finnhub-mcp/commit/9db7de8b063e065666875d242c80f686a4e3b937))
* validate before release-please so failed tests do not create orphan releases ([618a923](https://github.com/SalZaki/finnhub-mcp/commit/618a9232e831e4f398812ce29a226c4b7265e385))

## [1.18.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.17.0...v1.18.0) (2026-05-23)


### ✨ Features

* **p6a:** add get-news-pulse — completes P6 Wave A ([b295ece](https://github.com/SalZaki/finnhub-mcp/commit/b295ecede1f866724d67b5f9c74e9f36ca9b1352))
* **p6a:** add get-news-pulse tool — sentiment + headlines + week-over-week delta ([5e9fcc0](https://github.com/SalZaki/finnhub-mcp/commit/5e9fcc0a184d975825486cde8508320b1b4ee511))
* **p6a:** add get-price-summary — aggregated price stats ([319bec8](https://github.com/SalZaki/finnhub-mcp/commit/319bec81c6f1c2acf764b53b875bde881a1b92f2))
* **p6a:** add get-price-summary tool — aggregated price stats ([b9165e6](https://github.com/SalZaki/finnhub-mcp/commit/b9165e6b4dae86a67de2dc4760b27bb00df9aea3))

## [1.17.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.16.0...v1.17.0) (2026-05-23)


### ✨ Features

* **p6a:** add get-financials-snapshot — 10 curated KPIs ([9e40b63](https://github.com/SalZaki/finnhub-mcp/commit/9e40b63c2106fc2bfde2adc0e89a2babcf239372))
* **p6a:** add get-financials-snapshot tool — 10 curated KPIs ([8c69f3c](https://github.com/SalZaki/finnhub-mcp/commit/8c69f3c2c9ce8060473fbb26378aa7d0b6e22e9a))
* **p6a:** add get-peers tool — pattern reference for Wave A ([71c8cd6](https://github.com/SalZaki/finnhub-mcp/commit/71c8cd65d3b6d39985df4202d18d95d2080307a8))
* **p6a:** add get-peers tool — peer ticker lookup for a symbol ([1044cda](https://github.com/SalZaki/finnhub-mcp/commit/1044cda2d8a5294eb42aa41795bc25b5a54f44c2))

## [1.16.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.15.0...v1.16.0) (2026-05-23)


### ✨ Features

* gate premium endpoints with typed 403 handling ([eb956f5](https://github.com/SalZaki/finnhub-mcp/commit/eb956f573476e72f77d82ee5bcb0adb38339169c))
* **p5:** premium-endpoint gating via typed 403 handling ([b5f734a](https://github.com/SalZaki/finnhub-mcp/commit/b5f734a3b08422ae16b8a5976d87f18040d60386))

## [1.15.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.14.0...v1.15.0) (2026-05-23)


### ✨ Features

* **P4:** rate-limit awareness on envelope + api-status resource ([730a563](https://github.com/SalZaki/finnhub-mcp/commit/730a563f274eab7866fbb69f534ad254da44072b))
* surface Finnhub rate-limit on envelope and status resource ([ad39c08](https://github.com/SalZaki/finnhub-mcp/commit/ad39c080a63db74308af67cdb195bf40b4f66305))


### 🐛 Bug Fixes

* return JSON string from resource handlers so SDK marshals correctly ([890b205](https://github.com/SalZaki/finnhub-mcp/commit/890b205af30d7b118e355784a82fbac723e90f59))

## [1.14.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.13.1...v1.14.0) (2026-05-20)


### ✨ Features

* add ResolvedSymbol record and ISymbolResolver interface ([9165327](https://github.com/SalZaki/finnhub-mcp/commit/9165327e6897a73f1577ab3222802c9763ad45a9))
* add SymbolResolver with fast-path and cached ambiguous lookup ([b0c18fb](https://github.com/SalZaki/finnhub-mcp/commit/b0c18fbc409aa76601584b2226f567d315f19c7c))
* **P3:** symbol resolver service ([817625a](https://github.com/SalZaki/finnhub-mcp/commit/817625a4df3f3bad91c2dc9d42d1cf567eba90d5))


### ♻️ Refactoring

* route search-symbol next-actions through symbol resolver ([d1be65d](https://github.com/SalZaki/finnhub-mcp/commit/d1be65d4a2741e50565375c22df4ec4079cb4d15))

## [1.13.1](https://github.com/SalZaki/finnhub-mcp/compare/v1.13.0...v1.13.1) (2026-05-20)


### 🐛 Bug Fixes

* skip --urls injection in stdio mode ([75e54c8](https://github.com/SalZaki/finnhub-mcp/commit/75e54c8751df13a0377395953209c984d530db88))
* stop --urls 8080 collision and .env env-var clobbering in stdio dev flow ([b1ace7f](https://github.com/SalZaki/finnhub-mcp/commit/b1ace7fac7b2a0418af7069ad21d1489265e2401))
* stop .env from clobbering launcher-supplied env vars ([5f9699d](https://github.com/SalZaki/finnhub-mcp/commit/5f9699de8e01136ed8395af2e09dd2d5df2af2ca))

## [1.13.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.12.0...v1.13.0) (2026-05-20)


### ✨ Features

* add CacheTier enum and CacheOptions with tiered TTLs ([f2a8d61](https://github.com/SalZaki/finnhub-mcp/commit/f2a8d616a0de538e97c86f9fbdfd41797b5917d0))
* add HybridCache-backed IFinnHubCache implementation ([8c9ea2b](https://github.com/SalZaki/finnhub-mcp/commit/8c9ea2b182e48d9120e7f9249998060fd1a1d407))
* add IFinnHubCache abstraction and CacheKey helper ([060589d](https://github.com/SalZaki/finnhub-mcp/commit/060589d4e0e0163c1bcf08e238d730ea23d9d7e5))
* **P2:** HybridCache layer with tiered TTLs ([f84f2a9](https://github.com/SalZaki/finnhub-mcp/commit/f84f2a90924c6b870f07844e1e1e281c367867b8))
* route SearchService through IFinnHubCache ([24dec59](https://github.com/SalZaki/finnhub-mcp/commit/24dec598095e91f83ce51e78fd04d6be55c8ff2a))

## [1.12.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.11.3...v1.12.0) (2026-05-18)


### ✨ Features

* add char-count token estimator ([81014e7](https://github.com/SalZaki/finnhub-mcp/commit/81014e7710e803a5fe36a80ef557b68fc8fa322d))
* add IToolResponseEnvelope and shared models ([070252f](https://github.com/SalZaki/finnhub-mcp/commit/070252f46cbb9aa613d94d7878b04e89eb7f78d5))
* add tool invocation middleware via DelegatingMcpServerTool ([4584bfd](https://github.com/SalZaki/finnhub-mcp/commit/4584bfd514ece865b0d4eb100e7f2f3aecad94f1))
* **P1:** tool-shape contract + retrofit search-symbol to envelope ([4ceeed4](https://github.com/SalZaki/finnhub-mcp/commit/4ceeed4ec7b7e7bc7b27969302fda3598273a140))


### 🐛 Bug Fixes

* patch approx_tokens when SDK only populates Content text ([34d5220](https://github.com/SalZaki/finnhub-mcp/commit/34d52202950d5614ffe78e9522c66930cd962183))


### ♻️ Refactoring

* retrofit search-symbol to new envelope contract ([6c5f7c9](https://github.com/SalZaki/finnhub-mcp/commit/6c5f7c9f621c755d799db796b576bdef2d24a8dc))

## [1.11.3](https://github.com/SalZaki/finnhub-mcp/compare/v1.11.2...v1.11.3) (2026-05-18)


### 🐛 Bug Fixes

* replace boilerplate server instructions with Finnhub guidance ([8cbf38d](https://github.com/SalZaki/finnhub-mcp/commit/8cbf38da54cea0941596d3dc521cf855d0aeccc1))
* replace boilerplate server instructions with Finnhub guidance ([c692eb7](https://github.com/SalZaki/finnhub-mcp/commit/c692eb7528ee3c4c3028592b67236d0a5f573f82))

## [1.11.2](https://github.com/SalZaki/finnhub-mcp/compare/v1.11.1...v1.11.2) (2026-05-09)


### 👷 CI/CD

* switch release-please to manifest mode so changelog-sections take effect ([d3a5a6a](https://github.com/SalZaki/finnhub-mcp/commit/d3a5a6a6954724f9abe477d7f16aa4b99220c074))
* switch release-please to manifest mode so changelog-sections take effect ([76d1ed9](https://github.com/SalZaki/finnhub-mcp/commit/76d1ed9316609c626f543360fdd850b42965f6ba))

## [1.11.1](https://github.com/SalZaki/finnhub-mcp/compare/v1.11.0...v1.11.1) (2026-05-09)


### Reverts

* "docs: align README with current codebase (.NET 10, port 8080, exchanges resource)" ([8aae699](https://github.com/SalZaki/finnhub-mcp/commit/8aae6994cf10f097a53d5af2194553c15de14e5a))

## [1.11.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.10.2...v1.11.0) (2025-08-11)


### Features

* **resource:** add exchange resource ([a3d1d4a](https://github.com/SalZaki/finnhub-mcp/commit/a3d1d4a13bda54d5dac725bb671564325b02cc8c))
* **resource:** add exchange resource ([3249c80](https://github.com/SalZaki/finnhub-mcp/commit/3249c809e1bf9b05dc77910b15dfddc0d17e3a32))
* **resource:** add GetAllExchanges feature ([dad8df0](https://github.com/SalZaki/finnhub-mcp/commit/dad8df023abc47a216e6af42cb61582cd81b905a))

## [1.10.2](https://github.com/SalZaki/finnhub-mcp/compare/v1.10.1...v1.10.2) (2025-08-03)


### Bug Fixes

* **ci:** add bash shell to all build steps in release.yml ([3b6d4a6](https://github.com/SalZaki/finnhub-mcp/commit/3b6d4a69a07f4c5771cf27050c2510428f3bc03c))
* **ci:** add bash shell to all build steps in release.yml ([0a176ba](https://github.com/SalZaki/finnhub-mcp/commit/0a176ba326663180337a14cc9af344c4bc747fc7))

## [1.10.1](https://github.com/SalZaki/finnhub-mcp/compare/v1.10.0...v1.10.1) (2025-08-02)


### Bug Fixes

* resolve executable detection in prepare artifact step ([ed77cd3](https://github.com/SalZaki/finnhub-mcp/commit/ed77cd3168a63dde10232ccb076450808158ff7c))
* resolve executable detection in prepare artifact step ([37b484f](https://github.com/SalZaki/finnhub-mcp/commit/37b484f44a51bb6a9aeb833de2fad94447effb32))

## [1.10.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.9.0...v1.10.0) (2025-08-02)


### Features

* **ci:** : use POSIX-compatible executable detection in release.yml ([15beb0f](https://github.com/SalZaki/finnhub-mcp/commit/15beb0fb95e180d513fdc6aebda8cfd339c00498))
* **ci:** : use POSIX-compatible executable detection in release.yml ([c6ff3eb](https://github.com/SalZaki/finnhub-mcp/commit/c6ff3eb7e1791463687a55b3792c50dd1f11e4b8))

## [1.9.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.8.0...v1.9.0) (2025-08-02)


### Features

* **ci:** disable PublishTrimmed and partial TrimMode in release pipeline ([7d8f2d3](https://github.com/SalZaki/finnhub-mcp/commit/7d8f2d358494d602d0036ea88304c078b1acf521))
* **ci:** disable PublishTrimmed and partial TrimMode in release pipeline ([f6ef4ae](https://github.com/SalZaki/finnhub-mcp/commit/f6ef4ae85eebfecf926968c820ad48f64d13baa7))

## [1.8.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.7.0...v1.8.0) (2025-08-02)


### Features

* **ci:** improve release workflow with better error handling ([63ff5b2](https://github.com/SalZaki/finnhub-mcp/commit/63ff5b2e9e5d05bdd3a7e3f78dfd70d4ffae4326))
* **ci:** improve release workflow with better error handling ([e1e9cc7](https://github.com/SalZaki/finnhub-mcp/commit/e1e9cc7433f6ec14e94aaa4c4263e2423a74e0b1))

## [1.7.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.6.0...v1.7.0) (2025-08-02)


### Features

* **ci:** add supported runtime identifiers from build matrix ([2144625](https://github.com/SalZaki/finnhub-mcp/commit/21446257d589426ac3dfa6eb9b3fcadf122e7353))
* **ci:** add supported runtime identifiers from build matrix ([c84f0fa](https://github.com/SalZaki/finnhub-mcp/commit/c84f0faf8b45aee61f520d8618859c01f5681cfa))

## [1.6.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.5.0...v1.6.0) (2025-08-01)


### Features

* **app:** update release ([040db5e](https://github.com/SalZaki/finnhub-mcp/commit/040db5e4187bddb5692a5da7b0c88cc5f8936876))
* **app:** update release and app start-up ([06f2027](https://github.com/SalZaki/finnhub-mcp/commit/06f2027b172e4f5c2ed16335b5b55c4606892343))
* **app:** update release and app startup ([a09918f](https://github.com/SalZaki/finnhub-mcp/commit/a09918f2ddf52a190b5efb88b698f10cc9214883))

## [1.5.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.4.0...v1.5.0) (2025-07-29)


### Features

* **config:** resolve appsettings.json path relative to binary directory ([97e6959](https://github.com/SalZaki/finnhub-mcp/commit/97e695999f544f84fe1ab43d87f775aa7283ca62))
* **config:** resolve appsettings.json path relative to binary directory ([0ea9cb7](https://github.com/SalZaki/finnhub-mcp/commit/0ea9cb734c325bc4ca6f88aa5a98bb0730cb7b36))

## [1.4.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.3.0...v1.4.0) (2025-07-29)


### Features

* **ci:** push new release ([9d6daac](https://github.com/SalZaki/finnhub-mcp/commit/9d6daac95cfb469717965584a41b5e3614369b37))
* **ci:** push new release ([3ec16f8](https://github.com/SalZaki/finnhub-mcp/commit/3ec16f83d0d84003f9eff067e3262c9300f25246))


### Bug Fixes

* **ci:** fix dotnet publish error ([2ea8ff1](https://github.com/SalZaki/finnhub-mcp/commit/2ea8ff1f22bf525e685e33ba118938d7ad578dd5))
* **ci:** fix dotnet publish error ([13e6e2f](https://github.com/SalZaki/finnhub-mcp/commit/13e6e2f6c7756bc31480be176f26530115ad6ba0))

## [1.3.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.2.0...v1.3.0) (2025-07-29)


### Features

* **ci:** update release, readme and banner image ([873e770](https://github.com/SalZaki/finnhub-mcp/commit/873e77044e6df4a47b5c4c1cf5cb7ccb19bde1fd))
* **ci:** update release, readme and banner image ([4574441](https://github.com/SalZaki/finnhub-mcp/commit/457444107c1a5e8e68ea546c229fc6bea2af5979))

## [1.2.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.1.0...v1.2.0) (2025-07-27)


### Features

* **tool:** release finnhub api integration and search symbol tool wi… ([0fd720f](https://github.com/SalZaki/finnhub-mcp/commit/0fd720f71c116af520f3dc0d04692b7ed8017012))
* **tool:** release finnhub api integration and search symbol tool with ARM64 macOS support ([89d3564](https://github.com/SalZaki/finnhub-mcp/commit/89d35640b648ba99012240fd4283ece540d2482b))

## [1.1.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.0.0...v1.1.0) (2025-07-27)


### Features

* **tool:** release finnhub api integration and search symbol tool ([c14c9d1](https://github.com/SalZaki/finnhub-mcp/commit/c14c9d1a5b8f1b041d366a9b48f367cc32e5d962))
* **tool:** release finnhub api integration and search symbol tool ([d2d343c](https://github.com/SalZaki/finnhub-mcp/commit/d2d343c1658e7b68fe6f712703180265a27b3452))
