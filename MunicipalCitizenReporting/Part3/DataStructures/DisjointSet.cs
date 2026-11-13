using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.DataStructures
{
    public sealed class DisjointSet
    {
        private readonly Dictionary<string, string> parents = new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> ranks = new Dictionary<string, int>(StringComparer.Ordinal);
        public int Components { get; private set; }
        public DisjointSet(IEnumerable<string> vertices)
        { foreach (string vertex in vertices) if (!parents.ContainsKey(vertex)) { parents.Add(vertex, vertex); ranks.Add(vertex, 0); Components++; } }
        public string Find(string vertex)
        {
            if (!parents.ContainsKey(vertex)) throw new ArgumentException("Unknown vertex.", "vertex");
            string root = vertex; while (parents[root] != root) root = parents[root];
            while (vertex != root) { string next = parents[vertex]; parents[vertex] = root; vertex = next; }
            return root;
        }
        public bool Union(string a, string b)
        {
            string first = Find(a), second = Find(b); if (first == second) return false;
            if (ranks[first] < ranks[second]) parents[first] = second;
            else { parents[second] = first; if (ranks[first] == ranks[second]) ranks[first]++; }
            Components--; return true;
        }
    }
}
