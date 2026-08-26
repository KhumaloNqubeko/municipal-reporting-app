using System;
using System.Windows.Forms;
using MunicipalCitizenReporting.Forms;

namespace MunicipalCitizenReporting
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenuForm());
        }
    }
}
