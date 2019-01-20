﻿using System;
using System.Collections.Generic;

public class BinarySearchTree<T> where T : IComparable<T>
{

    private class Node
    {
        public Node(T value)
        {
            this.Value = value;
            this.Left = null;
            this.Right = null;
        }

        public T Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

    private Node root;

    public BinarySearchTree()
    {

    }

    private BinarySearchTree(Node root)
    {
        this.Copy(root);
    }

    private void Copy(Node node)
    {
        if(node == null)
        {
            return;
        }

        this.Insert(node.Value);
        this.Copy(node.Left);
        this.Copy(node.Right);
    }

    public void Insert(T value)
    {
        if(this.root == null)
        {
            this.root = new Node(value);
            return;
        }

        Node parent = null;
        Node current = this.root;

        while(current != null)
        {
            parent = current;
            if (current.Value.CompareTo(value) > 0)
            {     
                current = current.Left;
            }
            else if(current.Value.CompareTo(value) < 0)
            {
                current = current.Right;
            }
            else
            {               
                return;
            }
        }

        Node newNode = new Node(value);

        if(parent.Value.CompareTo(newNode.Value) > 0)
        {
            parent.Left = newNode;
        }
        else
        {
            parent.Right = newNode;
        }
    }

    public bool Contains(T value)
    {
        Node current = this.root;
        while (current != null)
        {
            if(current.Value.CompareTo(value) < 0)
            {
                current = current.Right;
            }
            else if(current.Value.CompareTo(value) > 0)
            {
                current = current.Left;
            }
            else
            {
                break;
            }
        }

        return current != null;
    }

    public void DeleteMin()
    {
        if(this.root == null)
        {
            return;
        }

        Node parent = null;
        Node min = this.root;

        while(min.Left != null)
        {
            parent = min;
            min = min.Left;
        }

        if(parent == null)
        {
            this.root = min.Right;
        }
        else
        {
            parent.Left = min.Right;
        }
    }

    public BinarySearchTree<T> Search(T item)
    {
        Node current = this.root;

        while(current != null)
        {
            if(item.CompareTo(current.Value) > 0)
            {
                current = current.Right;
            }
            else if(item.CompareTo(current.Value) < 0)
            {
                current = current.Left;
            }
            else
            {
                break;
            }
        }
        if(current == null)
        {
            return null;
        }
        else
        {
            return new BinarySearchTree<T>(current);
        }
    }



    public IEnumerable<T> Range(T startRange, T endRange)
    {
        Queue<T> queue = new Queue<T>();

        this.Range(this.root, queue, startRange, endRange);

        return queue;
    }

    private void Range(Node node, Queue<T> queue, T startRange, T endRange)
    {
        if(node == null)
        {
            return;
        }

        int nodeIneLowerRange = startRange.CompareTo(node.Value);
        int nodeInHigherRange = endRange.CompareTo(node.Value);

        if(nodeIneLowerRange < 0)
        {
            this.Range(node.Left, queue, startRange, endRange);
        }
        if(nodeIneLowerRange <= 0 && nodeInHigherRange >= 0)
        {
            queue.Enqueue(node.Value);
        }
        if(nodeInHigherRange > 0)
        {
            this.Range(node.Right, queue, startRange, endRange);
        }
    }

    public void EachInOrder(Action<T> action)
    {
        this.EachInOrder(this.root, action);
    }

    private void EachInOrder(Node node, Action<T> action)
    {
        if(node != null)
        {
            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        }
    }
}

public class Launcher
{
    public static void Main(string[] args)
    {
        BinarySearchTree<int> tree = new BinarySearchTree<int>();

        tree.Insert(4);
        tree.Insert(5);
        tree.Insert(17);
        tree.Insert(3);
        tree.Insert(25);

        Console.WriteLine(tree.Contains(3));

        var range = tree.Range(2, 20);
    }
}
