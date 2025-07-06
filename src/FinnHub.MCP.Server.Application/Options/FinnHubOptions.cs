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
/// Represents the configuration options for integrating with the FinnHub API service.
/// This class encapsulates all necessary settings including authentication credentials,
/// service endpoints, timeout configurations, and available API endpoints.
/// </summary>
/// <remarks>
/// <para>
/// This class is designed to be used with the .NET configuration system and is typically
/// populated from configuration sources such as appsettings.json, environment variables,
/// or other configuration providers through dependency injection.
/// </para>
/// <para>
/// The class uses init-only properties to ensure immutability after construction and
/// includes validation attributes to enforce configuration requirements during binding.
/// All properties are validated when the configuration is bound to ensure the FinnHub
/// service can be properly initialized and used.
/// </para>
/// </remarks>
/// <example>
/// Example configuration in appsettings.json:
/// <code>
/// {
///   "FinnHub": {
///     "ApiKey": "your-api-key-here",
///     "BaseUrl": "https://finnhub.io/api/v1",
///     "TimeoutSeconds": 30,
///     "EndPoints": [
///       {
///         "Name": "search-symbol",
///         "Url": "https://finnhub.io/api/v1/search",
///         "IsActive": true,
///         "Description": "Search for stock symbols"
///       }
///     ]
///   }
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class FinnHubOptions
{
    /// <summary>
    /// Gets the API key required for authenticating with the FinnHub API service.
    /// This key is obtained from the FinnHub developer portal and is required for all API calls.
    /// </summary>
    /// <value>
    /// A non-empty string containing the FinnHub API key. This property is required
    /// and must be provided during configuration to enable API access.
    /// </value>
    /// <remarks>
    /// <para>
    /// The API key should be kept secure and not exposed in client-side code or public repositories.
    /// It's recommended to store this value in secure configuration sources such as:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Environment variables</description></item>
    /// <item><description>Azure Key Vault or similar secret management services</description></item>
    /// <item><description>User secrets during development</description></item>
    /// <item><description>Secure configuration providers in production</description></item>
    /// </list>
    /// </remarks>
    [Required]
    public string ApiKey { get; init; } = string.Empty;

    /// <summary>
    /// Gets the base URL for the FinnHub API service.
    /// This URL serves as the foundation for constructing endpoint-specific URLs
    /// and should include the protocol, domain, and base path.
    /// </summary>
    /// <value>
    /// A valid URL string pointing to the FinnHub API base endpoint.
    /// This property is required and must be provided during configuration.
    /// The typical value is "https://finnhub.io/api/v1".
    /// </value>
    /// <remarks>
    /// The base URL is combined with individual endpoint paths to form complete API URLs.
    /// This allows for flexibility in targeting different API versions or environments
    /// (development, staging, production) by changing only the base URL configuration.
    /// </remarks>
    /// <example>
    /// Common base URL values:
    /// <list type="bullet">
    /// <item><description>"https://finnhub.io/api/v1" (production)</description></item>
    /// <item><description>"https://sandbox.finnhub.io/api/v1" (sandbox/testing)</description></item>
    /// </list>
    /// </example>
    [Required]
    public string BaseUrl { get; init; } = string.Empty;

    /// <summary>
    /// Gets the timeout duration in seconds for HTTP requests made to the FinnHub API.
    /// This value controls how long the client will wait for API responses before timing out.
    /// </summary>
    /// <value>
    /// An integer value between 1 and 60 seconds representing the HTTP request timeout.
    /// The default value is 10 seconds if not specified in configuration.
    /// </value>
    /// <remarks>
    /// <para>
    /// The timeout value should be chosen based on the expected response times of FinnHub API
    /// endpoints and the requirements of your application. Consider the following factors:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Network latency and reliability</description></item>
    /// <item><description>API endpoint response characteristics</description></item>
    /// <item><description>User experience requirements</description></item>
    /// <item><description>Rate limiting and API quotas</description></item>
    /// </list>
    /// <para>
    /// A shorter timeout provides better responsiveness but may result in timeouts for
    /// legitimate slow responses. A longer timeout is more forgiving but may impact
    /// user experience if the API is unresponsive.
    /// </para>
    /// </remarks>
    [Range(1, 60)]
    public int TimeoutSeconds { get; init; } = 10;

    /// <summary>
    /// Gets the collection of FinnHub API endpoints available for use by the MCP server.
    /// Each endpoint defines a specific API operation with its configuration details.
    /// </summary>
    /// <value>
    /// A list of <see cref="FinnHubEndPoint"/> objects representing the available API endpoints.
    /// This property defaults to an empty list if no endpoints are configured.
    /// </value>
    /// <remarks>
    /// <para>
    /// The endpoints collection defines which FinnHub API operations are available to the MCP server
    /// and how they should be configured. Each endpoint can be individually activated or deactivated
    /// without affecting other endpoints, providing flexibility in API feature management.
    /// </para>
    /// <para>
    /// Endpoints are typically discovered and registered automatically based on this configuration,
    /// making them available as tools within the MCP server framework.
    /// </para>
    /// </remarks>
    /// <example>
    /// Example endpoint configuration:
    /// <code>
    /// EndPoints = [
    ///     new FinnHubEndPoint
    ///     {
    ///         Name = "search-symbol",
    ///         Url = "https://finnhub.io/api/v1/search",
    ///         IsActive = true,
    ///         Description = "Search for stock symbols and company information"
    ///     },
    ///     new FinnHubEndPoint
    ///     {
    ///         Name = "stock-candles",
    ///         Url = "https://finnhub.io/api/v1/stock/candle",
    ///         IsActive = true,
    ///         Description = "Get historical stock price data"
    ///     }
    /// ]
    /// </code>
    /// </example>
    public List<FinnHubEndPoint> EndPoints { get; init; } = [];
}
