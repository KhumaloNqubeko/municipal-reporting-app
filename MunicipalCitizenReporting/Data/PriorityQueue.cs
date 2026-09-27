using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Data
{

    public sealed class PriorityQueue<TElement, TPriority>
    {
        private readonly List<KeyValuePair<TElement, TPriority>> heap =
            new List<KeyValuePair<TElement, TPriority>>();
        private readonly IComparer<TPriority> comparer;

        public PriorityQueue()
            : this(Comparer<TPriority>.Default)
        {
        }

        public PriorityQueue(IComparer<TPriority> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException("comparer");
        }

        public int Count { get { return heap.Count; } }

        public void Enqueue(TElement element, TPriority priority)
        {
            heap.Add(new KeyValuePair<TElement, TPriority>(element, priority));
            int index = heap.Count - 1;
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (comparer.Compare(heap[parent].Value, heap[index].Value) <= 0) break;
                Swap(parent, index);
                index = parent;
            }
        }

        public TElement Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("The priority queue is empty.");
            TElement result = heap[0].Key;
            int last = heap.Count - 1;
            heap[0] = heap[last];
            heap.RemoveAt(last);
            int index = 0;
            while (true)
            {
                int left = (index * 2) + 1;
                int right = left + 1;
                int smallest = index;
                if (left < heap.Count && comparer.Compare(heap[left].Value, heap[smallest].Value) < 0) smallest = left;
                if (right < heap.Count && comparer.Compare(heap[right].Value, heap[smallest].Value) < 0) smallest = right;
                if (smallest == index) break;
                Swap(index, smallest);
                index = smallest;
            }
            return result;
        }

        private void Swap(int first, int second)
        {
            KeyValuePair<TElement, TPriority> value = heap[first];
            heap[first] = heap[second];
            heap[second] = value;
        }
    }
}
