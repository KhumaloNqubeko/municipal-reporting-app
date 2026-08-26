namespace MunicipalCitizenReporting.Forms
{
    partial class ReportIssueForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.TableLayoutPanel formLayoutPanel;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.RichTextBox descriptionRichTextBox;
        private System.Windows.Forms.Label attachmentLabel;
        private System.Windows.Forms.FlowLayoutPanel attachmentPanel;
        private System.Windows.Forms.Button attachFileButton;
        private System.Windows.Forms.Button removeAttachmentButton;
        private System.Windows.Forms.Label attachmentPathLabel;
        private System.Windows.Forms.Panel progressPanel;
        private System.Windows.Forms.Label engagementLabel;
        private System.Windows.Forms.Label completionValueLabel;
        private System.Windows.Forms.ProgressBar completionProgressBar;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button submitReportButton;
        private System.Windows.Forms.OpenFileDialog attachmentOpenFileDialog;
        private System.Windows.Forms.ErrorProvider validationErrorProvider;

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
            this.components = new System.ComponentModel.Container();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.formLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.attachmentLabel = new System.Windows.Forms.Label();
            this.attachmentPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.attachFileButton = new System.Windows.Forms.Button();
            this.removeAttachmentButton = new System.Windows.Forms.Button();
            this.attachmentPathLabel = new System.Windows.Forms.Label();
            this.progressPanel = new System.Windows.Forms.Panel();
            this.engagementLabel = new System.Windows.Forms.Label();
            this.completionValueLabel = new System.Windows.Forms.Label();
            this.completionProgressBar = new System.Windows.Forms.ProgressBar();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.backButton = new System.Windows.Forms.Button();
            this.submitReportButton = new System.Windows.Forms.Button();
            this.attachmentOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.validationErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.headerPanel.SuspendLayout();
            this.formLayoutPanel.SuspendLayout();
            this.attachmentPanel.SuspendLayout();
            this.progressPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.validationErrorProvider)).BeginInit();
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
            this.headerPanel.Size = new System.Drawing.Size(934, 108);
            this.headerPanel.TabIndex = 0;
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.instructionLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.instructionLabel.Location = new System.Drawing.Point(35, 70);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(485, 21);
            this.instructionLabel.TabIndex = 1;
            this.instructionLabel.Text = "Tell us what happened. Fields marked with an asterisk (*) are required.";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(30, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(308, 50);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Report an Issue";
            // 
            // formLayoutPanel
            // 
            this.formLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formLayoutPanel.ColumnCount = 2;
            this.formLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.formLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.formLayoutPanel.Controls.Add(this.locationLabel, 0, 0);
            this.formLayoutPanel.Controls.Add(this.locationTextBox, 1, 0);
            this.formLayoutPanel.Controls.Add(this.categoryLabel, 0, 1);
            this.formLayoutPanel.Controls.Add(this.categoryComboBox, 1, 1);
            this.formLayoutPanel.Controls.Add(this.descriptionLabel, 0, 2);
            this.formLayoutPanel.Controls.Add(this.descriptionRichTextBox, 1, 2);
            this.formLayoutPanel.Controls.Add(this.attachmentLabel, 0, 3);
            this.formLayoutPanel.Controls.Add(this.attachmentPanel, 1, 3);
            this.formLayoutPanel.Location = new System.Drawing.Point(40, 132);
            this.formLayoutPanel.Name = "formLayoutPanel";
            this.formLayoutPanel.RowCount = 4;
            this.formLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.formLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.formLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.formLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.formLayoutPanel.Size = new System.Drawing.Size(854, 344);
            this.formLayoutPanel.TabIndex = 1;
            // 
            // locationLabel
            // 
            this.locationLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.locationLabel.AutoSize = true;
            this.locationLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.locationLabel.Location = new System.Drawing.Point(3, 17);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(92, 23);
            this.locationLabel.TabIndex = 0;
            this.locationLabel.Text = "Location *";
            // 
            // locationTextBox
            // 
            this.locationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.locationTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.locationTextBox.Location = new System.Drawing.Point(158, 13);
            this.locationTextBox.MaxLength = 200;
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(673, 30);
            this.locationTextBox.TabIndex = 1;
            this.locationTextBox.TextChanged += new System.EventHandler(this.FormFieldChanged);
            // 
            // categoryLabel
            // 
            this.categoryLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.categoryLabel.Location = new System.Drawing.Point(3, 75);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(94, 23);
            this.categoryLabel.TabIndex = 2;
            this.categoryLabel.Text = "Category *";
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryComboBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Items.AddRange(new object[] {
            "Roads",
            "Sanitation",
            "Water",
            "Electricity",
            "Waste Management",
            "Streetlights",
            "Public Safety",
            "Parks and Recreation",
            "Other"});
            this.categoryComboBox.Location = new System.Drawing.Point(158, 71);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(673, 31);
            this.categoryComboBox.TabIndex = 3;
            this.categoryComboBox.SelectedIndexChanged += new System.EventHandler(this.FormFieldChanged);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.descriptionLabel.Location = new System.Drawing.Point(3, 126);
            this.descriptionLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(112, 23);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Description *";
            // 
            // descriptionRichTextBox
            // 
            this.descriptionRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionRichTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.descriptionRichTextBox.Location = new System.Drawing.Point(158, 126);
            this.descriptionRichTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 23, 10);
            this.descriptionRichTextBox.MaxLength = 2000;
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.Size = new System.Drawing.Size(670, 132);
            this.descriptionRichTextBox.TabIndex = 5;
            this.descriptionRichTextBox.Text = "";
            this.descriptionRichTextBox.TextChanged += new System.EventHandler(this.FormFieldChanged);
            // 
            // attachmentLabel
            // 
            this.attachmentLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.attachmentLabel.AutoSize = true;
            this.attachmentLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.attachmentLabel.Location = new System.Drawing.Point(3, 293);
            this.attachmentLabel.Name = "attachmentLabel";
            this.attachmentLabel.Size = new System.Drawing.Size(101, 23);
            this.attachmentLabel.TabIndex = 6;
            this.attachmentLabel.Text = "Attachment";
            // 
            // attachmentPanel
            // 
            this.attachmentPanel.Controls.Add(this.attachFileButton);
            this.attachmentPanel.Controls.Add(this.removeAttachmentButton);
            this.attachmentPanel.Controls.Add(this.attachmentPathLabel);
            this.attachmentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attachmentPanel.Location = new System.Drawing.Point(155, 268);
            this.attachmentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.attachmentPanel.Name = "attachmentPanel";
            this.attachmentPanel.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.attachmentPanel.Size = new System.Drawing.Size(699, 76);
            this.attachmentPanel.TabIndex = 7;
            this.attachmentPanel.WrapContents = false;
            // 
            // attachFileButton
            // 
            this.attachFileButton.AutoSize = true;
            this.attachFileButton.BackColor = System.Drawing.Color.White;
            this.attachFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.attachFileButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.attachFileButton.Location = new System.Drawing.Point(3, 15);
            this.attachFileButton.Name = "attachFileButton";
            this.attachFileButton.Size = new System.Drawing.Size(104, 34);
            this.attachFileButton.TabIndex = 0;
            this.attachFileButton.Text = "Attach File";
            this.attachFileButton.UseVisualStyleBackColor = false;
            this.attachFileButton.Click += new System.EventHandler(this.attachFileButton_Click);
            // 
            // removeAttachmentButton
            // 
            this.removeAttachmentButton.AutoSize = true;
            this.removeAttachmentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeAttachmentButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.removeAttachmentButton.Location = new System.Drawing.Point(113, 15);
            this.removeAttachmentButton.Name = "removeAttachmentButton";
            this.removeAttachmentButton.Size = new System.Drawing.Size(83, 34);
            this.removeAttachmentButton.TabIndex = 1;
            this.removeAttachmentButton.Text = "Remove";
            this.removeAttachmentButton.UseVisualStyleBackColor = true;
            this.removeAttachmentButton.Visible = false;
            this.removeAttachmentButton.Click += new System.EventHandler(this.removeAttachmentButton_Click);
            // 
            // attachmentPathLabel
            // 
            this.attachmentPathLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.attachmentPathLabel.AutoEllipsis = true;
            this.attachmentPathLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.attachmentPathLabel.ForeColor = System.Drawing.Color.DimGray;
            this.attachmentPathLabel.Location = new System.Drawing.Point(202, 20);
            this.attachmentPathLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.attachmentPathLabel.Name = "attachmentPathLabel";
            this.attachmentPathLabel.Size = new System.Drawing.Size(450, 25);
            this.attachmentPathLabel.TabIndex = 2;
            this.attachmentPathLabel.Text = "No file selected (optional)";
            this.attachmentPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressPanel
            // 
            this.progressPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressPanel.BackColor = System.Drawing.Color.FromArgb(229, 242, 241);
            this.progressPanel.Controls.Add(this.engagementLabel);
            this.progressPanel.Controls.Add(this.completionValueLabel);
            this.progressPanel.Controls.Add(this.completionProgressBar);
            this.progressPanel.Location = new System.Drawing.Point(40, 493);
            this.progressPanel.Name = "progressPanel";
            this.progressPanel.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.progressPanel.Size = new System.Drawing.Size(854, 91);
            this.progressPanel.TabIndex = 2;
            // 
            // engagementLabel
            // 
            this.engagementLabel.AutoEllipsis = true;
            this.engagementLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.engagementLabel.ForeColor = System.Drawing.Color.FromArgb(26, 71, 97);
            this.engagementLabel.Location = new System.Drawing.Point(18, 12);
            this.engagementLabel.Name = "engagementLabel";
            this.engagementLabel.Size = new System.Drawing.Size(660, 25);
            this.engagementLabel.TabIndex = 0;
            this.engagementLabel.Text = "Let's get your issue reported.";
            // 
            // completionValueLabel
            // 
            this.completionValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.completionValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.completionValueLabel.ForeColor = System.Drawing.Color.FromArgb(26, 71, 97);
            this.completionValueLabel.Location = new System.Drawing.Point(704, 13);
            this.completionValueLabel.Name = "completionValueLabel";
            this.completionValueLabel.Size = new System.Drawing.Size(130, 24);
            this.completionValueLabel.TabIndex = 1;
            this.completionValueLabel.Text = "0% complete";
            this.completionValueLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // completionProgressBar
            // 
            this.completionProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.completionProgressBar.Location = new System.Drawing.Point(21, 51);
            this.completionProgressBar.Name = "completionProgressBar";
            this.completionProgressBar.Size = new System.Drawing.Size(812, 23);
            this.completionProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.completionProgressBar.TabIndex = 2;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.backButton);
            this.buttonPanel.Controls.Add(this.submitReportButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 603);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(934, 76);
            this.buttonPanel.TabIndex = 3;
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.backButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.backButton.Location = new System.Drawing.Point(40, 16);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(178, 43);
            this.backButton.TabIndex = 1;
            this.backButton.Text = "Back to Main Menu";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // submitReportButton
            // 
            this.submitReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitReportButton.BackColor = System.Drawing.Color.FromArgb(0, 121, 107);
            this.submitReportButton.FlatAppearance.BorderSize = 0;
            this.submitReportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.submitReportButton.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.submitReportButton.ForeColor = System.Drawing.Color.White;
            this.submitReportButton.Location = new System.Drawing.Point(716, 16);
            this.submitReportButton.Name = "submitReportButton";
            this.submitReportButton.Size = new System.Drawing.Size(178, 43);
            this.submitReportButton.TabIndex = 0;
            this.submitReportButton.Text = "Submit Report";
            this.submitReportButton.UseVisualStyleBackColor = false;
            this.submitReportButton.Click += new System.EventHandler(this.submitReportButton_Click);
            // 
            // attachmentOpenFileDialog
            // 
            this.attachmentOpenFileDialog.Filter = "Supported files (*.jpg;*.jpeg;*.png;*.pdf;*.doc;*.docx)|*.jpg;*.jpeg;*.png;*.pdf;*.doc;*.docx|Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Documents (*.pdf;*.doc;*.docx)|*.pdf;*.doc;*.docx";
            this.attachmentOpenFileDialog.FilterIndex = 1;
            this.attachmentOpenFileDialog.Title = "Attach supporting evidence";
            // 
            // validationErrorProvider
            // 
            this.validationErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.validationErrorProvider.ContainerControl = this;
            // 
            // ReportIssueForm
            // 
            this.AcceptButton = this.submitReportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 249);
            this.CancelButton = this.backButton;
            this.ClientSize = new System.Drawing.Size(934, 679);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.progressPanel);
            this.Controls.Add(this.formLayoutPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(760, 660);
            this.Name = "ReportIssueForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report an Issue - Municipal Citizen Services";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.formLayoutPanel.ResumeLayout(false);
            this.formLayoutPanel.PerformLayout();
            this.attachmentPanel.ResumeLayout(false);
            this.attachmentPanel.PerformLayout();
            this.progressPanel.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.validationErrorProvider)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
