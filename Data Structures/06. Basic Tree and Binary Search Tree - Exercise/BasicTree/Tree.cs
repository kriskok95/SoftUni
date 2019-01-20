using System;
using System.Collections.Generic;
using System.Linq;

public class Tree<T>
{
    public T Value { get; set; }

    public Tree<T> Parent { get; set; }
    public List<Tree<T>> Children { get; private set; }

    public Tree(T value, params Tree<T>[] children)
    {
        this.Value = value;
        this.Children = new List<Tree<T>>(children);
    }

    public void PrintTree(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}{this.Value}");
        foreach (var child in this.Children)
        {
            child.PrintTree(indent + 2);
        }
    }

    public List<T> GetLeafNodes(Tree<T> rooTree)
    {
        List<T> leafs = new List<T>();

        GetLeafNodes(rooTree, leafs);

        return leafs.OrderBy(x => x).ToList();
    }

    private void GetLeafNodes(Tree<T> root, List<T> leafs)
    {
        foreach (var child in root.Children)
        {
            if (child.Children.Count == 0)
            {
                leafs.Add(child.Value);
            }
            else
            {
                GetLeafNodes(child, leafs);
            }
        }
    }

    public List<T> GetMiddleNodes(Tree<T> rootTree)
    {
        List<T> middleNodes = new List<T>();

        GetMiddleNodes(rootTree, middleNodes);

        return middleNodes.OrderBy(x => x).ToList();
    }

    private void GetMiddleNodes(Tree<T> tree, List<T> result)
    {
        foreach (var child in tree.Children)
        {
            if (child.Children.Count > 0)
            {
                result.Add(child.Value);
                GetMiddleNodes(child, result);
            }       
        }
    }  
}

