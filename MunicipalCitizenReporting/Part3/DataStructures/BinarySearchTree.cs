using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class BinarySearchTree<TKey, TValue>
    {
        private sealed class Node
        {
            internal TKey Key; internal TValue Value; internal Node Left, Right;
            internal Node(TKey key, TValue value) { Key = key; Value = value; }
        }
        private Node root;
        private readonly IComparer<TKey> comparer;
        public int Count { get; private set; }
        public BinarySearchTree(IComparer<TKey> comparer = null) { this.comparer = comparer ?? Comparer<TKey>.Default; }
        // Duplicate keys replace the value without creating another node.
        public void Insert(TKey key, TValue value)
        {
            if (root == null) { root = new Node(key, value); Count++; return; }
            Node node = root;
            while (true)
            {
                int order = comparer.Compare(key, node.Key);
                if (order == 0) { node.Value = value; return; }
                if (order < 0)
                {
                    if (node.Left == null) { node.Left = new Node(key, value); Count++; return; } node = node.Left;
                }
                else { if (node.Right == null) { node.Right = new Node(key, value); Count++; return; } node = node.Right; }
            }
        }
        public bool TryFind(TKey key, out TValue value)
        {
            Node node = root;
            while (node != null) { int order = comparer.Compare(key, node.Key); if (order == 0) { value = node.Value; return true; } node = order < 0 ? node.Left : node.Right; }
            value = default(TValue); return false;
        }
        public bool Delete(TKey key)
        {
            Node parent = null, node = root;
            while (node != null && comparer.Compare(key, node.Key) != 0) { parent = node; node = comparer.Compare(key, node.Key) < 0 ? node.Left : node.Right; }
            if (node == null) return false;
            if (node.Left != null && node.Right != null)
            {
                Node successorParent = node, successor = node.Right;
                while (successor.Left != null) { successorParent = successor; successor = successor.Left; }
                node.Key = successor.Key; node.Value = successor.Value;
                parent = successorParent; node = successor;
            }
            Node child = node.Left ?? node.Right;
            if (parent == null) root = child;
            else if (parent.Left == node) parent.Left = child; else parent.Right = child;
            Count--; return true;
        }
        public IList<TKey> Inorder()
        {
            var result = new List<TKey>(); var stack = new Stack<Node>(); Node node = root;
            while (node != null || stack.Count > 0)
            {
                while (node != null) { stack.Push(node); node = node.Left; }
                node = stack.Pop(); result.Add(node.Key); node = node.Right;
            }
            return result.AsReadOnly();
        }
    }
}
