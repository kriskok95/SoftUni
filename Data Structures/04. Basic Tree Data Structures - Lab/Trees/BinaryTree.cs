using System;

public class BinaryTree<T>
{
    public T Value { get; set; }

    public BinaryTree<T> leftChild { get; set; }

    public BinaryTree<T> rightChild { get; set; }


    public BinaryTree(T value, BinaryTree<T> leftChild = null, BinaryTree<T> rightChild = null)
    {
        this.Value = value;
        this.leftChild = leftChild;
        this.rightChild = rightChild;
    }

    public void PrintIndentedPreOrder(int indent = 0)
    {
        Console.Write(new string(' ', 2 * indent));
        Console.WriteLine(this.Value);
        if(this.leftChild != null)
        {
            this.leftChild.PrintIndentedPreOrder(indent + 1);
        }
        if(this.rightChild != null)
        {
            this.rightChild.PrintIndentedPreOrder(indent + 1);
        }
    }

    public void EachInOrder(Action<T> action)
    {
        if(this.leftChild != null)
        {
            this.leftChild.EachInOrder(action);
        }

        action(this.Value);

        if(this.rightChild != null)
        {
            this.rightChild.EachInOrder(action);
        }
    }

    public void EachPostOrder(Action<T> action)
    {
        if(this.leftChild != null)
        {
            this.leftChild.EachPostOrder(action);
        }
        if(this.rightChild != null)
        {
            this.rightChild.EachPostOrder(action);
        }
        action(this.Value);
    }
}
