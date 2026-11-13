using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class BinaryTree<T>
    {
        public sealed class Node
        {
            public T Value { get; private set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node(T value) { Value = value; }
        }
        public Node Root { get; set; }
        public IList<T> Preorder() { return Walk(0); }
        public IList<T> Inorder() { return Walk(1); }
        public IList<T> Postorder() { return Walk(2); }
        private IList<T> Walk(int order) { var result = new List<T>(); Visit(Root, order, result); return result.AsReadOnly(); }
        private static void Visit(Node node, int order, List<T> result)
        {
            if (node == null) return;
            if (order == 0) result.Add(node.Value);
            Visit(node.Left, order, result);
            if (order == 1) result.Add(node.Value);
            Visit(node.Right, order, result);
            if (order == 2) result.Add(node.Value);
        }
        public IList<T> LevelOrder()
        {
            var result = new List<T>(); var queue = new Queue<Node>();
            if (Root != null) queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue(); result.Add(node.Value);
                if (node.Left != null) queue.Enqueue(node.Left);
                if (node.Right != null) queue.Enqueue(node.Right);
            }
            return result.AsReadOnly();
        }
    }
}
