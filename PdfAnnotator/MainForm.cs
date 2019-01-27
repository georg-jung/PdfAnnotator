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
using PdfAnnotator.Words;
using Word = PdfAnnotator.Pdf.Poppler.Word;

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
            using (var prgForm = new ProgressForm())
            {
                ofd.Filter = "PDF files|*.pdf|All files|*";
                if (ofd.ShowDialog() != DialogResult.OK) return;
                var x = new Analyzer();
                var pdf = new PdfFile { Path = ofd.FileName };

                prgForm.Show();
                prgForm.Report("PDF analysieren...");
                var analyzePageProgress = new Progress<int>(pg =>
                {
                    if (pg % 25 == 0) prgForm.Report($"Seite {pg} geladen.");
                });
                var res = await x.AnalyzeAsync(pdf, analyzePageProgress).ConfigureAwait(true);
                var we = new WordExtractor();
                var res2 = await we.ExtractAsync(res).ConfigureAwait(true);
                prgForm.Report("PDF geladen. Analysiere Worte...");
                var ordered = res2.OrderByDescending(w => w.Appearances.Count);
                wordsView.BeginUpdate();
                wordsView.Items.Clear();
                foreach (var w in ordered)
                {
                    var lvi = new ListViewItem { Text = w.Text };
                    lvi.SubItems.Add(w.Appearances.Count.ToString());
                    wordsView.Items.Add(lvi);
                }
                wordsView.EndUpdate();
                prgForm.Close();
            }
        }
    }
}
