# Changelog

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
