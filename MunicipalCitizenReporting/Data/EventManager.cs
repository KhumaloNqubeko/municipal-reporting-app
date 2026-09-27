using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Data
{
    public sealed class Recommendation
    {
        public MunicipalEvent Item { get; set; }
        public int Score { get; set; }
        public string Explanation { get; set; }
    }

    public sealed class EventManager
    {
        private static readonly EventManager instance = new EventManager();
        private readonly List<MunicipalEvent> allEvents = new List<MunicipalEvent>();
        private readonly PriorityQueue<MunicipalEvent, int> alertQueue = new PriorityQueue<MunicipalEvent, int>();
        private readonly Queue<MunicipalEvent> upcomingEvents = new Queue<MunicipalEvent>();
        private readonly Stack<SearchRecord> recentSearches = new Stack<SearchRecord>();
        private readonly Dictionary<int, MunicipalEvent> eventsById = new Dictionary<int, MunicipalEvent>();
        private readonly Dictionary<string, List<MunicipalEvent>> eventsByCategory =
            new Dictionary<string, List<MunicipalEvent>>(StringComparer.OrdinalIgnoreCase);
        private readonly SortedDictionary<DateTime, List<MunicipalEvent>> eventsByDate =
            new SortedDictionary<DateTime, List<MunicipalEvent>>();
        private readonly HashSet<string> uniqueCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<int> likedEventIds = new HashSet<int>();
        private readonly HashSet<int> savedEventIds = new HashSet<int>();
        private readonly HashSet<int> interestedEventIds = new HashSet<int>();
        private readonly Dictionary<string, int> categorySearchFrequency =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> keywordSearchFrequency =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private EventManager()
        {
            SeedSampleData();
            BuildIndexes();
        }

        public static EventManager Instance { get { return instance; } }
        public event EventHandler DataChanged;
        public ReadOnlyCollection<MunicipalEvent> AllEvents { get { return allEvents.AsReadOnly(); } }
        public int RecentSearchCount { get { return recentSearches.Count; } }
        public string MyArea { get; private set; }
        public bool OnlyMyArea { get; private set; }

        public void SetMyArea(string area, bool onlyMyArea)
        {
            MyArea = string.IsNullOrWhiteSpace(area) ? null : area.Trim();
            OnlyMyArea = onlyMyArea;
            OnDataChanged();
        }

        public IList<string> GetCategories()
        {
            return uniqueCategories.OrderBy(value => value).ToList();
        }

        public MunicipalEvent GetById(int id)
        {
            MunicipalEvent item;
            return eventsById.TryGetValue(id, out item) ? item : null;
        }

        public IList<MunicipalEvent> GetPriorityAlerts(int maximum)
        {
            var queue = new PriorityQueue<MunicipalEvent, int>();
            foreach (MunicipalEvent item in allEvents.Where(value => value.IsAlert && value.Date >= DateTime.Today))
                queue.Enqueue(item, item.Priority);

            var results = new List<MunicipalEvent>();
            while (queue.Count > 0 && results.Count < maximum) results.Add(queue.Dequeue());
            return results;
        }

        public IList<MunicipalEvent> Search(
            string keyword, string category, string dateFilter, EventType? eventType, bool savedOnly, bool recordSearch)
        {
            keyword = (keyword ?? string.Empty).Trim();
            category = category ?? "All categories";
            dateFilter = dateFilter ?? "All upcoming";

            IEnumerable<MunicipalEvent> query;
            if (!string.Equals(category, "All categories", StringComparison.OrdinalIgnoreCase) &&
                eventsByCategory.ContainsKey(category))
                query = eventsByCategory[category];
            else
                query = eventsByDate.Where(pair => pair.Key >= DateTime.Today).SelectMany(pair => pair.Value);

            query = query.Where(item => item.Date >= DateTime.Today);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(item => Contains(item.Title, keyword) || Contains(item.Description, keyword) ||
                    Contains(item.Category, keyword) || Contains(item.Location, keyword));
            }
            if (eventType.HasValue) query = query.Where(item => item.Type == eventType.Value);
            if (savedOnly) query = query.Where(item => savedEventIds.Contains(item.Id));
            if (OnlyMyArea && !string.IsNullOrWhiteSpace(MyArea)) query = query.Where(item => Contains(item.Location, MyArea));

            DateTime today = DateTime.Today;
            if (dateFilter == "Today") query = query.Where(item => item.Date.Date == today);
            else if (dateFilter == "Next 7 days") query = query.Where(item => item.Date.Date <= today.AddDays(7));
            else if (dateFilter == "Next 30 days") query = query.Where(item => item.Date.Date <= today.AddDays(30));

            if (recordSearch) RecordSearch(keyword, category, dateFilter, eventType);
            return query.OrderBy(item => item.Date).ThenBy(item => item.Priority).ToList();
        }

        public void ToggleLike(int eventId)
        {
            MunicipalEvent item = GetById(eventId);
            if (item == null) return;
            if (likedEventIds.Add(eventId))
            {
                item.IsLiked = true;
                item.LikeCount++;
            }
            else
            {
                likedEventIds.Remove(eventId);
                item.IsLiked = false;
                item.LikeCount = Math.Max(0, item.LikeCount - 1);
            }
            OnDataChanged();
        }

        public void ToggleSaved(int eventId)
        {
            MunicipalEvent item = GetById(eventId);
            if (item == null) return;
            item.IsSaved = savedEventIds.Add(eventId);
            if (!item.IsSaved) savedEventIds.Remove(eventId);
            OnDataChanged();
        }

        public void ToggleInterested(int eventId)
        {
            MunicipalEvent item = GetById(eventId);
            if (item == null || item.Type != EventType.Event) return;
            item.IsInterested = interestedEventIds.Add(eventId);
            if (item.IsInterested) item.InterestedCount++;
            else { interestedEventIds.Remove(eventId); item.InterestedCount = Math.Max(0, item.InterestedCount - 1); }
            OnDataChanged();
        }

        public IList<Recommendation> GetRecommendations(int maximum)
        {
            var results = new List<Recommendation>();
            HashSet<string> likedCategories = new HashSet<string>(
                likedEventIds.Select(id => eventsById[id].Category), StringComparer.OrdinalIgnoreCase);
            HashSet<string> savedCategories = new HashSet<string>(
                savedEventIds.Select(id => eventsById[id].Category), StringComparer.OrdinalIgnoreCase);
            HashSet<string> interestedCategories = new HashSet<string>(
                interestedEventIds.Select(id => eventsById[id].Category), StringComparer.OrdinalIgnoreCase);

            foreach (MunicipalEvent item in allEvents.Where(value => value.Date >= DateTime.Today))
            {
                int score = 0;
                var reasons = new List<string>();
                int categoryFrequency;
                if (categorySearchFrequency.TryGetValue(item.Category, out categoryFrequency))
                {
                    score += 5 * categoryFrequency;
                    reasons.Add("you frequently search for " + item.Category + " updates");
                }
                foreach (KeyValuePair<string, int> keyword in keywordSearchFrequency)
                {
                    if (Contains(item.Title, keyword.Key) || Contains(item.Description, keyword.Key) || Contains(item.Location, keyword.Key))
                    {
                        score += 3 * keyword.Value;
                        reasons.Add("it matches your search for “" + keyword.Key + "”");
                    }
                }
                if (likedCategories.Contains(item.Category)) { score += 4; reasons.Add("you liked similar items"); }
                if (savedCategories.Contains(item.Category)) { score += 4; reasons.Add("you saved similar items"); }
                if (interestedCategories.Contains(item.Category)) { score += 4; reasons.Add("you showed interest in similar events"); }
                if (!string.IsNullOrWhiteSpace(MyArea) && Contains(item.Location, MyArea)) { score += 3; reasons.Add("it is in " + MyArea + ", your selected area"); }
                // Stack enumeration starts at the newest entry, so this explicitly gives
                // the five most recent searches a small recency influence.
                bool recentMatch = recentSearches.Take(5).Any(search =>
                    (!string.IsNullOrWhiteSpace(search.Category) && search.Category != "All categories" &&
                        string.Equals(search.Category, item.Category, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(search.SearchText) &&
                        (Contains(item.Title, search.SearchText) || Contains(item.Description, search.SearchText) || Contains(item.Location, search.SearchText))));
                if (recentMatch) { score += 2; reasons.Add("it matches one of your recent searches"); }
                if (item.Date <= DateTime.Today.AddDays(7)) { score += 2; reasons.Add("it is happening soon"); }

                if (score > 0 && !item.IsSaved)
                    results.Add(new Recommendation { Item = item, Score = score, Explanation = "Recommended because " + string.Join(" and ", reasons.Distinct().Take(2)) + "." });
            }

            return results.OrderByDescending(value => value.Score).ThenBy(value => value.Item.Date)
                .GroupBy(value => value.Item.Id).Select(group => group.First()).Take(maximum).ToList();
        }

        private void RecordSearch(string keyword, string category, string dateFilter, EventType? eventType)
        {
            bool hasCategory = !string.IsNullOrWhiteSpace(category) && category != "All categories";
            if (string.IsNullOrWhiteSpace(keyword) && !hasCategory && dateFilter == "All upcoming" && !eventType.HasValue) return;

            recentSearches.Push(new SearchRecord
            {
                SearchText = keyword,
                Category = category,
                DateFilter = dateFilter,
                EventType = eventType,
                SearchedAt = DateTime.Now
            });
            if (hasCategory) Increment(categorySearchFrequency, category);
            foreach (string word in keyword.Split(new[] { ' ', ',', '.', ';' }, StringSplitOptions.RemoveEmptyEntries))
                if (word.Length >= 3) Increment(keywordSearchFrequency, word.ToLowerInvariant());
            OnDataChanged();
        }

        private static void Increment(Dictionary<string, int> dictionary, string key)
        {
            int value;
            dictionary[key] = dictionary.TryGetValue(key, out value) ? value + 1 : 1;
        }

        private static bool Contains(string source, string value)
        {
            return !string.IsNullOrEmpty(source) && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void BuildIndexes()
        {
            foreach (MunicipalEvent item in allEvents.OrderBy(value => value.Date))
            {
                eventsById[item.Id] = item;
                uniqueCategories.Add(item.Category);
                List<MunicipalEvent> categoryItems;
                if (!eventsByCategory.TryGetValue(item.Category, out categoryItems))
                    eventsByCategory[item.Category] = categoryItems = new List<MunicipalEvent>();
                categoryItems.Add(item);
                List<MunicipalEvent> dateItems;
                DateTime key = item.Date.Date;
                if (!eventsByDate.TryGetValue(key, out dateItems))
                    eventsByDate[key] = dateItems = new List<MunicipalEvent>();
                dateItems.Add(item);
                if (item.IsAlert) alertQueue.Enqueue(item, item.Priority);
                else upcomingEvents.Enqueue(item);
            }
        }

        private void SeedSampleData()
        {
            DateTime today = DateTime.Today;
            Add(1, "Major Water Supply Interruption", "Emergency notice for a temporary supply interruption while municipal teams repair a main pipeline. Residents should store water responsibly.", today.AddDays(1).AddHours(8), "Water", "Bergville and surrounding wards", EventType.Alert, 1, true, true, 24);
            Add(2, "Planned Electricity Interruption", "Planned outage notice for essential network maintenance. Treat all electrical equipment as live.", today.AddDays(3).AddHours(9), "Electricity", "Winterton central", EventType.Alert, 1, true, false, 18);
            Add(3, "Road Maintenance Notice", "Notice for resurfacing and short stop-and-go controls. Please allow extra travel time.", today.AddDays(5).AddHours(7), "Roads", "R74 approach to Bergville", EventType.Alert, 2, true, false, 9);
            Add(4, "Waste Collection Schedule Change", "Public notice explaining a once-off collection day adjustment for selected communities.", today.AddDays(7).AddHours(6), "Waste", "Selected Okhahlamba wards", EventType.Announcement, 3, false, false, 6);
            Add(5, "Mayoral Imbizo", "Meet municipal leadership, raise service-delivery concerns and hear feedback on current programmes.", today.AddDays(10).AddHours(10), "Community", "Bergville Sports Complex", EventType.Event, 3, false, false, 31);
            Add(6, "Youth Development Programme", "Information and registration session covering skills development, entrepreneurship and available youth support.", today.AddDays(14).AddHours(9), "Youth", "Winterton Community Hall", EventType.Event, 3, false, false, 16);
            Add(7, "Okhahlamba Community Sports Day", "A family-friendly programme featuring football, netball and recreational activities.", today.AddDays(18).AddHours(8), "Sports", "Bergville Sports Complex", EventType.Event, 3, false, false, 42);
            Add(8, "Drakensberg Tourism Community Forum", "Local tourism operators and residents can discuss responsible tourism and community opportunities.", today.AddDays(22).AddHours(11), "Tourism", "Winterton Town Hall", EventType.Event, 3, false, false, 12);
            Add(9, "Council Public Participation Meeting", "A public participation session on municipal priorities and community needs.", today.AddDays(25).AddHours(17), "Council", "Okhahlamba Council Chamber, Bergville", EventType.Event, 3, false, false, 7);
            Add(10, "Community Safety Awareness Session", "Practical safety guidance and contact information from community safety partners.", today.AddDays(8).AddHours(14), "Public Safety", "Ward community hall, Bergville", EventType.Announcement, 2, false, false, 11);
            Add(11, "Water Conservation Reminder", "Please report leaks and use water sparingly during periods of high demand.", today.AddDays(2).AddHours(12), "Water", "All Okhahlamba communities", EventType.Announcement, 3, false, false, 15);
            Add(12, "Community Information Session", "Learn how to use municipal reporting channels and prepare clear service requests.", today.AddDays(6).AddHours(16), "Service Delivery", "Winterton Library Hall", EventType.Event, 3, false, false, 20);
        }

        private void Add(int id, string title, string description, DateTime date, string category, string location, EventType type, int priority, bool alert, bool emergency, int likes)
        {
            allEvents.Add(new MunicipalEvent { Id = id, Title = title, Description = description, Date = date, Category = category, Location = location, Type = type, Priority = priority, IsAlert = alert, IsEmergency = emergency, LikeCount = likes, InterestedCount = type == EventType.Event ? Math.Max(3, likes / 2) : 0 });
        }

        private void OnDataChanged()
        {
            EventHandler handler = DataChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
