using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using MunicipalCitizenReporting.Part3.Forms;

namespace MunicipalCitizenReporting.Forms
{
    public partial class MainMenuForm
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MunicipalCitizenReporting.Part3.Demo.MunicipalDemoData.SeedRequests();
            // Append a row and shift lower dashboard controls equally; preserve existing services and their handlers.
            const int extraHeight = 82;
            var originalBounds = new Dictionary<Control, Rectangle>();
            foreach (Control control in Controls) originalBounds.Add(control, control.Bounds);
            Rectangle servicesBounds = servicesLayoutPanel.Bounds;
            SuspendLayout();
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + extraHeight);
            foreach (var entry in originalBounds)
            {
                Rectangle bounds = entry.Value;
                if (entry.Key != servicesLayoutPanel && bounds.Top >= servicesBounds.Bottom) bounds.Offset(0, extraHeight);
                entry.Key.Bounds = bounds;
            }
            servicesLayoutPanel.RowCount = 4; servicesLayoutPanel.Height = servicesBounds.Height + extraHeight;
            servicesLayoutPanel.RowStyles.Clear();
            for (int row = 0; row < 4; row++) servicesLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            AddServiceButton("SERVICE REQUEST STATUS\r\nTrack session requests", Color.FromArgb(24, 68, 88),
                delegate { OpenDialog(new ServiceRequestStatusForm()); }, 0, 3);
            AutoScroll = true;
            ResumeLayout(true);
        }
    }
}
