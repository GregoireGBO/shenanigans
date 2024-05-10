using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTestConsole
{
    public record SnippetTimer(string name, long ms);
    internal abstract class Snippet
    {
        public abstract IEnumerable<SnippetTimer> RunSnippet();
    }
}
