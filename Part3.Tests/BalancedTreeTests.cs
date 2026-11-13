using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalCitizenReporting.Part3.DataStructures;

namespace Part3.Tests
{
    internal static class BalancedTreeTests
    {
        internal static void Run()
        {
            int[][] cases = { new[] { 30, 20, 10 }, new[] { 30, 10, 20 }, new[] { 10, 20, 30 }, new[] { 10, 30, 20 } };
            string[] names = { "LL", "LR", "RR", "RL" };
            for (int c = 0; c < cases.Length; c++) { int[] keys = cases[c]; Program.Test("AVL.Rotation" + names[c], () => {
                var tree = new AvlTree<int, int>(); foreach (int key in keys) { tree.Insert(key, key); Check.True(tree.ValidateInvariants()); }
                Check.Equal(2, tree.Height); Check.Sequence(new[] { 10, 20, 30 }, tree.Inorder()); }); }
            Program.Test("Balanced.EmptyAndDuplicatePolicy", () => {
                var avl = new AvlTree<int, int>(); var rb = new RedBlackTree<int, int>(); int value;
                Check.True(avl.ValidateInvariants() && rb.ValidateInvariants()); Check.True(!avl.TryFind(1, out value) && !rb.TryFind(1, out value));
                avl.Insert(1, 5); avl.Insert(1, 8); rb.Insert(1, 5); rb.Insert(1, 8);
                Check.Equal(1, avl.Count); Check.Equal(1, rb.Count); Check.True(avl.TryFind(1, out value)); Check.Equal(8, value); Check.True(rb.TryFind(1, out value)); Check.Equal(8, value); });
            for (int seed = 1; seed <= 5; seed++) { int testSeed = seed; Program.Test("Balanced.RandomizedSeed" + seed, () => {
                var random = new Random(testSeed); var expected = new SortedDictionary<int, int>(); var avl = new AvlTree<int, int>(); var rb = new RedBlackTree<int, int>();
                for (int i = 0; i < 1000; i++) { int key = random.Next(800); expected[key] = i; avl.Insert(key, i); rb.Insert(key, i); Check.True(avl.ValidateInvariants(), "AVL invariant"); Check.True(rb.ValidateInvariants(), "RB invariant"); }
                Check.Sequence(expected.Keys, avl.Inorder()); Check.Sequence(expected.Keys, rb.Inorder());
                foreach (var item in expected) { int value; Check.True(avl.TryFind(item.Key, out value)); Check.Equal(item.Value, value); Check.True(rb.TryFind(item.Key, out value)); Check.Equal(item.Value, value); }
            }); }
            Program.Test("Balanced.AscendingDescending", () => {
                var avl = new AvlTree<int, int>(); var rb = new RedBlackTree<int, int>();
                foreach (int key in Enumerable.Range(0, 5000)) { avl.Insert(key, key); rb.Insert(key, key); }
                Check.True(avl.ValidateInvariants() && rb.ValidateInvariants()); Check.True(avl.Height < 20);
                avl = new AvlTree<int, int>(); rb = new RedBlackTree<int, int>(); foreach (int key in Enumerable.Range(0, 5000).Reverse()) { avl.Insert(key, key); rb.Insert(key, key); }
                Check.True(avl.ValidateInvariants() && rb.ValidateInvariants()); Check.Sequence(avl.Inorder(), rb.Inorder()); });
        }
    }
}
