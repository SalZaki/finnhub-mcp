// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;
using FinnHub.MCP.Server.Application.Profiles.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Profiles;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/profile2</c> endpoint as a company snapshot.
/// </summary>
[McpServerToolType]
public sealed class GetCompanyProfileTool(
    IProfilesService profilesService,
    ILogger<GetCompanyProfileTool> logger)
{
    /// <summary>
    /// Returns the company profile for a symbol. <c>view = "summary"</c> drops the
    /// cosmetic fields (logo, phone, weburl); <c>standard</c> and <c>full</c> include them.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.CompanyProfile.Name,
        Title = Constants.Tools.CompanyProfile.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.CompanyProfile.Description)]
    public async Task<ToolResponseEnvelope<GetCompanyProfileResponse>> GetCompanyProfileAsync(
        [Description(Constants.Tools.CompanyProfile.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.CompanyProfile.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.CompanyProfile.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = ProfilesInputValidator.ValidateSymbol(symbol);
            var validatedView = ProfilesInputValidator.ValidateView(view);

            var query = new GetCompanyProfileQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol,
                IncludeCosmeticFields = validatedView != ToolView.Summary
            };

            var result = await profilesService.GetProfileAsync(query, cancellationToken);

            logger.LogInformation(
                "Profile completed for {Symbol} in {ElapsedMs}ms",
                validatedSymbol, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.FromResult(
                result,
                validatedView,
                nextActions: BuildNextActions(result, validatedSymbol),
                explanation: BuildExplanation(result, validatedSymbol));
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "'{Tool}' was cancelled.", ToolName);
            throw;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Validation error in '{Tool}': {Message}", ToolName, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred running '{Tool}'.", ToolName);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogTrace("Finished '{Tool}' in {ElapsedMs}ms.", ToolName, stopwatch.ElapsedMilliseconds);
        }
    }

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetCompanyProfileResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-financials-snapshot", args, "valuation and profitability KPIs"),
            new NextAction("get-peers", args, "industry peer list for comparison"),
            new NextAction("get-quote", args, "current price snapshot")
        ];
    }

    private static string BuildExplanation(Result<GetCompanyProfileResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No profile available for '{symbol}'.";
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"{symbol} — {result.Data.Name} ({result.Data.Industry ?? "n/a"}, {result.Data.Country ?? "n/a"}).");
    }
}
