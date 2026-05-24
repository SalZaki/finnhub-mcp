// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

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
        // The test assembly resolves to tests/.../bin/Debug|Release/net10.0/.
        // The server's bin output is at src/FinnHub.MCP.Server/bin/<config>/net10.0/.
        // Walk back to the repo root then forward to the server project's bin.
        var here = AppContext.BaseDirectory;
        var configDir = new DirectoryInfo(here).Parent
                        ?? throw new InvalidOperationException("Could not find net10.0 parent.");
        var configName = configDir.Name; // "Debug" or "Release"

        // configDir is "Debug" or "Release" → ../bin → ../<test-project>
        // → ../tests → ../<repo-root>. That's four .Parent hops.
        var repoRoot = configDir.Parent?.Parent?.Parent?.Parent
                       ?? throw new InvalidOperationException("Could not walk back to repo root.");

        var serverBin = Path.Combine(
            repoRoot.FullName, "src", "FinnHub.MCP.Server", "bin", configName, "net10.0");

        if (!Directory.Exists(serverBin))
        {
            throw new DirectoryNotFoundException(
                $"Server bin path not found: {serverBin}. " +
                "Did you `dotnet build` the server project before running live-smoke?");
        }

        return serverBin;
    }
}
