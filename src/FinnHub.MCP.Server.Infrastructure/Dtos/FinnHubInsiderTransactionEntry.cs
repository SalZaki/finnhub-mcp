// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Wire DTO for a single entry in Finnhub's <c>/stock/insider-transactions</c> response.
/// </summary>
/// <remarks>
/// <c>change</c> is signed; <c>transactionPrice</c> is <c>0</c> for grants and gifts.
/// Dates arrive as ISO <c>yyyy-MM-dd</c> strings — the application-layer mapper parses
/// them into <see cref="DateOnly"/>. Property names are camelCase, overriding the
/// snake_case default in <see cref="Serialization.FinnHubJsonContext"/>.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubInsiderTransactionEntry
{
    /// <summary>Insider's full name as filed with the SEC.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Signed net share change (positive = acquisition, negative = disposition).</summary>
    [JsonPropertyName("change")]
    public long? Change { get; init; }

    /// <summary>Shares held by the insider after the transaction.</summary>
    [JsonPropertyName("share")]
    public long? Share { get; init; }

    /// <summary>Transaction date as ISO <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("transactionDate")]
    public string? TransactionDate { get; init; }

    /// <summary>Filing date as ISO <c>yyyy-MM-dd</c>; always on or after transaction date.</summary>
    [JsonPropertyName("filingDate")]
    public string? FilingDate { get; init; }

    /// <summary>Per-share transaction price; <c>0</c> for grants and gifts.</summary>
    [JsonPropertyName("transactionPrice")]
    public double? TransactionPrice { get; init; }

    /// <summary>SEC Form 4 transaction code (e.g. <c>P</c>, <c>S</c>, <c>G</c>).</summary>
    [JsonPropertyName("transactionCode")]
    public string? TransactionCode { get; init; }

    /// <summary>True when the transaction was in a derivative security.</summary>
    [JsonPropertyName("isDerivative")]
    public bool IsDerivative { get; init; }

    /// <summary>Reporting currency.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}
