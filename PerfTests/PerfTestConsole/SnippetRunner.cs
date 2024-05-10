using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTestConsole
{
    public record SnippetRunResult(string name, int nbOfRuns, double averageMs);

    internal class SnippetRunner
    {

        public IEnumerable<SnippetRunResult> Run(Snippet snippet, int nbOfRuns)
        {
            Dictionary<string, List<SnippetTimer>> allRunsCheckpoints = new();

            for (int i = 0; i < nbOfRuns; i++)
            {
                var runResults = snippet.RunSnippet();

                foreach (var runResult in runResults)
                {
                    if (!allRunsCheckpoints.ContainsKey(runResult.name))
                    {
                        allRunsCheckpoints[runResult.name] = new List<SnippetTimer>();
                    }

                    allRunsCheckpoints[runResult.name].Add(runResult);
                }

            }

            return _mergeCheckpointsStats(allRunsCheckpoints);
        }

        private IEnumerable<SnippetRunResult> _mergeCheckpointsStats(Dictionary<string, List<SnippetTimer>> allRunsCheckpoints)
        {
            List<SnippetRunResult> results = new();

            foreach (var kv in allRunsCheckpoints)
            {
                results.Add(_mergeSignleCheckpoint(kv.Key, kv.Value));
            }

            return results;
        }

        private SnippetRunResult _mergeSignleCheckpoint(string key, List<SnippetTimer> value) => new SnippetRunResult(
                key,
                value.Count,
                value.Select(v => v.ms).Average());
    }
}
