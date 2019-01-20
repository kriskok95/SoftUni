using System;
using System.Collections.Generic;

public class BinaryHeap<T> where T : IComparable<T>
{
    private List<T> heap;

    public BinaryHeap()
    {
        this.heap = new List<T>();
    }

    public int Count => this.heap.Count;

    public void Insert(T item)
    {
        this.heap.Add(item);
        this.HeapifyUp(this.Count - 1);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0 && IsLess(Parent(index), index))
        {
            this.Swap(index, Parent(index));
            index = Parent(index);
        }
    }

    private void Swap(int index, int parent)
    {
        T temp = this.heap[index];
        this.heap[index] = this.heap[parent];
        this.heap[parent] = temp;

    }

    private bool IsLess(int parent, int index)
    {
        return this.heap[parent].CompareTo(this.heap[index]) < 0;
    }

    private int Parent(int index)
    {
        return (index - 1) / 2;
    }

    public T Peek()
    {
        return this.heap[0];
    }

    public T Pull()
    {
        if (this.Count <= 0)
        {
            throw new InvalidOperationException();
        }

        T item = this.heap[0];

        this.Swap(0, this.Count - 1);
        this.heap.RemoveAt(this.Count - 1);
        this.HeapifyDown(0);

        return item;
    }

    private void HeapifyDown(int index)
    {
        while (index < this.Count / 2)
        {
            int child = Left(index);
            if (HasChild(child + 1) && IsLess(child, child + 1))
            {
                child = child + 1;
            }

            if (IsLess(child, index))
            {
                break;
            }

            this.Swap(index, child);
            index = child;
        }
    }

    public void DecreaseKey(T item)
    {
        int index = this.heap.IndexOf(item);

        this.HeapifyUp(index);
    }

    private bool HasChild(int childIndex)
    {
        return childIndex < this.Count;
    }


    private int Left(int index)
    {
        return index * 2 + 1;
    }
}
