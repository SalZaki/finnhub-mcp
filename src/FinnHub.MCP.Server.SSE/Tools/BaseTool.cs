// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.SSE.Tools;

/// <summary>
/// Abstract base class for MCP Server tools that provides common functionality for parameter validation,
/// response creation, and error handling. All MCP tools should inherit from this class to ensure
/// consistent behavior and response formatting.
/// </summary>
public abstract class BaseTool : McpServerTool
{
    /// <summary>
    /// JSON serialization options configured for consistent output formatting across all tools.
    /// Uses snake_case_upper naming policy and excludes null values from serialization.
    /// </summary>
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Creates a standardized error response with the specified data.
    /// </summary>
    /// <param name="data">The error data to include in the response. This will be serialized to JSON.</param>
    /// <returns>A <see cref="CallToolResponse"/> with IsError set to true and the serialized data as content.</returns>
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

    /// <summary>
    /// Creates a standardized success response with the specified data.
    /// </summary>
    /// <param name="data">The success data to include in the response. This will be serialized to JSON.</param>
    /// <returns>A <see cref="CallToolResponse"/> with IsError set to false and the serialized data as content.</returns>
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

    /// <summary>
    /// Validates that a required parameter exists in the arguments dictionary and is not null, empty, or whitespace.
    /// </summary>
    /// <param name="args">The arguments dictionary to validate against.</param>
    /// <param name="paramName">The name of the required parameter.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the parameter is missing, null, empty, or contains only whitespace.
    /// </exception>
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

    /// <summary>
    /// Retrieves an integer parameter from the arguments dictionary with optional validation.
    /// </summary>
    /// <param name="args">The arguments dictionary to retrieve the parameter from.</param>
    /// <param name="paramName">The name of the parameter to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the parameter is not found or cannot be parsed.</param>
    /// <param name="minValue">Optional minimum value for validation. If specified, values below this will throw an exception.</param>
    /// <param name="maxValue">Optional maximum value for validation. If specified, values above this will throw an exception.</param>
    /// <returns>The integer value of the parameter, or the default value if not found or unparseable.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the parameter value is outside the specified min/max range.
    /// </exception>
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

    /// <summary>
    /// Retrieves a string parameter from the arguments dictionary.
    /// </summary>
    /// <param name="args">The arguments dictionary to retrieve the parameter from.</param>
    /// <param name="paramName">The name of the parameter to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the parameter is not found. Defaults to null.</param>
    /// <returns>
    /// The string value of the parameter, the default value if not found, or an empty string if both
    /// the parameter and default value are null.
    /// </returns>
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

    /// <summary>
    /// Retrieves a boolean parameter from the arguments dictionary.
    /// </summary>
    /// <param name="args">The arguments dictionary to retrieve the parameter from.</param>
    /// <param name="paramName">The name of the parameter to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the parameter is not found or cannot be parsed. Defaults to false.</param>
    /// <returns>The boolean value of the parameter, or the default value if not found or unparseable.</returns>
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

    /// <summary>
    /// Creates a standardized validation error response for parameter validation failures.
    /// </summary>
    /// <param name="parameterName">The name of the parameter that failed validation.</param>
    /// <param name="message">A descriptive error message explaining the validation failure.</param>
    /// <returns>A <see cref="CallToolResponse"/> formatted as a validation error with timestamp.</returns>
    protected CallToolResponse CreateValidationErrorResponse(string parameterName, string message)
    {
        return this.CreateErrorResponse(new
        {
            error = "ValidationError",
            parameter = parameterName,
            message,
            timestamp = DateTimeOffset.UtcNow
        });
    }

    /// <summary>
    /// Creates a standardized operation error response for general operation failures.
    /// </summary>
    /// <param name="operation">The name of the operation that failed.</param>
    /// <param name="message">A descriptive error message explaining the operation failure.</param>
    /// <param name="exception">Optional exception that caused the failure. Currently not included in response but available for future use.</param>
    /// <returns>A <see cref="CallToolResponse"/> formatted as an operation error with timestamp.</returns>
    protected CallToolResponse CreateOperationErrorResponse(string operation, string message, Exception? exception = null)
    {
        var errorData = new
        {
            error = "OperationError",
            operation,
            message,
            timestamp = DateTimeOffset.UtcNow
        };

        return this.CreateErrorResponse(errorData);
    }
}
