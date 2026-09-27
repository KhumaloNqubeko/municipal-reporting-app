using System;

namespace MunicipalCitizenReporting.Models
{
    public sealed class SearchRecord
    {
        public string SearchText { get; set; }
        public string Category { get; set; }
        public DateTime? SelectedDate { get; set; }
        public string DateFilter { get; set; }
        public EventType? EventType { get; set; }
        public DateTime SearchedAt { get; set; }
    }
}
