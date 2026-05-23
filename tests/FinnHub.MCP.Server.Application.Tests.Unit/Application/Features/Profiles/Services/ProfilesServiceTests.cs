// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Profiles.Clients;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;
using FinnHub.MCP.Server.Application.Profiles.Services;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Profiles.Services;

public sealed class ProfilesServiceTests
{
    private readonly IProfilesApiClient _apiClient = Substitute.For<IProfilesApiClient>();
    private readonly IFinnHubCache _cache = Substitute.For<IFinnHubCache>();
    private readonly ProfilesService _sut;

    public ProfilesServiceTests()
    {
        this._cache
            .GetOrCreateAsync(
                Arg.Any<string>(),
                Arg.Any<CacheTier>(),
                Arg.Any<Func<CancellationToken, ValueTask<GetCompanyProfileResponse>>>(),
                Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Func<CancellationToken, ValueTask<GetCompanyProfileResponse>>>()(CancellationToken.None));

        this._sut = new ProfilesService(this._apiClient, this._cache, NullLogger<ProfilesService>.Instance);
    }

    [Fact]
    public async Task GetProfileAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetProfileAsync(null!));
    }

    [Fact]
    public async Task GetProfileAsync_PopulatedProfile_ReturnsSuccess()
    {
        var query = new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetProfileAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetCompanyProfileResponse { Ticker = "AAPL", Name = "Apple Inc.", Country = "US" });

        var result = await this._sut.GetProfileAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal("Apple Inc.", result.Data!.Name);
    }

    [Fact]
    public async Task GetProfileAsync_EmptyName_ReturnsNotFound()
    {
        var query = new GetCompanyProfileQuery { QueryId = "q1", Symbol = "UNKN" };
        this._apiClient.GetProfileAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetCompanyProfileResponse { Ticker = "UNKN", Name = null });

        var result = await this._sut.GetProfileAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetProfileAsync_PremiumRequired_MapsCorrectly()
    {
        var query = new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetProfileAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/api/v1/stock/profile2"));

        var result = await this._sut.GetProfileAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }
}
