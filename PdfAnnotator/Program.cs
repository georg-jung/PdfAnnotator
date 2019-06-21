using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfAnnotator
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!CheckPoppler())
            {
                Application.Exit();
                return;
            }

            Application.Run(new MainForm());
        }

        private static bool CheckPoppler()
        {
            var popplerPath = Pdf.Poppler.Analyzer.GetPdfToTextExePath();
            if (!File.Exists(popplerPath))
            {
                var dwlUrl = Pdf.Poppler.Analyzer.PopplerDownloadUrl;
                if (MessageBox.Show($"The file {popplerPath} was not found. This application needs poppler to work properly. Please download poppler and extract it at the given location. Click \"Yes\" if you want to open {dwlUrl} using your default browser.", "Missing dependency", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(dwlUrl);
                }
                return false;
            }
            return true;
        }
    }
}
