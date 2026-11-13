using System;
using System.Collections.Generic;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Algorithms
{
    public static class GraphTraversal
    {
        public static IList<string> BreadthFirst(WeightedGraph graph, string start)
        {
            var result = new List<string>(); if (!graph.ContainsVertex(start)) return result.AsReadOnly();
            var seen = new HashSet<string>(StringComparer.Ordinal); var queue = new Queue<string>(); seen.Add(start); queue.Enqueue(start);
            while (queue.Count > 0)
            {
                string vertex = queue.Dequeue(); result.Add(vertex);
                foreach (GraphEdge edge in graph.Neighbours(vertex)) if (seen.Add(edge.To)) queue.Enqueue(edge.To);
            }
            return result.AsReadOnly();
        }
        // BFS ignores weights: this is a path with the fewest handoffs, not minimum weighted cost.
        public static IList<string> ShortestUnweightedPath(WeightedGraph graph, string start, string end)
        {
            var result = new List<string>(); if (!graph.ContainsVertex(start) || !graph.ContainsVertex(end)) return result.AsReadOnly();
            var parents = new Dictionary<string, string>(StringComparer.Ordinal); var queue = new Queue<string>(); parents.Add(start, null); queue.Enqueue(start);
            while (queue.Count > 0)
            {
                string vertex = queue.Dequeue(); if (vertex == end) break;
                foreach (GraphEdge edge in graph.Neighbours(vertex)) if (!parents.ContainsKey(edge.To)) { parents.Add(edge.To, vertex); queue.Enqueue(edge.To); }
            }
            if (!parents.ContainsKey(end)) return result.AsReadOnly();
            for (string vertex = end; vertex != null; vertex = parents[vertex]) result.Add(vertex);
            result.Reverse(); return result.AsReadOnly();
        }
        public static IList<string> DepthFirst(WeightedGraph graph, string start)
        {
            var result = new List<string>(); if (!graph.ContainsVertex(start)) return result.AsReadOnly();
            var seen = new HashSet<string>(StringComparer.Ordinal); var stack = new Stack<string>(); stack.Push(start);
            while (stack.Count > 0)
            {
                string vertex = stack.Pop(); if (!seen.Add(vertex)) continue; result.Add(vertex);
                IList<GraphEdge> neighbours = graph.Neighbours(vertex);
                for (int i = neighbours.Count - 1; i >= 0; i--) if (!seen.Contains(neighbours[i].To)) stack.Push(neighbours[i].To);
            }
            return result.AsReadOnly();
        }
        public static bool HasDirectedCycle(WeightedGraph graph)
        {
            if (!graph.Directed) throw new ArgumentException("Cycle analysis here requires a directed graph.", "graph");
            var colours = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (string vertex in graph.Vertices) colours[vertex] = 0;
            // Explicit DFS frames avoid recursion depth limits on long handoff chains.
            foreach (string start in graph.Vertices)
            {
                if (colours[start] != 0) continue;
                var stack = new Stack<KeyValuePair<string, IEnumerator<GraphEdge>>>();
                colours[start] = 1; stack.Push(new KeyValuePair<string, IEnumerator<GraphEdge>>(start, graph.Neighbours(start).GetEnumerator()));
                while (stack.Count > 0)
                {
                    var frame = stack.Peek();
                    if (!frame.Value.MoveNext()) { frame.Value.Dispose(); colours[frame.Key] = 2; stack.Pop(); continue; }
                    string next = frame.Value.Current.To;
                    if (colours[next] == 1) { foreach (var pending in stack) pending.Value.Dispose(); return true; }
                    if (colours[next] == 0) { colours[next] = 1; stack.Push(new KeyValuePair<string, IEnumerator<GraphEdge>>(next, graph.Neighbours(next).GetEnumerator())); }
                }
            }
            return false;
        }
    }
}
