using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Forms
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            municipalLogoPictureBox.Image = Branding.LoadMunicipalLogo();
            InitialiseCitizenDashboard();
            EventManager.Instance.DataChanged += DashboardDataChanged;
            CitizenService.Instance.DataChanged += DashboardDataChanged;
            FormClosed += delegate { EventManager.Instance.DataChanged -= DashboardDataChanged; CitizenService.Instance.DataChanged -= DashboardDataChanged; };
            RefreshAlerts();
        }

        private Button notificationButton;
        private Button areaButton;
        private FlowLayoutPanel upcomingPanel;
        private FlowLayoutPanel recommendationsPanel;

        private void InitialiseCitizenDashboard()
        {
            statusButton.Visible = false;
            servicesLayoutPanel.RowCount = 3; servicesLayoutPanel.Height = 245;
            servicesLayoutPanel.RowStyles.Clear(); for (int i = 0; i < 3; i++) servicesLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
            AddServiceButton("OUTAGES\r\nService interruptions", Color.FromArgb(165, 73, 55), delegate { OpenDialog(new OutagesForm()); }, 0, 1);
            AddServiceButton("SERVICE CALENDAR\r\nActivities by date", Color.FromArgb(92, 108, 59), delegate { OpenDialog(new ServiceCalendarForm()); }, 1, 1);
            AddServiceButton("MUNICIPAL CONTACTS\r\nDepartments and help", Color.FromArgb(126, 82, 150), delegate { OpenDialog(new ContactsForm()); }, 2, 1);
            AddServiceButton("SAVED ITEMS\r\nYour favourites", Color.FromArgb(31, 105, 138), delegate { OpenDialog(new SavedItemsForm()); }, 0, 2);
            AddServiceButton("NOTIFICATIONS\r\nUpdates and reminders", Color.FromArgb(72, 96, 105), delegate { OpenDialog(new NotificationsForm()); }, 1, 2);
            AddServiceButton("MY AREA\r\nLocal preferences", Color.FromArgb(0, 121, 107), delegate { OpenDialog(new AreaPreferencesForm()); }, 2, 2);
            AddServiceButton("MY MUNICIPALITY\r\nServices and information", Color.FromArgb(72, 96, 105), delegate { OpenDialog(new MunicipalInfoForm()); }, 2, 0);

            notificationButton = new Button { FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 121, 107), ForeColor = Color.White, Location = new Point(585, 102), Size = new Size(120, 36), Cursor = Cursors.Hand, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold) }; notificationButton.FlatAppearance.BorderSize = 0; notificationButton.Click += delegate { OpenDialog(new NotificationsForm()); }; headerPanel.Controls.Add(notificationButton); notificationButton.BringToFront();
            areaButton = new Button { FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(24, 68, 88), Location = new Point(410, 102), Size = new Size(165, 36), Cursor = Cursors.Hand, Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold) }; areaButton.FlatAppearance.BorderSize = 0; areaButton.Click += delegate { OpenDialog(new AreaPreferencesForm()); }; headerPanel.Controls.Add(areaButton); areaButton.BringToFront();

            foreach (Control control in Controls) if (control is Label && control.Text == "Important Alerts") control.Location = new Point(35, 435);
            alertsFlowPanel.Location = new Point(39, 475); alertsFlowPanel.Size = new Size(923, 135);
            upcomingPanel = CreateSummaryPanel("UPCOMING", new Point(39, 630)); recommendationsPanel = CreateSummaryPanel("RECOMMENDED FOR YOU", new Point(505, 630));
            Controls.Add(upcomingPanel); Controls.Add(recommendationsPanel); upcomingPanel.BringToFront(); recommendationsPanel.BringToFront();
            RefreshCitizenDashboard();
        }

        private void AddServiceButton(string text, Color colour, EventHandler click, int column, int row)
        {
            var button = new Button { Dock = DockStyle.Fill }; StyleServiceButton(button, colour, text); button.Click += click; servicesLayoutPanel.Controls.Add(button, column, row);
        }

        private FlowLayoutPanel CreateSummaryPanel(string title, Point location)
        {
            var panel = new FlowLayoutPanel { Location = location, Size = new Size(457, 135), AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(10), BackColor = Color.White };
            panel.Controls.Add(new Label { Text = title, AutoSize = false, Size = new Size(410, 26), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88) }); return panel;
        }

        private void RefreshCitizenDashboard()
        {
            if (notificationButton == null) return;
            notificationButton.Text = "Notifications  " + CitizenService.Instance.UnreadCount;
            areaButton.Text = "My Area: " + (EventManager.Instance.MyArea ?? "Not selected");
            FillSummary(upcomingPanel, EventManager.Instance.AllEvents.Where(value => value.Date >= DateTime.Now && value.Type == EventType.Event).OrderBy(value => value.Date).Take(2).Select(value => value.Title + "  •  " + FriendlyDay(value.Date)));
            IList<Recommendation> recommendations = EventManager.Instance.GetRecommendations(2); FillSummary(recommendationsPanel, recommendations.Count == 0 ? new[] { "Interact with events to receive personalised recommendations." } : recommendations.Select(value => value.Item.Title + "\r\n" + value.Explanation));
        }

        private static string FriendlyDay(DateTime date) { return date.Date == DateTime.Today.AddDays(1) ? "Tomorrow" : date.ToString("ddd, dd MMM"); }
        private static void FillSummary(FlowLayoutPanel panel, IEnumerable<string> values)
        {
            if (panel == null) return; while (panel.Controls.Count > 1) panel.Controls.RemoveAt(1);
            foreach (string value in values) panel.Controls.Add(new Label { Text = value, AutoSize = false, Size = new Size(410, 42), ForeColor = Color.FromArgb(55, 62, 65), Padding = new Padding(5, 2, 5, 2) });
        }

        private void OpenDialog(Form form) { using (form) { Hide(); form.ShowDialog(this); Show(); Activate(); } }
        private void DashboardDataChanged(object sender, EventArgs e) { RefreshAlerts(); RefreshCitizenDashboard(); }

        private void reportIssuesButton_Click(object sender, EventArgs e)
        {
            using (var reportIssueForm = new ReportIssueForm())
            {
                Hide();
                reportIssueForm.ShowDialog(this);
                Show();
                Activate();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RefreshAlerts();
            RefreshCitizenDashboard();
        }

        private void eventsButton_Click(object sender, EventArgs e)
        {
            using (var form = new LocalEventsForm(EventManager.Instance))
            {
                Hide();
                form.ShowDialog(this);
                Show();
                Activate();
            }
        }

        private void statusButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Service Request Status is reserved for Part 3. Reports captured in this session remain available from Report an Issue.", "Coming in Part 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshAlerts()
        {
            alertsFlowPanel.Controls.Clear();
            foreach (MunicipalEvent alert in EventManager.Instance.GetPriorityAlerts(3))
            {
                ServiceInterruption interruption = CitizenService.Instance.Interruptions.FirstOrDefault(value => value.RelatedEventId == alert.Id);
                string timing = interruption == null
                    ? alert.Date.ToString("ddd, dd MMM • HH:mm") + "  |  " + alert.Location
                    : CitizenService.FriendlyStatus(interruption.Status) + "  |  " + interruption.Area +
                        (interruption.ExpectedRestorationTime.HasValue ? "  |  Expected " + interruption.ExpectedRestorationTime.Value.ToString("HH:mm") : string.Empty);
                var label = new Label
                {
                    Width = Math.Max(500, alertsFlowPanel.ClientSize.Width - 28), Height = 55,
                    Margin = new Padding(4, 4, 4, 5), Padding = new Padding(12, 7, 8, 5),
                    BackColor = alert.Priority == 1 ? Color.FromArgb(255, 235, 230) : Color.FromArgb(255, 247, 225),
                    ForeColor = Color.FromArgb(92, 53, 32), Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Text = (alert.IsEmergency ? "CRITICAL" : "NOTICE") + "  •  " + alert.Title + "\r\n" + timing,
                    Tag = alert.Id
                };
                label.Click += alertLabel_Click;
                alertsFlowPanel.Controls.Add(label);
            }
        }

        private void alertLabel_Click(object sender, EventArgs e)
        {
            var label = sender as Label;
            if (label == null) return;
            MunicipalEvent item = EventManager.Instance.GetById((int)label.Tag);
            if (item != null)
                using (var details = new EventDetailsForm(EventManager.Instance, item)) details.ShowDialog(this);
        }
    }
}
