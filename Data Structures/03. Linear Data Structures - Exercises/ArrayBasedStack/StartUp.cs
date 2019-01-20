using System;

public class StartUp
{
    static void Main(string[] args)
    {
        ArrayStack<int> array = new ArrayStack<int>();
        array.Push(20);
        array.Push(30);
        array.Push(200);
        var arr = array.ToArray();

        Console.WriteLine(string.Join(", ", arr));
    }
}

