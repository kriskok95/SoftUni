using System;

public class LinkedStack<T>
{
    private class Node
    {
        public Node NextNode { get; set; }
        public T Value { get; set; }

        public Node(T value, Node nextNode)
        {
            this.Value = value;
            this.NextNode = nextNode;

        }
    }

    private Node firstNode;

    public int Count { get; private set; }
    
    public void Push(T element)
    {
        this.firstNode = new Node(element, this.firstNode);
        this.Count++;
    }

    public T Pop()
    {
        if(this.Count == 0)
        {
            throw new InvalidOperationException();
        }
        T item = this.firstNode.Value;
        this.firstNode = this.firstNode.NextNode;
        this.Count--;

        return item;
    }

    public T[] ToArray()
    {
        T[] newArr = new T[this.Count];

        for (int i = 0; i < this.Count; i++)
        {
            newArr[i] = this.firstNode.Value;
            this.firstNode = this.firstNode.NextNode;
        }

        return newArr;
    }
}

