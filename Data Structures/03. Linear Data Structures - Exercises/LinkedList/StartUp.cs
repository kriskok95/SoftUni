using System;

class Program
{
    static void Main(string[] args)
    {
        LinkedStack<int> stack = new LinkedStack<int>();

        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
        stack.Push(10);

        Console.WriteLine(stack.Pop());

        Console.WriteLine(string.Join(", ", stack.ToArray()));


    }
}
