using System;
using System.Collections.Generic;
using System.Linq;

class SequenceNToM
{
    static void Main(string[] args)
    {
        int[] inputArgs = Console.ReadLine()
            .Split(new[] { ' ' })
            .Select(int.Parse)
            .ToArray();

        int n = inputArgs[0];
        int m = inputArgs[1];

        Queue<Item> queue = new Queue<Item>();
        Item item = new Item(n, null);
        queue.Enqueue(item);

        List<int> result = new List<int>();

        while(queue.Count > 0)
        {
            Item element = queue.Dequeue();
            if (element.Value < m)
            {
                queue.Enqueue(new Item(element.Value + 1, element));
                queue.Enqueue(new Item(element.Value + 2, element));
                queue.Enqueue(new Item(element.Value * 2, element));
            }

            if(element.Value == m)
            {         
                while(element != null)
                {
                    result.Add(element.Value);
                    element = element.PrevItem;
                }
                result.Reverse();
                Console.WriteLine(string.Join(" -> ", result));
                return;
            }
        }

    }

    class Item
    {
        public int Value { get; private set; }
        public Item PrevItem { get; set; }

        public Item(int value, Item item)
        {
            this.Value = value;
            this.PrevItem = item;
        }
    }
    
}


