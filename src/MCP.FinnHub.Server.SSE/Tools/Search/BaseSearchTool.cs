// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace MCP.FinnHub.Server.SSE.Tools.Search;

public abstract class BaseSearchTool : BaseTool
{
    private const int DefaultLimit = 10;

    private const int MaxLimit = 100;

    private const int MinLimit = 1;

    protected static int ValidateAndGetLimit(IReadOnlyDictionary<string, JsonElement>? args, string paramName = "limit")
    {
        return GetIntParameter(args, paramName, DefaultLimit, MinLimit, MaxLimit);
    }

    protected static string ValidateAndGetQuery(IReadOnlyDictionary<string, JsonElement>? args, string paramName = "query", int minLength = 1, int maxLength = 500)
    {
        ValidateRequiredParameter(args, paramName);

        var query = GetStringParameter(args, paramName);

        if (query.Length < minLength)
        {
            throw new ArgumentException($"Query must be at least {minLength} character(s) long.", paramName);
        }

        if (query.Length > maxLength)
        {
            throw new ArgumentException($"Query must be at most {maxLength} characters long.", paramName);
        }

        return query;
    }
}
