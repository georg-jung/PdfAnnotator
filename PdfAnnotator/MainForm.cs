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
using PdfAnnotator.Pdf;
using PdfAnnotator.Pdf.Poppler;
using PdfAnnotator.Persistence;
using PdfAnnotator.Persistence.Model;
using PdfAnnotator.Utils;
using PdfAnnotator.Words;
using IWord = PdfAnnotator.Words.IWord;

namespace PdfAnnotator
{
    public partial class MainForm : Form
    {
        private IReadOnlyList<IWord> _words;
        private Dictionary<IWord, Annotation.Annotation> _annotations;
        private PdfFile _openFile;
        private bool _unsaved = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void openPdfMenuItem_Click(object sender, EventArgs e)
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

                IReadOnlyList<IWord> words = null;
                prgForm.ShowWhile(async () =>
                {
                    var md5Task = Task.Run(() => _openFile.ComputeMd5());
                    prgForm.Report("Extracting words...");
                    var analyzePageProgress = new Progress<int>(pg =>
                    {
                        if (pg % 25 == 0) prgForm.Report($"Page {pg} loaded.");
                    });

                    var analyzer = new Analyzer();
                    var analysis = await analyzer.AnalyzeAsync(_openFile.Path, analyzePageProgress).ConfigureAwait(true);
                    await md5Task.ConfigureAwait(true);

                    prgForm.Report("Document loaded. Analyzing words...");
                    var we = new WordExtractor();
                    words = await we.ExtractAsync(analysis).ConfigureAwait(true);
                }, this);
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

                _words = words;
                _annotations = new Dictionary<IWord, Annotation.Annotation>();
                annotationsListView.Items.Clear();
            }

            var saved = Annotations.GetAnnotations(_openFile.Md5);
            if (saved == null)
            {
                var found = Annotations.GetAnnotationsByPath(_openFile.Path);
                if (found.Item1 == null || found.Item2 == null)
                    return;
                var oldPdf = found.Item1;
                if (MessageBox.Show(
                        $@"There are no saved annotations for the file you opened, but for another file which existed at the same path. 
Possibly you updated the file's contents. Do you want to load the saved annotations corresponding to the old file, which was last seen {oldPdf.LastSeen}?",
                        "File mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                saved = found.Item2;
            }

            var added = 0;
            foreach (var a in saved)
            {
                var wrd = _words.FirstOrDefault(w => w.Text == a.Word);
                if (wrd == null) continue;
                AddAnnotation(wrd, a.Content);
                added++;
            }

            if (added != saved.Count)
            {
                MessageBox.Show(
                    $"{added} of {saved.Count} saved annotations had corresponding words in this PDF document and were loaded.",
                    "Not all annotations loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshAnnotationsList()
        {
            annotationsListView.BeginUpdate();
            annotationsListView.Items.Clear();
            foreach (var annot in _annotations.Values)
            {
                var lvi = new ListViewItem { Text = annot.Subject.Text, Tag = annot };
                lvi.SubItems.Add(annot.Content);
                annotationsListView.Items.Add(lvi);
            }
            annotationsListView.EndUpdate();
        }

        private void createAnnotationButton_Click(object sender, EventArgs e)
        {
            CreateAnnotationForFocusedWord();
        }

        private void CreateAnnotationForFocusedWord(bool silent = false)
        {
            var focused = wordsView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is IWord word))
            {
                if (!silent)
                    MessageBox.Show("Please select a word first.", "No word selected", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            Annotation.Annotation annot;
            if (_annotations.TryGetValue(word, out annot))
            {
                if (EditAnnotation(annot) != DialogResult.OK) return;
                _unsaved = true;
                RefreshAnnotationsList();
                return;
            }

            annot = new Annotation.Annotation(word);

            if (EditAnnotation(annot) != DialogResult.OK) return;
            _unsaved = true;
            AddAnnotation(word, annot);
            SaveToDb();
        }

        private void SaveToDb()
        {
            var toSave = _annotations.Values.Select(a => new WordAnnotation() { Content = a.Content, Word = a.Subject.Text });
            Annotations.SaveAnnotations(_openFile, toSave);
            _unsaved = false;
        }

        private Annotation.Annotation AddAnnotation(IWord word, string content)
        {
            var ann = new Annotation.Annotation(word) { Content = content };
            AddAnnotation(word, ann);
            return ann;
        }

        private void AddAnnotation(IWord word, Annotation.Annotation value)
        {
            _annotations.Add(word, value);

            var lvi = new ListViewItem { Text = word.Text, Tag = value };
            lvi.SubItems.Add(value.Content);
            annotationsListView.Items.Add(lvi);
        }

        private void EditFocusedAnnotation(bool silent = false)
        {
            var focused = annotationsListView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is Annotation.Annotation annot))
            {
                if (!silent)
                    MessageBox.Show("Please select an annotation first.", "No annotation selected", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            if (EditAnnotation(annot) != DialogResult.OK) return;
            _unsaved = true;
            focused.SubItems[1].Text = annot.Content;
            SaveToDb();
        }

        private void DeleteFocusedAnnotation(bool silent = false)
        {
            var focused = annotationsListView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is Annotation.Annotation annot))
            {
                if (!silent)
                    MessageBox.Show("Please select an annotation first.", "No annotation selected", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            DeleteAnnotation(annot);
            focused.Remove();
        }

        private void DeleteAnnotation(Annotation.Annotation annotation)
        {
            // each annotation will only be in the dict once
            var item = _annotations.First(kvp => kvp.Value == annotation);
            _annotations.Remove(item.Key);

            _unsaved = true;
            SaveToDb();
        }

        private void editAnnotationButton_Click(object sender, EventArgs e)
        {
            EditFocusedAnnotation();
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
                await writer.WriteAnnotatedPdfAsync(_openFile.Path, _annotations.Values, sfd.FileName).ConfigureAwait(false);

                _unsaved = false;
            }
        }

        private void wordsView_ItemActivate(object sender, EventArgs e)
        {
            CreateAnnotationForFocusedWord(true);
        }

        private void deleteAnnotationButton_Click(object sender, EventArgs e)
        {
            DeleteFocusedAnnotation();
        }

        private void annotationsListView_ItemActivate(object sender, EventArgs e)
        {
            EditFocusedAnnotation(true);
        }
    }
}
