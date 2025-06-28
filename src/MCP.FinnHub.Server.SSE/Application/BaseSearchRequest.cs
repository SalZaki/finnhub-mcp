// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace MCP.FinnHub.Server.SSE.Application;

public abstract class BaseSearchQuery
{
    public int Limit { get; init; } = 10;

    public virtual void Validate()
    {
        if (this.Limit is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(this.Limit), this.Limit, "Limit must be between 1 and 100.");
        }
    }
}
