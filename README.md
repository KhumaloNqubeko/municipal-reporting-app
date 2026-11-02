# Okhahlamba Municipal Services Application

## Overview

This C# Windows Forms application is a citizen-facing academic project for Okhahlamba Local Municipality, KwaZulu-Natal. It combines the Part 1 issue-reporting workflow with a Part 2 municipal dashboard, local events, announcements, priority alerts, search, favourites, likes and behaviour-based recommendations.


## Requirements

- Windows 10 or Windows 11
- Visual Studio 2022 or later with the **.NET desktop development** workload
- .NET Framework 4.8 Developer/Targeting Pack

## How to Compile

1. Open `MunicipalCitizenReporting.sln` in Visual Studio.
2. Select **Debug** and **Any CPU**.
3. Choose **Build > Build Solution**, or press `Ctrl+Shift+B`.
4. Confirm the Output window reports a successful build.

Command-line build (when MSBuild or a current .NET SDK is installed):

```powershell
dotnet msbuild MunicipalCitizenReporting.sln /t:Rebuild /p:Configuration=Debug
```

The Debug executable is written to `MunicipalCitizenReporting\bin\Debug\MunicipalCitizenReporting.exe`.

## How to Run

Press `F5` in Visual Studio to debug or `Ctrl+F5` to run without debugging. The application starts on the Okhahlamba Municipal Services dashboard.

## Part 1 — Report Issues

Residents can enter a location, choose a service category, describe an issue, optionally select an attachment and submit it. Each valid report receives a session reference such as `REQ-1001` and a `Submitted` status. **View Captured Issues** shows the reports recorded during the current application session.

## Part 2 — Local Events & Announcements

- A redesigned Okhahlamba municipal dashboard with Report an Issue, Local Events & Announcements, and citizen Quick Services.
- A dashboard summary of the highest-priority active municipal sample alerts.
- Chronological event, announcement and alert cards with full details.
- Keyword search across title, description, category and location.
- Category, date, type and Saved filters, plus clear no-result states.
- Session likes/unlikes and save/remove-from-saved actions.
- A dismissible critical-alert banner.
- Transparent, behaviour-based recommendations with an explanation on each recommended card.
- Realistic sample contexts such as Bergville, Winterton and nearby Okhahlamba wards.

## Data Structures Used

The structures are owned by `Data/EventManager.cs`, where each has a functional purpose visible in the application.

### Stack

`Stack<SearchRecord>` stores meaningful searches in last-in-first-out order. A record is added when the citizen presses Search with a keyword or active filter; ordinary keystrokes are not recorded. Recent behaviour contributes to recommendations.

### Queue

`Queue<MunicipalEvent>` receives normal upcoming events and announcements in chronological seed order. It represents the ordinary first-in-first-out municipal information stream, separate from urgent alerts.

### PriorityQueue

`PriorityQueue<MunicipalEvent, int>` is a .NET Framework 4.8-compatible binary min-heap. It places priority 1 alerts before priorities 2 and 3. The dashboard and events-page banner consume active alerts in priority order. This custom generic is used because .NET Framework 4.8 predates the BCL `PriorityQueue` type.

### Dictionary

- `Dictionary<int, MunicipalEvent>` provides constant-time lookup for details, likes and saves.
- `Dictionary<string, List<MunicipalEvent>>` groups events by category for direct category filtering.
- Frequency dictionaries count category and keyword searches for recommendation scoring.

### SortedDictionary

`SortedDictionary<DateTime, List<MunicipalEvent>>` indexes items by date. The default upcoming view can traverse records chronologically and date filtering is straightforward.

### HashSet

- `HashSet<string>` provides a duplicate-free, sorted list for the Category filter.
- `HashSet<int>` collections prevent duplicate session likes and saved items, while also making toggles and recommendation preference checks efficient.

## Recommendation Algorithm

Recommendations are deterministic rather than random. Expired events are excluded and event IDs are grouped to prevent duplicates. Each candidate receives:

- `+5 × frequency` when its category matches a searched category.
- `+3 × frequency` when its title, description or location matches a searched keyword.
- `+4` when its category matches a liked item.
- `+4` when its category matches a saved item.
- `+2` when it matches one of the five newest records in the search-history stack.
- `+2` when it occurs within seven days.

