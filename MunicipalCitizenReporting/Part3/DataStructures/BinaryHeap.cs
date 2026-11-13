using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class BinaryHeap<T>
    {
        private readonly List<T> items = new List<T>();
        private readonly IComparer<T> comparer;
        public BinaryHeap(IComparer<T> comparer = null) { this.comparer = comparer ?? Comparer<T>.Default; }
        public int Count { get { return items.Count; } }
        public void Insert(T value)
        {
            items.Add(value); int index = items.Count - 1;
            while (index > 0) { int parent = (index - 1) / 2; if (comparer.Compare(items[parent], items[index]) <= 0) break; Swap(parent, index); index = parent; }
        }
        public T Peek() { if (Count == 0) throw new InvalidOperationException("The heap is empty."); return items[0]; }
        public T Extract()
        {
            T result = Peek(); int last = Count - 1; items[0] = items[last]; items.RemoveAt(last); int index = 0;
            while (index < Count)
            {
                int left = 2 * index + 1, right = left + 1, best = index;
                if (left < Count && comparer.Compare(items[left], items[best]) < 0) best = left;
                if (right < Count && comparer.Compare(items[right], items[best]) < 0) best = right;
                if (best == index) break; Swap(index, best); index = best;
            }
            return result;
        }
        private void Swap(int a, int b) { T value = items[a]; items[a] = items[b]; items[b] = value; }
        public bool ValidateInvariants() { for (int i = 1; i < Count; i++) if (comparer.Compare(items[(i - 1) / 2], items[i]) > 0) return false; return true; }
    }
}
