// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace MCP.FinnHub.Server.SSE.Application;

public enum ResultErrorType
{
    NotFound,
    Unknown,
    InvalidQuery,
    ServiceUnavailable,
    Timeout,
    InvalidResponse
}
