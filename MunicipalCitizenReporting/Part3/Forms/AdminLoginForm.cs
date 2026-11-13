using System.Drawing;
using System.Windows.Forms;

namespace MunicipalCitizenReporting.Part3.Forms
{
    internal sealed class AdminLoginForm : Form
    {
        internal AdminLoginForm()
        {
            Text = "Admin Sign-In";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(380, 230);
            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Segoe UI", 10);

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, Padding = new Padding(12)
            };
            var username = new TextBox { Width = 310, AccessibleName = "Username" };
            var password = new TextBox { Width = 310, AccessibleName = "Password", UseSystemPasswordChar = true };
            var message = new Label { AutoSize = true, ForeColor = Color.DarkRed };
            var signIn = new Button { Text = "Sign in", AutoSize = true };
            var cancel = new Button { Text = "Cancel", AutoSize = true, DialogResult = DialogResult.Cancel };
            signIn.Click += (sender, args) =>
            {
                if (username.Text.Trim() == "admin" && password.Text == "DemoAdmin2026")
                {
                    DialogResult = DialogResult.OK;
                    return;
                }
                message.Text = "Incorrect username or password.";
                password.Clear();
                password.Focus();
            };
            var actions = new FlowLayoutPanel { AutoSize = true };
            actions.Controls.Add(signIn);
            actions.Controls.Add(cancel);
            layout.Controls.Add(new Label { Text = "Local demo sign-in", AutoSize = true });
            layout.Controls.Add(new Label { Text = "Username", AutoSize = true });
            layout.Controls.Add(username);
            layout.Controls.Add(new Label { Text = "Password", AutoSize = true });
            layout.Controls.Add(password);
            layout.Controls.Add(message);
            layout.Controls.Add(actions);
            AcceptButton = signIn;
            CancelButton = cancel;
            Controls.Add(layout);
        }
    }
}
