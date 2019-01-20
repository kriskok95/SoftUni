using System;
using System.Net;
using System.Runtime.CompilerServices;

public static class Heap<T> where T : IComparable<T>
{
    public static void Sort(T[] arr)
    {
        int n = arr.Length;

        for (int i = n / 2; i >= 0; i--)
        {
            Down(arr, i, n);

        }

        for (int i = n - 1; i >= 0; i--)
        {
            Swap(arr, 0, i);
            Down(arr, 0, i);
        }
    }

    private static void Down(T[] arr, int current, int border)
    {
        while (current < border / 2)
        {
            int child = Left(current);

            if (child + 1 < border && IsLess(arr, child, child + 1))
            {
                child = child + 1;
            }

            if (IsLess(arr, child, current))
            {
                break;
            }

            Swap(arr, current, child);
            current = child;
        }
    }

    private static void Swap(T[] arr, int index, int other)
    {
        T temp = arr[index];
        arr[index] = arr[other];
        arr[other] = temp;
    }

    private static bool IsLess(T[] arr, int other, int index)
    {
        return arr[other].CompareTo(arr[index]) < 0;
    }

    private static int Left(int index)
    {
        return index * 2 + 1;
    }
}
