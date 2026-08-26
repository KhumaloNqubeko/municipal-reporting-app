using System;
using System.IO;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Forms
{
    public partial class ReportIssueForm : Form
    {
        private readonly IssueRepository issueRepository;
        private string selectedAttachmentPath = string.Empty;

        public ReportIssueForm()
            : this(IssueRepository.Instance)
        {
        }

        internal ReportIssueForm(IssueRepository issueRepository)
        {
            this.issueRepository = issueRepository ?? throw new ArgumentNullException("issueRepository");
            InitializeComponent();
            categoryComboBox.SelectedIndex = -1;
            UpdateProgress();
        }

        private void FormFieldChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void attachFileButton_Click(object sender, EventArgs e)
        {
            if (attachmentOpenFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            selectedAttachmentPath = attachmentOpenFileDialog.FileName;
            attachmentPathLabel.Text = Path.GetFileName(selectedAttachmentPath);
            attachmentPathLabel.ForeColor = System.Drawing.Color.FromArgb(26, 71, 97);
            attachmentPathLabel.AccessibleDescription = selectedAttachmentPath;
            removeAttachmentButton.Visible = true;
            UpdateProgress();
        }

        private void removeAttachmentButton_Click(object sender, EventArgs e)
        {
            selectedAttachmentPath = string.Empty;
            attachmentOpenFileDialog.FileName = string.Empty;
            attachmentPathLabel.Text = "No file selected (optional)";
            attachmentPathLabel.ForeColor = System.Drawing.Color.DimGray;
            attachmentPathLabel.AccessibleDescription = string.Empty;
            removeAttachmentButton.Visible = false;
            UpdateProgress();
        }

        private void submitReportButton_Click(object sender, EventArgs e)
        {
            if (!ValidateReport())
            {
                return;
            }

            try
            {
                ReportedIssue issue = issueRepository.AddIssue(
                    locationTextBox.Text.Trim(),
                    categoryComboBox.SelectedItem.ToString(),
                    descriptionRichTextBox.Text.Trim(),
                    selectedAttachmentPath);

                MessageBox.Show(
                    this,
                    "Thank you! Your municipal issue has been reported successfully.\n\nReference: #" + issue.Id,
                    "Report Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ResetForm();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    this,
                    "We could not submit your report. Please check the information and try again.",
                    "Unable to Submit Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ValidateReport()
        {
            validationErrorProvider.Clear();

            if (string.IsNullOrWhiteSpace(locationTextBox.Text))
            {
                ShowValidationError(locationTextBox, "Please enter the location of the issue.");
                return false;
            }

            if (categoryComboBox.SelectedIndex < 0)
            {
                ShowValidationError(categoryComboBox, "Please select a category for the issue.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(descriptionRichTextBox.Text))
            {
                ShowValidationError(descriptionRichTextBox, "Please describe the municipal issue.");
                return false;
            }

            return true;
        }

        private void ShowValidationError(Control control, string message)
        {
            validationErrorProvider.SetError(control, message);
            MessageBox.Show(
                this,
                message,
                "Information Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            control.Focus();
        }

        private void UpdateProgress()
        {
            int progress = 0;

            if (!string.IsNullOrWhiteSpace(locationTextBox.Text))
            {
                progress += 30;
                validationErrorProvider.SetError(locationTextBox, string.Empty);
            }

            if (categoryComboBox.SelectedIndex >= 0)
            {
                progress += 30;
                validationErrorProvider.SetError(categoryComboBox, string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(descriptionRichTextBox.Text))
            {
                progress += 30;
                validationErrorProvider.SetError(descriptionRichTextBox, string.Empty);
            }

            if (!string.IsNullOrEmpty(selectedAttachmentPath))
            {
                progress += 10;
            }

            completionProgressBar.Value = progress;
            completionValueLabel.Text = progress + "% complete";

            if (progress >= 90)
            {
                engagementLabel.Text = "Ready to submit! Thank you for helping improve your community.";
            }
            else if (progress > 0)
            {
                engagementLabel.Text = "Great progress — just a few more details.";
            }
            else
            {
                engagementLabel.Text = "Let's get your issue reported.";
            }
        }

        private void ResetForm()
        {
            locationTextBox.Clear();
            categoryComboBox.SelectedIndex = -1;
            descriptionRichTextBox.Clear();
            selectedAttachmentPath = string.Empty;
            attachmentOpenFileDialog.FileName = string.Empty;
            attachmentPathLabel.Text = "No file selected (optional)";
            attachmentPathLabel.ForeColor = System.Drawing.Color.DimGray;
            attachmentPathLabel.AccessibleDescription = string.Empty;
            removeAttachmentButton.Visible = false;
            validationErrorProvider.Clear();
            UpdateProgress();
            locationTextBox.Focus();
        }
    }
}
