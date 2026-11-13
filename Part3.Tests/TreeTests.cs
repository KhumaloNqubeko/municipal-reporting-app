using System.Linq;
using MunicipalCitizenReporting.Part3.DataStructures;

namespace Part3.Tests
{
    internal static class TreeTests
    {
        internal static void Run()
        {
            Program.Test("GeneralTree.EmptyAndMissing", () => { var tree = new GeneralTree<int>(); Check.Equal(0, tree.Preorder().Count); Check.Equal(0, tree.Postorder().Count); Check.True(tree.Find(7) == null); });
            Program.Test("GeneralTree.ParentAndTraversals", () => {
                var tree = new GeneralTree<int>(1); var child = tree.Root.AddChild(2); child.AddChild(4); tree.Root.AddChild(3);
                Check.True(child.Parent == tree.Root); Check.Equal(2, tree.Find(4).Parent.Value); Check.True(tree.Find(99) == null);
                Check.Sequence(new[] { 1, 2, 4, 3 }, tree.Preorder()); Check.Sequence(new[] { 4, 2, 3, 1 }, tree.Postorder()); });
            Program.Test("BinaryTree.FourTraversals", () => {
                var tree = new BinaryTree<int> { Root = new BinaryTree<int>.Node(1) { Left = new BinaryTree<int>.Node(2) { Left = new BinaryTree<int>.Node(4) }, Right = new BinaryTree<int>.Node(3) } };
                Check.Sequence(new[] { 1, 2, 4, 3 }, tree.Preorder()); Check.Sequence(new[] { 4, 2, 1, 3 }, tree.Inorder());
                Check.Sequence(new[] { 4, 2, 3, 1 }, tree.Postorder()); Check.Sequence(new[] { 1, 2, 3, 4 }, tree.LevelOrder()); });
            Program.Test("BinaryTree.Empty", () => { var tree = new BinaryTree<int>(); Check.Equal(0, tree.Preorder().Count + tree.Inorder().Count + tree.Postorder().Count + tree.LevelOrder().Count); });
            Program.Test("BST.EmptyDuplicatesAndSorted", () => {
                var tree = new BinarySearchTree<int, string>(); string value; Check.True(!tree.TryFind(2, out value)); Check.True(!tree.Delete(2));
                foreach (int key in new[] { 5, 2, 8, 1, 3 }) tree.Insert(key, key.ToString()); tree.Insert(2, "updated");
                Check.Equal(5, tree.Count); Check.True(tree.TryFind(2, out value)); Check.Equal("updated", value); Check.Sequence(new[] { 1, 2, 3, 5, 8 }, tree.Inorder()); });
            Program.Test("BST.DeleteLeafOneChildTwoChildrenRoot", () => {
                var tree = new BinarySearchTree<int, int>(); foreach (int key in new[] { 5, 2, 8, 1, 3, 7, 9, 6 }) tree.Insert(key, key);
                Check.True(tree.Delete(1)); Check.True(tree.Delete(7)); Check.True(tree.Delete(5)); Check.Sequence(new[] { 2, 3, 6, 8, 9 }, tree.Inorder()); Check.Equal(5, tree.Count);
                foreach (int key in new[] { 2, 3, 6, 8, 9 }) Check.True(tree.Delete(key)); Check.Equal(0, tree.Count); });
            Program.Test("BST.SkewedInput", () => {
                var tree = new BinarySearchTree<int, int>(); foreach (int key in Enumerable.Range(0, 2000)) tree.Insert(key, key);
                Check.Sequence(Enumerable.Range(0, 2000), tree.Inorder()); int value; Check.True(tree.TryFind(1999, out value)); Check.True(tree.Delete(1000)); Check.Equal(1999, tree.Count); });
        }
    }
}
