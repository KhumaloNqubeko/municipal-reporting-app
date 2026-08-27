using System;
using System.IO;
using System.Windows.Forms;
using MunicipalCitizenReporting.Data;
using MunicipalCitizenReporting.Models;

namespace MunicipalCitizenReporting.Forms
{
    public partial class CapturedIssuesForm : Form
    {
        private readonly IssueRepository issueRepository;

        public CapturedIssuesForm()
            : this(IssueRepository.Instance)
        {
        }

        internal CapturedIssuesForm(IssueRepository issueRepository)
        {
            this.issueRepository = issueRepository ?? throw new ArgumentNullException("issueRepository");
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadIssues();
        }

        private void LoadIssues()
        {
            issuesDataGridView.Rows.Clear();

            foreach (ReportedIssue issue in issueRepository.Issues)
            {
                string attachment = string.IsNullOrWhiteSpace(issue.AttachmentPath)
                    ? "None"
                    : Path.GetFileName(issue.AttachmentPath);

                issuesDataGridView.Rows.Add(
                    issue.ReferenceNumber,
                    issue.Location,
                    issue.Category,
                    issue.Description,
                    issue.DateReported,
                    issue.Status,
                    attachment);
            }

            countLabel.Text = issueRepository.Issues.Count == 1
                ? "1 issue captured during this session"
                : issueRepository.Issues.Count + " issues captured during this session";

            if (issueRepository.Issues.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "No issues have been reported during this session.",
                    "No Captured Issues",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
