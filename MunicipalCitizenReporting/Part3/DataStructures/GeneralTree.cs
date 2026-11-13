using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class GeneralTree<T>
    {
        public sealed class Node
        {
            private readonly List<Node> children = new List<Node>();
            public T Value { get; private set; }
            public Node Parent { get; private set; }
            public IList<Node> Children { get { return children.AsReadOnly(); } }
            internal Node(T value, Node parent) { Value = value; Parent = parent; }
            public Node AddChild(T value) { var node = new Node(value, this); children.Add(node); return node; }
        }
        public Node Root { get; private set; }
        public GeneralTree() { }
        public GeneralTree(T root) { Root = new Node(root, null); }
        public Node Find(T value) { return Find(Root, value); }
        private static Node Find(Node node, T value)
        {
            if (node == null) return null;
            if (EqualityComparer<T>.Default.Equals(node.Value, value)) return node;
            foreach (Node child in node.Children) { Node found = Find(child, value); if (found != null) return found; }
            return null;
        }
        public IList<T> Preorder() { var values = new List<T>(); Traverse(Root, values, false); return values.AsReadOnly(); }
        public IList<T> Postorder() { var values = new List<T>(); Traverse(Root, values, true); return values.AsReadOnly(); }
        private static void Traverse(Node node, List<T> values, bool post)
        {
            if (node == null) return;
            if (!post) values.Add(node.Value);
            foreach (Node child in node.Children) Traverse(child, values, post);
            if (post) values.Add(node.Value);
        }
    }
}
