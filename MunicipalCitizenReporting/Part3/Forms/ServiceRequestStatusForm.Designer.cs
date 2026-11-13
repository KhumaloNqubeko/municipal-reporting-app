using System.Drawing;
using System.Windows.Forms;

namespace MunicipalCitizenReporting.Part3.Forms
{
    public partial class ServiceRequestStatusForm
    {
        private TextBox searchBox, referenceBox, detailBox;
        private ComboBox statusFilter, categoryFilter, orderFilter;
        private Label summary, resultMessage;
        private DataGridView grid;
        private Button refreshButton;
        private void InitializeComponent()
        {
            Text = "Service Request Status — Okhahlamba"; ClientSize = new Size(1180, 720); MinimumSize = new Size(960, 620);
            StartPosition = FormStartPosition.CenterParent; Font = new Font("Segoe UI", 10F);
            BackColor = Color.FromArgb(242, 246, 247); AutoScaleMode = AutoScaleMode.Font;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, Padding = new Padding(16) };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            var heading = new Label { Text = "SERVICE REQUEST STATUS", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 68, 88) };
            layout.Controls.Add(heading, 0, 0);
            layout.Controls.Add(new Label { Text = "Session-only reports: closing the application clears requests. Priority and status history are not recorded.", Dock = DockStyle.Fill, AutoEllipsis = true }, 0, 1);
            summary = new Label { Text = "Loading requests…", Dock = DockStyle.Fill }; layout.Controls.Add(summary, 0, 2);
            var filters = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 2 };
            filters.RowStyles.Add(new RowStyle(SizeType.Absolute, 24)); filters.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            for (int i = 0; i < 4; i++) filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            searchBox = new TextBox { Dock = DockStyle.Fill, AccessibleName = "Search request text" };
            statusFilter = Choice("Status filter"); categoryFilter = Choice("Category filter"); orderFilter = Choice("Request order");
            orderFilter.Items.AddRange(new object[] { "Newest first", "Oldest first", "Submission order" }); orderFilter.SelectedIndex = 0;
            Control[] inputs = { searchBox, statusFilter, categoryFilter, orderFilter };
            string[] labels = { "Search text", "Status", "Category", "Order" };
            for (int i = 0; i < 4; i++) { filters.Controls.Add(new Label { Text = labels[i], AutoSize = true }, i, 0); filters.Controls.Add(inputs[i], i, 1); }
            filters.Controls.Add(ActionButton("&Reset", delegate { ResetFilters(); }), 4, 1); layout.Controls.Add(filters, 0, 3);
            var lookup = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            lookup.Controls.Add(new Label { Text = "Exact reference:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            referenceBox = new TextBox { Width = 180, AccessibleName = "Exact request reference" }; lookup.Controls.Add(referenceBox);
            lookup.Controls.Add(ActionButton("&Find", delegate { FindRequest(); }));
            lookup.Controls.Add(new Label { Text = "Example: REQ-1001", AutoSize = true, Padding = new Padding(6, 7, 0, 0) }); layout.Controls.Add(lookup, 0, 4);
            var content = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66)); content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AutoGenerateColumns = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                RowHeadersVisible = false, BackgroundColor = Color.White, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AccessibleName = "Submitted service requests" };
            AddColumn("Reference", "Reference", 100); AddColumn("Category", "Category", 95); AddColumn("Submitted", "Submitted", 120);
            AddColumn("Priority", "Priority", 95); AddColumn("Status", "Status", 85); AddColumn("Location", "Location", 110);
            grid.Columns[2].DefaultCellStyle.Format = "dd MMM yyyy HH:mm";
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 68, 88); grid.DefaultCellStyle.SelectionForeColor = Color.White;
            content.Controls.Add(grid, 0, 0);
            detailBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
                BackColor = Color.White, AccessibleName = "Request metadata and current status", Text = "Select a request to view its details." };
            content.Controls.Add(detailBox, 1, 0); layout.Controls.Add(content, 0, 5);
            var footer = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 1 };
            footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            resultMessage = new Label { Dock = DockStyle.Fill, AutoEllipsis = true, Text = "Ready", AccessibleName = "Request results and validation" };
            footer.Controls.Add(resultMessage, 0, 0);
            refreshButton = ActionButton("Re&fresh", async delegate { await ReloadAsync(); }); footer.Controls.Add(refreshButton, 1, 0);
            var planning = ActionButton("Service &Planning", delegate
            {
                using (var form = new ServicePlanningForm(displayedRequest?.Category))
                    form.ShowDialog(this);
            });
            footer.Controls.Add(planning, 2, 0);
            var admin = ActionButton("&Admin", async delegate
            {
                using (var login = new AdminLoginForm())
                {
                    if (login.ShowDialog(this) != DialogResult.OK)
                        return;
                }
                using (var form = new RequestAdminForm(displayedRequest?.Reference))
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                        return;
                    await ReloadAsync();
                    if (IsDisposed)
                        return;
                    referenceBox.Text = form.UpdatedReference;
                    FindRequest();
                }
            });
            footer.Controls.Add(admin, 3, 0);
            var back = ActionButton("&Back", delegate { Close(); }); footer.Controls.Add(back, 4, 0); CancelButton = back;
            foreach (Control control in footer.Controls) control.Dock = DockStyle.Fill;
            layout.Controls.Add(footer, 0, 6); Controls.Add(layout);
            searchBox.TextChanged += delegate { ScheduleFilter(); }; statusFilter.SelectedIndexChanged += delegate { ScheduleFilter(); };
            categoryFilter.SelectedIndexChanged += delegate { ScheduleFilter(); }; orderFilter.SelectedIndexChanged += delegate { ScheduleFilter(); };
            grid.SelectionChanged += delegate { ShowDetails(grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as Models.ServiceRequestViewModel); };
            referenceBox.KeyDown += delegate(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; FindRequest(); } };
        }
        private static ComboBox Choice(string name) { return new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = name }; }
        private static Button ActionButton(string text, System.EventHandler action)
        {
            var button = new Button { Text = text, AutoSize = false, Size = new Size(100, 32), TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(24, 68, 88), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, UseVisualStyleBackColor = false };
            button.Click += action; return button;
        }
        private void AddColumn(string property, string heading, int weight)
        { grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = heading, FillWeight = weight, SortMode = DataGridViewColumnSortMode.NotSortable }); }
    }
}
