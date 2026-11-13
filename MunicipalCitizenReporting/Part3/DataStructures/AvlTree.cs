using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class AvlTree<TKey, TValue>
    {
        private sealed class Node
        {
            internal TKey Key; internal TValue Value; internal Node Left, Right; internal int Height = 1;
            internal Node(TKey key, TValue value) { Key = key; Value = value; }
        }
        private Node root;
        private readonly IComparer<TKey> comparer;
        public int Count { get; private set; }
        public int Height { get { return H(root); } }
        public AvlTree(IComparer<TKey> comparer = null) { this.comparer = comparer ?? Comparer<TKey>.Default; }
        private static int H(Node node) { return node == null ? 0 : node.Height; }
        private static void Update(Node node) { node.Height = 1 + Math.Max(H(node.Left), H(node.Right)); }
        private static Node RotateRight(Node node)
        {
            Node pivot = node.Left; node.Left = pivot.Right; pivot.Right = node; Update(node); Update(pivot); return pivot;
        }
        private static Node RotateLeft(Node node)
        {
            Node pivot = node.Right; node.Right = pivot.Left; pivot.Left = node; Update(node); Update(pivot); return pivot;
        }
        public void Insert(TKey key, TValue value) { root = Insert(root, key, value); }
        private Node Insert(Node node, TKey key, TValue value)
        {
            if (node == null) { Count++; return new Node(key, value); }
            int order = comparer.Compare(key, node.Key);
            if (order == 0) { node.Value = value; return node; }
            if (order < 0) node.Left = Insert(node.Left, key, value); else node.Right = Insert(node.Right, key, value);
            Update(node); int balance = H(node.Left) - H(node.Right);
            if (balance > 1) { if (comparer.Compare(key, node.Left.Key) > 0) node.Left = RotateLeft(node.Left); return RotateRight(node); }
            if (balance < -1) { if (comparer.Compare(key, node.Right.Key) < 0) node.Right = RotateRight(node.Right); return RotateLeft(node); }
            return node;
        }
        public bool TryFind(TKey key, out TValue value)
        {
            Node node = root;
            while (node != null) { int order = comparer.Compare(key, node.Key); if (order == 0) { value = node.Value; return true; } node = order < 0 ? node.Left : node.Right; }
            value = default(TValue); return false;
        }
        public IList<TKey> Inorder() { var result = new List<TKey>(); Walk(root, result); return result.AsReadOnly(); }
        private static void Walk(Node node, List<TKey> result) { if (node == null) return; Walk(node.Left, result); result.Add(node.Key); Walk(node.Right, result); }
        public bool ValidateInvariants()
        {
            int height; if (!Validate(root, out height)) return false;
            IList<TKey> keys = Inorder(); if (keys.Count != Count) return false;
            for (int i = 1; i < keys.Count; i++) if (comparer.Compare(keys[i - 1], keys[i]) >= 0) return false;
            return true;
        }
        private static bool Validate(Node node, out int height)
        {
            height = 0; if (node == null) return true;
            int left, right; if (!Validate(node.Left, out left) || !Validate(node.Right, out right)) return false;
            height = Math.Max(left, right) + 1;
            return node.Height == height && Math.Abs(left - right) <= 1;
        }
    }
}
