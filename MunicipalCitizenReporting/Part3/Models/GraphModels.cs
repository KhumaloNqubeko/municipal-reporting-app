using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.Models
{
    public sealed class GraphEdge
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public double Weight { get; private set; }
        public GraphEdge(string from, string to, double weight) { From = from; To = to; Weight = weight; }
        public override string ToString() { return From + " — " + To + " : " + Weight.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture); }
    }
    public sealed class SpanningForest
    {
        public IList<GraphEdge> Edges { get; private set; }
        public double TotalWeight { get; private set; }
        public int Components { get; private set; }
        public SpanningForest(IList<GraphEdge> edges, double totalWeight, int components)
        { Edges = new List<GraphEdge>(edges).AsReadOnly(); TotalWeight = totalWeight; Components = components; }
    }
}
