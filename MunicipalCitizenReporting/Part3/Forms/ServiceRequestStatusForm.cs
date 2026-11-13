using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MunicipalCitizenReporting.Part3.Adapters;
using MunicipalCitizenReporting.Part3.Models;
using MunicipalCitizenReporting.Part3.Services;

namespace MunicipalCitizenReporting.Part3.Forms
{
    public partial class ServiceRequestStatusForm : Form
    {
        private ServiceRequestStatusService service = new ServiceRequestStatusService(new ServiceRequestViewModel[0]);
        private readonly Timer filterTimer = new Timer { Interval = 200 };
        private bool updating;
        private int revision;
        private ServiceRequestViewModel displayedRequest;
        public ServiceRequestStatusForm()
        {
            InitializeComponent(); filterTimer.Tick += async delegate { filterTimer.Stop(); await ApplyFiltersAsync(); };
            FormClosed += delegate { revision++; filterTimer.Stop(); filterTimer.Dispose(); };
        }
        protected override async void OnShown(EventArgs e) { base.OnShown(e); await ReloadAsync(); }
        private async Task ReloadAsync()
        {
            int current = ++revision; refreshButton.Enabled = false; updating = true;
            resultMessage.Text = "Loading current session reports…";
            string status = Selected(statusFilter), category = Selected(categoryFilter);
            try
            {
                var snapshot = new ExistingRequestReadAdapter().ReadSnapshot();
                var loaded = await Task.Run(() => new ServiceRequestStatusService(snapshot));
                if (IsDisposed || current != revision) return;
                service = loaded;
                Populate(statusFilter, "All statuses", service.Statuses, status);
                Populate(categoryFilter, "All categories", service.Categories, category);
                updating = false; await ApplyFiltersAsync();
            }
            catch (Exception) { if (!IsDisposed) resultMessage.Text = "Reports could not be loaded. Please refresh and try again."; }
            finally { updating = false; if (!IsDisposed) refreshButton.Enabled = true; }
        }
        private static void Populate(ComboBox combo, string all, System.Collections.Generic.IList<string> values, string selected)
        {
            combo.Items.Clear(); combo.Items.Add(all); foreach (string value in values) combo.Items.Add(value);
            int index = selected == null ? 0 : combo.Items.IndexOf(selected); combo.SelectedIndex = index < 0 ? 0 : index;
        }
        private static string Selected(ComboBox combo) { return combo.SelectedIndex > 0 ? combo.SelectedItem as string : null; }
        private void ScheduleFilter() { if (updating) return; revision++; filterTimer.Stop(); filterTimer.Start(); }
        private async Task ApplyFiltersAsync()
        {
            if (IsDisposed || updating) return;
            int current = ++revision; var source = service;
            string text = searchBox.Text, status = Selected(statusFilter), category = Selected(categoryFilter);
            bool newest = orderFilter.SelectedIndex == 0, priority = orderFilter.SelectedIndex == 2;
            try
            {
                var rows = await Task.Run(() => source.Filter(text, status, category, newest, priority));
                if (IsDisposed || current != revision) return;
                grid.DataSource = new BindingList<ServiceRequestViewModel>(rows.ToList());
                summary.Text = "Total: " + source.Requests.Count + "    Matching: " + rows.Count + "    Submitted: " + source.Requests.Count(r => r.Status == "Submitted");
                resultMessage.Text = rows.Count == 0 ? "No requests match. Submit a report or reset the filters." : rows.Count + " / " + source.Requests.Count + " requests" + (priority ? " — priority not recorded; oldest first" : "");
                if (rows.Count == 0) ShowDetails(null);
            }
            catch (Exception) { if (!IsDisposed) resultMessage.Text = "Filtering failed. Please reset the filters or refresh."; }
        }
        private void ResetFilters()
        { updating = true; searchBox.Clear(); if (statusFilter.Items.Count > 0) statusFilter.SelectedIndex = 0; if (categoryFilter.Items.Count > 0) categoryFilter.SelectedIndex = 0; orderFilter.SelectedIndex = 0; referenceBox.Clear(); updating = false; ScheduleFilter(); }
        private void FindRequest()
        {
            string message; ServiceRequestViewModel request = service.Lookup(referenceBox.Text, out message); resultMessage.Text = message;
            if (request == null) { ShowDetails(null); referenceBox.Focus(); return; }
            // Lookup covers the entire loaded session, even when the request is hidden by filters.
            ShowDetails(request); resultMessage.Text = message + " Details shown independently of grid filters.";
        }
        private void ShowDetails(ServiceRequestViewModel request)
        {
            displayedRequest = request;
            detailBox.Text = request == null ? "Select a request to view details. Status history is not recorded by the existing application." :
                request.Reference + Environment.NewLine + Environment.NewLine + "Category: " + request.Category + Environment.NewLine +
                "Submitted: " + request.Submitted.ToString("dd MMM yyyy HH:mm") + Environment.NewLine + "Priority: " + request.Priority + Environment.NewLine +
                "Current status: " + request.Status + Environment.NewLine + "Location: " + request.Location + Environment.NewLine +
                "Attachment: " + request.AttachmentFileName + Environment.NewLine + Environment.NewLine + request.Description + Environment.NewLine + Environment.NewLine +
                "Status history is not recorded by the existing application. Only the current status is available.";
        }
    }
}
