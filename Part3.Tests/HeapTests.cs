using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Models;
using MunicipalCitizenReporting.Part3.Demo;
using MunicipalCitizenReporting.Part3.Services;

namespace Part3.Tests
{
    internal static class HeapTests
    {
        internal static void Run()
        {
            Program.Test("Heap.MunicipalDemoPriorityThroughStatusService", () =>
            {
                var service = new ServiceRequestStatusService(MunicipalDemoData.PriorityRequests());
                var ordered = service.Filter(null, null, null, true, priorityOrder: true);
                Check.Sequence(new[] { "DEMO-1", "DEMO-2", "DEMO-3" },
                    ordered.Select(request => request.Reference));
            });
            Program.Test("Heap.EmptySingleAndPeek", () => { var heap = new BinaryHeap<int>(); Check.Throws<InvalidOperationException>(() => heap.Peek()); Check.Throws<InvalidOperationException>(() => heap.Extract()); heap.Insert(7); Check.Equal(7, heap.Peek()); Check.Equal(1, heap.Count); Check.Equal(7, heap.Extract()); Check.Equal(0, heap.Count); Check.True(heap.ValidateInvariants()); });
            Program.Test("Heap.RandomizedExtractionAndInvariants", () => {
                var random = new Random(71); var values = new List<int>(); var heap = new BinaryHeap<int>();
                for (int i = 0; i < 1000; i++) { int value = random.Next(100); values.Add(value); heap.Insert(value); Check.True(heap.ValidateInvariants()); }
                var actual = new List<int>(); while (heap.Count > 0) { actual.Add(heap.Extract()); Check.True(heap.ValidateInvariants()); } Check.Sequence(values.OrderBy(v => v), actual); });
            Program.Test("Heap.MaxComparator", () => { var heap = new BinaryHeap<int>(Comparer<int>.Create((a, b) => b.CompareTo(a))); foreach (int n in new[] { 2, 9, 1 }) heap.Insert(n); Check.Sequence(new[] { 9, 2, 1 }, new[] { heap.Extract(), heap.Extract(), heap.Extract() }); });
            Program.Test("Heap.PriorityTimestampIdAndReferenceTies", () => {
                DateTime day = new DateTime(2026, 1, 1); var heap = new BinaryHeap<ServiceRequestViewModel>(new RequestPriorityComparer());
                var values = new[] { Make(9, "Z", day, 2), Make(3, "C", day, 1), Make(2, "B", day, 1), Make(2, "A", day, 1), Make(8, "EARLY", day.AddDays(-1), 1), Make(1, "UNKNOWN", day.AddDays(-2), null) };
                foreach (var value in values) heap.Insert(value); var extracted = new List<string>(); while (heap.Count > 0) extracted.Add(heap.Extract().Reference);
                Check.Sequence(new[] { "EARLY", "A", "B", "C", "Z", "UNKNOWN" }, extracted); });
        }
        private static ServiceRequestViewModel Make(int id, string reference, DateTime date, int? priority)
        { return new ServiceRequestViewModel(id, reference, "Water", null, null, null, date, "Submitted", priority); }
    }
}
