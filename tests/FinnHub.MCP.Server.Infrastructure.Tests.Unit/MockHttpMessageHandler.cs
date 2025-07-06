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

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.LastRequest = request;

        if (this._exception != null)
        {
            throw this._exception;
        }

        return Task.FromResult(this._response ?? new HttpResponseMessage(HttpStatusCode.OK));
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
