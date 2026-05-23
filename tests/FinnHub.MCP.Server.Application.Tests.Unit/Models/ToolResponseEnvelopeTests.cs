// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Models;

public sealed class ToolResponseEnvelopeTests
{
    private static readonly Dictionary<string, string> s_symbolArgs =
        new(StringComparer.Ordinal) { ["symbol"] = "AAPL" };

    [Fact]
    public void Success_PopulatesAllFields()
    {
        var data = new SearchSymbolResponse { Symbols = [] };
        var nextActions = new[] { new NextAction("get-price-summary", s_symbolArgs, "stats") };

        var envelope = EnvelopeFactory.Success(
            data,
            ToolView.Standard,
            nextActions,
            explanation: "ok",
            sentimentSource: null,
            premium: false);

        Assert.True(envelope.IsSuccess);
        Assert.Same(data, envelope.Data);
        Assert.Null(envelope.ErrorMessage);
        Assert.Null(envelope.ErrorType);
        Assert.Equal(ToolView.Standard, envelope.View);
        Assert.Single(envelope.NextActions);
        Assert.Equal("ok", envelope.Explanation);
        Assert.Equal(0, envelope.ApproxTokens);
        Assert.Null(envelope.RateLimit);
        Assert.Null(envelope.SentimentSource);
        Assert.False(envelope.Premium);
    }

    [Fact]
    public void Failure_PopulatesErrorFields()
    {
        var envelope = EnvelopeFactory.Failure<SearchSymbolResponse>(
            "not found",
            ResultErrorType.NotFound,
            ToolView.Summary);

        Assert.False(envelope.IsSuccess);
        Assert.Null(envelope.Data);
        Assert.Equal("not found", envelope.ErrorMessage);
        Assert.Equal("NotFound", envelope.ErrorType);
        Assert.Equal(ToolView.Summary, envelope.View);
        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public void FromResult_SuccessResult_MapsToSuccessEnvelope()
    {
        var data = new SearchSymbolResponse { Symbols = [] };
        var result = new Result<SearchSymbolResponse>().Success(data);

        var envelope = EnvelopeFactory.FromResult(result, ToolView.Standard, explanation: "found");

        Assert.True(envelope.IsSuccess);
        Assert.Same(data, envelope.Data);
        Assert.Equal(ToolView.Standard, envelope.View);
        Assert.Equal("found", envelope.Explanation);
    }

    [Fact]
    public void FromResult_FailureResult_MapsToFailureEnvelopeWithParsedErrorType()
    {
        var result = new Result<SearchSymbolResponse>()
            .Failure("boom", ResultErrorType.ServiceUnavailable);

        var envelope = EnvelopeFactory.FromResult(result);

        Assert.False(envelope.IsSuccess);
        Assert.Null(envelope.Data);
        Assert.Equal("boom", envelope.ErrorMessage);
        Assert.Equal("ServiceUnavailable", envelope.ErrorType);
    }

    [Fact]
    public void FromResult_UnknownErrorTypeString_FallsBackToUnknown()
    {
        var result = new Result<SearchSymbolResponse>().Failure("???", ResultErrorType.Unknown);

        var envelope = EnvelopeFactory.FromResult(result);

        Assert.Equal("Unknown", envelope.ErrorType);
    }

    [Fact]
    public void FromResult_NullResult_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            EnvelopeFactory.FromResult<SearchSymbolResponse>(null!));
    }

    [Fact]
    public void FromResult_PremiumRequiredFailure_AutoSetsPremiumFlag()
    {
        var result = new Result<SearchSymbolResponse>()
            .Failure("premium-only", ResultErrorType.PremiumRequired);

        var envelope = EnvelopeFactory.FromResult(result);

        Assert.False(envelope.IsSuccess);
        Assert.Equal("PremiumRequired", envelope.ErrorType);
        Assert.True(envelope.Premium);
    }

    [Fact]
    public void FromResult_NonPremiumFailure_LeavesPremiumFalse()
    {
        var result = new Result<SearchSymbolResponse>()
            .Failure("nope", ResultErrorType.NotFound);

        var envelope = EnvelopeFactory.FromResult(result);

        Assert.False(envelope.Premium);
    }
}
