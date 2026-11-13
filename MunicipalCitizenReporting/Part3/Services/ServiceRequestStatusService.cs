using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Services
{
    public sealed class ServiceRequestStatusService
    {
        private readonly IList<ServiceRequestViewModel> requests;
        private readonly RequestIndexService index;
        private readonly GeneralTree<string> categoryTree = new GeneralTree<string>("Municipal services");
        private readonly BinarySearchTree<string, List<ServiceRequestViewModel>> categoryIndex =
            new BinarySearchTree<string, List<ServiceRequestViewModel>>(StringComparer.Ordinal);
        private readonly IList<ServiceRequestViewModel> chronological;
        public IList<ServiceRequestViewModel> Requests { get { return requests; } }
        public ServiceRequestStatusService(IEnumerable<ServiceRequestViewModel> snapshot)
        {
            requests = new List<ServiceRequestViewModel>(snapshot).AsReadOnly();
            index = new RequestIndexService(requests);

            // The ordinal preserves source order when timestamp and ID are both equal.
            var dates = new RedBlackTree<Tuple<DateTime, int, int>, ServiceRequestViewModel>();
            for (int position = 0; position < requests.Count; position++)
            {
                var request = requests[position];
                dates.Insert(Tuple.Create(request.Submitted, request.Id, position), request);
            }
            chronological = dates.Inorder().Select(key => requests[key.Item3]).ToList().AsReadOnly();
            foreach (var group in chronological.GroupBy(request => request.Category))
            {
                categoryTree.Root.AddChild(group.Key);
                categoryIndex.Insert(group.Key, group.ToList());
            }
        }
        public IList<string> Statuses { get { return requests.Select(r => r.Status).Distinct(StringComparer.Ordinal).OrderBy(s => s, StringComparer.Ordinal).ToList().AsReadOnly(); } }
        public IList<string> Categories
        {
            get
            {
                return categoryTree.Root.Children.Select(category => category.Value)
                    .OrderBy(category => category, StringComparer.Ordinal).ToList().AsReadOnly();
            }
        }
        public IList<ServiceRequestViewModel> Filter(string text, string status, string category, bool newestFirst, bool priorityOrder = false)
        {
            string query = (text ?? "").Trim();
            IEnumerable<ServiceRequestViewModel> candidates = chronological;
            if (category != null)
            {
                List<ServiceRequestViewModel> categoryRequests;
                if (!categoryIndex.TryFind(category, out categoryRequests))
                    return new List<ServiceRequestViewModel>().AsReadOnly();
                candidates = categoryRequests;
            }
            IEnumerable<ServiceRequestViewModel> filtered = candidates.Where(r =>
                (status == null || r.Status == status) &&
                (Contains(r.Reference, query) || Contains(r.Category, query) || Contains(r.Description, query) || Contains(r.Location, query)));
            if (!priorityOrder)
            {
                // Reverse timestamp groups only, preserving ascending IDs within a tie.
                if (newestFirst)
                    filtered = filtered.GroupBy(request => request.Submitted).Reverse().SelectMany(group => group);
                return filtered.ToList().AsReadOnly();
            }
            var heap = new BinaryHeap<ServiceRequestViewModel>(new RequestPriorityComparer()); foreach (var request in filtered) heap.Insert(request);
            var result = new List<ServiceRequestViewModel>(); while (heap.Count > 0) result.Add(heap.Extract()); return result.AsReadOnly();
        }
        private static bool Contains(string source, string query) { return source != null && source.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0; }
        public ServiceRequestViewModel Lookup(string input, out string message)
        {
            string reference = (input ?? "").Trim();
            if (reference.Length == 0) { message = "Enter a request reference, for example REQ-1001."; return null; }
            if (!IsCanonicalReference(reference)) { message = "Use the exact reference shown at submission, for example REQ-1001 (uppercase REQ)."; return null; }
            ServiceRequestViewModel request = index.Find(reference);
            message = request == null ? "No request with that reference exists in this session." : "Request found.";
            return request;
        }
        private static bool IsCanonicalReference(string reference)
        {
            if (!reference.StartsWith("REQ-", StringComparison.Ordinal) || reference.Length < 5 || reference[4] == '0') return false;
            for (int i = 4; i < reference.Length; i++) if (reference[i] < '0' || reference[i] > '9') return false;
            int id; return int.TryParse(reference.Substring(4), out id) && id > 0;
        }
    }
}