Candidates are sorted by descending score and then by date. The top three appear in **Recommended For You**, with a plain-language reason such as “Recommended because it matches your search for ‘water’.” Saved candidates are omitted so the panel remains useful for discovery.

## Project Structure

- `Models` — reported issues, municipal events, enums and search records.
- `Data` — Part 1 issue repository, Part 2 event manager and priority queue.
- `Forms` — dashboard, report workflow, captured reports, local events and event details.

## Citizen Features

All preferences and interactions below are session-based. Event, interruption and contact records are explicitly sample/demo content and are not official live municipal information.

### Municipal Alerts

Priority 1 records are critical/high-priority items, priority 2 records are important warnings, and priority 3 records are general notices. The dashboard reads alerts through the existing heap-backed priority queue. Where an alert is linked to a service interruption, the dashboard also displays the canonical interruption status, affected area and expected-restoration time.

### Outage Centre

**Outages & Service Interruptions** displays sample electricity, water, roads, waste and other disruptions. Citizens can filter by service type, search an affected area, limit the list to My Area, and open detailed status/restoration information. `ServiceInterruption.RelatedEventId` links an important interruption to the existing municipal alert record instead of creating a second independent alert store.

### My Area

The dashboard’s **My Area** control derives its choices from event and interruption locations. The selected community is held for the application session. It can limit event/outage results and adds `+3` to recommendations whose location matches the selected area, with a plain-language explanation.

### Notification Centre

The dashboard notification button shows the live unread count. Notifications cover high-priority announcements, emergency alerts, service interruptions and approaching saved events. Citizens can open the related item, mark all notifications read, or clear individual notifications. Reminder keys prevent duplicate reminders during the session.

### Local Events

The existing search, category/date/type filters, priority banner, details view and recommendations remain available. Cards continue to distinguish actual events, announcements and alerts.

### Likes and Saved Items

Like/Unlike and Save/Remove Saved remain session-based and use duplicate-safe ID sets. The new **Saved Items** centre brings all saved events and announcements together and supports details, removal, and event interest.

### I'm Interested

Actual community events show an independent **I'm Interested** toggle and count. Announcements and alerts do not show this action. A `HashSet<int>` prevents duplicate interest, and categories previously marked interesting add `+4` to relevant recommendations.

### Service Calendar

The municipal calendar combines existing event/announcement data with planned interruptions without maintaining another content dataset. A `SortedDictionary<DateTime, List<CalendarActivity>>` provides chronological date lookup. The standard month selector supplies previous/next navigation and a separate **Today** action returns to the current date.

### Municipal Contacts

Professional contact cards cover emergency, general, electricity, water, roads, waste, billing and community categories. Every seeded number/address is visibly labelled DEMO/SAMPLE and uses the reserved `.invalid` domain. Copy Number and Copy Email provide lightweight clipboard actions, and the model is ready for verified details to replace the seeds.

### Smart Recommendations

The existing deterministic algorithm is extended rather than replaced. It now scores searched categories (`+5 × frequency`), keywords (`+3 × frequency`), liked categories (`+4`), saved categories (`+4`), interested-event categories (`+4`), My Area matches (`+3`), recent searches (`+2`) and items happening within seven days (`+2`). Results remain sorted by score/date and explain up to two contributing reasons.

### Copy / Share Details

Events and announcements can copy clean formatted text containing the municipality, title, date, time, location and description. A small status message confirms the clipboard action so no disruptive message box is required.

### Data Structures

- `Stack<SearchRecord>` stores newest-first meaningful search history used by recommendations.
- `Queue<MunicipalEvent>` holds the ordinary chronological information stream; a reminder-processing queue handles approaching saved events.
- `PriorityQueue<MunicipalEvent, int>` orders dashboard alerts, including the event records linked to important outages.
- `Dictionary<int, MunicipalEvent>` provides ID lookup; category and frequency dictionaries support filtering and scoring.
- `SortedDictionary<DateTime, List<MunicipalEvent>>` indexes existing events, while the service-calendar projection uses a date-sorted activity dictionary.
- `HashSet<string>` maintains unique categories and notification deduplication keys. `HashSet<int>` collections prevent duplicate likes, saves and interests.

### Additional Navigation

The expanded Quick Services dashboard links Report Issue, Events & News, Outages, Service Calendar, Municipal Contacts, Saved Items, Notifications, My Municipality and My Area. It also shows important alerts, upcoming events and personalised recommendations.
