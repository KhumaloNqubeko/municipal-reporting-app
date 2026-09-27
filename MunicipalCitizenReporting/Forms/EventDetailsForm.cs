using System;
using System.Drawing;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Forms
{
    public sealed class EventDetailsForm : Form
    {
        private readonly EventManager manager;
        private readonly MunicipalEvent item;
        private readonly Button likeButton = new Button();
        private readonly Button saveButton = new Button();
        private readonly Button interestedButton = new Button();
        private readonly Label statusLabel = new Label();

        public EventDetailsForm(EventManager manager, MunicipalEvent item)
        {
            this.manager = manager ?? throw new ArgumentNullException("manager");
            this.item = item ?? throw new ArgumentNullException("item");
            BuildInterface();
            RefreshButtons();
        }

        private void BuildInterface()
        {
            Text = "Details - " + item.Title;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(700, 630);
            MinimumSize = new Size(620, 520);
            BackColor = Color.FromArgb(244, 247, 248);
            Font = new Font("Segoe UI", 9F);

            var header = new Panel { Dock = DockStyle.Top, Height = 112, BackColor = item.IsAlert ? Color.FromArgb(140, 48, 40) : Color.FromArgb(24, 68, 88), Padding = new Padding(28, 18, 28, 10) };
            var badge = new Label { AutoSize = true, ForeColor = Color.FromArgb(220, 245, 240), Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), Text = item.Type.ToString().ToUpperInvariant() + "  •  " + item.Category };
            var title = new Label { AutoEllipsis = true, Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 44), Size = new Size(630, 48), Text = item.Title };
            header.Controls.Add(badge); header.Controls.Add(title);

            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30, 25, 30, 15), AutoScroll = true };
            var date = CreateInfo("DATE & TIME", item.Date.ToString("dddd, dd MMMM yyyy 'at' HH:mm"), 15);
            var location = CreateInfo("LOCATION / AFFECTED AREA", item.Location, 79);
            var priority = CreateInfo("PRIORITY", item.IsAlert ? PriorityText(item.Priority) : "General information", 143);
            var descriptionTitle = new Label { Text = "FULL DESCRIPTION", Location = new Point(15, 210), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88) };
            var description = new Label { Text = item.Description, Location = new Point(15, 237), Size = new Size(590, 115), Font = new Font("Segoe UI", 10F), ForeColor = Color.FromArgb(45, 52, 55) };
            body.Controls.Add(date); body.Controls.Add(location); body.Controls.Add(priority); body.Controls.Add(descriptionTitle); body.Controls.Add(description);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 116, Padding = new Padding(28, 12, 20, 8), FlowDirection = FlowDirection.LeftToRight, BackColor = Color.White };
            StyleAction(likeButton, Color.FromArgb(0, 121, 107)); StyleAction(saveButton, Color.FromArgb(31, 105, 138));
            likeButton.Click += delegate { manager.ToggleLike(item.Id); RefreshButtons(); };
            saveButton.Click += delegate { manager.ToggleSaved(item.Id); RefreshButtons(); };
            StyleAction(interestedButton, Color.FromArgb(126, 82, 150));
            interestedButton.Click += delegate { manager.ToggleInterested(item.Id); RefreshButtons(); };
            var copy = new Button(); StyleAction(copy, Color.FromArgb(104, 115, 124)); copy.Text = "Copy Details";
            copy.Click += delegate { Clipboard.SetText(BuildShareText()); statusLabel.Text = "Details copied to clipboard."; };
            var close = new Button { Text = "Close", Size = new Size(110, 40), Margin = new Padding(12, 0, 0, 0), DialogResult = DialogResult.OK };
            actions.Controls.Add(likeButton); actions.Controls.Add(saveButton); if (item.Type == EventType.Event) actions.Controls.Add(interestedButton); actions.Controls.Add(copy); actions.Controls.Add(close);
            statusLabel.AutoSize = true; statusLabel.ForeColor = Color.FromArgb(0, 121, 107); statusLabel.Margin = new Padding(5, 8, 0, 0); actions.Controls.Add(statusLabel);
            Controls.Add(body); Controls.Add(actions); Controls.Add(header);
            AcceptButton = close; CancelButton = close;
        }

        private static Panel CreateInfo(string heading, string value, int top)
        {
            var panel = new Panel { Location = new Point(15, top), Size = new Size(590, 55) };
            panel.Controls.Add(new Label { Text = heading, AutoSize = true, Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88), Location = new Point(0, 0) });
            panel.Controls.Add(new Label { Text = value, AutoEllipsis = true, Size = new Size(585, 25), Font = new Font("Segoe UI", 10F), Location = new Point(0, 23) });
            return panel;
        }

        private static string PriorityText(int priority) { return priority == 1 ? "Critical / high priority" : priority == 2 ? "Important warning" : "General notice"; }
        private static void StyleAction(Button button, Color colour) { button.Size = new Size(150, 40); button.Margin = new Padding(0, 0, 10, 0); button.FlatStyle = FlatStyle.Flat; button.FlatAppearance.BorderSize = 0; button.BackColor = colour; button.ForeColor = Color.White; button.Cursor = Cursors.Hand; button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold); }
        private string BuildShareText()
        {
            return "Okhahlamba Local Municipality\r\n\r\n" + item.Title + "\r\n\r\nDate: " + item.Date.ToString("dd MMMM yyyy") + "\r\nTime: " + item.Date.ToString("HH:mm") + "\r\nLocation: " + item.Location + "\r\n\r\n";
        }
        private void RefreshButtons() { likeButton.Text = item.IsLiked ? "♥  Liked (" + item.LikeCount + ")" : "♡  Like (" + item.LikeCount + ")"; saveButton.Text = item.IsSaved ? "★  Saved" : "☆  Save"; interestedButton.Text = item.IsInterested ? "✓  Interested (" + item.InterestedCount + ")" : "I'm Interested (" + item.InterestedCount + ")"; }
    }
}
