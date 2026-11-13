using System;
using System.Collections.Generic;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class WeightedGraph
    {
        private readonly Dictionary<string, List<GraphEdge>> adjacency = new Dictionary<string, List<GraphEdge>>(StringComparer.Ordinal);
        private readonly List<GraphEdge> edges = new List<GraphEdge>();
        private readonly List<string> vertices = new List<string>();
        public bool Directed { get; private set; }
        public WeightedGraph(bool directed) { Directed = directed; }
        public IList<string> Vertices { get { return vertices.AsReadOnly(); } }
        public IList<GraphEdge> Edges { get { return edges.AsReadOnly(); } }
        public bool ContainsVertex(string vertex) { return vertex != null && adjacency.ContainsKey(vertex); }
        public void AddVertex(string vertex)
        {
            if (string.IsNullOrWhiteSpace(vertex)) throw new ArgumentException("A vertex needs a name.", "vertex");
            if (!adjacency.ContainsKey(vertex))
            {
                adjacency.Add(vertex, new List<GraphEdge>());
                int index = vertices.BinarySearch(vertex, StringComparer.Ordinal);
                vertices.Insert(~index, vertex);
            }
        }
        public void AddEdge(string from, string to, double weight = 1)
        {
            if (double.IsNaN(weight) || double.IsInfinity(weight) || weight < 0) throw new ArgumentOutOfRangeException("weight", "Use finite non-negative cost units.");
            // Validate both names before mutating the graph.
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to)) throw new ArgumentException("Both endpoints need names.");
            AddVertex(from); AddVertex(to);
            if (!Directed && StringComparer.Ordinal.Compare(from, to) > 0) { string swap = from; from = to; to = swap; }
            var edge = new GraphEdge(from, to, weight); edges.Add(edge); AddNeighbour(from, edge);
            if (!Directed && from != to) AddNeighbour(to, new GraphEdge(to, from, weight));
        }
        private void AddNeighbour(string vertex, GraphEdge edge)
        {
            // Maintain canonical adjacency order at mutation time so traversal remains O(V+E).
            List<GraphEdge> list = adjacency[vertex]; int index = list.Count;
            while (index > 0 && (StringComparer.Ordinal.Compare(list[index - 1].To, edge.To) > 0 ||
                (list[index - 1].To == edge.To && list[index - 1].Weight > edge.Weight))) index--;
            list.Insert(index, edge);
        }
        public IList<GraphEdge> Neighbours(string vertex)
        {
            if (!ContainsVertex(vertex)) return new List<GraphEdge>().AsReadOnly();
            return adjacency[vertex].AsReadOnly();
        }
    }
}
