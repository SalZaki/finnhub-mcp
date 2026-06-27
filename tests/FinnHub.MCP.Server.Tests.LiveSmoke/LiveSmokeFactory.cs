// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FinnHub.MCP.Server.Tests.LiveSmoke;

/// <summary>
/// Boots the real server in-process for live-smoke runs. Shared across tests
/// in a class to amortize the host-startup cost (~1s) over all tools.
/// </summary>
/// <remarks>
/// <para>
/// Inherits the ambient <c>FINNHUB_API_KEY</c> from the process environment —
/// the live-smoke workflow injects the GitHub secret via its <c>env:</c> block.
/// Locally, your <c>.env</c> is loaded by the server's Development startup path.
/// </para>
/// <para>
/// <c>Program.cs</c> derives its configuration base path from the running
/// process exe location (so AOT-published binaries can find their co-located
/// <c>appsettings.json</c>). Under <see cref="WebApplicationFactory{TEntryPoint}"/>
/// the exe is the test-host binary in <c>dotnet/</c>, which has no
/// <c>appsettings.json</c> — leading to a startup
/// <see cref="FileNotFoundException"/>. We override the content root to point
/// at the server project's bin output where the file actually lives.
/// </para>
/// </remarks>
public sealed class LiveSmokeFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(ServerBinPath());
    }

    private static string ServerBinPath()
    {
        // The server's bin path is injected at build time via
        // [AssemblyMetadata("ServerBinDir", ...)] (see the .csproj). Build-time injection survives
        // the test-project directory restructures that the old runtime parent-directory walk did not.
        var serverBin = typeof(LiveSmokeFactory).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "ServerBinDir")?.Value;

        if (string.IsNullOrEmpty(serverBin))
        {
            throw new InvalidOperationException(
                "ServerBinDir assembly metadata was not injected — check the test project's AssemblyAttribute.");
        }

        if (!Directory.Exists(serverBin))
        {
            throw new DirectoryNotFoundException(
                $"Server bin path not found: {serverBin}. " +
                "Did you `dotnet build` the server project before running live-smoke?");
        }

        return serverBin;
    }
}
