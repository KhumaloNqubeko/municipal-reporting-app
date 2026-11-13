using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Part3.Forms
{
    internal sealed class RequestAdminForm : Form
    {
        private static readonly string[] Statuses = { "Submitted", "In Progress", "Resolved" };
        internal string UpdatedReference { get; private set; }

        internal RequestAdminForm(string reference)
        {
            Text = "Request Administration";
            ClientSize = new Size(520, 210);
            MinimumSize = new Size(460, 230);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.Font;

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, Padding = new Padding(16)
            };
            var requests = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 380,
                AccessibleName = "Request reference", DisplayMember = nameof(ReportedIssue.ReferenceNumber) };
            var status = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 380,
                AccessibleName = "New status" };
            var current = new Label { AutoSize = true };
            var save = new Button { Text = "&Save status", AutoSize = true };
            var close = new Button { Text = "&Close", AutoSize = true, DialogResult = DialogResult.Cancel };
            CancelButton = close;
            AcceptButton = save;

            var issues = IssueRepository.Instance.Issues.ToList();
            requests.DataSource = issues;
            var selected = issues.FirstOrDefault(issue => issue.ReferenceNumber == reference);
            if (selected != null)
                requests.SelectedItem = selected;

            Action refresh = () =>
            {
                var issue = requests.SelectedItem as ReportedIssue;
                current.Text = issue == null ? "Submit a report first." : "Current status: " + issue.Status;
                status.Items.Clear();
                int index = issue == null ? -1 : Array.IndexOf(Statuses, issue.Status);
                if (index >= 0 && index + 1 < Statuses.Length)
                {
                    status.Items.Add(Statuses[index + 1]);
                    status.SelectedIndex = 0;
                }
                status.Enabled = save.Enabled = status.SelectedIndex >= 0;
            };
            requests.SelectedIndexChanged += (sender, args) => refresh();
            save.Click += (sender, args) =>
            {
                var issue = requests.SelectedItem as ReportedIssue;
                var next = status.SelectedItem as string;
                int index = issue == null ? -1 : Array.IndexOf(Statuses, issue.Status);
                if (index < 0 || index + 1 >= Statuses.Length || next != Statuses[index + 1])
                {
                    refresh();
                    return;
                }
                issue.Status = next;
                UpdatedReference = issue.ReferenceNumber;
                DialogResult = DialogResult.OK;
            };
            layout.Controls.Add(new Label { Text = "Session-only status updates", AutoSize = true });
            layout.Controls.Add(requests);
            layout.Controls.Add(current);
            layout.Controls.Add(status);
            var actions = new FlowLayoutPanel { AutoSize = true };
            actions.Controls.Add(save);
            actions.Controls.Add(close);
            layout.Controls.Add(actions);
            Controls.Add(layout);
            refresh();
        }
    }
}
