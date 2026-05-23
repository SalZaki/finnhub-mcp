// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Symbols;
using FinnHub.MCP.Server.Infrastructure.Dtos;

namespace FinnHub.MCP.Server.Infrastructure.Serialization;

/// <summary>
/// Source-generated <see cref="JsonSerializerContext"/> for FinnHub HTTP DTOs.
/// </summary>
/// <remarks>
/// <para>
/// Used by <c>FinnHubSearchApiClient</c> to deserialize provider responses without
/// reflection-based metadata, keeping the infrastructure layer compatible with
/// trimming and AOT publish modes.
/// </para>
/// <para>
/// JSON conventions applied to every registered type:
/// </para>
/// <list type="bullet">
///   <item><description>Property names are converted to <c>snake_case</c>.</description></item>
///   <item><description>Output is indented (whitespace) for human-readable diagnostics.</description></item>
///   <item><description>Properties whose value is <c>null</c> are omitted on serialization.</description></item>
///   <item><description>Property-name matching during deserialization is case-insensitive.</description></item>
/// </list>
/// <para>
/// Add new <see cref="JsonSerializableAttribute"/> entries here when a new
/// FinnHub DTO needs to be (de)serialized.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(FinnHubSearchResponse))]
[JsonSerializable(typeof(SearchSymbolResponse))]
[JsonSerializable(typeof(ResolvedSymbol))]
[JsonSerializable(typeof(ToolView))]
[JsonSerializable(typeof(NextAction))]
[JsonSerializable(typeof(RateLimitInfo))]
[JsonSerializable(typeof(ToolResponseEnvelope<SearchSymbolResponse>))]
[JsonSerializable(typeof(GetPeersResponse))]
[JsonSerializable(typeof(ToolResponseEnvelope<GetPeersResponse>))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(FinnHubFinancialsResponse))]
[JsonSerializable(typeof(GetFinancialsSnapshotResponse))]
[JsonSerializable(typeof(ToolResponseEnvelope<GetFinancialsSnapshotResponse>))]
[JsonSerializable(typeof(FinnHubCandleResponse))]
[JsonSerializable(typeof(GetPriceSummaryResponse))]
[JsonSerializable(typeof(ToolResponseEnvelope<GetPriceSummaryResponse>))]
[JsonSerializable(typeof(FinnHubNewsSentimentResponse))]
[JsonSerializable(typeof(FinnHubCompanyNewsArticle[]))]
[JsonSerializable(typeof(GetNewsPulseResponse))]
[JsonSerializable(typeof(ToolResponseEnvelope<GetNewsPulseResponse>))]
public partial class FinnHubJsonContext : JsonSerializerContext;
