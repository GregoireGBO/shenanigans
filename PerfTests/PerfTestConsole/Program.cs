// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

test_foreach_with_select_and_records(15);

//test_exception_time(15);

Console.WriteLine("END");
Console.ReadLine();

void test_exception_time(int v)
{
    for (int i = 0; i < 10; i++)
    {
        forEachException();
    }
}

void test_foreach_with_select_and_records(int nbRuns)
{
    for (int i = 0; i < nbRuns; i++)
    {
        forEachInForEachWithRecordAlloc(i);
    }
}

void forEachInForEachWithRecordAlloc(int someInt)
{
    var list1 = Enumerable.Range(0, 1000).Select(i => new MyInt(i));
    var list2 = Enumerable.Range(0, 1000).Select(i => new MyInt(i));

    var listIntermediary = new List<int>();
    var listRes = new List<MyInt>();
    var sw = Stopwatch.StartNew();

    foreach (var i in list1)
    {
        foreach (var i2 in list2)
        {
            listIntermediary.Add(i.v + i2.v);
        }
    }

    var ellapsedBeforeSelect = sw.ElapsedMilliseconds;

    listRes = listIntermediary.Select(r => new MyInt(r)).ToList();

    var ellapsedMs = sw.ElapsedMilliseconds;

    Console.WriteLine($"Took {ellapsedMs}ms for a count of {listIntermediary.Count} items. Select took {ellapsedMs - ellapsedBeforeSelect}ms, nested foreach took {ellapsedBeforeSelect}ms.");
}

void forEachException()
{
    int nbIter = 100;
    var sw = Stopwatch.StartNew();

    for (int i = 0; i < nbIter; i++)
    {
        try
        {
            throw new Exception(i.ToString());
        }
        catch (Exception e) { }
    }

    var ellapsedMs = sw.ElapsedMilliseconds;

    Console.WriteLine($"Took {ellapsedMs}ms for a count of {nbIter} exceptions thrown and caught.");
}

public record MyInt(int v);
