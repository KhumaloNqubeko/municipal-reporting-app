using System;
using System.Windows.Forms;

namespace MunicipalCitizenReporting.Forms
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void reportIssuesButton_Click(object sender, EventArgs e)
        {
            using (var reportIssueForm = new ReportIssueForm())
            {
                Hide();
                reportIssueForm.ShowDialog(this);
                Show();
                Activate();
            }
        }
    }
}
