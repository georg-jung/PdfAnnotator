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

namespace PdfAnnotator
{
    public partial class MainForm : Form
    {
        private IReadOnlyList<IWord> _words;
        private Dictionary<IWord, IAnnotation> _annotations;
        private PdfFile _openFile;
        private bool _unsaved = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void openPdfMenuItem_Click(object sender, EventArgs e)
        {
            if (_unsaved && _openFile != null)
            {
                if (MessageBox.Show("If you open a new file, unsaved changes will be lost. Do you want to continue?",
                        "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            }

            using (var ofd = new OpenFileDialog())
            using (var prgForm = new ProgressForm())
            {
                ofd.Filter = "PDF files|*.pdf|All files|*";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                _openFile = new PdfFile { Path = ofd.FileName };

                prgForm.Show();
                prgForm.Report("Extracting words...");
                var analyzePageProgress = new Progress<int>(pg =>
                {
                    if (pg % 25 == 0) prgForm.Report($"Page {pg} loaded.");
                });

                var analyzer = new Analyzer();
                var analysis = await analyzer.AnalyzeAsync(_openFile, analyzePageProgress).ConfigureAwait(true);

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
                annotationsListView.Items.Clear();
            }
        }

        private void createAnnotationButton_Click(object sender, EventArgs e)
        {
            var focused = wordsView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is IWord word))
            {
                MessageBox.Show("Please select a word first.", "No word selected", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (_annotations.ContainsKey(word))
            {
                MessageBox.Show("There already is an annotation for this word.", "Annotation exists",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            var annot = new Annotation.Annotation(word);

            if (EditAnnotation(annot) != DialogResult.OK) return;
            _unsaved = true;
            _annotations.Add(word, annot);

            var lvi = new ListViewItem { Text = word.Text, Tag = annot };
            lvi.SubItems.Add(annot.Content);
            annotationsListView.Items.Add(lvi);
        }

        private void editAnnotationButton_Click(object sender, EventArgs e)
        {
            var focused = annotationsListView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is Annotation.Annotation annot))
            {
                MessageBox.Show("Please select an annotation first.", "No annotation selected", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (EditAnnotation(annot) != DialogResult.OK) return;
            _unsaved = true;
            focused.SubItems[1].Text = annot.Content;
        }

        private static DialogResult EditAnnotation(Annotation.Annotation value)
        {
            using (var editFrm = new EditAnnotationForm())
            {
                editFrm.Value = value;
                return editFrm.ShowDialog();
            }
        }

        private async void createPdfMenuItem_Click(object sender, EventArgs e)
        {
            if (_openFile == null)
            {
                MessageBox.Show("You didn't add any annotation. Please start working on a document before saving.",
                        "Nothing changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF files|*.pdf|All files|*";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                var writer = new TextSharpAnnotationWriter();
                await writer.WriteAnnotatedPdfAsync(_openFile, _annotations.Values, sfd.FileName).ConfigureAwait(false);

                _unsaved = false;
            }
        }
    }
}
