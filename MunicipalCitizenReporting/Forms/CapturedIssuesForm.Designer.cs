namespace MunicipalCitizenReporting.Forms
{
    partial class CapturedIssuesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.DataGridView issuesDataGridView;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button backButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.issuesDataGridView = new System.Windows.Forms.DataGridView();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.backButton = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.issuesDataGridView)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(26, 71, 97);
            this.headerPanel.Controls.Add(this.instructionLabel);
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1100, 108);
            this.headerPanel.TabIndex = 0;
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.instructionLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.instructionLabel.Location = new System.Drawing.Point(35, 70);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(490, 21);
            this.instructionLabel.TabIndex = 1;
            this.instructionLabel.Text = "Read-only reports captured while this application is running.";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(30, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(297, 50);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Captured Issues";
            // 
            // countLabel
            // 
            this.countLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.countLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.countLabel.ForeColor = System.Drawing.Color.FromArgb(26, 71, 97);
            this.countLabel.Location = new System.Drawing.Point(35, 128);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(1030, 25);
            this.countLabel.TabIndex = 1;
            this.countLabel.Text = "0 issues captured during this session";
            // 
            // issuesDataGridView
            // 
            this.issuesDataGridView.AllowUserToAddRows = false;
            this.issuesDataGridView.AllowUserToDeleteRows = false;
            this.issuesDataGridView.AllowUserToOrderColumns = true;
            this.issuesDataGridView.AllowUserToResizeRows = false;
            this.issuesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.issuesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.issuesDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.issuesDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.issuesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.issuesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.issuesDataGridView.Columns.Add("ReferenceNumber", "Reference Number");
            this.issuesDataGridView.Columns.Add("Location", "Location");
            this.issuesDataGridView.Columns.Add("Category", "Category");
            this.issuesDataGridView.Columns.Add("Description", "Description");
            this.issuesDataGridView.Columns.Add("DateReported", "Date Reported");
            this.issuesDataGridView.Columns.Add("Status", "Status");
            this.issuesDataGridView.Columns.Add("Attachment", "Attachment");
            this.issuesDataGridView.Location = new System.Drawing.Point(40, 164);
            this.issuesDataGridView.MultiSelect = false;
            this.issuesDataGridView.Name = "issuesDataGridView";
            this.issuesDataGridView.ReadOnly = true;
            this.issuesDataGridView.RowHeadersVisible = false;
            this.issuesDataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.issuesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.issuesDataGridView.Size = new System.Drawing.Size(1020, 420);
            this.issuesDataGridView.TabIndex = 2;
            this.issuesDataGridView.Columns[0].FillWeight = 75F;
            this.issuesDataGridView.Columns[1].FillWeight = 100F;
            this.issuesDataGridView.Columns[2].FillWeight = 80F;
            this.issuesDataGridView.Columns[3].FillWeight = 180F;
            this.issuesDataGridView.Columns[4].FillWeight = 95F;
            this.issuesDataGridView.Columns[4].DefaultCellStyle.Format = "g";
            this.issuesDataGridView.Columns[5].FillWeight = 65F;
            this.issuesDataGridView.Columns[6].FillWeight = 90F;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.backButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 603);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(1100, 76);
            this.buttonPanel.TabIndex = 3;
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.backButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.backButton.Location = new System.Drawing.Point(40, 16);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(206, 43);
            this.backButton.TabIndex = 0;
            this.backButton.Text = "Back to Report Issue";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // CapturedIssuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 249);
            this.CancelButton = this.backButton;
            this.ClientSize = new System.Drawing.Size(1100, 679);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.issuesDataGridView);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(860, 560);
            this.Name = "CapturedIssuesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Captured Issues - Municipal Citizen Services";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.issuesDataGridView)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
