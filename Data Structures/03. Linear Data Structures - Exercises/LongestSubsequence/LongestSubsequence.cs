using System;
using System.Collections.Generic;
using System.Linq;
namespace LongestSubsequence
{
    class LongestSubsequence
    {
        static void Main(string[] args)
        {
            List<int> inputArgs = Console.ReadLine()
                .Split(new char[] { ' '})
                .Select(int.Parse)
                .ToList();

            List<int> logestSubSequence = FindTheLongestSubSequence(inputArgs);

            Console.WriteLine(string.Join(" ", logestSubSequence));
        }

        private static List<int> FindTheLongestSubSequence(List<int> inputArgs)
        {
            List<int> result = new List<int>();

            int elementForSearch = inputArgs[0];
            int currentSubsequenceCount = 0;
            int longestSubsequenceCount = 0;
            int number = inputArgs[0];

            for (int i = 0; i < inputArgs.Count; i++)
            {
                if(elementForSearch == inputArgs[i])
                {
                    currentSubsequenceCount++;
                    if(currentSubsequenceCount > longestSubsequenceCount)
                    {
                        longestSubsequenceCount = currentSubsequenceCount;
                        number = inputArgs[i];
                    }
                }
                else
                {
                    currentSubsequenceCount = 1;
                    elementForSearch = inputArgs[i];
                }
            }

            for (int i = 0; i < longestSubsequenceCount; i++)
            {
                result.Add(number);
            }
            return result;
        }
    }
}
