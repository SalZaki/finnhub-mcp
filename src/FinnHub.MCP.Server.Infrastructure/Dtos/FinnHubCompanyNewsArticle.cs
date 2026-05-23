// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>Wire DTO for a single article in the Finnhub <c>/company-news</c> array response.</summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubCompanyNewsArticle
{
    /// <summary>Article headline.</summary>
    [JsonPropertyName("headline")]
    public string? Headline { get; init; }

    /// <summary>Public URL of the article.</summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>Publisher name.</summary>
    [JsonPropertyName("source")]
    public string? Source { get; init; }

    /// <summary>Unix-epoch (seconds) publication time.</summary>
    [JsonPropertyName("datetime")]
    public long Datetime { get; init; }
}
