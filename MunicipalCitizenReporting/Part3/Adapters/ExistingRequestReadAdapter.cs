using System;
using System.Collections.Generic;
using System.IO;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Adapters
{
    public sealed class ExistingRequestReadAdapter : IRequestReadAdapter
    {
        // Call on the owning UI thread: the frozen repository does not implement synchronization.
        public IList<ServiceRequestViewModel> ReadSnapshot()
        {
            var result = new List<ServiceRequestViewModel>();
            foreach (var issue in IssueRepository.Instance.Issues)
            {
                string attachment = null;
                try { attachment = string.IsNullOrWhiteSpace(issue.AttachmentPath) ? null : Path.GetFileName(issue.AttachmentPath); }
                catch (ArgumentException) { attachment = null; }
                result.Add(new ServiceRequestViewModel(issue.Id, issue.ReferenceNumber, issue.Category, issue.Description,
                    issue.Location, attachment, issue.DateReported, issue.Status));
            }
            return result.AsReadOnly();
        }
    }
}
