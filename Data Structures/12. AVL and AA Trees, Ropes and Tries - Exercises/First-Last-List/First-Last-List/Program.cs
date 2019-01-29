using System;

public class Program
{
    static void Main(string[] args)
    {
        FirstLastList<int> list = new FirstLastList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        var result = list.First(3);
        Console.WriteLine();
    }
}
