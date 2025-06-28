// --------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of the MCP FinnHub project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Net.Http.Headers;

namespace MCP.FinnHub.Server.SSE.Tests.Unit;

public class MockHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
    : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handlerFunc =
        handlerFunc ?? throw new ArgumentNullException(nameof(handlerFunc));

    public MockHttpMessageHandler()
        : this(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
    {
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) => this._handlerFunc(request);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Cleanup if needed
        }

        base.Dispose(disposing);
    }
}
