namespace MunicipalCitizenReporting.Forms
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.TableLayoutPanel servicesLayoutPanel;
        private System.Windows.Forms.Button reportIssuesButton;
        private System.Windows.Forms.Button eventsButton;
        private System.Windows.Forms.Button statusButton;
        private System.Windows.Forms.FlowLayoutPanel alertsFlowPanel;
        private System.Windows.Forms.PictureBox municipalLogoPictureBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.municipalLogoPictureBox != null && this.municipalLogoPictureBox.Image != null)
                this.municipalLogoPictureBox.Image.Dispose();
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            var municipalLabel = new System.Windows.Forms.Label();
            var titleLabel = new System.Windows.Forms.Label();
            var subtitleLabel = new System.Windows.Forms.Label();
            var logoHost = new System.Windows.Forms.Panel();
            this.municipalLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.servicesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.reportIssuesButton = new System.Windows.Forms.Button();
            this.eventsButton = new System.Windows.Forms.Button();
            this.statusButton = new System.Windows.Forms.Button();
            var alertsTitleLabel = new System.Windows.Forms.Label();
            this.alertsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            var demoLabel = new System.Windows.Forms.Label();
            var footerLabel = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout(); this.servicesLayoutPanel.SuspendLayout(); this.SuspendLayout();

            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(24, 68, 88);
            logoHost.Dock = System.Windows.Forms.DockStyle.Right; logoHost.Width = 245; logoHost.Padding = new System.Windows.Forms.Padding(12, 25, 30, 25); logoHost.BackColor = System.Drawing.Color.FromArgb(24, 68, 88);
            this.municipalLogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill; this.municipalLogoPictureBox.BackColor = System.Drawing.Color.White;
            this.municipalLogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; this.municipalLogoPictureBox.TabStop = false;
            this.municipalLogoPictureBox.AccessibleName = "Okhahlamba Local Municipality logo"; logoHost.Controls.Add(this.municipalLogoPictureBox);
            this.headerPanel.Controls.Add(municipalLabel); this.headerPanel.Controls.Add(titleLabel); this.headerPanel.Controls.Add(subtitleLabel); this.headerPanel.Controls.Add(logoHost); logoHost.BringToFront();
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top; this.headerPanel.Height = 150;
            municipalLabel.AutoSize = true; municipalLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            municipalLabel.ForeColor = System.Drawing.Color.FromArgb(145, 220, 205); municipalLabel.Location = new System.Drawing.Point(36, 20); municipalLabel.Text = "OKHAHLAMBA LOCAL MUNICIPALITY";
            titleLabel.AutoSize = true; titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 25F, System.Drawing.FontStyle.Bold);
            titleLabel.ForeColor = System.Drawing.Color.White; titleLabel.Location = new System.Drawing.Point(31, 43); titleLabel.Text = "Municipal Services";
            subtitleLabel.AutoSize = true; subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F); subtitleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            subtitleLabel.Location = new System.Drawing.Point(37, 108); subtitleLabel.Text = "Serving our community • Select a service to get started";

            this.servicesLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.servicesLayoutPanel.ColumnCount = 3;
            this.servicesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.servicesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.servicesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.334F));
            this.servicesLayoutPanel.Controls.Add(this.reportIssuesButton, 0, 0); this.servicesLayoutPanel.Controls.Add(this.eventsButton, 1, 0); this.servicesLayoutPanel.Controls.Add(this.statusButton, 2, 0);
            this.servicesLayoutPanel.Location = new System.Drawing.Point(34, 178); this.servicesLayoutPanel.Size = new System.Drawing.Size(932, 125);
            this.reportIssuesButton.Dock = System.Windows.Forms.DockStyle.Fill; StyleServiceButton(this.reportIssuesButton, System.Drawing.Color.FromArgb(0, 121, 107), "REPORT AN ISSUE\r\n\r\nPotholes, water, electricity and other issues");
            this.reportIssuesButton.Click += new System.EventHandler(this.reportIssuesButton_Click);
            this.eventsButton.Dock = System.Windows.Forms.DockStyle.Fill; StyleServiceButton(this.eventsButton, System.Drawing.Color.FromArgb(31, 105, 138), "LOCAL EVENTS & ANNOUNCEMENTS\r\n\r\nEvents, notices and community updates");
            this.eventsButton.Click += new System.EventHandler(this.eventsButton_Click);
            this.statusButton.Dock = System.Windows.Forms.DockStyle.Fill; StyleServiceButton(this.statusButton, System.Drawing.Color.FromArgb(104, 115, 124), "SERVICE REQUEST STATUS\r\n\r\nTrack requests • Coming in Part 3");
            this.statusButton.Visible = false;
            this.statusButton.Click += new System.EventHandler(this.statusButton_Click);

            alertsTitleLabel.AutoSize = true; alertsTitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            alertsTitleLabel.ForeColor = System.Drawing.Color.FromArgb(24, 68, 88); alertsTitleLabel.Location = new System.Drawing.Point(35, 330); alertsTitleLabel.Text = "Important Alerts";
            this.alertsFlowPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.alertsFlowPanel.AutoScroll = true; this.alertsFlowPanel.BackColor = System.Drawing.Color.White; this.alertsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.alertsFlowPanel.Location = new System.Drawing.Point(39, 375); this.alertsFlowPanel.Padding = new System.Windows.Forms.Padding(8); this.alertsFlowPanel.Size = new System.Drawing.Size(923, 198); this.alertsFlowPanel.WrapContents = false;
            demoLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left; demoLabel.AutoSize = true; demoLabel.ForeColor = System.Drawing.Color.DimGray;

            footerLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right; footerLabel.AutoSize = true; footerLabel.ForeColor = System.Drawing.Color.DimGray;
            footerLabel.Location = new System.Drawing.Point(714, 810); footerLabel.Text = "Working together for our community";

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F); this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(242, 246, 247); this.ClientSize = new System.Drawing.Size(1000, 850);
            this.Controls.Add(footerLabel); this.Controls.Add(demoLabel); this.Controls.Add(this.alertsFlowPanel); this.Controls.Add(alertsTitleLabel); this.Controls.Add(this.servicesLayoutPanel); this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F); this.MinimumSize = new System.Drawing.Size(980, 760); this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Okhahlamba Municipal Services";
            this.headerPanel.ResumeLayout(false); this.headerPanel.PerformLayout(); this.servicesLayoutPanel.ResumeLayout(false); this.ResumeLayout(false); this.PerformLayout();
        }

        private static void StyleServiceButton(System.Windows.Forms.Button button, System.Drawing.Color colour, string text)
        {
            button.BackColor = colour; button.Cursor = System.Windows.Forms.Cursors.Hand; button.FlatAppearance.BorderSize = 0; button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold); button.ForeColor = System.Drawing.Color.White;
            button.Margin = new System.Windows.Forms.Padding(6); button.Text = text; button.UseVisualStyleBackColor = false;
        }
    }
}
