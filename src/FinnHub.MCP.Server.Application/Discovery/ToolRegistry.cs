// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Application.Discovery;

/// <summary>
/// BM25 keyword index over a fixed set of <see cref="ToolDescriptor"/>s. Pure C#, no external
/// dependency and no embeddings (per spec §3 P7). The index is built once at construction;
/// the descriptor set is supplied by the host so the Application layer stays decoupled from the
/// tool registrations in <c>Program.cs</c>.
/// </summary>
public sealed partial class ToolRegistry : IToolRegistry
{
    // Standard BM25 free parameters.
    private const double K1 = 1.2d;
    private const double B = 0.75d;

    // Tokens with no discriminating value across short natural-language intents.
    private static readonly FrozenSet<string> s_stopWords = FrozenSet.Create(
        StringComparer.Ordinal,
        "a", "an", "and", "any", "are", "as", "at", "be", "been", "by", "can", "do", "does",
        "for", "from", "get", "give", "how", "i", "in", "into", "is", "it", "its", "me", "my",
        "of", "on", "or", "show", "so", "tell", "that", "the", "their", "them", "there", "this",
        "to", "was", "were", "what", "whats", "when", "where", "which", "who", "will", "with",
        "would", "you", "your");

    private readonly ToolDescriptor[] _descriptors;
    private readonly Dictionary<string, int>[] _termFrequencies;
    private readonly int[] _docLengths;
    private readonly Dictionary<string, int> _documentFrequency;
    private readonly double _averageDocLength;

    /// <summary>
    /// Builds the index over <paramref name="descriptors"/>.
    /// </summary>
    /// <param name="descriptors">The tool descriptors to index. Order is preserved as the tie-break for equal scores.</param>
    public ToolRegistry(IEnumerable<ToolDescriptor> descriptors)
    {
        ArgumentNullException.ThrowIfNull(descriptors);

        this._descriptors = [.. descriptors];
        this._termFrequencies = new Dictionary<string, int>[this._descriptors.Length];
        this._docLengths = new int[this._descriptors.Length];
        this._documentFrequency = new Dictionary<string, int>(StringComparer.Ordinal);

        long totalLength = 0;

        for (var i = 0; i < this._descriptors.Length; i++)
        {
            var tokens = Tokenize(BuildDocument(this._descriptors[i]));
            var termFrequency = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (var token in tokens)
            {
                termFrequency[token] = termFrequency.GetValueOrDefault(token) + 1;
            }

            this._termFrequencies[i] = termFrequency;
            this._docLengths[i] = tokens.Count;
            totalLength += tokens.Count;

            foreach (var term in termFrequency.Keys)
            {
                this._documentFrequency[term] = this._documentFrequency.GetValueOrDefault(term) + 1;
            }
        }

        this._averageDocLength = this._descriptors.Length == 0
            ? 0d
            : (double)totalLength / this._descriptors.Length;
    }

    /// <inheritdoc />
    public IReadOnlyList<ToolDescriptor> Descriptors => this._descriptors;

    /// <inheritdoc />
    public IReadOnlyList<ToolMatch> Search(string intent, int topN = 5)
    {
        if (string.IsNullOrWhiteSpace(intent) || topN <= 0 || this._descriptors.Length == 0)
        {
            return [];
        }

        var queryTerms = Tokenize(intent).Distinct(StringComparer.Ordinal).ToArray();

        if (queryTerms.Length == 0)
        {
            return [];
        }

        var scored = new List<ToolMatch>(this._descriptors.Length);

        for (var i = 0; i < this._descriptors.Length; i++)
        {
            if (!this._descriptors[i].Searchable)
            {
                continue;
            }

            var score = this.ScoreDocument(i, queryTerms);

            if (score > 0d)
            {
                scored.Add(new ToolMatch(this._descriptors[i], score));
            }
        }

        // Descending score; OrderBy is stable, so descriptor order breaks ties.
        return [.. scored.OrderByDescending(m => m.Score).Take(topN)];
    }

    private double ScoreDocument(int docIndex, IEnumerable<string> queryTerms)
    {
        var termFrequency = this._termFrequencies[docIndex];
        var docLength = this._docLengths[docIndex];
        var score = 0d;

        foreach (var term in queryTerms)
        {
            if (!termFrequency.TryGetValue(term, out var tf))
            {
                continue;
            }

            var df = this._documentFrequency.GetValueOrDefault(term);
            var idf = Math.Log(1d + ((this._descriptors.Length - df + 0.5d) / (df + 0.5d)));

            var denominator = tf + (K1 * (1d - B + (B * docLength / this._averageDocLength)));
            score += idf * (tf * (K1 + 1d)) / denominator;
        }

        return score;
    }

    // Field weights, applied as token repetition (a simple BM25F). Identifying fields (name,
    // title) and the curated example intents carry more signal for intent matching than the
    // auto-generated description, which contains incidental tokens (tickers, ISINs, parameter
    // names) that would otherwise let a stray word dominate the ranking.
    private const int NameTitleWeight = 2;
    private const int ExampleWeight = 4;

    private static string BuildDocument(ToolDescriptor descriptor)
    {
        var examples = string.Join(' ', descriptor.Examples);
        var parts = new List<string>();

        for (var i = 0; i < NameTitleWeight; i++)
        {
            parts.Add(descriptor.Name);
            parts.Add(descriptor.Title);
        }

        parts.Add(descriptor.Description);

        for (var i = 0; i < ExampleWeight; i++)
        {
            parts.Add(examples);
        }

        return string.Join(' ', parts);
    }

    private static List<string> Tokenize(string text)
    {
        var tokens = new List<string>();

        foreach (var raw in TokenSplitter().Split(text))
        {
            if (raw.Length < 2)
            {
                continue;
            }

            var token = raw.ToLowerInvariant();

            if (!s_stopWords.Contains(token))
            {
                tokens.Add(token);
            }
        }

        return tokens;
    }

    [GeneratedRegex(@"[^a-zA-Z0-9]+", RegexOptions.Compiled)]
    private static partial Regex TokenSplitter();
}
