using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculateSequenceWithAQueue
{
    class CalculateSequence
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            Queue<int> helpQueue = new Queue<int>();
            Queue<int> numbers = new Queue<int>();

            helpQueue.Enqueue(n);
            numbers.Enqueue(n);

            for (int i = 0; i < 17; i++)
            {
                int searchingNumber = helpQueue.Dequeue();
                numbers.Enqueue(searchingNumber + 1);
                numbers.Enqueue(2 * searchingNumber + 1);
                numbers.Enqueue(searchingNumber + 2);

                helpQueue.Enqueue(searchingNumber + 1);
                helpQueue.Enqueue(2 * searchingNumber + 1);
                helpQueue.Enqueue(searchingNumber + 2);

            }

            var result = numbers.Take(50);

            Console.WriteLine(string.Join(", ", result));
        }
    }
}
