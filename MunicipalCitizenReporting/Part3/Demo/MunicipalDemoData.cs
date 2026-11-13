using System;
using System.Collections.Generic;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Demo
{
    public static class MunicipalDemoData
    {
        private static bool requestsSeeded;

        internal static void SeedRequests()
        {
            if (requestsSeeded)
                return;

            var repository = Data.IssueRepository.Instance;
            repository.AddIssue("Bergville", "Water", "Sample report: Water supply interrupted in Ward 09.", null);
            repository.AddIssue("Winterton", "Roads", "Sample report: Pothole near the community hall.", null);
            repository.AddIssue("Bergville", "Electricity", "Sample report: Power outage near the library.", null);
            repository.AddIssue("Bergville", "Streetlights", "Sample report: Streetlights on Mshengu Street are out.", null);
            repository.AddIssue("Ladysmith", "Waste Management", "Sample report: Scheduled waste collection was missed.", null);
            requestsSeeded = true;
        }

        public static WeightedGraph Handoffs()
        {
            var graph = new WeightedGraph(true);
            graph.AddEdge("Intake", "Roads"); graph.AddEdge("Intake", "Water");
            graph.AddEdge("Roads", "Inspection"); graph.AddEdge("Water", "Inspection");
            graph.AddEdge("Inspection", "Works"); graph.AddEdge("Works", "Inspection");
            graph.AddVertex("Unassigned"); return graph;
        }
        public static WeightedGraph MaintenanceNetwork()
        {
            var graph = new WeightedGraph(false);
            graph.AddEdge("Site A", "Site B", 2); graph.AddEdge("Site A", "Site C", 5);
            graph.AddEdge("Site B", "Site C", 1); graph.AddEdge("Site B", "Site D", 4);
            graph.AddEdge("Site C", "Site D", 3); graph.AddVertex("Isolated site"); return graph;
        }
        public static IList<ServiceRequestViewModel> PriorityRequests()
        {
            DateTime submitted = new DateTime(2026, 1, 1, 9, 0, 0);
            return new List<ServiceRequestViewModel> {
                new ServiceRequestViewModel(3, "DEMO-3", "Roads", "Demonstration only", "Demo site", null, submitted, "DEMO", 2),
                new ServiceRequestViewModel(2, "DEMO-2", "Water", "Demonstration only", "Demo site", null, submitted, "DEMO", 1),
                new ServiceRequestViewModel(1, "DEMO-1", "Water", "Demonstration only", "Demo site", null, submitted, "DEMO", 1)
            }.AsReadOnly();
        }
    }
}
