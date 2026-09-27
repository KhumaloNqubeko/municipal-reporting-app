using System;

namespace MunicipalCitizenReporting.Models
{
    public enum ServiceType { Electricity, Water, Roads, Waste, Other }
    public enum InterruptionStatus { Scheduled, InProgress, Investigating, RepairsInProgress, Restored, Resolved }
    public enum NotificationType { Announcement, Emergency, ServiceInterruption, EventReminder }

    public sealed class ServiceInterruption
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Area { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? ExpectedRestorationTime { get; set; }
        public InterruptionStatus Status { get; set; }
        public int Priority { get; set; }
        public bool IsPlanned { get; set; }
        public DateTime LastUpdated { get; set; }
        public int RelatedEventId { get; set; }
    }

    public sealed class CitizenNotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public int? RelatedItemId { get; set; }
        public bool RelatedToInterruption { get; set; }
    }

    public sealed class MunicipalContact
    {
        public string Department { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OperatingHours { get; set; }
        public bool IsEmergency { get; set; }
    }

    public sealed class CalendarActivity
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public int RelatedItemId { get; set; }
        public bool IsInterruption { get; set; }
    }
}
