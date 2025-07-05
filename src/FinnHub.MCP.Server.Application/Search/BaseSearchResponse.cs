// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Search;

public abstract class BaseSearchResponse
{
    public string Query { get; init; } = string.Empty;

    public TimeSpan SearchDuration { get; init; }

    public DateTime SearchTimestamp { get; init; }

    public abstract bool HasResults { get; }

    public abstract int TotalCount { get; }

    public string Source { get; init; } = string.Empty;

    public bool IsFromCache { get; init; }
}
