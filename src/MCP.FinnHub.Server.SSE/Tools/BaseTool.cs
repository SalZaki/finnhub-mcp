// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of the MCP FinnHub project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace MCP.FinnHub.Server.SSE.Tools;

public abstract class BaseTool : McpServerTool
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    protected CallToolResponse CreateErrorResponse(object data)
    {
        return new CallToolResponse
        {
            Content =
            [
                new Content
                {
                    Type = "text",
                    Text = JsonSerializer.Serialize(data, this._options),

                }
            ],
            IsError = true
        };
    }

    protected CallToolResponse CreateSuccessResponse(object data)
    {
        return new CallToolResponse
        {
            Content =
            [
                new Content
                {
                    Type = "text",
                    Text = JsonSerializer.Serialize(data, this._options)
                }
            ],
            IsError = false
        };
    }

    protected static void ValidateRequiredParameter(IReadOnlyDictionary<string, JsonElement>? args, string paramName)
    {
        if (args == null || !args.TryGetValue(paramName, out var value))
        {
            throw new ArgumentException($"Required parameter '{paramName}' is missing.");
        }

        var stringValue = value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            _ => value.ToString()
        };

        if (string.IsNullOrWhiteSpace(stringValue))
        {
            throw new ArgumentException($"Required parameter '{paramName}' is missing or empty.");
        }
    }

    protected static int GetIntParameter(IReadOnlyDictionary<string, JsonElement>? args, string paramName, int defaultValue, int? minValue = null, int? maxValue = null)
    {
        if (args?.TryGetValue(paramName, out var value) != true)
        {
            return defaultValue;
        }

        int result;

        if (value.TryGetInt32(out var intValue))
        {
            result = intValue;
        }
        else if (int.TryParse(value.ToString(), out var parsedValue))
        {
            result = parsedValue;
        }
        else
        {
            return defaultValue;
        }

        if (result < minValue)
        {
            throw new ArgumentOutOfRangeException(paramName, result, $"Parameter '{paramName}' must be at least {minValue.Value}.");
        }

        if (maxValue.HasValue && result > maxValue.Value)
        {
            throw new ArgumentOutOfRangeException(paramName, result, $"Parameter '{paramName}' must be at most {maxValue.Value}.");
        }

        return result;
    }

    protected static string GetStringParameter(IReadOnlyDictionary<string, JsonElement>? args, string paramName, string? defaultValue = null)
    {
        if (args?.TryGetValue(paramName, out var value) != true)
        {
            return defaultValue ?? string.Empty;
        }

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString() ?? defaultValue ?? string.Empty,
            JsonValueKind.Null or JsonValueKind.Undefined => defaultValue ?? string.Empty,
            _ => value.ToString()
        };
    }

    protected static bool GetBoolParameter(IReadOnlyDictionary<string, JsonElement>? args, string paramName, bool defaultValue = false)
    {
        if (args?.TryGetValue(paramName, out var value) != true)
        {
            return defaultValue;
        }

        return value.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => bool.TryParse(value.ToString(), out var boolValue) ? boolValue : defaultValue
        };
    }

    protected CallToolResponse CreateValidationErrorResponse(string parameterName, string message)
    {
        return this.CreateErrorResponse(new
        {
            error = "ValidationError",
            parameter = parameterName,
            message = message,
            timestamp = DateTimeOffset.UtcNow
        });
    }

    protected CallToolResponse CreateOperationErrorResponse(string operation, string message, Exception? exception = null)
    {
        var errorData = new
        {
            error = "OperationError",
            operation = operation,
            message = message,
            timestamp = DateTimeOffset.UtcNow
        };

        return this.CreateErrorResponse(errorData);
    }
}
