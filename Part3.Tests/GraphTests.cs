using System;
using System.Linq;
using MunicipalCitizenReporting.Part3.Algorithms;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Demo;

namespace Part3.Tests
{
    internal static class GraphTests
    {
        internal static void Run()
        {
            Program.Test("Graph.BfsDfsAndShortestPath", () => {
                var graph = new WeightedGraph(true); graph.AddEdge("A", "C", 1); graph.AddEdge("A", "B", 99); graph.AddEdge("B", "D", 99); graph.AddEdge("C", "E", 1); graph.AddEdge("E", "D", 1); graph.AddVertex("Z");
                Check.Sequence(new[] { "A", "B", "C", "D", "E" }, GraphTraversal.BreadthFirst(graph, "A"));
                Check.Sequence(new[] { "A", "B", "D", "C", "E" }, GraphTraversal.DepthFirst(graph, "A"));
                Check.Sequence(new[] { "A", "B", "D" }, GraphTraversal.ShortestUnweightedPath(graph, "A", "D"));
                Check.Equal(0, GraphTraversal.ShortestUnweightedPath(graph, "A", "Z").Count); Check.Sequence(new[] { "Z" }, GraphTraversal.DepthFirst(graph, "Z")); Check.True(!GraphTraversal.HasDirectedCycle(graph)); });
            Program.Test("Graph.CycleSelfLoopAndIsolated", () => {
                var graph = MunicipalDemoData.Handoffs(); Check.True(GraphTraversal.HasDirectedCycle(graph)); Check.Equal(5, GraphTraversal.BreadthFirst(graph, "Intake").Count);
                Check.Equal(0, GraphTraversal.BreadthFirst(graph, "missing").Count); Check.Sequence(new[] { "Unassigned" }, GraphTraversal.ShortestUnweightedPath(graph, "Unassigned", "Unassigned"));
                var self = new WeightedGraph(true); self.AddEdge("X", "X"); Check.True(GraphTraversal.HasDirectedCycle(self)); });
            Program.Test("Graph.EmptyAndValidation", () => {
                var graph = new WeightedGraph(true); Check.True(!GraphTraversal.HasDirectedCycle(graph)); Check.Equal(0, GraphTraversal.DepthFirst(graph, "A").Count);
                foreach (double invalid in new[] { -1.0, double.NaN, double.PositiveInfinity }) Check.Throws<ArgumentOutOfRangeException>(() => graph.AddEdge("A", "B", invalid));
                Check.Throws<ArgumentException>(() => graph.AddEdge("A", "")); Check.Equal(0, graph.Vertices.Count);
                Check.Throws<ArgumentException>(() => GraphTraversal.HasDirectedCycle(new WeightedGraph(false))); });
            Program.Test("Graph.MunicipalHandoffTraversalAndPath", () =>
            {
                var graph = MunicipalDemoData.Handoffs();
                Check.Sequence(new[] { "Intake", "Roads", "Water", "Inspection", "Works" },
                    GraphTraversal.BreadthFirst(graph, "Intake"));
                Check.Sequence(new[] { "Intake", "Roads", "Inspection", "Works", "Water" },
                    GraphTraversal.DepthFirst(graph, "Intake"));
                Check.Sequence(new[] { "Intake", "Roads", "Inspection", "Works" },
                    GraphTraversal.ShortestUnweightedPath(graph, "Intake", "Works"));
                Check.Equal(0, GraphTraversal.ShortestUnweightedPath(graph, "Intake", "Unassigned").Count);
            });
            Program.Test("Graph.LongChainCycleAnalysis", () => {
                var graph = new WeightedGraph(true); for (int i = 0; i < 5000; i++) graph.AddEdge(i.ToString("D5"), (i + 1).ToString("D5"));
                Check.True(!GraphTraversal.HasDirectedCycle(graph)); graph.AddEdge("05000", "00000"); Check.True(GraphTraversal.HasDirectedCycle(graph)); });
            Program.Test("MST.KnownOptimumAndDisconnectedForest", () => {
                var forest = MinimumSpanningTree.Kruskal(MunicipalDemoData.MaintenanceNetwork()); Check.Equal(6.0, forest.TotalWeight); Check.Equal(2, forest.Components); Check.Equal(3, forest.Edges.Count);
                Check.Sequence(new[] { "Site B-Site C", "Site A-Site B", "Site C-Site D" }, forest.Edges.Select(e => e.From + "-" + e.To)); });
            Program.Test("MST.TiesDeterministicAndCanonicalEndpoints", () => {
                var graph = new WeightedGraph(false); graph.AddEdge("C", "B", 1); graph.AddEdge("B", "A", 1); graph.AddEdge("C", "A", 1); graph.AddEdge("A", "A", 0); graph.AddEdge("A", "B", 1);
                var forest = MinimumSpanningTree.Kruskal(graph); Check.Equal(2.0, forest.TotalWeight); Check.Equal(1, forest.Components); Check.Sequence(new[] { "A-B", "A-C" }, forest.Edges.Select(e => e.From + "-" + e.To)); });
            Program.Test("MST.EmptySingleAndDirectedRejected", () => {
                var graph = new WeightedGraph(false); Check.Equal(0, MinimumSpanningTree.Kruskal(graph).Components); graph.AddVertex("A"); var forest = MinimumSpanningTree.Kruskal(graph);
                Check.Equal(1, forest.Components); Check.Equal(0, forest.Edges.Count); Check.Equal(0.0, forest.TotalWeight); Check.Throws<ArgumentException>(() => MinimumSpanningTree.Kruskal(new WeightedGraph(true))); });
            Program.Test("MST.WeightOverflow", () => { var graph = new WeightedGraph(false); graph.AddEdge("A", "B", double.MaxValue); graph.AddEdge("B", "C", double.MaxValue); Check.Throws<OverflowException>(() => MinimumSpanningTree.Kruskal(graph)); });
            Program.Test("DisjointSet.UnionFind", () => { var sets = new DisjointSet(new[] { "A", "B", "C", "A" }); Check.Equal(3, sets.Components); Check.True(sets.Union("A", "B")); Check.True(!sets.Union("B", "A")); Check.Equal(sets.Find("A"), sets.Find("B")); Check.Equal(2, sets.Components); Check.Throws<ArgumentException>(() => sets.Find("missing")); });
        }
    }
}
