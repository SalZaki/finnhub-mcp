// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Defines the categories of errors that can occur during operations in the FinnHub MCP Server.
/// </summary>
/// <remarks>
/// This enumeration is used in conjunction with the <see cref="Result{T}"/> class to provide
/// structured error categorization. It helps consumers of the API understand the nature of
/// failures and implement appropriate error handling strategies.
/// </remarks>
public enum ResultErrorType
{
    /// <summary>
    /// Indicates that a requested resource or entity could not be found.
    /// </summary>
    /// <remarks>
    /// This error type is typically used when searching for specific symbols, exchanges,
    /// or other financial data that doesn't exist in the system or external API.
    /// </remarks>
    NotFound,

    /// <summary>
    /// Indicates an unspecified or unclassified error occurred.
    /// </summary>
    /// <remarks>
    /// This is the default error type used when the specific cause of failure cannot be
    /// determined or doesn't fit into other defined categories. It should be used sparingly
    /// and accompanied by descriptive error messages.
    /// </remarks>
    Unknown,

    /// <summary>
    /// Indicates that the provided query or request parameters are invalid or malformed.
    /// </summary>
    /// <remarks>
    /// This error type is used when input validation fails, such as when query strings
    /// are too short, too long, contain invalid characters, or don't meet format requirements.
    /// </remarks>
    InvalidQuery,

    /// <summary>
    /// Indicates that the external service (FinnHub API) is temporarily unavailable.
    /// </summary>
    /// <remarks>
    /// This error type is used when the FinnHub API returns service unavailable responses,
    /// is experiencing downtime, or when connectivity issues prevent successful API calls.
    /// Consumers should implement retry logic for this error type.
    /// </remarks>
    ServiceUnavailable,

    /// <summary>
    /// Indicates that an operation exceeded its allowed time limit.
    /// </summary>
    /// <remarks>
    /// This error type is used when API calls or internal operations take longer than
    /// the configured timeout period. It suggests temporary performance issues that
    /// may resolve with retry attempts.
    /// </remarks>
    Timeout,

    /// <summary>
    /// Indicates that the response from an external service was invalid or could not be processed.
    /// </summary>
    /// <remarks>
    /// This error type is used when the FinnHub API returns data in an unexpected format,
    /// contains invalid JSON, or doesn't conform to the expected schema. It typically
    /// indicates issues with the external service or API version mismatches.
    /// </remarks>
    InvalidResponse
}
