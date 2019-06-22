using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private EditContext _ctx;
        private bool _unsaved = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenPdf(string path)
        {
            using (var prgForm = new ProgressForm())
            {
                _ctx = new EditContext(new PdfFile { Path = path });
                
                IReadOnlyList<IWord> words = null;
                prgForm.ShowWhile(async () =>
                {
                    var md5Task = Task.Run(() => _ctx.OpenFile.ComputeMd5());
                    prgForm.Report("Extracting words...");
                    var analyzePageProgress = new Progress<int>(pg =>
                    {
                        if (pg % 25 == 0) prgForm.Report($"Page {pg} loaded.");
                    });

                    var analyzer = new Analyzer();
                    var analysis = await analyzer.AnalyzeAsync(_ctx.OpenFile.Path, analyzePageProgress).ConfigureAwait(true);
                    await md5Task.ConfigureAwait(true);

                    prgForm.Report("Document loaded. Analyzing words...");
                    var we = new WordExtractor();
                    words = await we.ExtractAsync(analysis).ConfigureAwait(true);
                }, this);
                ListWordsInOpenDocument(words);
                _ctx.Annotations = new Dictionary<IWord, Annotation.Annotation>();
                annotationsListView.Items.Clear();
            }
            LoadSavedAnnotationsForOpenFile();
        }

        private void ListWordsInOpenDocument(IReadOnlyList<IWord> words)
        {
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

            _ctx.Words = words;
        }

        private void LoadSavedAnnotationsForOpenFile()
        {
            var saved = Annotations.GetAnnotations(_ctx.OpenFile.Md5);
            if (saved == null)
            {
                var found = Annotations.GetAnnotationsByPath(_ctx.OpenFile.Path);
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
                var wrd = _ctx.Words.FirstOrDefault(w => w.Text == a.Word);
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

        private bool ShouldOpenFile()
        {
            if (_unsaved && _ctx != null)
            {
                if (MessageBox.Show("If you open a new file, unsaved changes will be lost. Do you want to continue?",
                        "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return false;
            }

            return true;
        }

        private void openPdfMenuItem_Click(object sender, EventArgs e)
        {
            if (!ShouldOpenFile()) return;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF files|*.pdf|All files|*";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                OpenPdf(ofd.FileName);
            }
        }

        private void RefreshAnnotationsList()
        {
            annotationsListView.BeginUpdate();
            annotationsListView.Items.Clear();
            foreach (var annot in _ctx.Annotations.Values)
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
            if (_ctx.Annotations.TryGetValue(word, out annot))
            {
                if (EditAnnotation(annot) != DialogResult.OK) return;
                _unsaved = true;
                RefreshAnnotationsList();
                SaveToDb();
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
            var toSave = _ctx.Annotations.Values.Select(a => new WordAnnotation() { Content = a.Content, Word = a.Subject.Text });
            Annotations.SaveAnnotations(_ctx.OpenFile, toSave);
            _unsaved = false;
            LoadLruList();
        }

        private Annotation.Annotation AddAnnotation(IWord word, string content)
        {
            var ann = new Annotation.Annotation(word) { Content = content };
            AddAnnotation(word, ann);
            return ann;
        }

        private void AddAnnotation(IWord word, Annotation.Annotation value)
        {
            _ctx.Annotations.Add(word, value);

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
            var item = _ctx.Annotations.First(kvp => kvp.Value == annotation);
            _ctx.Annotations.Remove(item.Key);

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
            if (_ctx == null)
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
                await writer.WriteAnnotatedPdfAsync(_ctx.OpenFile.Path, _ctx.Annotations.Values, sfd.FileName).ConfigureAwait(false);

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

        private void LoadLruList()
        {
            var pdfs = Annotations.GetLruPdfs();
            openLruPdfMenuItem.DropDownItems.Clear();
            foreach (var pdf in pdfs)
            {
                var fileName = Path.GetFileName(pdf.Path);
                var txt = $"{fileName} {pdf.LastSeen}";
                var tsi = openLruPdfMenuItem.DropDownItems.Add(txt);
                tsi.ToolTipText = pdf.Path;
                tsi.AutoToolTip = true;
                tsi.Click += (s, e) => LoadPdf(pdf.Path);
            }

            openLruPdfMenuItem.Enabled = pdfs.Count > 0;
        }

        private void LoadPdf(string path)
        {
            if (!ShouldOpenFile()) return;
            OpenPdf(path);
        }
        
        private void MainForm_Shown(object sender, EventArgs e)
        {
            LoadLruList();
        }

        private void allAnnotationsMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new AllAnnotationsForm(_ctx))
            {
                frm.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            Text = string.Format(Text, fileVersion);
        }
    }
}
