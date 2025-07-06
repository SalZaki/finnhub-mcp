// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FinnHub.MCP.Server.Application.Options;

/// <summary>
/// Represents a configuration model for FinnHub API endpoints used by the MCP Server.
/// This class defines the structure for endpoint configuration including identification,
/// URL routing, activation status, and descriptive information.
/// </summary>
/// <remarks>
/// This class is typically used in configuration files (appsettings.json) or dependency injection
/// scenarios to define multiple FinnHub API endpoints that the server can interact with.
/// The class uses init-only properties to ensure immutability after construction and includes
/// validation attributes to enforce required fields during configuration binding.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubEndPoint
{
    /// <summary>
    /// Gets the unique identifier name for this FinnHub endpoint.
    /// This name is used to reference the endpoint throughout the application
    /// and should be unique within the endpoint collection.
    /// </summary>
    /// <value>
    /// A non-empty string that uniquely identifies this endpoint configuration.
    /// This property is required and must be provided during configuration.
    /// </value>
    /// <example>
    /// Examples of endpoint names might include:
    /// <list type="bullet">
    /// <item><description>"search-symbol"</description></item>
    /// <item><description>"stock-candles"</description></item>
    /// <item><description>"company-profile"</description></item>
    /// </list>
    /// </example>
    [Required]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the complete URL for this FinnHub API endpoint.
    /// This URL is used to make HTTP requests to the FinnHub API service
    /// and should include the protocol, domain, and path components.
    /// </summary>
    /// <value>
    /// A valid URL string pointing to the FinnHub API endpoint.
    /// This property is required and must be provided during configuration.
    /// </value>
    /// <example>
    /// Example URLs might include:
    /// <list type="bullet">
    /// <item><description>"https://finnhub.io/api/v1/search"</description></item>
    /// <item><description>"https://finnhub.io/api/v1/stock/candle"</description></item>
    /// <item><description>"https://finnhub.io/api/v1/stock/profile2"</description></item>
    /// </list>
    /// </example>
    [Required]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this endpoint is currently active and available for use.
    /// Inactive endpoints will be skipped during endpoint discovery and will not be available
    /// for API calls, allowing for easy enable/disable functionality without removing configuration.
    /// </summary>
    /// <value>
    /// <c>true</c> if the endpoint is active and should be used; <c>false</c> if the endpoint
    /// should be ignored. The default value is <c>true</c>.
    /// </value>
    /// <remarks>
    /// This property is useful for temporarily disabling endpoints during maintenance,
    /// testing scenarios, or when certain API features need to be disabled without
    /// removing the entire configuration entry.
    /// </remarks>
    public bool IsActive { get; init; } = true;

    /// <summary>
    /// Gets an optional human-readable description of what this endpoint provides or does.
    /// This description can be used for documentation, logging, or user interface purposes
    /// to help identify the purpose and functionality of the endpoint.
    /// </summary>
    /// <value>
    /// A descriptive string explaining the endpoint's purpose, or an empty string if no
    /// description is provided. This property is optional and defaults to an empty string.
    /// </value>
    /// <example>
    /// Example descriptions might include:
    /// <list type="bullet">
    /// <item><description>"Search for stock symbols and company information"</description></item>
    /// <item><description>"Retrieve historical stock price data and candle charts"</description></item>
    /// <item><description>"Get detailed company profile and financial information"</description></item>
    /// </list>
    /// </example>
    public string Description { get; init; } = string.Empty;
}
