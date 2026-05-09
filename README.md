
![Finnhub-MCP-Server](https://github.com/SalZaki/finnhub-mcp/blob/main/assets/github-banner.png)

<h4>

![Build Status](https://github.com/salzaki/finnhub-mcp/actions/workflows/dotnet.yml/badge.svg)
![codecov](https://codecov.io/gh/salzaki/finnhub-mcp/branch/main/graph/badge.svg)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green)
![MCP](https://img.shields.io/badge/AI-MCP-purple)
![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg)
[![GitHub issues with bug label](https://img.shields.io/github/issues-raw/salzaki/finnhub-mcp/bug?label=bugs&logo=github&color=red)](https://github.com/salzaki/finnhub-mcp/issues?q=is%3Aissue+is%3Aopen+label%3Abug)
![GitHub Repo stars](https://img.shields.io/github/stars/salzaki/finnhub-mcp?style=social)

</h4>

**⚠️ This repository is under active development. The service will be live soon. Stay tuned! ⚠️**

# 👋 Welcome to Finnhub MCP Server

## 🎯 Project Overview

A robust **Model Context Protocol (MCP) Server** that integrates with **[Finnhub](https://finnhub.io/)**'s institutional-grade financial data APIs. Provides both Server-Sent Events (SSE) and Standard Input/Output (STDIO) transport protocols for streaming live financial data with modern .NET practices and Finnhub API best practices.

## 🚀 Key Features

- ✅ **Symbol Search Tool** with validated and sanitized user input
- ✅ **Finnhub API Rate Limiting**
    - Respects free tier limits (60 requests/minute)
    - Implements exponential backoff for 429 responses
    - Configurable rate limiting for different subscription tiers
- ✅ **Robust Input Validation**
    - Query & exchange inputs sanitized using regex patterns
    - Length and pattern constraints enforced at runtime and via JSON Schema
    - Symbol validation against Finnhub's supported formats
- ✅ **Secure by Default**
    - Actively defends against common injection vectors (`<script>`, SQL-style, etc.)
    - Allowed character sets enforced through schema and code
    - API key stored securely via environment variables
- ✅ **Resilient HTTP Communication**
    - Typed `HttpClient` with retry & circuit breaker policies (via [Polly](https://github.com/App-vNext/Polly))
    - Connection pooling with `SocketsHttpHandler`
    - Automatic retry with jitter for failed requests
- ✅ **WebSocket Support** (Coming Soon)
    - Real-time streaming via Finnhub WebSocket API
    - Automatic reconnection handling
    - Subscription management for multiple symbols

## 📊 Finnhub API Integration & Best Practices

This server implements Finnhub's recommended best practices:

### Rate Limiting
- **Free Tier**: 60 requests/minute with automatic throttling
- **Premium Tiers**: Configurable limits based on your subscription
- Implements exponential backoff for rate limit responses (HTTP 429)
- Request queuing to prevent API limit violations

### Error Handling
- Comprehensive error handling for all Finnhub API responses
- Graceful degradation when API is unavailable
- Retry logic with exponential backoff for transient failures
- Proper handling of 401 (Unauthorized) and 429 (Rate Limit) responses

### Data Validation
- Input sanitization for all symbol and parameter inputs
- Validates symbols against Finnhub's supported formats
- Exchange code validation for international markets
- Query parameter encoding to prevent injection attacks

### Performance Optimization
- HTTP connection pooling and reuse
- Efficient JSON deserialization with System.Text.Json
- Async/await patterns throughout for non-blocking operations
- Memory-efficient streaming for large datasets

## 🚧 Available & Upcoming MCP Tools

### ✅ Currently Available
- **Symbol Search Tool** — search for stock symbols with fuzzy matching

### 🔄 In Development
- **Real-Time Quote Tool** — live prices for stock, forex & crypto
- **Company Profile Tool** — detailed company information and metrics
- **Basic Financial Tool** — key financial metrics and ratios

### 📋 Planned Features
- **Advanced Fundamentals Tool** — comprehensive financial statements
- **Earnings Calendar Tool** — track earnings release dates and estimates
- **News & Sentiment Tool** — company news with sentiment analysis
- **Insider Trading Tool** — insider transaction data
- **Market Status Tool** — trading hours and market holidays
- **Technical Indicators** — RSI, MACD, moving averages
- **Economic Data Tool** — GDP, inflation, employment data
- **Crypto Data Tool** — cryptocurrency prices and market data

## 🚀 Getting Started

### 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later
- A valid [Finnhub API Key](https://finnhub.io/dashboard) (free tier available)

### 🔑 Getting Your Finnhub API Key

1. Visit [Finnhub.io](https://finnhub.io/) and create a free account
2. Navigate to your [Dashboard](https://finnhub.io/dashboard)
3. Copy your API key from the dashboard
4. **Note**: Free tier provides 60 requests/minute and access to basic market data

### 📦 Installation

```bash
git clone https://github.com/SalZaki/finnhub-mcp.git
cd finnhub-mcp
dotnet restore
```

### 🔐 API Key Configuration

**Security Note**: Never commit your API key to version control. Always use environment variables or secure configuration management.

#### Option 1: Environment Variable (Recommended)

**macOS/Linux:**
```bash
export FINNHUB_API_KEY="your_api_key_here"
```

**Windows PowerShell:**
```powershell
$env:FINNHUB_API_KEY="your_api_key_here"
```

**Windows Command Prompt:**
```cmd
set FINNHUB_API_KEY=your_api_key_here
```

#### Option 2: .env File (Development Only)

Create a `.env` file in the root directory:
```bash
echo "FINNHUB_API_KEY=your_api_key_here" > .env
```

**Important**: The `.env` file is automatically loaded during development but should never be committed to version control.

### 🚀 Running the Server

#### HTTP Transport (Default)
```bash
dotnet run --project src/FinnHub.MCP.Server --urls http://localhost:5101
```

#### STDIO Transport
```bash
dotnet run --project src/FinnHub.MCP.Server -- --stdio
```

#### Docker Support
```bash
docker build -t finnhub-mcp .
docker run -e FINNHUB_API_KEY="your_api_key_here" -p 5101:5101 finnhub-mcp
```

## 📖 API Usage Examples

### Symbol Search
```bash
# Search for Apple stock
curl -X POST "http://localhost:5101/mcp/tools/symbol_search" \
  -H "Content-Type: application/json" \
  -d '{"query": "AAPL"}'
```

### Real-Time Quote (Coming Soon)
```bash
# Get real-time quote for Apple
curl -X POST "http://localhost:5101/mcp/tools/quote" \
  -H "Content-Type: application/json" \
  -d '{"symbol": "AAPL"}'
```

## ⚙️ Configuration

### Rate Limiting Configuration
```json
{
  "Finnhub": {
    "RateLimit": {
      "RequestsPerMinute": 60,
      "BurstLimit": 10,
      "RetryAfterSeconds": 60
    }
  }
}
```

### Logging Configuration
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "FinnHub.MCP.Server": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧪 Testing

### Unit Tests
```bash
dotnet test src/FinnHub.MCP.Server.Tests
```

### Integration Tests
```bash
# Set test API key
export FINNHUB_TEST_API_KEY="your_test_api_key"
dotnet test src/FinnHub.MCP.Server.IntegrationTests
```

### Load Testing
```bash
# Test rate limiting behavior
dotnet run --project tests/FinnHub.MCP.LoadTests
```

## 🔧 Development

### Building from Source
```bash
dotnet build
```

### Running with Hot Reload
```bash
dotnet watch run --project src/FinnHub.MCP.Server
```

### Code Quality
```bash
# Run code analysis
dotnet format
dotnet run --project tools/CodeAnalysis
```

## 📊 Monitoring & Observability

- **Health Checks**: `/health` endpoint for monitoring
- **Metrics**: Prometheus metrics via `/metrics`
- **Tracing**: OpenTelemetry integration
- **Logging**: Structured logging with Serilog

## 🛠️ Tech Stack

- **Framework**: [.NET 8](https://dotnet.microsoft.com/) with ASP.NET Core
- **Resilience**: [Polly](https://github.com/App-vNext/Polly) for retry policies and circuit breakers
- **Validation**: [JsonSchema.Net](https://github.com/gregsdennis/json-everything) for input validation
- **Testing**: [xUnit](https://xunit.net/) with [Moq](https://github.com/moq/moq4) for mocking
- **Configuration**: [DotNetEnv](https://github.com/tonerdo/dotnet-env) for environment management
- **Logging**: [Serilog](https://serilog.net/) for structured logging
- **Monitoring**: [OpenTelemetry](https://opentelemetry.io/) for observability
- **CI/CD**: GitHub Actions with automated testing and deployment

### Development Setup
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass (`dotnet test`)
6. Submit a pull request

## 📄 License

This project is licensed under the MIT License, see [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [FinnHub.io](https://finnhub.io/) for providing excellent financial data APIs
- [Model Context Protocol](https://github.com/modelcontextprotocol) for the MCP specification
- The .NET community for excellent libraries and tools

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/SalZaki/finnhub-mcp/issues)
- **Discussions**: [GitHub Discussions](https://github.com/SalZaki/finnhub-mcp/discussions)
- **Documentation**: [Wiki](https://github.com/SalZaki/finnhub-mcp/wiki)
- **Finnhub Support**: [Finnhub Documentation](https://finnhub.io/docs/api)