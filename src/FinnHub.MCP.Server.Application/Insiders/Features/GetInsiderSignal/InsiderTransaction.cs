// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;

/// <summary>
/// A single insider transaction parsed from Finnhub's <c>/stock/insider-transactions</c> feed.
/// </summary>
/// <remarks>
/// <see cref="Change"/> is signed: positive = acquisition, negative = disposition / gift.
/// <see cref="TransactionCode"/> follows the SEC Form 4 code list — common values are
/// <c>P</c> (open-market purchase), <c>S</c> (open-market sale), <c>A</c> (grant),
/// <c>G</c> (bona fide gift), <c>M</c> (option exercise), <c>F</c> (tax-withholding sale).
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class InsiderTransaction
{
    /// <summary>Insider's full name as filed with the SEC.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Net change in shares — positive for acquisitions, negative for dispositions
    /// (sells / gifts / tax withholdings).
    /// </summary>
    [JsonPropertyName("change")]
    public required long Change { get; init; }

    /// <summary>Shares held by the insider after the transaction.</summary>
    [JsonPropertyName("share")]
    public long? Share { get; init; }

    /// <summary>Transaction date as filed (UTC calendar day).</summary>
    [JsonPropertyName("transaction_date")]
    public required DateOnly TransactionDate { get; init; }

    /// <summary>SEC filing date (UTC calendar day); always on or after the transaction date.</summary>
    [JsonPropertyName("filing_date")]
    public DateOnly? FilingDate { get; init; }

    /// <summary>Per-share transaction price; <c>0</c> for gifts and grants.</summary>
    [JsonPropertyName("transaction_price")]
    public double? TransactionPrice { get; init; }

    /// <summary>SEC Form 4 transaction code (e.g. <c>P</c>, <c>S</c>, <c>G</c>).</summary>
    [JsonPropertyName("transaction_code")]
    public string? TransactionCode { get; init; }

    /// <summary>True when the transaction was in a derivative security (option, RSU).</summary>
    [JsonPropertyName("is_derivative")]
    public bool IsDerivative { get; init; }

    /// <summary>Reporting currency (e.g. <c>USD</c>); empty for filings that omit it.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}
