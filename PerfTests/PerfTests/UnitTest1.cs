using System.Diagnostics;

namespace PerfTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ForEachInForEachWithRecords()
        {
            var list1 = Enumerable.Range(0, 1000).Select(i => new MyInt(i));
            var list2 = Enumerable.Range(0, 1000).Select(i => new MyInt(i));
            var listRes = new List<MyInt>();    
            var sw = Stopwatch.StartNew();

            foreach (var i in list1)
            {
                foreach (var i2 in list2)
                {
                    listRes.Add(new MyInt(i.v + i2.v));
                }
            }

            var ellapsedMs = sw.ElapsedMilliseconds;

            Debug.WriteLine($"Took {ellapsedMs}ms for a count of {listRes.Count} items in result list.");
        }

        private record MyInt(int v);
    }
}