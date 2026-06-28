# Changelog

## [1.22.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.21.0...v1.22.0) (2026-06-28)


### ✨ Features

* (infra): add finhubb infra project ([19d99b3](https://github.com/SalZaki/finnhub-mcp/commit/19d99b3c959e5a72828aeea39763995292322611))
* (infra): add finnhub infra unit tests project ([22950a1](https://github.com/SalZaki/finnhub-mcp/commit/22950a1636b2604f8dde2a514c20e9b626c700df))
* (infra): run dotnet format ([70bbfa3](https://github.com/SalZaki/finnhub-mcp/commit/70bbfa3fe25ad960baaa4a25970d5f3176524289))
* add CacheTier enum and CacheOptions with tiered TTLs ([f2a8d61](https://github.com/SalZaki/finnhub-mcp/commit/f2a8d616a0de538e97c86f9fbdfd41797b5917d0))
* add char-count token estimator ([81014e7](https://github.com/SalZaki/finnhub-mcp/commit/81014e7710e803a5fe36a80ef557b68fc8fa322d))
* add HybridCache-backed IFinnHubCache implementation ([8c9ea2b](https://github.com/SalZaki/finnhub-mcp/commit/8c9ea2b182e48d9120e7f9249998060fd1a1d407))
* add IFinnHubCache abstraction and CacheKey helper ([060589d](https://github.com/SalZaki/finnhub-mcp/commit/060589d4e0e0163c1bcf08e238d730ea23d9d7e5))
* add IToolResponseEnvelope and shared models ([070252f](https://github.com/SalZaki/finnhub-mcp/commit/070252f46cbb9aa613d94d7878b04e89eb7f78d5))
* add ResolvedSymbol record and ISymbolResolver interface ([9165327](https://github.com/SalZaki/finnhub-mcp/commit/9165327e6897a73f1577ab3222802c9763ad45a9))
* add SymbolResolver with fast-path and cached ambiguous lookup ([b0c18fb](https://github.com/SalZaki/finnhub-mcp/commit/b0c18fbc409aa76601584b2226f567d315f19c7c))
* add tool invocation middleware via DelegatingMcpServerTool ([4584bfd](https://github.com/SalZaki/finnhub-mcp/commit/4584bfd514ece865b0d4eb100e7f2f3aecad94f1))
* **app:** update release ([040db5e](https://github.com/SalZaki/finnhub-mcp/commit/040db5e4187bddb5692a5da7b0c88cc5f8936876))
* **app:** update release and app start-up ([06f2027](https://github.com/SalZaki/finnhub-mcp/commit/06f2027b172e4f5c2ed16335b5b55c4606892343))
* **app:** update release and app startup ([a09918f](https://github.com/SalZaki/finnhub-mcp/commit/a09918f2ddf52a190b5efb88b698f10cc9214883))
* **chore:** dotnet format ([5f86a08](https://github.com/SalZaki/finnhub-mcp/commit/5f86a08426d75a20566f824058b69a8fb01179e5))
* **chore:** replace SearchSymbol exceptions with ApiClientException types ([22c9e4b](https://github.com/SalZaki/finnhub-mcp/commit/22c9e4b04b52dfe3c91f810ed7600e5e80fec227))
* **ci:** : use POSIX-compatible executable detection in release.yml ([15beb0f](https://github.com/SalZaki/finnhub-mcp/commit/15beb0fb95e180d513fdc6aebda8cfd339c00498))
* **ci:** : use POSIX-compatible executable detection in release.yml ([c6ff3eb](https://github.com/SalZaki/finnhub-mcp/commit/c6ff3eb7e1791463687a55b3792c50dd1f11e4b8))
* **ci:** add supported runtime identifiers from build matrix ([2144625](https://github.com/SalZaki/finnhub-mcp/commit/21446257d589426ac3dfa6eb9b3fcadf122e7353))
* **ci:** add supported runtime identifiers from build matrix ([c84f0fa](https://github.com/SalZaki/finnhub-mcp/commit/c84f0faf8b45aee61f520d8618859c01f5681cfa))
* **ci:** add test execution and coverage reporting to GitHub Actions ([616278e](https://github.com/SalZaki/finnhub-mcp/commit/616278e1dbea283f310027b7764659d5b13140d1))
* **ci:** disable PublishTrimmed and partial TrimMode in release pipeline ([7d8f2d3](https://github.com/SalZaki/finnhub-mcp/commit/7d8f2d358494d602d0036ea88304c078b1acf521))
* **ci:** disable PublishTrimmed and partial TrimMode in release pipeline ([f6ef4ae](https://github.com/SalZaki/finnhub-mcp/commit/f6ef4ae85eebfecf926968c820ad48f64d13baa7))
* **ci:** improve release workflow with better error handling ([63ff5b2](https://github.com/SalZaki/finnhub-mcp/commit/63ff5b2e9e5d05bdd3a7e3f78dfd70d4ffae4326))
* **ci:** improve release workflow with better error handling ([e1e9cc7](https://github.com/SalZaki/finnhub-mcp/commit/e1e9cc7433f6ec14e94aaa4c4263e2423a74e0b1))
* **ci:** push new release ([9d6daac](https://github.com/SalZaki/finnhub-mcp/commit/9d6daac95cfb469717965584a41b5e3614369b37))
* **ci:** push new release ([3ec16f8](https://github.com/SalZaki/finnhub-mcp/commit/3ec16f83d0d84003f9eff067e3262c9300f25246))
* **ci:** scheduled live-smoke against real Finnhub (closes [#170](https://github.com/SalZaki/finnhub-mcp/issues/170)) ([#185](https://github.com/SalZaki/finnhub-mcp/issues/185)) ([43493dd](https://github.com/SalZaki/finnhub-mcp/commit/43493ddaba2da2fc45681cd7a1e03694f7cb2d2e))
* **ci:** update release, readme and banner image ([873e770](https://github.com/SalZaki/finnhub-mcp/commit/873e77044e6df4a47b5c4c1cf5cb7ccb19bde1fd))
* **ci:** update release, readme and banner image ([4574441](https://github.com/SalZaki/finnhub-mcp/commit/457444107c1a5e8e68ea546c229fc6bea2af5979))
* **config:** add env config file ([8e6b3e1](https://github.com/SalZaki/finnhub-mcp/commit/8e6b3e11eb10bc97cc5b285c4f5ea7dfd0d83d07))
* **config:** add POCO classes for FinnHub configuration binding ([8f50c57](https://github.com/SalZaki/finnhub-mcp/commit/8f50c57f6a31b275457cc1067379751fdef0330b))
* **config:** add structured FinnHub section to appsettings.json ([be5cce8](https://github.com/SalZaki/finnhub-mcp/commit/be5cce8de0609072ea10d26736bf6bfb0563de95))
* **config:** resolve appsettings.json path relative to binary directory ([97e6959](https://github.com/SalZaki/finnhub-mcp/commit/97e695999f544f84fe1ab43d87f775aa7283ca62))
* **config:** resolve appsettings.json path relative to binary directory ([0ea9cb7](https://github.com/SalZaki/finnhub-mcp/commit/0ea9cb734c325bc4ca6f88aa5a98bb0730cb7b36))
* **core:** introduce source-generated JSON serialization and improve error response handling ([233b5d0](https://github.com/SalZaki/finnhub-mcp/commit/233b5d027478643974d70f5b0e31090c3147239c))
* **core:** introduce source-generated JSON serialization and improve error response handling ([09349f2](https://github.com/SalZaki/finnhub-mcp/commit/09349f2b5e3e630a338403ca4d46cc3e640991f1))
* **core:** introduce source-generated JSON serialization and improve error response handling ([e5ccb3d](https://github.com/SalZaki/finnhub-mcp/commit/e5ccb3de7d494d201356cc59215be4f98a422cd9))
* **cors:** configure default CORS policy for public API access ([2e8658b](https://github.com/SalZaki/finnhub-mcp/commit/2e8658ba8be1a10143c84787b953872a9d69e138)), closes [#13](https://github.com/SalZaki/finnhub-mcp/issues/13)
* **doc:** minor change ([405a301](https://github.com/SalZaki/finnhub-mcp/commit/405a30190c3f2a85d688ba48875ccac09f38aad0))
* **doc:** update readme file ([718a3d5](https://github.com/SalZaki/finnhub-mcp/commit/718a3d50fe358fea4b8f5c808833e9d58ccc63f5))
* gate premium endpoints with typed 403 handling ([eb956f5](https://github.com/SalZaki/finnhub-mcp/commit/eb956f573476e72f77d82ee5bcb0adb38339169c))
* **health:** add structured health check endpoints ([512215a](https://github.com/SalZaki/finnhub-mcp/commit/512215ac5cb4910951161b86540baa61a659ca8e))
* **http:** implement connection pooling and resilience with Polly ([ebff430](https://github.com/SalZaki/finnhub-mcp/commit/ebff4301ac6aa21dc10e03348e80fc05b925a70d))
* **http:** implement connection pooling and resilience with Polly ([b4ba1ff](https://github.com/SalZaki/finnhub-mcp/commit/b4ba1ff7ec4ddbbecf79a8724f90d0f5c4e32f7a))
* **infra:** implement FinnHubApiClient with robust error handling an… ([8ba990f](https://github.com/SalZaki/finnhub-mcp/commit/8ba990fd95db68bf3671229f94751cfb0c5d7c9e))
* **infra:** implement FinnHubApiClient with robust error handling and infrastructure support ([1843042](https://github.com/SalZaki/finnhub-mcp/commit/1843042a28bc99b5ef797b3770c4dd9562790a62))
* **mcp:** register and configure MCP Server with assembly resources ([c0b33bd](https://github.com/SalZaki/finnhub-mcp/commit/c0b33bd191b90828be9cc9702b983a5f7ae47440))
* **npm:** npm/npx distribution channel for the MCP server ([#471](https://github.com/SalZaki/finnhub-mcp/issues/471)) ([96420c9](https://github.com/SalZaki/finnhub-mcp/commit/96420c940b6b41fa89614825c0bd2ae488b63943))
* **P1:** tool-shape contract + retrofit search-symbol to envelope ([4ceeed4](https://github.com/SalZaki/finnhub-mcp/commit/4ceeed4ec7b7e7bc7b27969302fda3598273a140))
* **P2:** HybridCache layer with tiered TTLs ([f84f2a9](https://github.com/SalZaki/finnhub-mcp/commit/f84f2a90924c6b870f07844e1e1e281c367867b8))
* **P3:** symbol resolver service ([817625a](https://github.com/SalZaki/finnhub-mcp/commit/817625a4df3f3bad91c2dc9d42d1cf567eba90d5))
* **P4:** rate-limit awareness on envelope + api-status resource ([730a563](https://github.com/SalZaki/finnhub-mcp/commit/730a563f274eab7866fbb69f534ad254da44072b))
* **p5:** premium-endpoint gating via typed 403 handling ([b5f734a](https://github.com/SalZaki/finnhub-mcp/commit/b5f734a3b08422ae16b8a5976d87f18040d60386))
* **p6a:** add get-financials-snapshot — 10 curated KPIs ([9e40b63](https://github.com/SalZaki/finnhub-mcp/commit/9e40b63c2106fc2bfde2adc0e89a2babcf239372))
* **p6a:** add get-financials-snapshot tool — 10 curated KPIs ([8c69f3c](https://github.com/SalZaki/finnhub-mcp/commit/8c69f3c2c9ce8060473fbb26378aa7d0b6e22e9a))
* **p6a:** add get-news-pulse — completes P6 Wave A ([b295ece](https://github.com/SalZaki/finnhub-mcp/commit/b295ecede1f866724d67b5f9c74e9f36ca9b1352))
* **p6a:** add get-news-pulse tool — sentiment + headlines + week-over-week delta ([5e9fcc0](https://github.com/SalZaki/finnhub-mcp/commit/5e9fcc0a184d975825486cde8508320b1b4ee511))
* **p6a:** add get-peers tool — pattern reference for Wave A ([71c8cd6](https://github.com/SalZaki/finnhub-mcp/commit/71c8cd65d3b6d39985df4202d18d95d2080307a8))
* **p6a:** add get-peers tool — peer ticker lookup for a symbol ([1044cda](https://github.com/SalZaki/finnhub-mcp/commit/1044cda2d8a5294eb42aa41795bc25b5a54f44c2))
* **p6a:** add get-price-summary — aggregated price stats ([319bec8](https://github.com/SalZaki/finnhub-mcp/commit/319bec81c6f1c2acf764b53b875bde881a1b92f2))
* **p6a:** add get-price-summary tool — aggregated price stats ([b9165e6](https://github.com/SalZaki/finnhub-mcp/commit/b9165e6b4dae86a67de2dc4760b27bb00df9aea3))
* **p6a:** cross-link next_actions across Wave A tools + README ([5538cae](https://github.com/SalZaki/finnhub-mcp/commit/5538caefdd49faa5de9477be18c6e6fc2bc79769))
* **p6a:** cross-link next_actions across Wave A tools and document in README ([dedb7dd](https://github.com/SalZaki/finnhub-mcp/commit/dedb7dd93c4c0ee6b8ccdcb9ceb6aac278c9f290))
* **p6b:** add get-quote and get-company-profile (P6 Wave B) ([a6cdeba](https://github.com/SalZaki/finnhub-mcp/commit/a6cdebaada632a447543712bda4917a91a5a1649))
* **p6b:** add get-quote and get-company-profile (P6 Wave B) ([d4b0789](https://github.com/SalZaki/finnhub-mcp/commit/d4b0789f4e14252118386b9eb8186a1ca0952999))
* **prompt:** add /compare-peers slash command ([#435](https://github.com/SalZaki/finnhub-mcp/issues/435)) ([1d16fec](https://github.com/SalZaki/finnhub-mcp/commit/1d16fec0f79e9c52a83a25bffdecce07f7c37021)), closes [#222](https://github.com/SalZaki/finnhub-mcp/issues/222)
* **prompt:** add /news-pulse slash command ([#436](https://github.com/SalZaki/finnhub-mcp/issues/436)) ([d415e00](https://github.com/SalZaki/finnhub-mcp/commit/d415e00fb07d2732fa361c61716541a8070ff84f)), closes [#223](https://github.com/SalZaki/finnhub-mcp/issues/223)
* **prompt:** add /research-ticker slash command ([#434](https://github.com/SalZaki/finnhub-mcp/issues/434)) ([18bd371](https://github.com/SalZaki/finnhub-mcp/commit/18bd3712b11bceda771d2c4a1d47c770873e86d2)), closes [#221](https://github.com/SalZaki/finnhub-mcp/issues/221)
* **resource:** add capabilities catalog resource ([#220](https://github.com/SalZaki/finnhub-mcp/issues/220)) ([#428](https://github.com/SalZaki/finnhub-mcp/issues/428)) ([b11cf22](https://github.com/SalZaki/finnhub-mcp/commit/b11cf22f7e048cf0fb8aed4f20041ce93f927a73))
* **resource:** add exchange resource ([a3d1d4a](https://github.com/SalZaki/finnhub-mcp/commit/a3d1d4a13bda54d5dac725bb671564325b02cc8c))
* **resource:** add exchange resource ([3249c80](https://github.com/SalZaki/finnhub-mcp/commit/3249c809e1bf9b05dc77910b15dfddc0d17e3a32))
* **resource:** add GetAllExchanges feature ([dad8df0](https://github.com/SalZaki/finnhub-mcp/commit/dad8df023abc47a216e6af42cb61582cd81b905a))
* **resource:** serve full exchanges catalog from curated reference ([#224](https://github.com/SalZaki/finnhub-mcp/issues/224)) ([#426](https://github.com/SalZaki/finnhub-mcp/issues/426)) ([89c030b](https://github.com/SalZaki/finnhub-mcp/commit/89c030b55fc7c5bce101c0fa383769911521852c))
* route SearchService through IFinnHubCache ([24dec59](https://github.com/SalZaki/finnhub-mcp/commit/24dec598095e91f83ce51e78fd04d6be55c8ff2a))
* **schema:** enforce pattern and length constraints for query and exchange inputs ([ce3d699](https://github.com/SalZaki/finnhub-mcp/commit/ce3d69981a7cb2afa808fa171cd5847f969b6652))
* **schema:** enforce pattern and length constraints for query and exchange inputs ([4949942](https://github.com/SalZaki/finnhub-mcp/commit/49499424bb5c6666a63f0261bea0338e922af688))
* **search-symbol:** add MCP tool for querying stock symbols via FinnHub ([#38](https://github.com/SalZaki/finnhub-mcp/issues/38)) ([03a14a1](https://github.com/SalZaki/finnhub-mcp/commit/03a14a10be2cf13e3066e4372f376bf98e46c06f))
* **searchsymbols:** create search feature and search symbols tool ([3230da6](https://github.com/SalZaki/finnhub-mcp/commit/3230da682efda07abbd86a3626ac70dad062db7b))
* **service:** add FinnHubService and StockSymbol model ([f131d8e](https://github.com/SalZaki/finnhub-mcp/commit/f131d8e46ff00ac5cc39b7a28f9a6765fbc5718a))
* **service:** introduce domain-specific exceptions for symbol search ([85349a4](https://github.com/SalZaki/finnhub-mcp/commit/85349a4aa4cc390d8178414b41b008ac7f689a61))
* **service:** introduce domain-specific exceptions for symbol search and improve error handling ([4eb15f8](https://github.com/SalZaki/finnhub-mcp/commit/4eb15f8f5633bfb7b6d90e3ffeb1b31823140ae3))
* surface Finnhub rate-limit on envelope and status resource ([ad39c08](https://github.com/SalZaki/finnhub-mcp/commit/ad39c080a63db74308af67cdb195bf40b4f66305))
* **tool:** add get-exchange-symbols listing via /stock/symbol ([#433](https://github.com/SalZaki/finnhub-mcp/issues/433)) ([48a2151](https://github.com/SalZaki/finnhub-mcp/commit/48a2151b08cac2e40a55520e65754c145b1709de)), closes [#225](https://github.com/SalZaki/finnhub-mcp/issues/225)
* **tool:** release finnhub api integration and search symbol tool ([c14c9d1](https://github.com/SalZaki/finnhub-mcp/commit/c14c9d1a5b8f1b041d366a9b48f367cc32e5d962))
* **tool:** release finnhub api integration and search symbol tool ([d2d343c](https://github.com/SalZaki/finnhub-mcp/commit/d2d343c1658e7b68fe6f712703180265a27b3452))
* **tool:** release finnhub api integration and search symbol tool wi… ([0fd720f](https://github.com/SalZaki/finnhub-mcp/commit/0fd720f71c116af520f3dc0d04692b7ed8017012))
* **tool:** release finnhub api integration and search symbol tool with ARM64 macOS support ([89d3564](https://github.com/SalZaki/finnhub-mcp/commit/89d35640b648ba99012240fd4283ece540d2482b))
* **tools:** add get-calendar earnings dispatch ([#214](https://github.com/SalZaki/finnhub-mcp/issues/214)) ([#421](https://github.com/SalZaki/finnhub-mcp/issues/421)) ([50cdf75](https://github.com/SalZaki/finnhub-mcp/commit/50cdf7598439875001d814dfee8c578faf80b313))
* **tools:** add get-calendar economic kind ([#423](https://github.com/SalZaki/finnhub-mcp/issues/423)) ([701f1c1](https://github.com/SalZaki/finnhub-mcp/commit/701f1c10aba9554a26f6c62b9843ad6cd4afca5e)), closes [#216](https://github.com/SalZaki/finnhub-mcp/issues/216)
* **tools:** add get-calendar ipo kind ([#215](https://github.com/SalZaki/finnhub-mcp/issues/215)) ([#422](https://github.com/SalZaki/finnhub-mcp/issues/422)) ([5ecbc4f](https://github.com/SalZaki/finnhub-mcp/commit/5ecbc4f1dc40427c63a67199f165003ed81f31e6))
* **tools:** add get-insider-signal tool ([#424](https://github.com/SalZaki/finnhub-mcp/issues/424)) ([b916ca1](https://github.com/SalZaki/finnhub-mcp/commit/b916ca1286b400beaa3a28bcd004843842cf3331)), closes [#217](https://github.com/SalZaki/finnhub-mcp/issues/217)
* **tools:** add get-recommendations tool ([#425](https://github.com/SalZaki/finnhub-mcp/issues/425)) ([f09e5eb](https://github.com/SalZaki/finnhub-mcp/commit/f09e5eb45ef2f55ab8e358c8dfcc60dfd0a3b8c1)), closes [#218](https://github.com/SalZaki/finnhub-mcp/issues/218)
* **tools:** add search-tools meta-tool for intent-based discovery ([#219](https://github.com/SalZaki/finnhub-mcp/issues/219)) ([#427](https://github.com/SalZaki/finnhub-mcp/issues/427)) ([fab8f41](https://github.com/SalZaki/finnhub-mcp/commit/fab8f41a28f2d4180c0df5c6a848a27e95a0b632))
* **validation:** add input validation and sanitisation for input parameters ([cdbe8bb](https://github.com/SalZaki/finnhub-mcp/commit/cdbe8bb180bffe1d79dc7ece80a423511a35c430))
* **validation:** add input validation and sanitization for query and exchange parameters ([7d3d338](https://github.com/SalZaki/finnhub-mcp/commit/7d3d338731b01640b72f8ef4fe1dcbade09d33f7))


### 🐛 Bug Fixes

* **application:** financials NotFound, resolver trim-length + search cleanup ([51acb2a](https://github.com/SalZaki/finnhub-mcp/commit/51acb2ab24af0364e0930c6f8a7755113cebd029))
* **application:** include the date window in NewsService cache keys ([6ad65ba](https://github.com/SalZaki/finnhub-mcp/commit/6ad65ba2a92a3d7b3169acc3c04d35683ba3abc7)), closes [#387](https://github.com/SalZaki/finnhub-mcp/issues/387)
* **application:** NotFound for empty financials + trim-before-length in resolver ([997eecd](https://github.com/SalZaki/finnhub-mcp/commit/997eecd6e8424ad59df10756116c6bf23fbca533))
* **build:** update project name in solution file ([54b4202](https://github.com/SalZaki/finnhub-mcp/commit/54b42025aef154158b5d371f4485376d64a23115))
* **build:** update project name in solution file ([b731a18](https://github.com/SalZaki/finnhub-mcp/commit/b731a183172c690e75e5530e9ab222dd2d454503))
* **ci-cd:** remove deprecated v3 artifact actions ([0847159](https://github.com/SalZaki/finnhub-mcp/commit/08471597386db4b2057d225b611a15c8c947a66d))
* **ci-cd:** remove deprecated v3 artifact actions ([3a417d8](https://github.com/SalZaki/finnhub-mcp/commit/3a417d89b2923492b483e9cdbeb1b56a1fa685e6))
* **ci-cd:** update build step ([863dbb8](https://github.com/SalZaki/finnhub-mcp/commit/863dbb8143d701bdc0f644da46a0b908c249e7af))
* **ci-cd:** update build step ([c0efbf0](https://github.com/SalZaki/finnhub-mcp/commit/c0efbf0ece1f5d8161f76438436e2ebc727498df))
* **ci-cd:** update build strategy ([49492b4](https://github.com/SalZaki/finnhub-mcp/commit/49492b4a9daf54a4b496092affe409e47706440e))
* **ci:** add bash shell to all build steps in release.yml ([3b6d4a6](https://github.com/SalZaki/finnhub-mcp/commit/3b6d4a69a07f4c5771cf27050c2510428f3bc03c))
* **ci:** add bash shell to all build steps in release.yml ([0a176ba](https://github.com/SalZaki/finnhub-mcp/commit/0a176ba326663180337a14cc9af344c4bc747fc7))
* **ci:** add format, test and coverage jobs ([9c4f137](https://github.com/SalZaki/finnhub-mcp/commit/9c4f137c2ab74dc322be6f3b9024729dc5c213a0))
* **ci:** correct solution path for dotnet restore and build ([7ae7aa4](https://github.com/SalZaki/finnhub-mcp/commit/7ae7aa44a8cdf4a2f9da0669a1ee8ea7d8ed4693))
* **ci:** correct test coverage output path and show test results ([5617d04](https://github.com/SalZaki/finnhub-mcp/commit/5617d04de0e30b1a40e6b7310bed42523cfc4c00))
* **ci:** fix dotnet publish error ([2ea8ff1](https://github.com/SalZaki/finnhub-mcp/commit/2ea8ff1f22bf525e685e33ba118938d7ad578dd5))
* **ci:** fix dotnet publish error ([13e6e2f](https://github.com/SalZaki/finnhub-mcp/commit/13e6e2f6c7756bc31480be176f26530115ad6ba0))
* **ci:** fix issue in checkout code step ([4cb5ddd](https://github.com/SalZaki/finnhub-mcp/commit/4cb5ddda05f9bd8d6a259f10d5f533e2e02cb4d5))
* **ci:** format code files, fix issues with ci-cd pipeline and readme file ([5f69528](https://github.com/SalZaki/finnhub-mcp/commit/5f695286fa2c1836f829471b124bf64f51dc6929))
* **ci:** remove --no-build and --no-restore from test job to resolve test execution issues ([f93a21b](https://github.com/SalZaki/finnhub-mcp/commit/f93a21ba54115a028893a7d71fa582834c79e1c3))
* **ci:** remove solution path for dotnet restore and build ([acc8acd](https://github.com/SalZaki/finnhub-mcp/commit/acc8acda766937745df8840f90ee65305e88fe28))
* **ci:** restore PDB generation in Debug builds so coverlet can instrument ([f898319](https://github.com/SalZaki/finnhub-mcp/commit/f89831914acb811e581db126bbbefc493d19f4d8))
* **ci:** restore PDB generation in Debug builds so coverlet can instrument ([2665cd1](https://github.com/SalZaki/finnhub-mcp/commit/2665cd1814f9ad0493c6c2aed6199b074bced7b7))
* **ci:** update step in ci ([970ad12](https://github.com/SalZaki/finnhub-mcp/commit/970ad122ddbd37c6ef2563adbaf135784b0aa518))
* **ci:** update test and coverage steps in ci ([4f4062b](https://github.com/SalZaki/finnhub-mcp/commit/4f4062b780195d431d1d83ca626242f9364907a5))
* **ci:** upload artifact action version to v4 ([9b018b3](https://github.com/SalZaki/finnhub-mcp/commit/9b018b38523439cce711f7165e80d1b1cac72043))
* **ci:** upload test and coverage step ([7c29cd4](https://github.com/SalZaki/finnhub-mcp/commit/7c29cd48ff04eac8b06ff4f23e9e301a2ef4487e))
* **ci:** upload test and coverage step ([e336787](https://github.com/SalZaki/finnhub-mcp/commit/e336787cf8c12024661dd4bc99a2251f789fd1d0))
* **ci:** upload test step ([81b5dfe](https://github.com/SalZaki/finnhub-mcp/commit/81b5dfe2b8e41086722b581a008cbc9180cd3504))
* **config:** add trailing slash to Development appsettings BaseUrl ([#411](https://github.com/SalZaki/finnhub-mcp/issues/411)) ([c6fd9ba](https://github.com/SalZaki/finnhub-mcp/commit/c6fd9ba152df0a4a84ee67436723354a32ae3110)), closes [#395](https://github.com/SalZaki/finnhub-mcp/issues/395)
* **infra:** preserve inner exception, use ServiceUnavailable for transport failures ([#418](https://github.com/SalZaki/finnhub-mcp/issues/418)) ([8c9c83c](https://github.com/SalZaki/finnhub-mcp/commit/8c9c83cbe32f537a2779d56e687a955b3983537a))
* **middleware:** propagate declared view into budget-exceeded envelope ([#414](https://github.com/SalZaki/finnhub-mcp/issues/414)) ([a4cc4bf](https://github.com/SalZaki/finnhub-mcp/commit/a4cc4bf526bac484a35f10e55ec1bb360db72411)), closes [#394](https://github.com/SalZaki/finnhub-mcp/issues/394)
* **news:** catch ApiClientPremiumRequiredException on company-news path ([#415](https://github.com/SalZaki/finnhub-mcp/issues/415)) ([89591f7](https://github.com/SalZaki/finnhub-mcp/commit/89591f72e92eb0600616914b66b2427c4d2b32cc)), closes [#392](https://github.com/SalZaki/finnhub-mcp/issues/392)
* **p6a:** tolerate mixed types in /stock/metric response ([2e7214e](https://github.com/SalZaki/finnhub-mcp/commit/2e7214eb742be5c148a82a0f0e1c69e17589c1bc))
* **p6a:** tolerate mixed types in /stock/metric response ([a6bf456](https://github.com/SalZaki/finnhub-mcp/commit/a6bf4563ee2349d5eb40048f9e78cba68693e526))
* **p6:** BaseUrl trailing-slash so relative request URIs hit /api/v1/ ([bf79257](https://github.com/SalZaki/finnhub-mcp/commit/bf792570c8c40cbb3eecffe0818835dd775fffac))
* **p6:** BaseUrl trailing-slash so relative request URIs hit /api/v1/ ([205f1e3](https://github.com/SalZaki/finnhub-mcp/commit/205f1e3bb49a07335202e57d3a3dd78393f5efde))
* **p6:** nullable defensive DTOs + real Finnhub fixtures ([039373a](https://github.com/SalZaki/finnhub-mcp/commit/039373a01e6cc7293ef66c3aea03f2e0d47b9b69))
* **p6:** nullable defensive DTOs + real Finnhub fixtures for client tests ([99f0b5a](https://github.com/SalZaki/finnhub-mcp/commit/99f0b5a23841ccd8dc90c851af279dea744feeb4))
* patch approx_tokens when SDK only populates Content text ([34d5220](https://github.com/SalZaki/finnhub-mcp/commit/34d52202950d5614ffe78e9522c66930cd962183))
* **peers:** project once, derive explanation and total_count from projected result ([#413](https://github.com/SalZaki/finnhub-mcp/issues/413)) ([8444d00](https://github.com/SalZaki/finnhub-mcp/commit/8444d002f3038aa6bea7b015eff4a49fbe91004c)), closes [#393](https://github.com/SalZaki/finnhub-mcp/issues/393)
* **release:** fix cross-platform artifact extraction in release workflow ([5d65f0e](https://github.com/SalZaki/finnhub-mcp/commit/5d65f0e29dab65e048897e5cb792dcfa08762f2f)), closes [#77](https://github.com/SalZaki/finnhub-mcp/issues/77)
* **release:** fix release.yml for cross-platform compatibility and proper artifact packaging ([99f920f](https://github.com/SalZaki/finnhub-mcp/commit/99f920f8d92fd55f6bd0aa716c474d31bc7a58da))
* replace boilerplate server instructions with Finnhub guidance ([8cbf38d](https://github.com/SalZaki/finnhub-mcp/commit/8cbf38da54cea0941596d3dc521cf855d0aeccc1))
* replace boilerplate server instructions with Finnhub guidance ([c692eb7](https://github.com/SalZaki/finnhub-mcp/commit/c692eb7528ee3c4c3028592b67236d0a5f573f82))
* resolve executable detection in prepare artifact step ([ed77cd3](https://github.com/SalZaki/finnhub-mcp/commit/ed77cd3168a63dde10232ccb076450808158ff7c))
* resolve executable detection in prepare artifact step ([37b484f](https://github.com/SalZaki/finnhub-mcp/commit/37b484f44a51bb6a9aeb833de2fad94447effb32))
* **restore:** re-add project in solution file ([bbdb2a1](https://github.com/SalZaki/finnhub-mcp/commit/bbdb2a1813392caddaa9216237ada843437242f6))
* return JSON string from resource handlers so SDK marshals correctly ([890b205](https://github.com/SalZaki/finnhub-mcp/commit/890b205af30d7b118e355784a82fbac723e90f59))
* **server:** connection  issues ([90d769b](https://github.com/SalZaki/finnhub-mcp/commit/90d769ba5d540073fac01c17888f669560a234e0))
* **server:** dotnet format issues ([ed2f5f4](https://github.com/SalZaki/finnhub-mcp/commit/ed2f5f446db0afc72ab9436acaf3ffb69a024d0b))
* skip --urls injection in stdio mode ([75e54c8](https://github.com/SalZaki/finnhub-mcp/commit/75e54c8751df13a0377395953209c984d530db88))
* stop --urls 8080 collision and .env env-var clobbering in stdio dev flow ([b1ace7f](https://github.com/SalZaki/finnhub-mcp/commit/b1ace7fac7b2a0418af7069ad21d1489265e2401))
* stop .env from clobbering launcher-supplied env vars ([5f9699d](https://github.com/SalZaki/finnhub-mcp/commit/5f9699de8e01136ed8395af2e09dd2d5df2af2ca))
* **test:** accept typed NotFound as graceful degradation in live-smoke ([#187](https://github.com/SalZaki/finnhub-mcp/issues/187)) ([684b50f](https://github.com/SalZaki/finnhub-mcp/commit/684b50ff2fd533a3b7cfa87a32229bb68156edca))
* **transport:** bind STDIO Kestrel to an ephemeral port, not 5000 ([#432](https://github.com/SalZaki/finnhub-mcp/issues/432)) ([1115b12](https://github.com/SalZaki/finnhub-mcp/commit/1115b12403bd279419518758f789ee1ff47cf48f))


### ♻️ Refactoring

* (server): add xml comments and format ([b3d9c6e](https://github.com/SalZaki/finnhub-mcp/commit/b3d9c6e30261a0dea27de4906c80ec89923d57eb))
* (server): fix ci/cd issue ([6c0c777](https://github.com/SalZaki/finnhub-mcp/commit/6c0c7774c76587d96cbb873d3546bdbac65c9353))
* (server): rename solution ([84401c4](https://github.com/SalZaki/finnhub-mcp/commit/84401c49d602d8c9a03c45269bc6dab60c833a70))
* (server): rename solution name in CI/CD ([d2234d6](https://github.com/SalZaki/finnhub-mcp/commit/d2234d6cf1e3e311e3d3b0b88b6461bcbc1d6ca2))
* (server): rename test project directory in CI/CD ([7bf21af](https://github.com/SalZaki/finnhub-mcp/commit/7bf21affc814d2f094319a127d8b0bb076a135a3))
* (server): reorganise solution ([5ae9c66](https://github.com/SalZaki/finnhub-mcp/commit/5ae9c660ce1e72e3146647a9243347d29e356f74))
* **api:** replace SearchSymbol exceptions with ApiClientException ([1d5a181](https://github.com/SalZaki/finnhub-mcp/commit/1d5a181f6ede06da75d1f22f875a13574033f7c0))
* **application:** collapse search base classes, make StockSymbol init-only ([8486e23](https://github.com/SalZaki/finnhub-mcp/commit/8486e237c1cf6df9b7c66e79067814fc71de1123))
* **ci/cd:** add codecov config file ([9796884](https://github.com/SalZaki/finnhub-mcp/commit/9796884b10814f899e537bdb5d19ed232d00cc2d))
* **ci/cd:** add new release action and update minor changes to build action ([a33983e](https://github.com/SalZaki/finnhub-mcp/commit/a33983e72e2d9c9506fc6c315c712566cc5ea8d8))
* **config:** update and remove config settings ([ff253c4](https://github.com/SalZaki/finnhub-mcp/commit/ff253c436bb3e80d094a2520ae7c618e83b6e6b8))
* convert Result&lt;T&gt;.Success/.Failure to static factories ([#420](https://github.com/SalZaki/finnhub-mcp/issues/420)) ([5698508](https://github.com/SalZaki/finnhub-mcp/commit/5698508e005ecac5c00a8550b4f1dfc462ef20ee)), closes [#399](https://github.com/SalZaki/finnhub-mcp/issues/399)
* **fix:** update failing unit tests ([a4810b7](https://github.com/SalZaki/finnhub-mcp/commit/a4810b7be920d1249731cdd18b555e26114a4c60))
* **http:** inject named HttpClient directly into FinnHubSearchApiClient ([d84ac65](https://github.com/SalZaki/finnhub-mcp/commit/d84ac65353cb30ed20ec165b6651db1a043b7f6e))
* **http:** inject named HttpClient directly into FinnHubSearchApiClient ([8b95e70](https://github.com/SalZaki/finnhub-mcp/commit/8b95e70439be19d01e7674cd759c299eee35c5a4))
* **infra:** extract shared error handler + unify cache-key construction ([374bc03](https://github.com/SalZaki/finnhub-mcp/commit/374bc03bbeb9ebbf83aaf4ddbaca6d8cd365176a))
* **infra:** extract shared error handler + unify cache-key construction ([28c30ff](https://github.com/SalZaki/finnhub-mcp/commit/28c30ff800dfabfe324b9633143f1e1a2b1f835d)), closes [#387](https://github.com/SalZaki/finnhub-mcp/issues/387)
* **infra:** stop retrying cancellations, add backoff jitter, drop unused package ([#451](https://github.com/SalZaki/finnhub-mcp/issues/451)) ([f72f513](https://github.com/SalZaki/finnhub-mcp/commit/f72f5134341dabad3d8554b4e799c890d8e94f9b))
* **json:** replace JsonSerializer.Deserialize with source generation context ([dd34086](https://github.com/SalZaki/finnhub-mcp/commit/dd3408628322c3a81cfae697ada8c28a13aef0c1))
* **json:** replace JsonSerializer.Deserialize with source generation context ([80adb82](https://github.com/SalZaki/finnhub-mcp/commit/80adb820b5c1e550528ce93475a50ce70af7f61f))
* **json:** replace JsonSerializer.Deserialize with source generation context ([683df86](https://github.com/SalZaki/finnhub-mcp/commit/683df8695e5ad88b0bd0be1f313a2a09015cec6c))
* **json:** replace JsonSerializer.Deserialize with source generation context ([78d86b4](https://github.com/SalZaki/finnhub-mcp/commit/78d86b4a569983a9354d330cce6a029c29b86bba))
* **project:** rename project from Finnhub.MCP.Server.SSE to Finnhub.MCP.Server ([ec900ec](https://github.com/SalZaki/finnhub-mcp/commit/ec900ecb971a959a570dc63e336bd9a267e42552))
* **project:** rename project from Finnhub.MCP.Server.SSE to Finnhub.MCP.Server ([a3b0de1](https://github.com/SalZaki/finnhub-mcp/commit/a3b0de1e3fc51f907095676b37df4e42f985c703))
* propagate cancellation as typed exception instead of demoting to Unknown ([#416](https://github.com/SalZaki/finnhub-mcp/issues/416)) ([ac65d31](https://github.com/SalZaki/finnhub-mcp/commit/ac65d311d4af529ebfffd10f95e9d3990c7c5f5b)), closes [#391](https://github.com/SalZaki/finnhub-mcp/issues/391)
* replace base classes with Result&lt;T&gt; pattern and extract validators ([8ee66f6](https://github.com/SalZaki/finnhub-mcp/commit/8ee66f6da4abe926fcc52718d47a8d6235b15a99))
* replace base classes with Result&lt;T&gt; pattern and extract validators ([26d4d57](https://github.com/SalZaki/finnhub-mcp/commit/26d4d575ed47aaf4ec34036018d6f1b31b61a629))
* retrofit search-symbol to new envelope contract ([6c5f7c9](https://github.com/SalZaki/finnhub-mcp/commit/6c5f7c9f621c755d799db796b576bdef2d24a8dc))
* route search-symbol next-actions through symbol resolver ([d1be65d](https://github.com/SalZaki/finnhub-mcp/commit/d1be65d4a2741e50565375c22df4ec4079cb4d15))
* **search:** align Search client to the Wave A/B shape (safe axes) ([#453](https://github.com/SalZaki/finnhub-mcp/issues/453)) ([1a58486](https://github.com/SalZaki/finnhub-mcp/commit/1a584863df43b0db823fbfeed2aec30ec9173be6))
* **tool:** consolidate Wave A/B tool scaffold and validators ([#450](https://github.com/SalZaki/finnhub-mcp/issues/450)) ([5ef9770](https://github.com/SalZaki/finnhub-mcp/commit/5ef9770434f61f0b7508d698ecd2de11cafec191))
* update minor changes ([f056fd1](https://github.com/SalZaki/finnhub-mcp/commit/f056fd18c291c4643b096d1ae0eb0d6598211824))


### ⏪ Reverts

* "docs: align README with current codebase (.NET 10, port 8080, exchanges resource)" ([8aae699](https://github.com/SalZaki/finnhub-mcp/commit/8aae6994cf10f097a53d5af2194553c15de14e5a))


### 💄 Style

* fix file encoding and remove redundant usings ([25e3ce6](https://github.com/SalZaki/finnhub-mcp/commit/25e3ce6aebe6937b32e06a15b3ad91d8b6cc133f))


### 👷 CI/CD

* add PR title validator + CONTRIBUTING.md for commitizen flow ([2d5d541](https://github.com/SalZaki/finnhub-mcp/commit/2d5d5414b8b9a3622f917e2f40bd6e7e57ead31f))
* bump the github-actions group with 2 updates ([#459](https://github.com/SalZaki/finnhub-mcp/issues/459)) ([eb9e2ea](https://github.com/SalZaki/finnhub-mcp/commit/eb9e2ea033729ca64384eef8fb034bbbfbb338b0))
* bump the github-actions group with 8 updates ([#444](https://github.com/SalZaki/finnhub-mcp/issues/444)) ([f64a341](https://github.com/SalZaki/finnhub-mcp/commit/f64a341d6f17e433fb42dd3158469d643d74cf62))
* enforce conventional commits via Husky.Net + PR title validator ([b739281](https://github.com/SalZaki/finnhub-mcp/commit/b739281e419c391cc1dd4d05faaa17bd4d79ff23))
* gate releases through release branch ([a95f026](https://github.com/SalZaki/finnhub-mcp/commit/a95f026f973727d5d3bbbab3da5aff964293ce28))
* gate releases through release branch ([ff516aa](https://github.com/SalZaki/finnhub-mcp/commit/ff516aad903c2a85f291208a87d56887149c73cd))
* **release:** harden npm publish with smoke-pack + publishConfig ([#472](https://github.com/SalZaki/finnhub-mcp/issues/472)) ([bfeab14](https://github.com/SalZaki/finnhub-mcp/commit/bfeab14188afae5b74e258276643bcbcc46ccaf5))
* **release:** publish to npm via OIDC only (drop NPM_TOKEN) ([#480](https://github.com/SalZaki/finnhub-mcp/issues/480)) ([a826c21](https://github.com/SalZaki/finnhub-mcp/commit/a826c21438d691a1a77d1f15c05a1bf15a23f76e))
* **release:** run validate on promotion PRs to release ([#476](https://github.com/SalZaki/finnhub-mcp/issues/476)) ([a534991](https://github.com/SalZaki/finnhub-mcp/commit/a5349917f0400882be36bbf944aee03189f514bf))
* **release:** switch to single-branch release-please on main ([#481](https://github.com/SalZaki/finnhub-mcp/issues/481)) ([205bbd0](https://github.com/SalZaki/finnhub-mcp/commit/205bbd0fece8507a9ed83cfcbcf0cafd3ba68332))
* remove dorny/test-reporter — the real source of PR CI flakes ([c630a10](https://github.com/SalZaki/finnhub-mcp/commit/c630a10b4c48a458b2691714d4be944929eaeac6))
* remove obsolete sync-changelog workflow ([#483](https://github.com/SalZaki/finnhub-mcp/issues/483)) ([7e1d284](https://github.com/SalZaki/finnhub-mcp/commit/7e1d2845fe19b11836532bdd20040d3f6e644c7e))
* switch release-please to manifest mode so changelog-sections take effect ([d3a5a6a](https://github.com/SalZaki/finnhub-mcp/commit/d3a5a6a6954724f9abe477d7f16aa4b99220c074))
* switch release-please to manifest mode so changelog-sections take effect ([76d1ed9](https://github.com/SalZaki/finnhub-mcp/commit/76d1ed9316609c626f543360fdd850b42965f6ba))
* target release branch from release-please ([9fdcc46](https://github.com/SalZaki/finnhub-mcp/commit/9fdcc46b6cd8d8fc5d916967c46296add27898ab))
* target release branch from release-please ([9db7de8](https://github.com/SalZaki/finnhub-mcp/commit/9db7de8b063e065666875d242c80f686a4e3b937))
* **test:** add diagnostics to unit test step ([07a82da](https://github.com/SalZaki/finnhub-mcp/commit/07a82da12e54ea8bdfe80ddb13dc4282ddc40874))
* validate before release-please so failed tests do not create orphan releases ([618a923](https://github.com/SalZaki/finnhub-mcp/commit/618a9232e831e4f398812ce29a226c4b7265e385))

## [1.21.0](https://github.com/SalZaki/finnhub-mcp/compare/v1.20.1...v1.21.0) (2026-06-27)


### ✨ Features

* **ci:** scheduled live-smoke against real Finnhub (closes [#170](https://github.com/SalZaki/finnhub-mcp/issues/170)) ([#185](https://github.com/SalZaki/finnhub-mcp/issues/185)) ([43493dd](https://github.com/SalZaki/finnhub-mcp/commit/43493ddaba2da2fc45681cd7a1e03694f7cb2d2e))
* **npm:** npm/npx distribution channel for the MCP server ([#471](https://github.com/SalZaki/finnhub-mcp/issues/471)) ([96420c9](https://github.com/SalZaki/finnhub-mcp/commit/96420c940b6b41fa89614825c0bd2ae488b63943))
* **prompt:** add /compare-peers slash command ([#435](https://github.com/SalZaki/finnhub-mcp/issues/435)) ([1d16fec](https://github.com/SalZaki/finnhub-mcp/commit/1d16fec0f79e9c52a83a25bffdecce07f7c37021)), closes [#222](https://github.com/SalZaki/finnhub-mcp/issues/222)
* **prompt:** add /news-pulse slash command ([#436](https://github.com/SalZaki/finnhub-mcp/issues/436)) ([d415e00](https://github.com/SalZaki/finnhub-mcp/commit/d415e00fb07d2732fa361c61716541a8070ff84f)), closes [#223](https://github.com/SalZaki/finnhub-mcp/issues/223)
* **prompt:** add /research-ticker slash command ([#434](https://github.com/SalZaki/finnhub-mcp/issues/434)) ([18bd371](https://github.com/SalZaki/finnhub-mcp/commit/18bd3712b11bceda771d2c4a1d47c770873e86d2)), closes [#221](https://github.com/SalZaki/finnhub-mcp/issues/221)
* **resource:** add capabilities catalog resource ([#220](https://github.com/SalZaki/finnhub-mcp/issues/220)) ([#428](https://github.com/SalZaki/finnhub-mcp/issues/428)) ([b11cf22](https://github.com/SalZaki/finnhub-mcp/commit/b11cf22f7e048cf0fb8aed4f20041ce93f927a73))
* **resource:** serve full exchanges catalog from curated reference ([#224](https://github.com/SalZaki/finnhub-mcp/issues/224)) ([#426](https://github.com/SalZaki/finnhub-mcp/issues/426)) ([89c030b](https://github.com/SalZaki/finnhub-mcp/commit/89c030b55fc7c5bce101c0fa383769911521852c))
* **tool:** add get-exchange-symbols listing via /stock/symbol ([#433](https://github.com/SalZaki/finnhub-mcp/issues/433)) ([48a2151](https://github.com/SalZaki/finnhub-mcp/commit/48a2151b08cac2e40a55520e65754c145b1709de)), closes [#225](https://github.com/SalZaki/finnhub-mcp/issues/225)
* **tools:** add get-calendar earnings dispatch ([#214](https://github.com/SalZaki/finnhub-mcp/issues/214)) ([#421](https://github.com/SalZaki/finnhub-mcp/issues/421)) ([50cdf75](https://github.com/SalZaki/finnhub-mcp/commit/50cdf7598439875001d814dfee8c578faf80b313))
* **tools:** add get-calendar economic kind ([#423](https://github.com/SalZaki/finnhub-mcp/issues/423)) ([701f1c1](https://github.com/SalZaki/finnhub-mcp/commit/701f1c10aba9554a26f6c62b9843ad6cd4afca5e)), closes [#216](https://github.com/SalZaki/finnhub-mcp/issues/216)
* **tools:** add get-calendar ipo kind ([#215](https://github.com/SalZaki/finnhub-mcp/issues/215)) ([#422](https://github.com/SalZaki/finnhub-mcp/issues/422)) ([5ecbc4f](https://github.com/SalZaki/finnhub-mcp/commit/5ecbc4f1dc40427c63a67199f165003ed81f31e6))
* **tools:** add get-insider-signal tool ([#424](https://github.com/SalZaki/finnhub-mcp/issues/424)) ([b916ca1](https://github.com/SalZaki/finnhub-mcp/commit/b916ca1286b400beaa3a28bcd004843842cf3331)), closes [#217](https://github.com/SalZaki/finnhub-mcp/issues/217)
* **tools:** add get-recommendations tool ([#425](https://github.com/SalZaki/finnhub-mcp/issues/425)) ([f09e5eb](https://github.com/SalZaki/finnhub-mcp/commit/f09e5eb45ef2f55ab8e358c8dfcc60dfd0a3b8c1)), closes [#218](https://github.com/SalZaki/finnhub-mcp/issues/218)
* **tools:** add search-tools meta-tool for intent-based discovery ([#219](https://github.com/SalZaki/finnhub-mcp/issues/219)) ([#427](https://github.com/SalZaki/finnhub-mcp/issues/427)) ([fab8f41](https://github.com/SalZaki/finnhub-mcp/commit/fab8f41a28f2d4180c0df5c6a848a27e95a0b632))


### 🐛 Bug Fixes

* **application:** financials NotFound, resolver trim-length + search cleanup ([51acb2a](https://github.com/SalZaki/finnhub-mcp/commit/51acb2ab24af0364e0930c6f8a7755113cebd029))
* **application:** include the date window in NewsService cache keys ([6ad65ba](https://github.com/SalZaki/finnhub-mcp/commit/6ad65ba2a92a3d7b3169acc3c04d35683ba3abc7)), closes [#387](https://github.com/SalZaki/finnhub-mcp/issues/387)
* **application:** NotFound for empty financials + trim-before-length in resolver ([997eecd](https://github.com/SalZaki/finnhub-mcp/commit/997eecd6e8424ad59df10756116c6bf23fbca533))
* **config:** add trailing slash to Development appsettings BaseUrl ([#411](https://github.com/SalZaki/finnhub-mcp/issues/411)) ([c6fd9ba](https://github.com/SalZaki/finnhub-mcp/commit/c6fd9ba152df0a4a84ee67436723354a32ae3110)), closes [#395](https://github.com/SalZaki/finnhub-mcp/issues/395)
* **infra:** preserve inner exception, use ServiceUnavailable for transport failures ([#418](https://github.com/SalZaki/finnhub-mcp/issues/418)) ([8c9c83c](https://github.com/SalZaki/finnhub-mcp/commit/8c9c83cbe32f537a2779d56e687a955b3983537a))
* **middleware:** propagate declared view into budget-exceeded envelope ([#414](https://github.com/SalZaki/finnhub-mcp/issues/414)) ([a4cc4bf](https://github.com/SalZaki/finnhub-mcp/commit/a4cc4bf526bac484a35f10e55ec1bb360db72411)), closes [#394](https://github.com/SalZaki/finnhub-mcp/issues/394)
* **news:** catch ApiClientPremiumRequiredException on company-news path ([#415](https://github.com/SalZaki/finnhub-mcp/issues/415)) ([89591f7](https://github.com/SalZaki/finnhub-mcp/commit/89591f72e92eb0600616914b66b2427c4d2b32cc)), closes [#392](https://github.com/SalZaki/finnhub-mcp/issues/392)
* **peers:** project once, derive explanation and total_count from projected result ([#413](https://github.com/SalZaki/finnhub-mcp/issues/413)) ([8444d00](https://github.com/SalZaki/finnhub-mcp/commit/8444d002f3038aa6bea7b015eff4a49fbe91004c)), closes [#393](https://github.com/SalZaki/finnhub-mcp/issues/393)
* **test:** accept typed NotFound as graceful degradation in live-smoke ([#187](https://github.com/SalZaki/finnhub-mcp/issues/187)) ([684b50f](https://github.com/SalZaki/finnhub-mcp/commit/684b50ff2fd533a3b7cfa87a32229bb68156edca))
* **transport:** bind STDIO Kestrel to an ephemeral port, not 5000 ([#432](https://github.com/SalZaki/finnhub-mcp/issues/432)) ([1115b12](https://github.com/SalZaki/finnhub-mcp/commit/1115b12403bd279419518758f789ee1ff47cf48f))


### ♻️ Refactoring

* **application:** collapse search base classes, make StockSymbol init-only ([8486e23](https://github.com/SalZaki/finnhub-mcp/commit/8486e237c1cf6df9b7c66e79067814fc71de1123))
* convert Result&lt;T&gt;.Success/.Failure to static factories ([#420](https://github.com/SalZaki/finnhub-mcp/issues/420)) ([5698508](https://github.com/SalZaki/finnhub-mcp/commit/5698508e005ecac5c00a8550b4f1dfc462ef20ee)), closes [#399](https://github.com/SalZaki/finnhub-mcp/issues/399)
* **infra:** extract shared error handler + unify cache-key construction ([374bc03](https://github.com/SalZaki/finnhub-mcp/commit/374bc03bbeb9ebbf83aaf4ddbaca6d8cd365176a))
* **infra:** extract shared error handler + unify cache-key construction ([28c30ff](https://github.com/SalZaki/finnhub-mcp/commit/28c30ff800dfabfe324b9633143f1e1a2b1f835d)), closes [#387](https://github.com/SalZaki/finnhub-mcp/issues/387)
* **infra:** stop retrying cancellations, add backoff jitter, drop unused package ([#451](https://github.com/SalZaki/finnhub-mcp/issues/451)) ([f72f513](https://github.com/SalZaki/finnhub-mcp/commit/f72f5134341dabad3d8554b4e799c890d8e94f9b))
* propagate cancellation as typed exception instead of demoting to Unknown ([#416](https://github.com/SalZaki/finnhub-mcp/issues/416)) ([ac65d31](https://github.com/SalZaki/finnhub-mcp/commit/ac65d311d4af529ebfffd10f95e9d3990c7c5f5b)), closes [#391](https://github.com/SalZaki/finnhub-mcp/issues/391)
* **search:** align Search client to the Wave A/B shape (safe axes) ([#453](https://github.com/SalZaki/finnhub-mcp/issues/453)) ([1a58486](https://github.com/SalZaki/finnhub-mcp/commit/1a584863df43b0db823fbfeed2aec30ec9173be6))
* **tool:** consolidate Wave A/B tool scaffold and validators ([#450](https://github.com/SalZaki/finnhub-mcp/issues/450)) ([5ef9770](https://github.com/SalZaki/finnhub-mcp/commit/5ef9770434f61f0b7508d698ecd2de11cafec191))


### 👷 CI/CD

* bump the github-actions group with 2 updates ([#459](https://github.com/SalZaki/finnhub-mcp/issues/459)) ([eb9e2ea](https://github.com/SalZaki/finnhub-mcp/commit/eb9e2ea033729ca64384eef8fb034bbbfbb338b0))
* bump the github-actions group with 8 updates ([#444](https://github.com/SalZaki/finnhub-mcp/issues/444)) ([f64a341](https://github.com/SalZaki/finnhub-mcp/commit/f64a341d6f17e433fb42dd3158469d643d74cf62))
* **release:** harden npm publish with smoke-pack + publishConfig ([#472](https://github.com/SalZaki/finnhub-mcp/issues/472)) ([bfeab14](https://github.com/SalZaki/finnhub-mcp/commit/bfeab14188afae5b74e258276643bcbcc46ccaf5))

## [1.20.1](https://github.com/SalZaki/finnhub-mcp/compare/v1.20.0...v1.20.1) (2026-05-24)


### 🐛 Bug Fixes

* **ci:** restore PDB generation in Debug builds so coverlet can instrument ([f898319](https://github.com/SalZaki/finnhub-mcp/commit/f89831914acb811e581db126bbbefc493d19f4d8))
* **ci:** restore PDB generation in Debug builds so coverlet can instrument ([2665cd1](https://github.com/SalZaki/finnhub-mcp/commit/2665cd1814f9ad0493c6c2aed6199b074bced7b7))

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
