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

        public MainForm()
        {
            InitializeComponent();
        }

        private void ClearContextAndUi()
        {
            _ctx = null;
            wordsView.Items.Clear();
            annotationsListView.Items.Clear();
        }

        private void OpenPdf(string path)
        {
            ClearContextAndUi();
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
                    IAnalysis analysis;
                    try
                    {
                        analysis = await analyzer.AnalyzeAsync(_ctx.OpenFile.Path, analyzePageProgress).ConfigureAwait(true);
                        await md5Task.ConfigureAwait(true);
                    }
                    catch (Exception ex)
                    {
                        var msg = $"An error occured while opening the selected file: {ex.Message}";
                        MessageBox.Show(this, msg, "Could not open file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ClearContextAndUi();
                        return;
                    }

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
                (var oldPdf, var annotations) = Annotations.GetAnnotationsByPath(_ctx.OpenFile.Path);
                if (oldPdf == null || annotations == null)
                    return;
                if (MessageBox.Show(
                        $@"There are no saved annotations for the file you opened, but for another file which existed at the same path. 
Possibly you updated the file's contents. Do you want to load the saved annotations corresponding to the old file, which was last seen {oldPdf.LastSeen}?",
                        "File mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                saved = annotations;
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
            if (_ctx?.Unsaved == true)
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
                _ctx.Unsaved = true;
                RefreshAnnotationsList();
                SaveToDb();
                return;
            }

            annot = new Annotation.Annotation(word);

            if (EditAnnotation(annot) != DialogResult.OK) return;
            AddAnnotation(word, annot);
            _ctx.Unsaved = true;
            SaveToDb();
        }

        private void SaveToDb()
        {
            _ctx.SaveToDb();
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
            _ctx.Unsaved = true;
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

            _ctx.Unsaved = true;
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

                _ctx.Unsaved = false;
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
            var pdfs = Annotations.GetLruPdfs().OrderByDescending(pdf => pdf.LastSeen).ToList();
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
                if (frm.DidChangesToAnnotationsInContext) RefreshAnnotationsList();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            Text = string.Format(Text, fileVersion);
        }

        private void ExportDatabaseToolsMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "PdfAnnotator databases (*.pdfannotatordb)|*.pdfannotatordb";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Database.Export(sfd.FileName);
                    MessageBox.Show(this, $"A copy of your local database was successfully saved to {sfd.FileName}. You can use it as a backup, replace another users database with it using the \"Restore Database\" function or import it's contents into another database.", "Database Export Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ImportFromExistingDatabaseToolsMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "PdfAnnotator databases (*.pdfannotatordb)|*.pdfannotatordb";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ClearContextAndUi();
                    var stats = Annotations.ImportUnseenPdfs(ofd.FileName);
                    var msg = $"Success. Imported {stats.annotationCount} annotations for {stats.pdfCount} new documents, your database has never seen.";
                    if (stats.mergeCandidatesCount == 0)
                    {
                        MessageBox.Show(this, msg, "Import Data from Existing Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        msg = $"{msg} Furthermore there exist annotations for {stats.mergeCandidatesCount} documentes your database has annotations for too. Do you want to import these annotations too? If there are annotations for the same word in your database and the imported file, your annotation will be prefered.";
                        if (MessageBox.Show(this, msg, "Import Data from Existing Database", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                        var mergeStats = Annotations.MergeSeenPdfs(ofd.FileName);
                        msg = $"Success. Added {stats.annotationCount} annotations to {stats.pdfCount} documents during merge.";
                        MessageBox.Show(this, msg, "Import Data from Existing Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void RestoreDatabseToolsMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.Exists() && MessageBox.Show(this, $"This functionality replaces your existing database with another one you can choose next. This means all your existing work saved in your local database will be lost. Are you sure you want to proceed?", "Restore Database - Potential Loss of Data", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "PdfAnnotator databases (*.pdfannotatordb)|*.pdfannotatordb";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ClearContextAndUi();
                    Database.Restore(ofd.FileName);
                }
            }
        }

        private void DocumentsInDatabaseToolsMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new AllDocumentsForm(_ctx))
            {
                frm.ShowDialog();
                if (frm.DidChangesToAnnotationsInContext) RefreshAnnotationsList();
            }
        }
    }
}
