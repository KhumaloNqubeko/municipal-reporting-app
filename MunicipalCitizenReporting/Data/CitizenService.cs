using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Data
{
    public sealed class CitizenService
    {
        private static readonly CitizenService instance = new CitizenService();
        private readonly List<ServiceInterruption> interruptions = new List<ServiceInterruption>();
        private readonly List<CitizenNotification> notifications = new List<CitizenNotification>();
        private readonly List<MunicipalContact> contacts = new List<MunicipalContact>();
        private readonly HashSet<string> notificationKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Queue<MunicipalEvent> reminderQueue = new Queue<MunicipalEvent>();

        private CitizenService()
        {
            SeedInterruptions();
            SeedContacts();
            GenerateInitialNotifications();
            EventManager.Instance.DataChanged += delegate { GenerateSavedEventReminders(); OnDataChanged(); };
        }

        public static CitizenService Instance { get { return instance; } }
        public event EventHandler DataChanged;
        public ReadOnlyCollection<ServiceInterruption> Interruptions { get { return interruptions.AsReadOnly(); } }
        public ReadOnlyCollection<MunicipalContact> Contacts { get { return contacts.AsReadOnly(); } }
        public ReadOnlyCollection<CitizenNotification> Notifications { get { return notifications.AsReadOnly(); } }
        public int UnreadCount { get { return notifications.Count(value => !value.IsRead); } }

        public IList<string> GetAreas()
        {
            return EventManager.Instance.AllEvents.Select(GetSpecificArea)
                .Concat(interruptions.Select(value => value.Area))
                .Where(value => !string.IsNullOrWhiteSpace(value) && value.IndexOf("All ", StringComparison.OrdinalIgnoreCase) < 0 && value.IndexOf("Selected ", StringComparison.OrdinalIgnoreCase) < 0)
                .Select(NormaliseArea).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(value => value).ToList();
        }

        public IList<ServiceInterruption> SearchInterruptions(ServiceType? type, string areaText, bool myAreaOnly)
        {
            IEnumerable<ServiceInterruption> query = interruptions.Where(value => value.Status != InterruptionStatus.Resolved);
            if (type.HasValue) query = query.Where(value => value.ServiceType == type.Value);
            if (!string.IsNullOrWhiteSpace(areaText)) query = query.Where(value => Contains(value.Area, areaText));
            if (myAreaOnly && !string.IsNullOrWhiteSpace(EventManager.Instance.MyArea))
                query = query.Where(value => Contains(value.Area, EventManager.Instance.MyArea));
            return query.OrderBy(value => value.Priority).ThenBy(value => value.StartDateTime).ToList();
        }

        public ServiceInterruption GetInterruption(int id) { return interruptions.FirstOrDefault(value => value.Id == id); }
        public CitizenNotification GetNotification(int id) { return notifications.FirstOrDefault(value => value.Id == id); }

        public void MarkRead(int id)
        {
            CitizenNotification item = GetNotification(id);
            if (item != null && !item.IsRead) { item.IsRead = true; OnDataChanged(); }
        }

        public void MarkAllRead()
        {
            foreach (CitizenNotification item in notifications) item.IsRead = true;
            OnDataChanged();
        }

        public void RemoveNotification(int id)
        {
            CitizenNotification item = GetNotification(id);
            if (item != null) { notifications.Remove(item); OnDataChanged(); }
        }

        public SortedDictionary<DateTime, List<CalendarActivity>> BuildCalendar()
        {
            var calendar = new SortedDictionary<DateTime, List<CalendarActivity>>();
            foreach (MunicipalEvent item in EventManager.Instance.AllEvents)
                AddActivity(calendar, new CalendarActivity { Date = item.Date, Title = item.Title, Category = item.Category, Area = item.Location, RelatedItemId = item.Id });
            foreach (ServiceInterruption item in interruptions.Where(value => value.IsPlanned))
                AddActivity(calendar, new CalendarActivity { Date = item.StartDateTime, Title = item.Title, Category = item.ServiceType.ToString(), Area = item.Area, RelatedItemId = item.Id, IsInterruption = true });
            return calendar;
        }

        private static void AddActivity(SortedDictionary<DateTime, List<CalendarActivity>> calendar, CalendarActivity item)
        {
            List<CalendarActivity> list;
            if (!calendar.TryGetValue(item.Date.Date, out list)) calendar[item.Date.Date] = list = new List<CalendarActivity>();
            list.Add(item);
        }

        private void GenerateInitialNotifications()
        {
            foreach (ServiceInterruption item in interruptions.Where(value => value.Priority <= 2))
                AddNotification("outage:" + item.Id, item.Title, item.Area + " • " + FriendlyStatus(item.Status), item.Priority == 1 ? NotificationType.Emergency : NotificationType.ServiceInterruption, item.Id, true);
            foreach (MunicipalEvent item in EventManager.Instance.AllEvents.Where(value => value.IsAlert && value.Priority <= 2).Take(2))
                AddNotification("event:" + item.Id, item.Title, item.Location, item.IsEmergency ? NotificationType.Emergency : NotificationType.Announcement, item.Id, false);
        }

        private void GenerateSavedEventReminders()
        {
            foreach (MunicipalEvent item in EventManager.Instance.AllEvents.Where(value => value.Type == EventType.Event && value.IsSaved && value.Date <= DateTime.Now.AddDays(2) && value.Date >= DateTime.Now))
                reminderQueue.Enqueue(item);
            while (reminderQueue.Count > 0)
            {
                MunicipalEvent item = reminderQueue.Dequeue();
                string when = item.Date.Date == DateTime.Today.AddDays(1) ? "tomorrow" : "soon";
                AddNotification("reminder:" + item.Id, item.Title + " is " + when, item.Location, NotificationType.EventReminder, item.Id, false);
            }
        }

        private void AddNotification(string key, string title, string message, NotificationType type, int relatedId, bool interruption)
        {
            if (!notificationKeys.Add(key)) return;
            notifications.Add(new CitizenNotification { Id = notifications.Count + 1, Title = title, Message = message, Type = type, CreatedAt = DateTime.Now.AddMinutes(-(notifications.Count * 17 + 10)), RelatedItemId = relatedId, RelatedToInterruption = interruption });
        }

        private void SeedInterruptions()
        {
            DateTime now = DateTime.Now;
            interruptions.Add(new ServiceInterruption { Id = 101, Title = "Planned Electricity Interruption", Description = "Interruption for essential network maintenance. Treat all electrical equipment as live.", ServiceType = ServiceType.Electricity, Area = "Winterton", StartDateTime = DateTime.Today.AddDays(1).AddHours(9), ExpectedRestorationTime = DateTime.Today.AddDays(1).AddHours(15), Status = InterruptionStatus.Scheduled, Priority = 1, IsPlanned = true, LastUpdated = now.AddMinutes(-25), RelatedEventId = 2 });
            interruptions.Add(new ServiceInterruption { Id = 102, Title = "Water Supply Interruption", Description = "Notice while teams investigate low water pressure and repair a supply main.", ServiceType = ServiceType.Water, Area = "Bergville", StartDateTime = now.AddHours(-2), ExpectedRestorationTime = now.AddHours(4), Status = InterruptionStatus.RepairsInProgress, Priority = 1, IsPlanned = false, LastUpdated = now.AddMinutes(-10), RelatedEventId = 1 });
            interruptions.Add(new ServiceInterruption { Id = 103, Title = "Road Maintenance and Stop/Go", Description = "Resurfacing notice. Allow additional travel time through the affected section.", ServiceType = ServiceType.Roads, Area = "Bergville", StartDateTime = DateTime.Today.AddDays(3).AddHours(7), ExpectedRestorationTime = DateTime.Today.AddDays(3).AddHours(17), Status = InterruptionStatus.Scheduled, Priority = 2, IsPlanned = true, LastUpdated = now.AddHours(-3), RelatedEventId = 3 });
            interruptions.Add(new ServiceInterruption { Id = 104, Title = "Waste Collection Delay", Description = "Collection disruption affecting selected routes. Revised collection information will follow.", ServiceType = ServiceType.Waste, Area = "Winterton", StartDateTime = DateTime.Today, Status = InterruptionStatus.Investigating, Priority = 3, IsPlanned = false, LastUpdated = now.AddHours(-1) });
        }


        private void SeedContacts()
        {
            contacts.Add(new MunicipalContact { Department = "Emergency Services", Description = "Eergency coordination contact", Phone = "0360 0001", Email = "emergency@okhahlamba.co.za", OperatingHours = "24 hours", IsEmergency = true });
            contacts.Add(new MunicipalContact { Department = "General Enquiries", Description = "General municipal information and service guidance", Phone = "036 000 0002", Email = "enquiries@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
            contacts.Add(new MunicipalContact { Department = "Electricity", Description = "Electricity faults and supply enquiries", Phone = "036 000 0003", Email = "electricity@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
            contacts.Add(new MunicipalContact { Department = "Water & Sanitation", Description = "Water supply, leaks and sanitation enquiries", Phone = "036 000 0004", Email = "water@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
            contacts.Add(new MunicipalContact { Department = "Roads", Description = "Municipal road and stormwater enquiries", Phone = "036 000 0005", Email = "roads@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
            contacts.Add(new MunicipalContact { Department = "Waste Management", Description = "Refuse collection and waste enquiries", Phone = "036 000 0006", Email = "waste@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
            contacts.Add(new MunicipalContact { Department = "Billing / Accounts", Description = "Account and billing enquiries", Phone = "036 000 0007", Email = "accounts@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
            contacts.Add(new MunicipalContact { Department = "Community Services", Description = "Community programmes and facilities", Phone = "036 000 0008", Email = "community@okhahlamba.co.za", OperatingHours = "08:00 - 16:30 weekdays" });
        }

        public static string FriendlyStatus(InterruptionStatus status) { return status.ToString().Replace("InProgress", "In progress").Replace("RepairsInProgress", "Repairs in progress"); }
        private static bool Contains(string source, string value) { return !string.IsNullOrEmpty(source) && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0; }
        private static string GetSpecificArea(MunicipalEvent item) { return item.Location; }
        private static string NormaliseArea(string area)
        {
            if (Contains(area, "Bergville")) return "Bergville";
            if (Contains(area, "Winterton")) return "Winterton";
            return area.Split(',')[0].Trim();
        }
        private void OnDataChanged() { EventHandler handler = DataChanged; if (handler != null) handler(this, EventArgs.Empty); }
    }
}
