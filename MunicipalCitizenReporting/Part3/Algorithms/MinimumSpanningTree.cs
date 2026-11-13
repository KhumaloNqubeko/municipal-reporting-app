using System;
using System.Collections.Generic;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Algorithms
{
    public static class MinimumSpanningTree
    {
        public static SpanningForest Kruskal(WeightedGraph graph)
        {
            if (graph.Directed) throw new ArgumentException("Kruskal requires an undirected network.", "graph");
            var candidates = new List<GraphEdge>(graph.Edges);
            candidates.Sort(delegate(GraphEdge a, GraphEdge b)
            { int c = a.Weight.CompareTo(b.Weight); if (c == 0) c = StringComparer.Ordinal.Compare(a.From, b.From); return c != 0 ? c : StringComparer.Ordinal.Compare(a.To, b.To); });
            var sets = new DisjointSet(graph.Vertices); var selected = new List<GraphEdge>(); double total = 0;
            foreach (GraphEdge edge in candidates) if (sets.Union(edge.From, edge.To))
            { total += edge.Weight; if (double.IsInfinity(total)) throw new OverflowException("Total weight exceeds the supported range."); selected.Add(edge); }
            return new SpanningForest(selected, total, sets.Components);
        }
    }
}
