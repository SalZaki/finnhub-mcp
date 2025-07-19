// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

namespace FinnHub.MCP.Server.Tools;

/// <summary>
/// Source-generated JSON serialization context for tool response Models.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseUpper,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(OperationErrorResponse))]
[JsonSerializable(typeof(ValidationErrorResponse))]
[JsonSerializable(typeof(Result<SearchSymbolResponse>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(object))]
public partial class ToolJsonContext : JsonSerializerContext;
