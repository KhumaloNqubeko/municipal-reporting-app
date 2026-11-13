using System;
using System.Linq;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Part3.Adapters;
using MunicipalCitizenReporting.Part3.Models;
using MunicipalCitizenReporting.Part3.Services;

namespace Part3.Tests
{
    internal static class RequestIntegrationTests
    {
        internal static void Run()
        {
            Program.Test("Integration.RealRepositorySnapshotWithoutMutation", () => {
                var repository = IssueRepository.Instance; var issue = repository.AddIssue("Test location", "Water", "Integration test request", @"C:\demo\proof.pdf");
                int count = repository.Issues.Count; var adapter = new ExistingRequestReadAdapter(); var snapshot = adapter.ReadSnapshot(); var row = snapshot.Single(r => r.Id == issue.Id);
                Check.Equal(issue.ReferenceNumber, row.Reference); Check.Equal("proof.pdf", row.AttachmentFileName); Check.Equal("Not available", row.Priority); Check.Equal("Submitted", row.Status);
                Check.Equal(count, repository.Issues.Count); Check.Equal("Integration test request", issue.Description);
                issue.Description = "Changed in source by test"; Check.Equal("Integration test request", row.Description); Check.Equal("Changed in source by test", adapter.ReadSnapshot().Single(r => r.Id == issue.Id).Description);
                issue.Description = "Integration test request";
                var later = repository.AddIssue("Later location", "Roads", "New submission", null); Check.Equal(count + 1, adapter.ReadSnapshot().Count); Check.Equal(issue.Id + 1, later.Id);
                string message; Check.True(new ServiceRequestStatusService(adapter.ReadSnapshot()).Lookup(later.ReferenceNumber, out message) != null);
            });
            Program.Test("Integration.ExactTrimmedInvalidAndMissingLookup", () => {
                var service = new ServiceRequestStatusService(Data()); string message; Check.Equal(1001, service.Lookup("  REQ-1001  ", out message).Id);
                foreach (string invalid in new[] { null, "", "   ", "req-1001", "1001", "REQ-01001", "REQ-1x", "REQ-+1001", "REQ-99999999999999" }) Check.True(service.Lookup(invalid, out message) == null);
                Check.True(service.Lookup("REQ-9999", out message) == null); Check.True(message.Contains("No request")); });
            Program.Test("Integration.ReloadReflectsSourceStatusChange", () =>
            {
                var issue = IssueRepository.Instance.AddIssue("Test site", "Water", "Status refresh fixture", null);
                var adapter = new ExistingRequestReadAdapter();
                var original = new ServiceRequestStatusService(adapter.ReadSnapshot());
                issue.Status = "Resolved";
                var refreshed = new ServiceRequestStatusService(adapter.ReadSnapshot());

                string message;
                Check.Equal("Submitted", original.Lookup(issue.ReferenceNumber, out message).Status);
                Check.Equal("Resolved", refreshed.Lookup(issue.ReferenceNumber, out message).Status);
                Check.True(refreshed.Filter(null, "Resolved", "Water", true)
                    .Any(request => request.Reference == issue.ReferenceNumber));
            });
            Program.Test("Integration.FilterSortResetAndEmpty", () => {
                var service = new ServiceRequestStatusService(Data());
                Check.Sequence(new[] { 1003, 1002, 1001 }, service.Filter(null, null, null, true).Select(r => r.Id));
                Check.Sequence(new[] { 1001, 1002, 1003 }, service.Filter(null, null, null, false).Select(r => r.Id));
                Check.Sequence(new[] { 1001 }, service.Filter("  LEAK  ", "Submitted", "Water", true).Select(r => r.Id));
                Check.Equal(0, service.Filter("no match", null, null, true).Count); Check.Equal(3, service.Filter("", null, null, true).Count);
                Check.Sequence(new[] { "Resolved", "Submitted" }, service.Statuses); Check.Sequence(new[] { "Roads", "Water" }, service.Categories);
                Check.Sequence(new[] { 1001, 1002, 1003 }, service.Filter(null, null, null, true, true).Select(r => r.Id));
                Check.Equal(0, new ServiceRequestStatusService(new ServiceRequestViewModel[0]).Filter(null, null, null, true).Count); });
            Program.Test("Integration.MissingFieldsHonest", () => { var row = new ServiceRequestViewModel(1, "REQ-1", null, "", " ", null, DateTime.MinValue, null); Check.Equal("Not available", row.Category); Check.Equal("Not available", row.Status); Check.Equal("Not available", row.AttachmentFileName); Check.True(!row.Severity.HasValue); });
            Program.Test("Integration.CategoryAndDateIndexesPreserveTiedRequests", () =>
            {
                var date = new DateTime(2026, 1, 1);
                var snapshot = new[]
                {
                    new ServiceRequestViewModel(3, "REQ-3", "Water", "", "", null, date, "Submitted"),
                    new ServiceRequestViewModel(2, "REQ-2", "Roads", "", "", null, date.AddDays(1), "Submitted"),
                    new ServiceRequestViewModel(1, "REQ-1", "Water", "", "", null, date, "Submitted")
                };
                var service = new ServiceRequestStatusService(snapshot);
                Check.Sequence(new[] { "Roads", "Water" }, service.Categories);
                Check.Sequence(new[] { 2, 1, 3 }, service.Filter(null, null, null, true).Select(r => r.Id));
                Check.Sequence(new[] { 1, 3, 2 }, service.Filter(null, null, null, false).Select(r => r.Id));
                Check.Sequence(new[] { 1, 3 }, service.Filter(null, "Submitted", "Water", true).Select(r => r.Id));
                Check.Equal(0, service.Filter(null, null, "Unknown", true).Count);
            });
            Program.Test("Regression.Part2AlertsSearchAndRecommendations", () => {
                var manager = EventManager.Instance; Check.True(manager.AllEvents.Count > 0); Check.True(manager.GetPriorityAlerts(3).Count > 0);
                var found = manager.GetById(manager.AllEvents.First().Id); Check.True(found != null); Check.True(manager.GetRecommendations(3) != null);
                Check.True(CitizenService.Instance.Interruptions.Count > 0); Check.True(CitizenService.Instance.BuildCalendar().Count > 0); });
        }
        private static ServiceRequestViewModel[] Data()
        {
            DateTime day = new DateTime(2026, 1, 1);
            return new[] { new ServiceRequestViewModel(1001, "REQ-1001", "Water", "Pipe leak", "North", null, day, "Submitted"),
                new ServiceRequestViewModel(1002, "REQ-1002", "Roads", "Pothole", "West", null, day.AddDays(1), "Submitted"),
                new ServiceRequestViewModel(1003, "REQ-1003", "Water", "Valve", "East", null, day.AddDays(2), "Resolved") };
        }
    }
}
