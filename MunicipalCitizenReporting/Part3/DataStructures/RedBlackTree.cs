using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    // Left-leaning red-black insertion; null leaves are black. Deletion is intentionally unsupported.
    public sealed class RedBlackTree<TKey, TValue>
    {
        private sealed class Node
        {
            internal TKey Key; internal TValue Value; internal Node Left, Right; internal bool Red = true;
            internal Node(TKey key, TValue value) { Key = key; Value = value; }
        }
        private Node root;
        private readonly IComparer<TKey> comparer;
        public int Count { get; private set; }
        public RedBlackTree(IComparer<TKey> comparer = null) { this.comparer = comparer ?? Comparer<TKey>.Default; }
        private static bool IsRed(Node node) { return node != null && node.Red; }
        private static Node RotateLeft(Node node)
        {
            Node pivot = node.Right; node.Right = pivot.Left; pivot.Left = node; pivot.Red = node.Red; node.Red = true; return pivot;
        }
        private static Node RotateRight(Node node)
        {
            Node pivot = node.Left; node.Left = pivot.Right; pivot.Right = node; pivot.Red = node.Red; node.Red = true; return pivot;
        }
        public void Insert(TKey key, TValue value) { root = Insert(root, key, value); root.Red = false; }
        private Node Insert(Node node, TKey key, TValue value)
        {
            if (node == null) { Count++; return new Node(key, value); }
            int order = comparer.Compare(key, node.Key);
            if (order < 0) node.Left = Insert(node.Left, key, value); else if (order > 0) node.Right = Insert(node.Right, key, value); else node.Value = value;
            if (IsRed(node.Right) && !IsRed(node.Left)) node = RotateLeft(node);
            if (IsRed(node.Left) && IsRed(node.Left.Left)) node = RotateRight(node);
            if (IsRed(node.Left) && IsRed(node.Right)) { node.Red = !node.Red; node.Left.Red = !node.Left.Red; node.Right.Red = !node.Right.Red; }
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
            if (IsRed(root) || BlackHeight(root) < 0) return false;
            IList<TKey> keys = Inorder(); if (keys.Count != Count) return false;
            for (int i = 1; i < keys.Count; i++) if (comparer.Compare(keys[i - 1], keys[i]) >= 0) return false;
            return true;
        }
        private static int BlackHeight(Node node)
        {
            if (node == null) return 1;
            if (IsRed(node) && (IsRed(node.Left) || IsRed(node.Right))) return -1;
            // The insertion algorithm additionally maintains left-leaning links.
            if (IsRed(node.Right)) return -1;
            int left = BlackHeight(node.Left), right = BlackHeight(node.Right);
            return left < 0 || right < 0 || left != right ? -1 : left + (node.Red ? 0 : 1);
        }
    }
}
