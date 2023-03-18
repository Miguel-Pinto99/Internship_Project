using System.Security.Cryptography.X509Certificates;

int[] valuesArray = new int[] { 0, 1, 2 };

List<int> valuesList = new List<int> { 0, 1, 2, 3 };

IEnumerable<int> valuesEnum = Enumerable.Range(0, 5);
                              

IEnumerable<int> valuesEnumSelect4 = from value in Enumerable.Range(0, 5)
                              select 4;

foreach (int value in valuesArray)
{
    Console.WriteLine(value);
}

Console.WriteLine();

foreach (int value in valuesEnum)
{
    Console.WriteLine(value);
}

Console.WriteLine();

foreach (int value in valuesList)
{
    Console.WriteLine(value);
}

Console.WriteLine();

foreach (int value in valuesEnumSelect4)
{
    Console.WriteLine(value);
}


Console.WriteLine();
Console.WriteLine($"Print an List<int>(): {valuesList}");
Console.WriteLine($"Print an IEnumerable<int>(): {valuesEnum}");
Console.WriteLine($"Print an List<int>.Max(): {valuesList.Max()}");
Console.WriteLine($"Print an IEnumerable<int>.Max(): {valuesEnum.Max()}");
Console.WriteLine($"Print an IEnumerable<int>.ToList().Count: {valuesEnum.ToList().Count}");
//!!!!Console.WriteLine($"Print an IEnumerable<int>.ToList().Count: {valuesEnum.Count}")!!!!!!;
Console.WriteLine($"Print an IEnumerable<int>.GetEnumerator.Current: {valuesEnum.GetEnumerator().Current}");
Console.WriteLine($"Print an IEnumerable<int>.GetEnumerator.MoveNext: {valuesEnum.GetEnumerator().MoveNext()}");
Console.WriteLine($"Print an IEnumerable<int>.Where(x => x <=5): {valuesEnum.Where(x => x <=3).ToList()}");

Console.WriteLine();

double average = valuesArray.Average();
Console.WriteLine($"Average: {average}");
List<int> list = valuesArray.ToList();
Console.WriteLine($"To List: {list.Count}");
List<int> listEnums = valuesEnum.ToList();
Console.WriteLine($"To List From Enums: {listEnums}");
