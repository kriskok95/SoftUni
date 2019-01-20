using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedList<T> : IEnumerable<T>
{
    private class Node
    {
        private T value;

        private Node nextNode;

        public Node(T value)
        {
            this.Value = value;
        }

        public Node NextNode
        {
            get { return this.nextNode; }
            set { this.nextNode = value; }
        }

        public T Value { get => value; set => this.value = value; }
    }

    private Node head;
    private Node tail;

    public LinkedList()
    {

    }

    public int Count { get; private set; }

    public void AddFirst(T item)
    {
        Node oldNode = this.head;
        this.head = new Node(item);
        this.head.NextNode = oldNode;

        if (IsEmpy())
        {
            this.tail = head;
        }

        this.Count++;
    }

    public void AddLast(T item)
    {
        Node oldTail = this.tail;
        this.tail = new Node(item);

        if (IsEmpy())
        {
            this.head = this.tail;
        }
        else
        {
            oldTail.NextNode = this.tail;
        }
        this.Count++;
    }

    public T RemoveFirst()
    {
        if (IsEmpy())
        {
            throw new InvalidOperationException();
        }

        T element = this.head.Value;

        if (this.Count == 1)
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

    public T RemoveLast()
    {
        if (IsEmpy())
        {
            throw new InvalidOperationException();
        }
        
        T element = this.tail.Value;

        if(this.Count == 1)
        {
            this.tail = null;
            this.head = null;
        }
        else
        {
            Node newTail = GetSecondToLast(element);
            newTail.NextNode = null;
            
            this.tail = newTail;
        }
        this.Count--;
        return element;
    }

    private Node GetSecondToLast(T element)
    {
        Node current = this.head;

        while (current.NextNode != this.tail)
        {
            current = current.NextNode;
        }

        return current;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node current = this.head;
        while(current != null)
        {
            yield return current.Value;
            current = current.NextNode;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private bool IsEmpy()
    {
        return this.Count == 0;
    }
}
