using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class FirstLastList<T> : IFirstLastList<T> where T : IComparable<T>
{
    private LinkedList<T> insertedValues;
    private OrderedBag<LinkedListNode<T>> ascendingElements;
    private OrderedBag<LinkedListNode<T>> descendingElement;
  

    public FirstLastList()
    {
        this.insertedValues = new LinkedList<T>();
        this.ascendingElements = new OrderedBag<LinkedListNode<T>>((x,y) => x.Value.CompareTo(y.Value));
        this.descendingElement = new OrderedBag<LinkedListNode<T>>((x,y) => y.Value.CompareTo(x.Value));
    }

    public int Count
    {
        get
        {
            return this.insertedValues.Count;
        }
    }

    public void Add(T element)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(element);
        this.insertedValues.AddLast(node);
        this.ascendingElements.Add(node);
        this.descendingElement.Add(node);
    }

    public void Clear()
    {
        this.insertedValues.Clear();
        this.ascendingElements.Clear();
        this.descendingElement.Clear();
    }

    public IEnumerable<T> First(int count)
    {

        IsInBounds(count);
        LinkedListNode<T> current = this.insertedValues.First;
        int iteration = 0;

        while (iteration < count)
        {
            yield return current.Value;
            current = current.Next;
            iteration++;
        }
    }

    private void IsInBounds(int count)
    {
        if (this.insertedValues.Count < count)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public IEnumerable<T> Last(int count)
    {
        this.IsInBounds(count);
        LinkedListNode<T> current = this.insertedValues.Last;
        int iteration = 0;

        while(iteration < count)
        {
            yield return current.Value;
            current = current.Previous;
            iteration++;
        }
    }

    public IEnumerable<T> Max(int count)
    {
        this.IsInBounds(count);
        return this.descendingElement.Take(count).Select(x => x.Value);
    }

    public IEnumerable<T> Min(int count)
    {
        this.IsInBounds(count);
        return this.ascendingElements.Take(count).Select(x => x.Value);
    }

    public int RemoveAll(T element)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(element);

        foreach (var item in this.ascendingElements.Range(node, true, node, true))
        {
            this.insertedValues.Remove(item);
        }

        int count = this.ascendingElements.RemoveAllCopies(node);
        this.descendingElement.RemoveAllCopies(node);

        return count;
    }
}
