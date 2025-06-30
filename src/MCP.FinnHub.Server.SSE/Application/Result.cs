// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace MCP.FinnHub.Server.SSE.Application;

public sealed class Result<T>
{
    [JsonPropertyName("is_success")]
    public bool IsSuccess { get; private init; }

    [JsonPropertyName("data")]
    public T? Data { get; private init; }

    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; private init; }

    [JsonPropertyName("error_type")]
    public string? ErrorType { get; private init; }

    public Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public Result<T> Failure(
        string errorMessage,
        ResultErrorType errorType = ResultErrorType.Unknown) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorType = errorType.ToString(),
        };
}
