using System;

class LinkedQueue
{
    static void Main(string[] args)
    {
        LinkedQueue<int> queue = new LinkedQueue<int>();

        queue.Enqueue(5);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(10);

        Console.WriteLine(queue.Dequeue());
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        
    }
}

