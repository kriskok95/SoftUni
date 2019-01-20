using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

class StartUp
{
    private static Dictionary<int, Tree<int>> nodeByValue = new Dictionary<int, Tree<int>>();

    static void Main()
    {
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n - 1; i++)
        {
            int[] family = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            int parent = family[0];
            int child = family[1];

            if (!nodeByValue.ContainsKey(parent))
            {
                nodeByValue.Add(parent, new Tree<int>(parent));
            }

            if (!nodeByValue.ContainsKey(child))
            {
                nodeByValue.Add(child, new Tree<int>(child));
            }

            Tree<int> parentTree = nodeByValue[parent];
            Tree<int> childTree = nodeByValue[child];

            parentTree.Children.Add(childTree);
            childTree.Parent = parentTree;
            
        }       
        PrintSubtreesByGivensum();
        
    }

    private static void PrintSubtreesByGivensum()
    {
        int targetSum = int.Parse(Console.ReadLine());

        List<Tree<int>> nodes = new List<Tree<int>>();

        Tree<int> root = GetRootNode();

        DFS(root, nodes);

        Console.WriteLine($"Subtrees of sum {targetSum}:");

        foreach (var tree in nodes)
        {
            int sum = CalculateSubTreeSum(tree);

            if (sum == targetSum)
            {
                List<int> subtree = new List<int>();
                GetSubTreePreOrder(tree, subtree);
                Console.WriteLine(string.Join(" ", subtree));
            }
        }
    }

    private static void GetSubTreePreOrder(Tree<int> tree, List<int> subtree)
    {
        subtree.Add(tree.Value);

        foreach (var child in tree.Children)
        {
            GetSubTreePreOrder(child, subtree);
        }
    }

    private static int CalculateSubTreeSum(Tree<int> tree)
    {
        int sum = tree.Value;

        foreach (var child in tree.Children)
        {
            sum += CalculateSubTreeSum(child);
        }

        return sum;
    }

    private static void DFS(Tree<int> root, List<Tree<int>> nodes)
    {
        foreach (var child in root.Children)
        {
            DFS(child, nodes);
        }

        nodes.Add(root);

    }

    private static Dictionary<int, List<int>> GetSumOfPaths(int sumOfPath)
    {
        Dictionary<int, List<int>> paths = new Dictionary<int, List<int>>();
        int countPaths = 1;

        List<int> currentPath = new List<int>();

        var leafs = nodeByValue.Values
            .Where(x => x.Children.Count == 0)
            .Select(x => x.Value)
            .ToList();

        foreach (var leaf in leafs)
        {
            Tree<int> tree = nodeByValue[leaf];

            while (tree.Parent != null)
            {
                currentPath.Add(tree.Value);
                tree = tree.Parent;
            }
            currentPath.Add(tree.Value);

            if (currentPath.Sum(x => x) == sumOfPath)
            {
                currentPath.Reverse();
                paths.Add(countPaths++, currentPath.ToList());
               
            }
            currentPath.Clear();
        }

        return paths;
    }

    public static Tree<int> GetRootNode()
    {
        return nodeByValue.Values.FirstOrDefault(x => x.Parent == null);
    }

    private static int GetDeepestLeaf()
    {
        List<int> leafs = nodeByValue.Values
            .Where(x => x.Children.Count == 0)
            .Select(x => x.Value)
            .ToList();

        Tree<int> root = GetRootNode();
        int maxDepth = 0;
        int result = 0;

        foreach (var leaf in leafs)
        {
            Tree<int> currentTree = nodeByValue[leaf];
            int currentMaxDepth = 1;
            while (currentTree.Parent != null)
            {
                currentTree = currentTree.Parent;
                currentMaxDepth++;
            }

            if (currentMaxDepth > maxDepth)
            {
                maxDepth = currentMaxDepth;
                result = leaf;
            }

        }

        return result;
    }

    public static List<int> GetLongestPath()
    {
        List<int> result = new List<int>();

        List<int> leafs = nodeByValue.Values
            .Where(x => x.Children.Count == 0)
            .Select(x => x.Value)
            .ToList();

        int maxDepth = 0;

        foreach (var leaf in leafs)
        {
            Tree<int> currentTree = nodeByValue[leaf];
            List<int> currentElements = new List<int>();

            int currentDepth = 1;

            while (currentTree.Parent != null)
            {
                currentDepth++;
                currentElements.Add(currentTree.Value);
                currentTree = currentTree.Parent;
            }
            currentElements.Add(currentTree.Value);

            if (currentDepth > maxDepth)
            {
                maxDepth = currentDepth;
                result = currentElements.ToList();
            }
            currentElements.Clear();        
        }
        result.Reverse();

        return result;
    }
}
