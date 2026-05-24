// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>Wire DTO for the Finnhub <c>/stock/profile2</c> endpoint.</summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubProfileResponse
{
    /// <summary>Ticker symbol.</summary>
    [JsonPropertyName("ticker")] public string? Ticker { get; init; }

    /// <summary>Company name.</summary>
    [JsonPropertyName("name")] public string? Name { get; init; }

    /// <summary>ISO country code.</summary>
    [JsonPropertyName("country")] public string? Country { get; init; }

    /// <summary>Reporting currency code.</summary>
    [JsonPropertyName("currency")] public string? Currency { get; init; }

    /// <summary>Listing exchange.</summary>
    [JsonPropertyName("exchange")] public string? Exchange { get; init; }

    /// <summary>IPO date.</summary>
    [JsonPropertyName("ipo")] public string? Ipo { get; init; }

    /// <summary>Market capitalisation (USD millions).</summary>
    [JsonPropertyName("marketCapitalization")] public double? MarketCapitalization { get; init; }

    /// <summary>Total shares outstanding (millions).</summary>
    [JsonPropertyName("shareOutstanding")] public double? ShareOutstanding { get; init; }

    /// <summary>Finnhub industry classification.</summary>
    [JsonPropertyName("finnhubIndustry")] public string? FinnhubIndustry { get; init; }

    /// <summary>Logo URL.</summary>
    [JsonPropertyName("logo")] public string? Logo { get; init; }

    /// <summary>Company phone number.</summary>
    [JsonPropertyName("phone")] public string? Phone { get; init; }

    /// <summary>Company website URL.</summary>
    [JsonPropertyName("weburl")] public string? WebUrl { get; init; }
}
