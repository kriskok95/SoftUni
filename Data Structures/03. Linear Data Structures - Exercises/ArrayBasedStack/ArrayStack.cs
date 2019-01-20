using System;

public class ArrayStack<T>
{
    private const int InitialCapacity = 16;
    private T[] elements;

    public int Count { get; private set; }

    public ArrayStack(int capacity = InitialCapacity)
    {
        this.elements = new T[InitialCapacity];
    }

    public void Push(T element)
    {
        if(elements.Length == this.Count)
        {
            this.Grow();
        }
        this.elements[this.Count++] = element;
    }

    public T Pop()
    {
        if(this.Count == 0)
        {
            throw new InvalidOperationException();
        }
        T element = this.elements[--this.Count];

        return element;
    }

    public T[] ToArray()
    {
        T[] result = new T[this.Count];

        for (int i = 0; i < this.Count; i++)
        {
            result[i] = this.elements[i];
        }
        return result;
    }

    private void Grow()
    {
        T[] newArr = new T[this.elements.Length * 2];

        for (int i = 0; i < this.Count; i++)
        {
            newArr[i] = elements[i];
        }
        this.elements = newArr;
    }
}

