// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;
using FinnHub.MCP.Server.Application.Profiles.Services;
using FinnHub.MCP.Server.Tools.Profiles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Profiles;

public sealed class GetCompanyProfileToolTests
{
    private readonly IProfilesService _service = Substitute.For<IProfilesService>();
    private readonly GetCompanyProfileTool _sut;

    public GetCompanyProfileToolTests()
    {
        this._sut = new GetCompanyProfileTool(this._service, NullLogger<GetCompanyProfileTool>.Instance);
    }

    [Fact]
    public async Task GetCompanyProfileAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetCompanyProfileAsync("!!!"));
    }

    [Fact]
    public async Task GetCompanyProfileAsync_SummaryView_OmitsCosmeticFields()
    {
        this._service.GetProfileAsync(Arg.Any<GetCompanyProfileQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetCompanyProfileResponse>().Success(
                new GetCompanyProfileResponse { Ticker = "AAPL", Name = "Apple" }));

        await this._sut.GetCompanyProfileAsync("AAPL", view: "summary");

        await this._service.Received(1).GetProfileAsync(
            Arg.Is<GetCompanyProfileQuery>(q => !q.IncludeCosmeticFields),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCompanyProfileAsync_StandardView_RequestsCosmeticFields()
    {
        this._service.GetProfileAsync(Arg.Any<GetCompanyProfileQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetCompanyProfileResponse>().Success(
                new GetCompanyProfileResponse { Ticker = "AAPL", Name = "Apple" }));

        await this._sut.GetCompanyProfileAsync("AAPL", view: "standard");

        await this._service.Received(1).GetProfileAsync(
            Arg.Is<GetCompanyProfileQuery>(q => q.IncludeCosmeticFields),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCompanyProfileAsync_Success_PopulatesNextActions()
    {
        this._service.GetProfileAsync(Arg.Any<GetCompanyProfileQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetCompanyProfileResponse>().Success(
                new GetCompanyProfileResponse { Ticker = "AAPL", Name = "Apple" }));

        var envelope = await this._sut.GetCompanyProfileAsync("AAPL");

        Assert.Equal(3, envelope.NextActions.Count);
        Assert.Equal("get-financials-snapshot", envelope.NextActions[0].Tool);
        Assert.Equal("get-peers", envelope.NextActions[1].Tool);
        Assert.Equal("get-quote", envelope.NextActions[2].Tool);
    }
}
