using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfAnnotator.Annotation;
using PdfAnnotator.Pdf.Poppler;
using PdfAnnotator.Words;
using Word = PdfAnnotator.Pdf.Poppler.Word;

namespace PdfAnnotator
{
    public partial class MainForm : Form
    {
        private IReadOnlyList<IWord> _words;
        private Dictionary<IWord, IAnnotation> _annotations;
        private PdfFile _openFile;

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
                _openFile = new PdfFile { Path = ofd.FileName };

                prgForm.Show();
                prgForm.Report("Extracting words...");
                var analyzePageProgress = new Progress<int>(pg =>
                {
                    if (pg % 25 == 0) prgForm.Report($"Page {pg} loaded.");
                });

                var analysis = await x.AnalyzeAsync(_openFile, analyzePageProgress).ConfigureAwait(true);

                prgForm.Report("Document loaded. Analysing words...");
                var we = new WordExtractor();
                var words = await we.ExtractAsync(analysis).ConfigureAwait(true);
                var ordered = words.OrderByDescending(w => w.Appearances.Count);

                wordsView.BeginUpdate();
                wordsView.Items.Clear();
                foreach (var w in ordered)
                {
                    var lvi = new ListViewItem { Text = w.Text, Tag = w };
                    lvi.SubItems.Add(w.Appearances.Count.ToString());
                    wordsView.Items.Add(lvi);
                }
                wordsView.EndUpdate();
                prgForm.Close();

                _words = words;
                _annotations = new Dictionary<IWord, IAnnotation>();
            }
        }

        private void createAnnotationButton_Click(object sender, EventArgs e)
        {

        }
    }
}
