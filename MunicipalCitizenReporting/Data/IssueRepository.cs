using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Data
{
    public sealed class IssueRepository
    {
        private static readonly IssueRepository instance = new IssueRepository();
        private readonly List<ReportedIssue> issues = new List<ReportedIssue>();
        private int nextIssueId = 1001;

        private IssueRepository()
        {
        }

        public static IssueRepository Instance
        {
            get { return instance; }
        }

        public ReadOnlyCollection<ReportedIssue> Issues
        {
            get { return issues.AsReadOnly(); }
        }

        public ReportedIssue AddIssue(
            string location,
            string category,
            string description,
            string attachmentPath)
        {
            var issue = new ReportedIssue
            {
                Id = nextIssueId++,
                Location = location,
                Category = category,
                Description = description,
                AttachmentPath = attachmentPath,
                DateReported = DateTime.Now
            };

            issues.Add(issue);
            return issue;
        }
    }
}
