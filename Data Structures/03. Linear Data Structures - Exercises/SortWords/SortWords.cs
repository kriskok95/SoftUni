namespace SortWords
{
    using System;
    using System.Linq;

    class SortWords
    {
        static void Main(string[] args)
        {
            Console.WriteLine(String.Join(" ", Console.ReadLine().Split().OrderBy(s => s).ToList()));
        }
    }
}


