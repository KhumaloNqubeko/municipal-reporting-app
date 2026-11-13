using System;
using MunicipalCitizenReporting.Part3.Algorithms;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Demo;

namespace MunicipalCitizenReporting.Part3.Services
{
    internal static class ServicePlanning
    {
        internal static string BuildSummary(string category, string destination)
        {
            // Each left branch matches its category. The right branch continues to the next decision.
            var routing = new BinaryTree<string>
            {
                Root = new BinaryTree<string>.Node("Water")
                {
                    Left = new BinaryTree<string>.Node("Water"),
                    Right = new BinaryTree<string>.Node("Roads")
                    {
                        Left = new BinaryTree<string>.Node("Roads"),
                        Right = new BinaryTree<string>.Node("Intake")
                    }
                }
            };
            var branch = routing.Root;
            while (branch.Left != null)
                branch = string.Equals(category, branch.Value, StringComparison.OrdinalIgnoreCase)
                    ? branch.Left : branch.Right;

            var handoffs = MunicipalDemoData.Handoffs();
            var path = GraphTraversal.ShortestUnweightedPath(handoffs, branch.Value, destination);
            var reachable = GraphTraversal.DepthFirst(handoffs, branch.Value);
            var network = MinimumSpanningTree.Kruskal(MunicipalDemoData.MaintenanceNetwork());
            string line = Environment.NewLine;
            return "Starting team: " + branch.Value + line +
                "Fewest handoffs to " + destination + ": " +
                (path.Count == 0 ? "No route in the sample network" : string.Join(" → ", path)) + line +
                "Teams reachable from " + branch.Value + ": " + string.Join(", ", reachable) + line +
                "Handoff network review: " + (GraphTraversal.HasDirectedCycle(handoffs)
                    ? "A loop exists between teams." : "No loops found.") + line + line +
                "Maintenance site connections" + line +
                string.Join(line, network.Edges) + line +
                "Total: " + network.TotalWeight + " sample cost units across " +
                network.Components + " separate groups, including the isolated site.";
        }
    }
}
