// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;

namespace FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

public abstract class SearchSymbolException(string message, Exception? inner = null)
    : FinnHubMcpServerException(message, inner)
{
    public virtual string ErrorCode => "SEARCH_SYMBOL_ERROR";
}

public sealed class SearchSymbolHttpException(string message, HttpStatusCode statusCode, string? responseContent = null)
    : SearchSymbolException(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    public string? ResponseContent { get; } = responseContent;
}

public sealed class SearchSymbolTimeoutException(string message, Exception? innerException = null)
    : SearchSymbolException(message, innerException);

public sealed class SearchSymbolDeserializationException(string message, Exception? innerException = null)
    : SearchSymbolException(message, innerException);

public sealed class SearchSymbolCancelledException(string message, Exception? innerException = null)
    : SearchSymbolException(message, innerException);

public sealed class SearchSymbolUnexpectedException(string message, Exception? innerException = null)
    : SearchSymbolException(message, innerException);
