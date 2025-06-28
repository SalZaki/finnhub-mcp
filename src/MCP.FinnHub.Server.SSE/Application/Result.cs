// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace MCP.FinnHub.Server.SSE.Application;

[ExcludeFromCodeCoverage]
public sealed class Result<T>
{
    public bool IsSuccess { get; private init; }

    public T? Data { get; private init; }

    public string? ErrorMessage { get; private init; }

    public ResultErrorType ErrorType { get; private init; }

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
        ErrorType = errorType
    };
}
