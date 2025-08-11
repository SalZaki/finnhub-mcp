// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net.Mime;
using System.Text.Json.Serialization.Metadata;

namespace FinnHub.MCP.Server.Resources;

/// <summary>
/// Base class for MCP server-side resources providing JSON serialization via <c>ReadResourceResult</c>.
/// </summary>
public abstract class BaseResource : McpServerResource
{
    /// <summary>
    /// Creates a <see cref="ReadResourceResult"/> containing serialized JSON content from the provided data.
    /// </summary>
    /// <typeparam name="T">The type of the data object to serialize.</typeparam>
    /// <param name="data">The object containing the data to be serialized.</param>
    /// <param name="uri">
    /// The URI at which the resource content can be accessed or identified.
    /// This should match the resource's <c>UriTemplate</c> after expansion.
    /// </param>
    /// <param name="typeInfo">
    /// The <see cref="JsonTypeInfo{T}"/> instance used for system‑generated or custom source‑generated serialization.
    /// </param>
    /// <returns>
    /// A <see cref="ReadResourceResult"/> that includes a <see cref="TextResourceContents"/>
    /// instance with the JSON text, MIME type "application/json", and the provided URI.
    /// </returns>
    protected static ReadResourceResult? CreateResponse<T>(
        T data,
        string uri,
        JsonTypeInfo<T> typeInfo)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(typeInfo);

        return new ReadResourceResult
        {
            Contents =
            {
                new TextResourceContents
                {
                    Text = JsonSerializer.Serialize(data, typeInfo),
                    MimeType = MediaTypeNames.Application.Json,
                    Uri = uri
                }
            }
        };
    }
}
