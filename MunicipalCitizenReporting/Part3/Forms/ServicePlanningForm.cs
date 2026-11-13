using System;
using System.Drawing;
using System.Windows.Forms;
using MunicipalCitizenReporting.Part3.Services;

namespace MunicipalCitizenReporting.Part3.Forms
{
    internal sealed class ServicePlanningForm : Form
    {
        internal ServicePlanningForm(string category)
        {
            Text = "Service Planning — Sample data";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(680, 465);
            MinimumSize = new Size(520, 420);
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.Font;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 1 };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label
            {
                AutoSize = true,
                Text = "Sample municipal routes and costs. Request status stays on the tracking screen."
            }, 0, 0);

            var choices = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
            var service = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = "Issue category", Width = 150 };
            service.Items.AddRange(new object[] { "Water", "Roads", "Other" });
            service.SelectedIndex = string.Equals(category, "Water", StringComparison.OrdinalIgnoreCase) ? 0 :
                string.Equals(category, "Roads", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
            var destination = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = "Destination team", Width = 150 };
            destination.Items.AddRange(new object[] { "Works", "Inspection", "Unassigned" });
            destination.SelectedIndex = 0;
            choices.Controls.Add(new Label { Text = "Category", AutoSize = true, Anchor = AnchorStyles.Left });
            choices.Controls.Add(service);
            choices.Controls.Add(new Label { Text = "Destination", AutoSize = true, Anchor = AnchorStyles.Left });
            choices.Controls.Add(destination);
            layout.Controls.Add(choices, 0, 1);

            var summary = new TextBox
            {
                Dock = DockStyle.Fill, Multiline = true, ReadOnly = true,
                ScrollBars = ScrollBars.Vertical, AccessibleName = "Sample service plan"
            };
            layout.Controls.Add(summary, 0, 2);
            Action refresh = () => summary.Text = ServicePlanning.BuildSummary(
                (string)service.SelectedItem, (string)destination.SelectedItem);
            service.SelectedIndexChanged += (sender, args) => refresh();
            destination.SelectedIndexChanged += (sender, args) => refresh();
            refresh();

            var back = new Button { Text = "Back", AutoSize = true, Anchor = AnchorStyles.Right, DialogResult = DialogResult.Cancel };
            CancelButton = back;
            layout.Controls.Add(back, 0, 3);
            Controls.Add(layout);
        }
    }
}
