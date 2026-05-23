// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;
using FinnHub.MCP.Server.Resources.Status;

namespace FinnHub.MCP.Server.Resources;

/// <summary>
/// Source-generated <see cref="JsonSerializerContext"/> for MCP resource wire shapes.
/// </summary>
/// <remarks>
/// The MCP SDK only marshals a fixed set of resource handler return types
/// (<c>ResourceContents</c>, <c>string</c>, etc.) — anything else throws
/// <c>InvalidOperationException</c>. Resource handlers serialize their payload
/// to JSON via this context and return a <see cref="string"/>; the SDK wraps the
/// result in a <c>TextResourceContents</c> using the declared <c>MimeType</c>.
/// </remarks>
[ExcludeFromCodeCoverage]
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ApiStatusSnapshot))]
[JsonSerializable(typeof(ExchangesResponse))]
internal sealed partial class ResourceJsonContext : JsonSerializerContext;
