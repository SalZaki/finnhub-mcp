// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;

/// <summary>
/// Wire response for <c>get-company-profile</c>. Cosmetic fields (logo, phone, weburl)
/// are null on the default summary view; populated on standard/full.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetCompanyProfileResponse
{
    /// <summary>Ticker symbol.</summary>
    [JsonPropertyName("ticker")]
    public required string Ticker { get; init; }

    /// <summary>Company name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>ISO country code (e.g. <c>US</c>).</summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    /// <summary>Reporting currency code (e.g. <c>USD</c>).</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>Listing exchange (e.g. <c>NASDAQ NMS - GLOBAL MARKET</c>).</summary>
    [JsonPropertyName("exchange")]
    public string? Exchange { get; init; }

    /// <summary>IPO date.</summary>
    [JsonPropertyName("ipo")]
    public string? Ipo { get; init; }

    /// <summary>Market capitalisation (USD millions).</summary>
    [JsonPropertyName("market_cap")]
    public double? MarketCap { get; init; }

    /// <summary>Total shares outstanding (millions).</summary>
    [JsonPropertyName("share_outstanding")]
    public double? ShareOutstanding { get; init; }

    /// <summary>Finnhub industry classification.</summary>
    [JsonPropertyName("industry")]
    public string? Industry { get; init; }

    /// <summary>Logo URL. Populated only on standard/full views.</summary>
    [JsonPropertyName("logo")]
    public string? Logo { get; init; }

    /// <summary>Company phone number. Populated only on standard/full views.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }

    /// <summary>Company website URL. Populated only on standard/full views.</summary>
    [JsonPropertyName("weburl")]
    public string? WebUrl { get; init; }
}
