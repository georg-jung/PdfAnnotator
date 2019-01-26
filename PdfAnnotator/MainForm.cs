using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfAnnotator.Pdf.Poppler;

namespace PdfAnnotator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void openPdfMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF files|*.pdf|All files|*";
                if (ofd.ShowDialog() != DialogResult.OK) return;
                var x = new Analyzer();
                var pdf = new PdfFile {Path = ofd.FileName};
                var res = await x.AnalyzeAsync(pdf).ConfigureAwait(false);
            }
        }
    }
}
