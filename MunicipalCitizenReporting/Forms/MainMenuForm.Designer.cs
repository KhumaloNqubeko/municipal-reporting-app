namespace MunicipalCitizenReporting.Forms
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.TableLayoutPanel servicesLayoutPanel;
        private System.Windows.Forms.Button reportIssuesButton;
        private System.Windows.Forms.Button eventsButton;
        private System.Windows.Forms.Button statusButton;
        private System.Windows.Forms.Label footerLabel;

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
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.servicesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.reportIssuesButton = new System.Windows.Forms.Button();
            this.eventsButton = new System.Windows.Forms.Button();
            this.statusButton = new System.Windows.Forms.Button();
            this.footerLabel = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.servicesLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(26, 71, 97);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(32, 24, 32, 18);
            this.headerPanel.Size = new System.Drawing.Size(784, 126);
            this.headerPanel.TabIndex = 0;
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.AutoSize = true;
            this.subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.subtitleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.subtitleLabel.Location = new System.Drawing.Point(36, 78);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(300, 23);
            this.subtitleLabel.TabIndex = 1;
            this.subtitleLabel.Text = "Welcome. Select a municipal service below.";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 23F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(30, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(499, 52);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Municipal Citizen Services";
            // 
            // servicesLayoutPanel
            // 
            this.servicesLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.servicesLayoutPanel.ColumnCount = 1;
            this.servicesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.servicesLayoutPanel.Controls.Add(this.reportIssuesButton, 0, 0);
            this.servicesLayoutPanel.Controls.Add(this.eventsButton, 0, 1);
            this.servicesLayoutPanel.Controls.Add(this.statusButton, 0, 2);
            this.servicesLayoutPanel.Location = new System.Drawing.Point(110, 172);
            this.servicesLayoutPanel.Name = "servicesLayoutPanel";
            this.servicesLayoutPanel.RowCount = 3;
            this.servicesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.servicesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.servicesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.servicesLayoutPanel.Size = new System.Drawing.Size(564, 239);
            this.servicesLayoutPanel.TabIndex = 1;
            // 
            // reportIssuesButton
            // 
            this.reportIssuesButton.BackColor = System.Drawing.Color.FromArgb(0, 121, 107);
            this.reportIssuesButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.reportIssuesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportIssuesButton.FlatAppearance.BorderSize = 0;
            this.reportIssuesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportIssuesButton.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.reportIssuesButton.ForeColor = System.Drawing.Color.White;
            this.reportIssuesButton.Location = new System.Drawing.Point(3, 3);
            this.reportIssuesButton.Name = "reportIssuesButton";
            this.reportIssuesButton.Size = new System.Drawing.Size(558, 73);
            this.reportIssuesButton.TabIndex = 0;
            this.reportIssuesButton.Text = "Report Issues";
            this.reportIssuesButton.UseVisualStyleBackColor = false;
            this.reportIssuesButton.Click += new System.EventHandler(this.reportIssuesButton_Click);
            // 
            // eventsButton
            // 
            this.eventsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsButton.Enabled = false;
            this.eventsButton.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.eventsButton.Location = new System.Drawing.Point(3, 82);
            this.eventsButton.Name = "eventsButton";
            this.eventsButton.Size = new System.Drawing.Size(558, 73);
            this.eventsButton.TabIndex = 1;
            this.eventsButton.Text = "Local Events and Announcements  —  Coming Soon";
            this.eventsButton.UseVisualStyleBackColor = true;
            // 
            // statusButton
            // 
            this.statusButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusButton.Enabled = false;
            this.statusButton.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.statusButton.Location = new System.Drawing.Point(3, 161);
            this.statusButton.Name = "statusButton";
            this.statusButton.Size = new System.Drawing.Size(558, 75);
            this.statusButton.TabIndex = 2;
            this.statusButton.Text = "Service Request Status  —  Coming Soon";
            this.statusButton.UseVisualStyleBackColor = true;
            // 
            // footerLabel
            // 
            this.footerLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.footerLabel.AutoSize = true;
            this.footerLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.footerLabel.ForeColor = System.Drawing.Color.DimGray;
            this.footerLabel.Location = new System.Drawing.Point(222, 467);
            this.footerLabel.Name = "footerLabel";
            this.footerLabel.Size = new System.Drawing.Size(340, 20);
            this.footerLabel.TabIndex = 2;
            this.footerLabel.Text = "Working together for a better, safer community.";
            // 
            // MainMenuForm
            // 
            this.AcceptButton = this.reportIssuesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 249);
            this.ClientSize = new System.Drawing.Size(784, 521);
            this.Controls.Add(this.footerLabel);
            this.Controls.Add(this.servicesLayoutPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(680, 520);
            this.Name = "MainMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Municipal Citizen Services";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.servicesLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
