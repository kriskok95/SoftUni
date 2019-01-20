namespace SumAndAverage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class SumAndAverage
    {
        static void Main(string[] args)
        {
            List<int> inputNumbers = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToList();

            int totalSum = inputNumbers.Sum();
            double averageNumber = inputNumbers.Average();

            Console.WriteLine($"Sum={totalSum}; Average={averageNumber:F2}");

        }
    }
}
