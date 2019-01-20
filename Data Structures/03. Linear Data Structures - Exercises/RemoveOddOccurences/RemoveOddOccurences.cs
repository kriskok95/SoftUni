namespace RemoveOddOccurences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class RemoveOddOccurences
    {
        static void Main(string[] args)
        {
            List<int> inputArgs = Console.ReadLine()
                 .Split(new char[] { ' ' })
                 .Select(int.Parse)
                 .ToList();

            Dictionary<int, int> numberOfOccurences = new Dictionary<int, int>();

            for (int i = 0; i < inputArgs.Count; i++)
            {
                if (!numberOfOccurences.ContainsKey(inputArgs[i]))
                {
                    numberOfOccurences.Add(inputArgs[i], 1);
                }
                else
                {
                    numberOfOccurences[inputArgs[i]]++;
                }
            }

            foreach (var number in numberOfOccurences)
            {
                if(number.Value % 2 != 0)
                {
                    inputArgs.RemoveAll(x => x == number.Key);
                }
            }

            Console.WriteLine(string.Join(" ", inputArgs));
        }
    }
}
