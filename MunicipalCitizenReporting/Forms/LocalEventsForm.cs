using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Forms
{
    public sealed class LocalEventsForm : Form
    {
        private readonly EventManager manager;
        private readonly TextBox searchBox = new TextBox();
        private readonly ComboBox categoryBox = new ComboBox();
        private readonly ComboBox dateBox = new ComboBox();
        private readonly Label alertText = new Label();
        private readonly Panel alertPanel = new Panel();
        private readonly FlowLayoutPanel resultsPanel = new FlowLayoutPanel();
        private readonly FlowLayoutPanel recommendationPanel = new FlowLayoutPanel();
        private readonly Label resultsLabel = new Label();
        private readonly Label recommendationSubtitle = new Label();
        private readonly SplitContainer contentSplit = new SplitContainer();
        private readonly PictureBox municipalLogo = new PictureBox();
        private EventType? selectedType;
        private bool savedOnly;

        public LocalEventsForm() : this(EventManager.Instance) { }

        internal LocalEventsForm(EventManager manager)
        {
            this.manager = manager ?? throw new ArgumentNullException("manager");
            BuildInterface();
            PopulateCategories();
            manager.DataChanged += Manager_DataChanged;
            FormClosed += delegate
            {
                manager.DataChanged -= Manager_DataChanged;
                if (municipalLogo.Image != null) municipalLogo.Image.Dispose();
            };
            RefreshAlert();
            PerformSearch(false);
        }

        private void BuildInterface()
        {
            Text = "Local Events & Announcements - Okhahlamba"; StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1120, 720); MinimumSize = new Size(900, 650); BackColor = Color.FromArgb(242, 246, 247); Font = new Font("Segoe UI", 9F);
            var header = new Panel { Dock = DockStyle.Top, Height = 105, BackColor = Color.FromArgb(24, 68, 88) };
            header.Controls.Add(new Label { Text = "Local Events & Announcements", AutoSize = true, Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(28, 14) });
            header.Controls.Add(new Label { Text = "Stay informed about what’s happening in Okhahlamba", AutoSize = true, Font = new Font("Segoe UI", 10F), ForeColor = Color.WhiteSmoke, Location = new Point(32, 65) });
            var back = new Button { Text = "←  Back to Main Menu", Dock = DockStyle.Fill, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(24, 68, 88), Cursor = Cursors.Hand, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold) };
            back.Click += delegate { Close(); };
            var backHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 31, 25, 31), BackColor = Color.FromArgb(24, 68, 88) };
            backHost.Controls.Add(back);
            var logoHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 21, 10, 21), BackColor = Color.FromArgb(24, 68, 88) };
            municipalLogo.Dock = DockStyle.Fill; municipalLogo.BackColor = Color.White; municipalLogo.SizeMode = PictureBoxSizeMode.Zoom;
            municipalLogo.AccessibleName = "Okhahlamba Local Municipality logo"; municipalLogo.Image = Branding.LoadMunicipalLogo();
            logoHost.Controls.Add(municipalLogo);
            var headerActions = new TableLayoutPanel { Dock = DockStyle.Right, Width = 430, ColumnCount = 2, RowCount = 1, BackColor = Color.FromArgb(24, 68, 88), Margin = Padding.Empty, Padding = Padding.Empty };
            headerActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            headerActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            headerActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            headerActions.Controls.Add(logoHost, 0, 0); headerActions.Controls.Add(backHost, 1, 0);
            header.Controls.Add(headerActions); headerActions.BringToFront();

            alertPanel.Dock = DockStyle.Top; alertPanel.Height = 72; alertPanel.BackColor = Color.FromArgb(255, 236, 230); alertPanel.Padding = new Padding(25, 12, 15, 8);
            alertText.AutoEllipsis = true; alertText.Dock = DockStyle.Fill; alertText.Padding = new Padding(0, 0, 8, 0); alertText.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold); alertText.ForeColor = Color.FromArgb(110, 43, 35); alertText.Cursor = Cursors.Hand; alertText.Click += alertText_Click;
            var dismiss = new Button { Text = "Dismiss", Dock = DockStyle.Fill, FlatStyle = FlatStyle.Flat };
            dismiss.Click += delegate { alertPanel.Visible = false; };
            var dismissHost = new Panel { Dock = DockStyle.Right, Width = 110, Padding = new Padding(5, 7, 5, 13), BackColor = Color.FromArgb(255, 236, 230) };
            dismissHost.Controls.Add(dismiss); alertPanel.Controls.Add(alertText); alertPanel.Controls.Add(dismissHost); dismissHost.BringToFront();

            var filters = new Panel { Dock = DockStyle.Top, Height = 133, BackColor = Color.White, Padding = new Padding(28, 14, 20, 10) };
            searchBox.Location = new Point(30, 17); searchBox.Size = new Size(310, 30); searchBox.Font = new Font("Segoe UI", 10F); searchBox.AccessibleName = "Search events and announcements";
            categoryBox.Location = new Point(352, 17); categoryBox.Size = new Size(200, 30); categoryBox.DropDownStyle = ComboBoxStyle.DropDownList;
            dateBox.Location = new Point(564, 17); dateBox.Size = new Size(155, 30); dateBox.DropDownStyle = ComboBoxStyle.DropDownList; dateBox.Items.AddRange(new object[] { "All upcoming", "Today", "Next 7 days", "Next 30 days" }); dateBox.SelectedIndex = 0;
            var search = MakeButton("Search", Color.FromArgb(0, 121, 107), 735, 15, 105); search.Click += delegate { PerformSearch(true); };
            var clear = MakeButton("Clear Filters", Color.FromArgb(104, 115, 124), 850, 15, 120); clear.Click += clear_Click;
            filters.Controls.Add(searchBox); filters.Controls.Add(categoryBox); filters.Controls.Add(dateBox); filters.Controls.Add(search); filters.Controls.Add(clear);
            string[] tabs = { "All", "Events", "Announcements", "Alerts", "Saved" };
            for (int index = 0; index < tabs.Length; index++)
            {
                Button tab = MakeButton(tabs[index], index == 0 ? Color.FromArgb(31, 105, 138) : Color.FromArgb(88, 105, 113), 30 + (index * 128), 74, 118);
                tab.Tag = tabs[index]; tab.Click += tab_Click; filters.Controls.Add(tab);
            }
            categoryBox.SelectedIndexChanged += delegate { if (categoryBox.Focused) PerformSearch(false); };
            dateBox.SelectedIndexChanged += delegate { if (dateBox.Focused) PerformSearch(false); };

            contentSplit.Size = new Size(1000, 400); contentSplit.SplitterDistance = 640;
            contentSplit.Panel1MinSize = 460; contentSplit.Panel2MinSize = 340;
            contentSplit.Dock = DockStyle.Fill; contentSplit.FixedPanel = FixedPanel.Panel2; contentSplit.BackColor = Color.FromArgb(226, 233, 235);
            contentSplit.Padding = new Padding(25, 18, 25, 18);
            contentSplit.Panel1.BackColor = Color.FromArgb(242, 246, 247); contentSplit.Panel2.BackColor = Color.FromArgb(242, 246, 247);
            resultsLabel.Dock = DockStyle.Top; resultsLabel.Height = 40; resultsLabel.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold); resultsLabel.ForeColor = Color.FromArgb(24, 68, 88);
            resultsPanel.Dock = DockStyle.Fill; resultsPanel.AutoScroll = true; resultsPanel.FlowDirection = FlowDirection.TopDown; resultsPanel.WrapContents = false; resultsPanel.Padding = new Padding(0, 0, 8, 0);
            contentSplit.Panel1.Controls.Add(resultsPanel); contentSplit.Panel1.Controls.Add(resultsLabel);
            var recommendedTitle = new Label { Text = "Recommended For You", Dock = DockStyle.Top, Height = 35, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88) };
            recommendationSubtitle.Dock = DockStyle.Top; recommendationSubtitle.Height = 42; recommendationSubtitle.ForeColor = Color.DimGray; recommendationSubtitle.Text = "Based on your searches, likes and saved items";
            recommendationPanel.Dock = DockStyle.Fill; recommendationPanel.AutoScroll = true; recommendationPanel.FlowDirection = FlowDirection.TopDown; recommendationPanel.WrapContents = false;
            contentSplit.Panel2.Controls.Add(recommendationPanel); contentSplit.Panel2.Controls.Add(recommendationSubtitle); contentSplit.Panel2.Controls.Add(recommendedTitle);
            Controls.Add(contentSplit); Controls.Add(filters); Controls.Add(alertPanel); Controls.Add(header);
            AcceptButton = search; CancelButton = back;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            if (Width > workingArea.Width - 30 || Height > workingArea.Height - 30)
                Size = new Size(Math.Max(MinimumSize.Width, workingArea.Width - 30), Math.Max(MinimumSize.Height, workingArea.Height - 30));

            int desiredLeftWidth = Math.Max(contentSplit.Panel1MinSize, contentSplit.ClientSize.Width - 390);
            int maximumLeftWidth = contentSplit.ClientSize.Width - contentSplit.Panel2MinSize - contentSplit.SplitterWidth;
            contentSplit.SplitterDistance = Math.Min(desiredLeftWidth, maximumLeftWidth);
            PerformSearch(false);
        }

        private static Button MakeButton(string text, Color colour, int left, int top, int width)
        {
            return new Button { Text = text, BackColor = colour, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 }, Cursor = Cursors.Hand, Location = new Point(left, top), Size = new Size(width, 36), Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold) };
        }

        private void PopulateCategories()
        {
            categoryBox.Items.Add("All categories");
            foreach (string category in manager.GetCategories()) categoryBox.Items.Add(category);
            categoryBox.SelectedIndex = 0;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            searchBox.Clear(); categoryBox.SelectedIndex = 0; dateBox.SelectedIndex = 0; selectedType = null; savedOnly = false; PerformSearch(false);
        }

        private void tab_Click(object sender, EventArgs e)
        {
            string value = (string)((Button)sender).Tag; savedOnly = value == "Saved";
            selectedType = value == "Events" ? EventType.Event : value == "Announcements" ? EventType.Announcement : value == "Alerts" ? EventType.Alert : (EventType?)null;
            PerformSearch(false);
        }

        private void PerformSearch(bool record)
        {
            IList<MunicipalEvent> results = manager.Search(searchBox.Text, categoryBox.SelectedItem as string, dateBox.SelectedItem as string, selectedType, savedOnly, record);
            resultsPanel.SuspendLayout(); resultsPanel.Controls.Clear();
            resultsLabel.Text = results.Count + (results.Count == 1 ? " item" : " items") + (savedOnly ? " saved" : " found");
            if (results.Count == 0)
                resultsPanel.Controls.Add(new Label { Text = savedOnly ? "You have no saved events yet. Select ☆ Save on an item to add it here." : "No events or announcements matched your search. Try clearing a filter.", AutoSize = false, Size = new Size(650, 80), Padding = new Padding(15), BackColor = Color.White, ForeColor = Color.DimGray });
            else foreach (MunicipalEvent item in results) resultsPanel.Controls.Add(CreateCard(item, false, null));
            resultsPanel.ResumeLayout(); RefreshRecommendations();
        }

        private Control CreateCard(MunicipalEvent item, bool compact, string explanation)
        {
            int width = compact ? 330 : Math.Max(620, resultsPanel.ClientSize.Width - 30);
            var card = new Panel { Width = width, Height = compact ? 180 : 202, Margin = new Padding(0, 0, 0, 12), Padding = new Padding(15), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Color accent = item.IsAlert ? Color.FromArgb(170, 57, 48) : Color.FromArgb(0, 121, 107);
            card.Controls.Add(new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accent });
            var badge = new Label { Text = item.Type.ToString().ToUpperInvariant() + "  •  " + item.Category, AutoSize = true, ForeColor = accent, Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold), Location = new Point(18, 12) };
            var title = new Label { Text = item.Title, AutoEllipsis = true, Font = new Font("Segoe UI Semibold", compact ? 11F : 12F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 48, 56), Location = new Point(18, 34), Size = new Size(width - 45, 28) };
            var meta = new Label { Text = item.Date.ToString("ddd, dd MMM • HH:mm") + "  |  " + item.Location, AutoEllipsis = true, ForeColor = Color.DimGray, Location = new Point(18, 65), Size = new Size(width - 45, 24) };
            var description = new Label { Text = explanation ?? item.Description, AutoEllipsis = true, ForeColor = Color.FromArgb(55, 62, 65), Location = new Point(18, 90), Size = new Size(width - 40, compact ? 38 : 32), Font = explanation == null ? Font : new Font("Segoe UI", 8.5F, FontStyle.Italic) };
            card.Controls.Add(badge); card.Controls.Add(title); card.Controls.Add(meta); card.Controls.Add(description);
            var details = MakeButton("View Details", Color.FromArgb(31, 105, 138), 18, compact ? 135 : 124, compact ? 105 : 115); details.Height = 30; details.Tag = item.Id; details.Click += details_Click;
            var like = MakeButton(item.IsLiked ? "♥ Liked " + item.LikeCount : "♡ Like " + item.LikeCount, Color.FromArgb(0, 121, 107), compact ? 132 : 143, compact ? 135 : 124, compact ? 90 : 105); like.Height = 30; like.Tag = item.Id; like.Click += like_Click;
            var save = MakeButton(item.IsSaved ? "★ Saved" : "☆ Save", Color.FromArgb(95, 105, 112), compact ? 230 : 258, compact ? 135 : 124, compact ? 82 : 90); save.Height = 30; save.Tag = item.Id; save.Click += save_Click;
            card.Controls.Add(details); card.Controls.Add(like); card.Controls.Add(save);
            if (!compact && item.Type == EventType.Event)
            {
                var interested = MakeButton(item.IsInterested ? "✓ Interested " + item.InterestedCount : "I'm Interested " + item.InterestedCount, Color.FromArgb(126, 82, 150), 18, 162, 155); interested.Height = 30; interested.Tag = item.Id; interested.Click += interested_Click; card.Controls.Add(interested);
            }
            if (!compact && item.Type != EventType.Alert)
            {
                var copy = MakeButton("Copy Details", Color.FromArgb(104, 115, 124), item.Type == EventType.Event ? 183 : 18, 162, 120); copy.Height = 30; copy.Tag = item.Id; copy.Click += copy_Click; card.Controls.Add(copy);
            }
            return card;
        }

        private void RefreshRecommendations()
        {
            recommendationPanel.SuspendLayout(); recommendationPanel.Controls.Clear();
            IList<Recommendation> items = manager.GetRecommendations(3);
            recommendationSubtitle.Text = manager.RecentSearchCount == 0 ? "Search, like, save, show interest or choose My Area to personalise this list." : "Personalised from " + manager.RecentSearchCount + " meaningful search(es), interactions and My Area.";
            if (items.Count == 0) recommendationPanel.Controls.Add(new Label { Text = "No personalised recommendations yet.", Size = new Size(320, 60), ForeColor = Color.DimGray, Padding = new Padding(8) });
            else foreach (Recommendation value in items) recommendationPanel.Controls.Add(CreateCard(value.Item, true, value.Explanation));
            recommendationPanel.ResumeLayout();
        }

        private void RefreshAlert()
        {
            MunicipalEvent alert = manager.GetPriorityAlerts(1).FirstOrDefault();
            alertPanel.Visible = alert != null;
            if (alert != null) { alertText.Tag = alert.Id; alertText.Text = "⚠ IMPORTANT MUNICIPAL ALERT  •  " + alert.Title + "\r\n" + alert.Description; }
        }

        private void alertText_Click(object sender, EventArgs e) { if (alertText.Tag != null) OpenDetails((int)alertText.Tag); }
        private void details_Click(object sender, EventArgs e) { OpenDetails((int)((Control)sender).Tag); }
        private void like_Click(object sender, EventArgs e) { manager.ToggleLike((int)((Control)sender).Tag); }
        private void save_Click(object sender, EventArgs e) { manager.ToggleSaved((int)((Control)sender).Tag); }
        private void interested_Click(object sender, EventArgs e) { manager.ToggleInterested((int)((Control)sender).Tag); }
        private void copy_Click(object sender, EventArgs e)
        {
            MunicipalEvent item = manager.GetById((int)((Control)sender).Tag); if (item == null) return;
            Clipboard.SetText("Okhahlamba Local Municipality\r\n\r\n" + item.Title + "\r\nDate: " + item.Date.ToString("dd MMMM yyyy") + "\r\nTime: " + item.Date.ToString("HH:mm") + "\r\nLocation: " + item.Location);
            resultsLabel.Text = "Details copied to clipboard.";
        }
        private void OpenDetails(int id) { MunicipalEvent item = manager.GetById(id); if (item != null) using (var form = new EventDetailsForm(manager, item)) form.ShowDialog(this); }
        private void Manager_DataChanged(object sender, EventArgs e) { PerformSearch(false); }
    }
}
