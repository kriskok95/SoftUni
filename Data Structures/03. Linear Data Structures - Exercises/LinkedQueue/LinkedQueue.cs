
using System;

public class LinkedQueue<T>
{
    private QueueNode head;
    private QueueNode tail;

    public int Count { get; private set; }

    public void Enqueue(T element)
    {
        if (this.Count == 0)
        {
            this.head = this.tail = new QueueNode(element);
        }
        else
        {
            var oldTail = this.tail;
            this.tail = new QueueNode(element);
            this.tail.PrevNode = oldTail;
            oldTail.NextNode = this.tail;

        }
        this.Count++;

    }

    public T Dequeue()
    {
        if(this.Count == 0)
        {
            throw new InvalidOperationException();
        }

        T element = this.head.Value;
        
        if(this.Count == 1)
        {
            this.head = null;
            this.tail = null;
        }
        else
        {
            this.head = this.head.NextNode;
        }

        this.Count--;
        return element;
    }

    public T[] ToArray()
    {
        T[] newArr = new T[this.Count];

        for (int i = 0; i < this.Count; i++)
        {
            newArr[i] = this.head.Value;
            this.head = this.head.NextNode;
        }

        return newArr;
    }

    private class QueueNode
    {
        public T Value { get;  private set; }

        public QueueNode NextNode { get; set; }

        public QueueNode PrevNode { get; set; }

        public QueueNode(T value)
        {
            this.Value = value;
        }
        
    }
}

