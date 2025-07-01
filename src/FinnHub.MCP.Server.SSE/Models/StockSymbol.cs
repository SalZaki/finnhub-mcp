// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.SSE.Models;

/// <summary>
/// Represents a symbol search result returned by the MCP server, enriched for AI agent discovery and semantic tooling.
/// </summary>
public sealed class StockSymbol
{
    /// <summary>
    /// The unique trading symbol used to identify the security.
    /// Example: AAPL for Apple Inc.
    /// </summary>
    [JsonPropertyName("symbol")]
    [Description("Unique trading symbol (e.g., AAPL)")]
    public required string Symbol { get; init; }

    /// <summary>
    /// A user-friendly version of the symbol for display purposes.
    /// Often the same as <see cref="Symbol"/> but may include formatting.
    /// </summary>
    [JsonPropertyName("display_symbol")]
    [Description("User-friendly or formatted symbol for display")]
    public required string DisplaySymbol { get; init; }

    /// <summary>
    /// A textual description of the company or security.
    /// Example: "Apple Inc."
    /// </summary>
    [JsonPropertyName("description")]
    [Description("Company or security name")]
    public required string Description { get; init; }

    /// <summary>
    /// The type of the financial instrument.
    /// Example: "Common Stock", "ETF", "Bond"
    /// </summary>
    [JsonPropertyName("type")]
    [Description("Security type, e.g. 'Common Stock' or 'ETF'")]
    public required string Type { get; init; }

    /// <summary>
    /// Optional stock exchange the symbol is listed on.
    /// Example: "NASDAQ", "NYSE"
    /// </summary>
    [JsonPropertyName("exchange")]
    [Description("Stock exchange code, e.g., NASDAQ")]
    public string? Exchange { get; init; }

    /// <summary>
    /// Optional trading currency.
    /// Example: "USD"
    /// </summary>
    [JsonPropertyName("currency")]
    [Description("Trading currency (e.g., USD)")]
    public string? Currency { get; init; }

    /// <summary>
    /// Optional country code of the issuing entity.
    /// Example: "US", "JP"
    /// </summary>
    [JsonPropertyName("country")]
    [Description("Issuer's country (e.g., US)")]
    public string? Country { get; init; }

    /// <summary>
    /// International Securities Identification Number (ISIN).
    /// Used globally to identify securities.
    /// </summary>
    [JsonPropertyName("isin")]
    [Description("International Securities Identification Number")]
    // ReSharper disable once InconsistentNaming
    public string? ISIN { get; init; }

    /// <summary>
    /// The Committee on Uniform Security Identification Procedures (CUSIP) code.
    /// Used primarily in North America.
    /// </summary>
    [JsonPropertyName("cusip")]
    [Description("US-based CUSIP identifier")]
    // ReSharper disable once InconsistentNaming
    public string? CUSIP { get; set; }

    /// <summary>
    /// Financial Instrument Global Identifier (FIGI).
    /// A globally unique identifier issued by Bloomberg.
    /// </summary>
    [JsonPropertyName("figi")]
    [Description("Bloomberg FIGI identifier")]
    // ReSharper disable once InconsistentNaming
    public string? FIGI { get; set; }

    /// <summary>
    /// A numeric score (0.0 to 1.0) representing the confidence of the match between the query and this result.
    /// Used to help AI agents in ranking or filtering.
    /// </summary>
    [JsonPropertyName("confidence_score")]
    [Description("Search match confidence score (0.0–1.0)")]
    public double ConfidenceScore { get; set; }

    /// <summary>
    /// Indicates the field that most likely caused this result to match.
    /// Examples: "symbol", "description", "isin"
    /// </summary>
    [JsonPropertyName("matched_by")]
    [Description("What field matched the query (symbol, name, etc.)")]
    public string? MatchedBy { get; set; }

    /// <summary>
    /// Optional ranking of the result in terms of relevance or order returned by the data provider.
    /// </summary>
    [JsonPropertyName("rank")]
    [Description("Result order from the data provider")]
    public int? QueryRelevanceRank { get; set; }

    /// <summary>
    /// Timestamp (UTC) when the data was retrieved from the source provider.
    /// </summary>
    [JsonPropertyName("retrieved_at_utc")]
    [Description("Timestamp when the result was retrieved from provider")]
    public DateTime RetrievedAtUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional industry or sector to which the security belongs.
    /// </summary>
    [JsonPropertyName("sector")]
    [Description("Optional industry or sector information")]
    public string? Sector { get; set; }

    /// <summary>
    /// Indicates whether this is the primary listing for the security.
    /// </summary>
    [JsonPropertyName("is_primary_listing")]
    [Description("Whether this is the primary listing for the symbol")]
    public bool? IsPrimaryListing { get; set; }

    /// <summary>
    /// Optional URL to the company or asset logo.
    /// Used for better display experience in UIs or AI dashboards.
    /// </summary>
    [JsonPropertyName("logo_url")]
    [Description("Optional logo or branding URL")]
    public string? LogoUrl { get; set; }
}
