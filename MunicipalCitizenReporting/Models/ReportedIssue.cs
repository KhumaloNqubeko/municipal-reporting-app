using System;

namespace MunicipalCitizenReporting.Models
{
    public class ReportedIssue
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string AttachmentPath { get; set; }

        public DateTime DateReported { get; set; }
    }
}
