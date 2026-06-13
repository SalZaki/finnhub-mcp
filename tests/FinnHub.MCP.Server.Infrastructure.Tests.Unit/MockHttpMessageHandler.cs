// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit;

/// <summary>
/// Mock HTTP message handler for unit testing HTTP client behavior.
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage? _response;
    private Exception? _exception;

    public HttpRequestMessage? LastRequest { get; private set; }

    public void SetResponse(HttpStatusCode statusCode, string content)
    {
        this._response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        this._exception = null;
    }

    public void SetException(Exception exception)
    {
        this._exception = exception;
        this._response = null;
    }

    /// <summary>
    /// Sets a response with the given status and lets the caller configure it (e.g. add response
    /// headers). Lets header-sensitive tests share this handler instead of hand-rolling a stub.
    /// </summary>
    public void SetResponse(HttpStatusCode statusCode, Action<HttpResponseMessage> configureResponse)
    {
        var response = new HttpResponseMessage(statusCode);
        configureResponse(response);
        this._response = response;
        this._exception = null;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.LastRequest = request;

        if (this._exception != null)
        {
            throw this._exception;
        }

        var response = this._response ?? new HttpResponseMessage(HttpStatusCode.OK);
        response.RequestMessage = request;
        return Task.FromResult(response);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this._response?.Dispose();
        }
        base.Dispose(disposing);
    }
}
