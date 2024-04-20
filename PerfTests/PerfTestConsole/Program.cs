// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

test_foreach_with_select_and_records(10);

test_exception_time(10);

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
    var listIntermediary = new List<My2Ints>();
    var listRes = new List<MyInt>();
    var sw = Stopwatch.StartNew();

    foreach (var i in list1)
    {
        foreach (var i2 in list2)
        {
            listIntermediary.Add(new My2Ints(i, i2));
            //listIntermediary.Add(new My2Ints(i with { v = i.v + 1 }, i2 with { v = i2.v + 1 }));
        }
    }
    var ellapsedBeforeSelect = sw.ElapsedMilliseconds;

    listRes = listIntermediary.Select(r => new MyInt(r.v1.v + r.v2.v + someInt)).ToList();

    var ellapsedMs = sw.ElapsedMilliseconds;

    Console.WriteLine($"Took {ellapsedMs}ms for a count of {listIntermediary.Count} items. Select took {ellapsedMs - ellapsedBeforeSelect}ms, nested foreach took {ellapsedBeforeSelect}ms.");
}

void forEachException()
{
    int nbIter = 10;
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
public record My2Ints(MyInt v1, MyInt v2);
