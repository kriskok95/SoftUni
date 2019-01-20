namespace ReverseNumbersWithAStack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class ReverseNumbers
    {
        static void Main(string[] args)
        {
            int[] inputNumbers = Console.ReadLine()
                .Split(new [] { ' '})
                .Select(int.Parse)
                .ToArray();

            Stack<int> result = new Stack<int>(inputNumbers);

            Console.WriteLine(string.Join(" ", result));               
        }
    }
}
