using System;

public class ArrayList<T>
{
    private const int InditialCapacity = 2;

    private T[] array;

    public ArrayList()
    {
        this.array = new T[InditialCapacity];
    }

    public int Count { get; private set; }

    public T this[int index]
    {
        get
        {
            if(index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return this.array[index];
        }

         set
        {
            if (index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.array[index] = value;
        }
    }

    public void Add(T item)
    {
        if(this.Count >= this.array.Length)
        {
            Resize();
        }
        this.array[this.Count++] = item;
    }  

    public T RemoveAt(int index)
    {
        if(index < 0 || index >= this.Count)
        {
            throw new ArgumentOutOfRangeException();
        }
        T element = this.array[index];
        this.Count--;
        for (int i = index; i < this.Count; i++)
        {
            this.array[i] = this.array[i + 1];
        }

        return element;
    }

    private void Resize()
    {
        T[] newArr = new T[this.array.Length * 2];
        Array.Copy(this.array, newArr, this.array.Length);
        this.array = newArr;
    }
}
