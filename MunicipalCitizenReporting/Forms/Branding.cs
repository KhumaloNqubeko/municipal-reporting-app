using System.Drawing;
using System.IO;
using System.Reflection;

namespace MunicipalCitizenReporting.Forms
{
    internal static class Branding
    {
        private const string LogoResourceName = "MunicipalCitizenReporting.Assets.ukhahlamba_logo.jpg";

        public static Image LoadMunicipalLogo()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(LogoResourceName))
            {
                if (stream == null) return null;
                using (Image source = Image.FromStream(stream))
                    return new Bitmap(source);
            }
        }
    }
}
