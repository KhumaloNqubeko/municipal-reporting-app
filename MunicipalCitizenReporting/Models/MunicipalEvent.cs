using System;

namespace MunicipalCitizenReporting.Models
{
    public enum EventType
    {
        Event,
        Announcement,
        Alert
    }

    public sealed class MunicipalEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public EventType Type { get; set; }
        public int Priority { get; set; }
        public bool IsAlert { get; set; }
        public bool IsEmergency { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsSaved { get; set; }
        public bool IsInterested { get; set; }
        public int InterestedCount { get; set; }
    }
}
