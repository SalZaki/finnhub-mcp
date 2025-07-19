// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Represents the result of an operation that can either succeed with data or fail with an error.
/// </summary>
/// <typeparam name="T">The type of data returned when the operation succeeds.</typeparam>
/// <remarks>
/// This class implements the Result pattern to provide a type-safe way to handle operation outcomes
/// without relying on exceptions for control flow. It includes JSON serialization support for API responses.
/// The class is immutable after construction to ensure thread safety and prevent accidental modification.
/// </remarks>
/// <example>
/// <code>
/// // Creating a successful result
/// var successResult = new Result&lt;string&gt;().Success("Operation completed");
///
/// // Creating a failed result
/// var failureResult = new Result&lt;string&gt;().Failure("Something went wrong", ResultErrorType.ValidationError);
///
/// // Using the result
/// if (result.IsSuccess)
/// {
///     Console.WriteLine(result.Data);
/// }
/// else
/// {
///     Console.WriteLine($"Error: {result.ErrorMessage}");
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation succeeded and <see cref="Data"/> contains the result;
    /// <c>false</c> if the operation failed and error information is available.
    /// </value>
    [JsonPropertyName("is_success")]
    public bool IsSuccess { get; private init; }

    /// <summary>
    /// Gets the data returned by the operation when successful.
    /// </summary>
    /// <value>
    /// The operation result data when <see cref="IsSuccess"/> is <c>true</c>,
    /// or <c>null</c> when the operation failed.
    /// </value>
    [JsonPropertyName("data")]
    public T? Data { get; private init; }

    /// <summary>
    /// Gets the error message when the operation failed.
    /// </summary>
    /// <value>
    /// A descriptive error message when <see cref="IsSuccess"/> is <c>false</c>,
    /// or <c>null</c> when the operation succeeded.
    /// </value>
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; private init; }

    /// <summary>
    /// Gets the categorized error type when the operation failed.
    /// </summary>
    /// <value>
    /// A string representation of the <see cref="ResultErrorType"/> when <see cref="IsSuccess"/> is <c>false</c>,
    /// or <c>null</c> when the operation succeeded.
    /// </value>
    [JsonPropertyName("error_type")]
    public string? ErrorType { get; private init; }

    /// <summary>
    /// Creates a successful result containing the specified data.
    /// </summary>
    /// <param name="data">The data to include in the successful result.</param>
    /// <returns>A new <see cref="Result{T}"/> instance representing a successful operation.</returns>
    /// <remarks>
    /// This method creates a result where <see cref="IsSuccess"/> is <c>true</c> and
    /// <see cref="Data"/> contains the provided value. Error properties will be <c>null</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = new Result&lt;User&gt;().Success(new User { Name = "John Doe" });
    /// Console.WriteLine(result.IsSuccess); // True
    /// Console.WriteLine(result.Data.Name); // "John Doe"
    /// </code>
    /// </example>
    public Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    /// <summary>
    /// Creates a failed result with the specified error message and optional error type.
    /// </summary>
    /// <param name="errorMessage">A descriptive message explaining why the operation failed.</param>
    /// <param name="errorType">The category of error that occurred. Defaults to <see cref="ResultErrorType.Unknown"/>.</param>
    /// <returns>A new <see cref="Result{T}"/> instance representing a failed operation.</returns>
    /// <remarks>
    /// This method creates a result where <see cref="IsSuccess"/> is <c>false</c>,
    /// <see cref="ErrorMessage"/> contains the provided message, and <see cref="ErrorType"/>
    /// contains the string representation of the error type. The <see cref="Data"/> property will be <c>null</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = new Result&lt;User&gt;().Failure("User not found", ResultErrorType.NotFound);
    /// Console.WriteLine(result.IsSuccess); // False
    /// Console.WriteLine(result.ErrorMessage); // "User not found"
    /// Console.WriteLine(result.ErrorType); // "NotFound"
    /// </code>
    /// </example>
    public Result<T> Failure(
        string errorMessage,
        ResultErrorType errorType = ResultErrorType.Unknown) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorType = errorType.ToString(),
        };
}
