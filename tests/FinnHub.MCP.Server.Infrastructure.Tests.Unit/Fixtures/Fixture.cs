// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;

/// <summary>
/// Loads frozen Finnhub HTTP responses from <c>tests/Fixtures/finnhub/*.json</c>.
/// Fixtures are real upstream payloads captured via <c>tests/Fixtures/finnhub/capture.sh</c>,
/// so client tests prove the parsers handle the actual wire shape rather than synthetic data
/// we made up. See <c>tests/Fixtures/README.md</c> for the why and how-to-refresh.
/// </summary>
internal static class Fixture
{
    /// <summary>
    /// Reads a fixture JSON file. <paramref name="name"/> is the filename without the
    /// <c>.json</c> suffix (e.g. <c>quote-AAPL</c>).
    /// </summary>
    public static string LoadFinnHub(string name)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "finnhub", name + ".json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Fixture '{name}' not found at {path}. " +
                "Ensure the file exists under tests/Fixtures/finnhub/ and the test csproj copies it to output.",
                path);
        }
        return File.ReadAllText(path);
    }
}
