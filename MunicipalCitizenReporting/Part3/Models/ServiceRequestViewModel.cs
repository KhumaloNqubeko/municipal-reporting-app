using System;
using System.Collections.Generic;

namespace MunicipalCitizenReporting.Part3.Models
{
    public sealed class ServiceRequestViewModel
    {
        public int Id { get; private set; }
        public string Reference { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public string AttachmentFileName { get; private set; }
        public DateTime Submitted { get; private set; }
        public string Status { get; private set; }
        public int? Severity { get; private set; }
        public string Priority { get { return Severity.HasValue ? "DEMO severity " + Severity.Value : "Not available"; } }
        public ServiceRequestViewModel(int id, string reference, string category, string description, string location,
            string attachmentFileName, DateTime submitted, string status, int? demoSeverity = null)
        {
            Id = id; Reference = reference; Category = Display(category); Description = Display(description);
            Location = Display(location); AttachmentFileName = Display(attachmentFileName); Submitted = submitted;
            Status = Display(status); Severity = demoSeverity;
        }
        private static string Display(string value) { return string.IsNullOrWhiteSpace(value) ? "Not available" : value; }
    }
    // Lower severity numbers rank first. Real requests have no severity; timestamp then numeric ID break ties.
    public sealed class RequestPriorityComparer : IComparer<ServiceRequestViewModel>
    {
        public int Compare(ServiceRequestViewModel a, ServiceRequestViewModel b)
        {
            if (ReferenceEquals(a, b)) return 0;
            if (a == null) return 1; if (b == null) return -1;
            int c = (a.Severity ?? int.MaxValue).CompareTo(b.Severity ?? int.MaxValue);
            if (c == 0) c = a.Submitted.CompareTo(b.Submitted);
            if (c == 0) c = a.Id.CompareTo(b.Id);
            return c != 0 ? c : StringComparer.Ordinal.Compare(a.Reference, b.Reference);
        }
    }
}
