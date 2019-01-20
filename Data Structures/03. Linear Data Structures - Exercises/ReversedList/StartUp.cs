using System;

namespace ReversedList
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var list = new ReversedList<int>();

            for (int i = 1; i <= 10; i++)
            {
                list.Add(i);
            }

            Console.WriteLine(list[2]);
            list.RemoveAt(2);
            Console.WriteLine(list[2]);

            foreach (var el in list)
            {
                Console.WriteLine(el);
            }
        }
    }
}
