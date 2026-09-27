using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Forms
{
    internal static class CitizenUi
    {
        public static Button Button(string text, EventHandler click, Color? colour = null)
        {
            var button = new Button { Text = text, Height = 36, AutoSize = true, Padding = new Padding(12, 0, 12, 0), Margin = new Padding(5), FlatStyle = FlatStyle.Flat, BackColor = colour ?? Color.FromArgb(31, 105, 138), ForeColor = Color.White, Cursor = Cursors.Hand, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold) };
            button.FlatAppearance.BorderSize = 0; button.Click += click; return button;
        }

        public static Panel Header(Form form, string title, string subtitle)
        {
            var header = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(24, 68, 88) };
            header.Controls.Add(new Label { Text = title, AutoSize = true, Location = new Point(28, 14), Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold), ForeColor = Color.White });
            header.Controls.Add(new Label { Text = subtitle, AutoSize = true, Location = new Point(32, 61), Font = new Font("Segoe UI", 9.5F), ForeColor = Color.WhiteSmoke });
            Button back = Button("← Main Menu", delegate { form.Close(); }, Color.White); back.ForeColor = Color.FromArgb(24, 68, 88); back.Anchor = AnchorStyles.Top | AnchorStyles.Right; back.Location = new Point(form.ClientSize.Width - 155, 31); header.Controls.Add(back);
            return header;
        }

        public static Label Empty(string text)
        {
            return new Label { Text = text, AutoSize = false, Size = new Size(700, 75), Padding = new Padding(18), Margin = new Padding(5, 10, 5, 5), BackColor = Color.White, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 10F) };
        }

        public static void Base(Form form, string title, Size size)
        {
            form.Text = title; form.StartPosition = FormStartPosition.CenterParent; form.ClientSize = size; form.MinimumSize = new Size(800, 600); form.BackColor = Color.FromArgb(242, 246, 247); form.Font = new Font("Segoe UI", 9F);
        }
    }

    public sealed class AreaPreferencesForm : Form
    {
        private readonly ComboBox areaBox = new ComboBox();
        private readonly CheckBox onlyArea = new CheckBox();
        public AreaPreferencesForm()
        {
            CitizenUi.Base(this, "My Area", new Size(520, 300)); MinimumSize = Size; MaximumSize = Size;
            Panel header = CitizenUi.Header(this, "My Area", "Prioritise municipal information relevant to your community");
            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(35, 25, 35, 15) };
            body.Controls.Add(new Label { Text = "SELECT YOUR AREA", AutoSize = true, Location = new Point(10, 15), ForeColor = Color.FromArgb(24, 68, 88), Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold) });
            areaBox.DropDownStyle = ComboBoxStyle.DropDownList; areaBox.Location = new Point(10, 45); areaBox.Width = 390; areaBox.Items.Add("No area selected");
            foreach (string area in CitizenService.Instance.GetAreas()) areaBox.Items.Add(area);
            string current = EventManager.Instance.MyArea; int index = string.IsNullOrWhiteSpace(current) ? 0 : areaBox.FindStringExact(current); areaBox.SelectedIndex = index < 0 ? 0 : index;
            onlyArea.Text = "Only show updates for My Area"; onlyArea.AutoSize = true; onlyArea.Location = new Point(10, 88); onlyArea.Checked = EventManager.Instance.OnlyMyArea;
            Button save = CitizenUi.Button("Save Preference", delegate { EventManager.Instance.SetMyArea(areaBox.SelectedIndex <= 0 ? null : areaBox.SelectedItem.ToString(), onlyArea.Checked); DialogResult = DialogResult.OK; Close(); }, Color.FromArgb(0, 121, 107)); save.Location = new Point(10, 125);
            body.Controls.Add(areaBox); body.Controls.Add(onlyArea); body.Controls.Add(save); Controls.Add(body); Controls.Add(header);
        }
    }

    public sealed class OutagesForm : Form
    {
        private readonly FlowLayoutPanel list = new FlowLayoutPanel();
        private readonly ComboBox typeBox = new ComboBox();
        private readonly TextBox areaBox = new TextBox();
        private readonly CheckBox onlyArea = new CheckBox();
        public OutagesForm()
        {
            CitizenUi.Base(this, "Outages & Service Interruptions", new Size(980, 700));
            var filters = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.White, Padding = new Padding(25, 18, 20, 10) };
            typeBox.DropDownStyle = ComboBoxStyle.DropDownList; typeBox.Items.Add("All services"); foreach (string value in Enum.GetNames(typeof(ServiceType))) typeBox.Items.Add(value); typeBox.SelectedIndex = 0; typeBox.Location = new Point(25, 20); typeBox.Width = 175;
            areaBox.Location = new Point(215, 20); areaBox.Width = 220; areaBox.Text = string.Empty;
            onlyArea.Text = "Only My Area"; onlyArea.AutoSize = true; onlyArea.Location = new Point(455, 23); onlyArea.Checked = EventManager.Instance.OnlyMyArea;
            Button search = CitizenUi.Button("Apply Filters", delegate { RefreshList(); }, Color.FromArgb(0, 121, 107)); search.Location = new Point(585, 14); filters.Controls.AddRange(new Control[] { typeBox, areaBox, onlyArea, search });
            list.Dock = DockStyle.Fill; list.AutoScroll = true; list.FlowDirection = FlowDirection.TopDown; list.WrapContents = false; list.Padding = new Padding(25, 18, 15, 10);
            Controls.Add(list); Controls.Add(filters); Controls.Add(CitizenUi.Header(this, "Outages & Service Interruptions", "")); RefreshList();
        }
        private void RefreshList()
        {
            ServiceType? type = typeBox.SelectedIndex <= 0 ? (ServiceType?)null : (ServiceType)Enum.Parse(typeof(ServiceType), typeBox.SelectedItem.ToString());
            IList<ServiceInterruption> values = CitizenService.Instance.SearchInterruptions(type, areaBox.Text, onlyArea.Checked); list.Controls.Clear();
            if (values.Count == 0) { list.Controls.Add(CitizenUi.Empty("No active service interruptions in your selected area.")); return; }
            foreach (ServiceInterruption item in values)
            {
                var card = new Panel { Width = 880, Height = 175, Margin = new Padding(0, 0, 0, 12), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(20) };
                Color accent = item.Priority == 1 ? Color.FromArgb(180, 52, 45) : Color.FromArgb(230, 145, 30); card.Controls.Add(new Panel { Dock = DockStyle.Left, Width = 6, BackColor = accent });
                card.Controls.Add(new Label { Text = item.ServiceType.ToString().ToUpperInvariant() + (item.IsPlanned ? " • PLANNED" : " • UNPLANNED"), Location = new Point(25, 14), AutoSize = true, ForeColor = accent, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold) });
                card.Controls.Add(new Label { Text = item.Title, Location = new Point(25, 39), Size = new Size(600, 28), Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold) });
                card.Controls.Add(new Label { Text = item.Area + "\r\n" + item.StartDateTime.ToString("ddd, dd MMM • HH:mm") + (item.ExpectedRestorationTime.HasValue ? "  —  expected " + item.ExpectedRestorationTime.Value.ToString("HH:mm") : string.Empty), Location = new Point(25, 71), Size = new Size(610, 48), ForeColor = Color.DimGray });
                card.Controls.Add(new Label { Text = CitizenService.FriendlyStatus(item.Status).ToUpperInvariant() + "\r\nUpdated " + item.LastUpdated.ToString("HH:mm"), Location = new Point(660, 25), Size = new Size(180, 45), ForeColor = accent, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), TextAlign = ContentAlignment.TopRight });
                Button details = CitizenUi.Button("View Details", delegate { ShowDetails(item); }); details.Location = new Point(700, 104); card.Controls.Add(details); list.Controls.Add(card);
            }
        }
        private void ShowDetails(ServiceInterruption item)
        {
            var detail = new Form(); CitizenUi.Base(detail, item.Title, new Size(650, 480));
            Panel header = CitizenUi.Header(detail, item.Title, item.ServiceType + " • " + item.Area);
            var text = new Label { Dock = DockStyle.Fill, Padding = new Padding(35, 30, 35, 20), Font = new Font("Segoe UI", 10F), Text = "STATUS\r\n" + CitizenService.FriendlyStatus(item.Status) + "\r\n\r\nSTART\r\n" + item.StartDateTime.ToString("dddd, dd MMMM yyyy 'at' HH:mm") + "\r\n\r\nEXPECTED RESTORATION\r\n" + (item.ExpectedRestorationTime.HasValue ? item.ExpectedRestorationTime.Value.ToString("dddd, dd MMMM yyyy 'at' HH:mm") : "To be confirmed") };
            detail.Controls.Add(text); detail.Controls.Add(header); detail.ShowDialog(this);
        }
    }

    public sealed class NotificationsForm : Form
    {
        private readonly FlowLayoutPanel list = new FlowLayoutPanel();
        public NotificationsForm()
        {
            CitizenUi.Base(this, "Notifications", new Size(850, 650));
            var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(25, 7, 10, 5), BackColor = Color.White };
            toolbar.Controls.Add(CitizenUi.Button("Mark all read", delegate { CitizenService.Instance.MarkAllRead(); RefreshList(); }, Color.FromArgb(0, 121, 107)));
            list.Dock = DockStyle.Fill; list.AutoScroll = true; list.FlowDirection = FlowDirection.TopDown; list.WrapContents = false; list.Padding = new Padding(25, 18, 15, 10);
            Controls.Add(list); Controls.Add(toolbar); Controls.Add(CitizenUi.Header(this, "Notifications", "Important updates and reminders in one place")); CitizenService.Instance.DataChanged += Changed; FormClosed += delegate { CitizenService.Instance.DataChanged -= Changed; }; RefreshList();
        }
        private void Changed(object sender, EventArgs e) { RefreshList(); }
        private void RefreshList()
        {
            list.Controls.Clear(); IList<CitizenNotification> items = CitizenService.Instance.Notifications.OrderByDescending(value => value.CreatedAt).ToList();
            if (items.Count == 0) { list.Controls.Add(CitizenUi.Empty("You're all caught up.")); return; }
            foreach (CitizenNotification item in items)
            {
                var card = new Panel { Width = 750, Height = 112, Margin = new Padding(0, 0, 0, 10), Padding = new Padding(15), BackColor = item.IsRead ? Color.White : Color.FromArgb(234, 247, 244), BorderStyle = BorderStyle.FixedSingle };
                card.Controls.Add(new Label { Text = (item.IsRead ? string.Empty : "NEW  •  ") + item.Title, Location = new Point(16, 12), Size = new Size(500, 25), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88) });
                card.Controls.Add(new Label { Text = item.Message + "\r\n" + Relative(item.CreatedAt), Location = new Point(16, 40), Size = new Size(510, 52), ForeColor = Color.DimGray });
                Button view = CitizenUi.Button("View", delegate { ViewRelated(item); }); view.Location = new Point(540, 14); card.Controls.Add(view);
                Button clear = CitizenUi.Button("Clear", delegate { CitizenService.Instance.RemoveNotification(item.Id); }, Color.FromArgb(104, 115, 124)); clear.Location = new Point(635, 14); card.Controls.Add(clear); list.Controls.Add(card);
            }
        }
        private void ViewRelated(CitizenNotification notice)
        {
            CitizenService.Instance.MarkRead(notice.Id);
            if (notice.RelatedToInterruption) { using (var form = new OutagesForm()) form.ShowDialog(this); }
            else if (notice.RelatedItemId.HasValue) { MunicipalEvent item = EventManager.Instance.GetById(notice.RelatedItemId.Value); if (item != null) using (var form = new EventDetailsForm(EventManager.Instance, item)) form.ShowDialog(this); }
        }
        private static string Relative(DateTime value) { TimeSpan age = DateTime.Now - value; return age.TotalMinutes < 60 ? Math.Max(1, (int)age.TotalMinutes) + " minutes ago" : age.TotalHours < 24 ? (int)age.TotalHours + " hours ago" : "Yesterday"; }
    }

    public sealed class ContactsForm : Form
    {
        public ContactsForm()
        {
            CitizenUi.Base(this, "Municipal Contacts", new Size(900, 680));
            var list = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(25, 18, 15, 10) };
            foreach (MunicipalContact item in CitizenService.Instance.Contacts.OrderByDescending(value => value.IsEmergency).ThenBy(value => value.Department))
            {
                var card = new Panel { Width = 800, Height = 150, Margin = new Padding(0, 0, 0, 12), BackColor = item.IsEmergency ? Color.FromArgb(255, 238, 234) : Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(18) };
                card.Controls.Add(new Label { Text = item.Department.ToUpperInvariant() + (item.IsEmergency ? "  •  EMERGENCY" : string.Empty), Location = new Point(18, 14), AutoSize = true, Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = item.IsEmergency ? Color.FromArgb(155, 45, 38) : Color.FromArgb(24, 68, 88) });
                card.Controls.Add(new Label { Text = item.Description + "\r\n☎ " + item.Phone + "    ✉ " + item.Email + "\r\nHours: " + item.OperatingHours, Location = new Point(18, 43), Size = new Size(740, 58), ForeColor = Color.FromArgb(55, 62, 65) });
                Button number = CitizenUi.Button("Copy Number", delegate { Clipboard.SetText(item.Phone); }, Color.FromArgb(0, 121, 107)); number.Location = new Point(18, 105); card.Controls.Add(number);
                Button email = CitizenUi.Button("Copy Email", delegate { Clipboard.SetText(item.Email); }, Color.FromArgb(31, 105, 138)); email.Location = new Point(150, 105); card.Controls.Add(email); list.Controls.Add(card);
            }
            Controls.Add(list); Controls.Add(CitizenUi.Header(this, "Municipal Contacts", ""));
        }
    }

    public sealed class ServiceCalendarForm : Form
    {
        private readonly MonthCalendar calendar = new MonthCalendar();
        private readonly FlowLayoutPanel activities = new FlowLayoutPanel();
        private SortedDictionary<DateTime, List<CalendarActivity>> index;
        public ServiceCalendarForm()
        {
            CitizenUi.Base(this, "Municipal Service Calendar", new Size(900, 650)); index = CitizenService.Instance.BuildCalendar();
            var left = new Panel { Dock = DockStyle.Left, Width = 290, Padding = new Padding(25) }; calendar.MaxSelectionCount = 1; calendar.DateSelected += delegate { RefreshDate(); }; left.Controls.Add(calendar);
            Button today = CitizenUi.Button("Today", delegate { calendar.SetDate(DateTime.Today); RefreshDate(); }, Color.FromArgb(0, 121, 107)); today.Location = new Point(25, 220); left.Controls.Add(today);
            activities.Dock = DockStyle.Fill; activities.AutoScroll = true; activities.FlowDirection = FlowDirection.TopDown; activities.WrapContents = false; activities.Padding = new Padding(20);
            Controls.Add(activities); Controls.Add(left); Controls.Add(CitizenUi.Header(this, "Municipal Service Calendar", "Events, meetings, announcements and planned maintenance by date")); RefreshDate();
        }
        private void RefreshDate()
        {
            activities.Controls.Clear(); DateTime date = calendar.SelectionStart.Date;
            activities.Controls.Add(new Label { Text = date.ToString("dddd, dd MMMM yyyy"), AutoSize = false, Size = new Size(510, 45), Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88) });
            List<CalendarActivity> values;
            if (!index.TryGetValue(date, out values) || values.Count == 0) { activities.Controls.Add(CitizenUi.Empty("No municipal activities scheduled for this date.")); return; }
            foreach (CalendarActivity item in values.OrderBy(value => value.Date)) activities.Controls.Add(new Label { Text = "•  " + item.Title + "\r\n   " + item.Date.ToString("HH:mm") + " • " + item.Category + " • " + item.Area, Size = new Size(510, 70), Margin = new Padding(0, 0, 0, 8), Padding = new Padding(12), BackColor = Color.White, ForeColor = Color.FromArgb(45, 52, 55) });
        }
    }

    public sealed class SavedItemsForm : Form
    {
        private readonly FlowLayoutPanel list = new FlowLayoutPanel();
        public SavedItemsForm()
        {
            CitizenUi.Base(this, "Saved Items", new Size(900, 680)); list.Dock = DockStyle.Fill; list.AutoScroll = true; list.FlowDirection = FlowDirection.TopDown; list.WrapContents = false; list.Padding = new Padding(25, 18, 15, 10);
            Controls.Add(list); Controls.Add(CitizenUi.Header(this, "Saved Items", "Your saved events and announcements")); EventManager.Instance.DataChanged += Changed; FormClosed += delegate { EventManager.Instance.DataChanged -= Changed; }; RefreshList();
        }
        private void Changed(object sender, EventArgs e) { RefreshList(); }
        private void RefreshList()
        {
            list.Controls.Clear(); IList<MunicipalEvent> values = EventManager.Instance.AllEvents.Where(value => value.IsSaved).OrderBy(value => value.Date).ToList();
            if (values.Count == 0) { list.Controls.Add(CitizenUi.Empty("You haven't saved any events or announcements yet.")); return; }
            foreach (MunicipalEvent item in values)
            {
                var card = new Panel { Width = 800, Height = 125, Margin = new Padding(0, 0, 0, 10), Padding = new Padding(16), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
                card.Controls.Add(new Label { Text = item.Type.ToString().ToUpperInvariant() + " • " + item.Category, Location = new Point(16, 12), AutoSize = true, ForeColor = Color.FromArgb(0, 121, 107), Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold) });
                card.Controls.Add(new Label { Text = item.Title + "\r\n" + item.Date.ToString("ddd, dd MMM • HH:mm") + " • " + item.Location, Location = new Point(16, 38), Size = new Size(490, 52), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold) });
                Button view = CitizenUi.Button("View Details", delegate { using (var form = new EventDetailsForm(EventManager.Instance, item)) form.ShowDialog(this); }); view.Location = new Point(520, 18); card.Controls.Add(view);
                Button remove = CitizenUi.Button("Remove Saved", delegate { EventManager.Instance.ToggleSaved(item.Id); }, Color.FromArgb(104, 115, 124)); remove.Location = new Point(650, 18); card.Controls.Add(remove);
                if (item.Type == EventType.Event) { Button interest = CitizenUi.Button(item.IsInterested ? "✓ Interested (" + item.InterestedCount + ")" : "I'm Interested (" + item.InterestedCount + ")", delegate { EventManager.Instance.ToggleInterested(item.Id); }, Color.FromArgb(0, 121, 107)); interest.Location = new Point(520, 67); card.Controls.Add(interest); }
                list.Controls.Add(card);
            }
        }
    }

    public sealed class MunicipalInfoForm : Form
    {
        public MunicipalInfoForm()
        {
            CitizenUi.Base(this, "My Municipality", new Size(820, 620)); Panel header = CitizenUi.Header(this, "My Municipality", "");
            var text = new Label { Dock = DockStyle.Fill, Padding = new Padding(45, 35, 45, 25), Font = new Font("Segoe UI", 10.5F), ForeColor = Color.FromArgb(45, 52, 55), Text = "ABOUT OKHAHLAMBA\r\n\r\nWe are a municipality that has developed a culture which embodies a commitment to the implementation of a clean administration driven by good governance, and this has proven to be a successful formula in achieving and maintaining favourable audit outcomes from the Auditor General of South Africa.\r\n\r\nSERVICES\r\n\r\n• Electricity\r\n• Water & Sanitation\r\n• Roads and Stormwater\r\n• Waste Management\r\n• Community Services" };
            Controls.Add(text); Controls.Add(header);
        }
    }
}
