using System;
using System.Collections.Generic;
using System.Linq;

namespace CountOfOccurences
{
    class CountOfOccurences
    {
        static void Main(string[] args)
        {
            List<int> inputArgs = Console.ReadLine()
                .Split(new char[] { ' ' })
                .Select(int.Parse)
                .ToList();

            Dictionary<int, int> numberOfOccurrences = new Dictionary<int, int>();

            for (int i = 0; i < inputArgs.Count; i++)
            {
                if (!numberOfOccurrences.ContainsKey(inputArgs[i]))
                {
                    numberOfOccurrences.Add(inputArgs[i], 1);
                }
                else
                {
                    numberOfOccurrences[inputArgs[i]]++;
                }
            }

            foreach (var number in numberOfOccurrences.OrderBy(x => x.Key))
            {
                Console.WriteLine($"{number.Key} -> {number.Value} times");
            }
        }
    }
}
