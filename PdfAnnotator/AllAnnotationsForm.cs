using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfAnnotator.Persistence;
using PdfAnnotator.Persistence.Model;
using MoreLinq;

namespace PdfAnnotator
{
    public partial class AllAnnotationsForm : Form
    {
        public bool DidChangesToAnnotationsInContext { get; private set; }
        private readonly EditContext _context;
        private List<WordAnnotation> _annotations;
        private readonly HashSet<string> _wordsInDoc = new HashSet<string>();

        internal AllAnnotationsForm(EditContext context)
        {
            InitializeComponent();
            _context = context;
            var wordsInContext = context != null && context.Words != null;
            if (wordsInContext) context.Words.ForEach(w => _wordsInDoc.Add(w.Text.ToLower()));
            takeAnnotationButton.Enabled = wordsInContext;
        }

        private void AllAnnotationsForm_Load(object sender, EventArgs e)
        {
            _annotations = Annotations.GetAnnotations(true);
            RefreshAnnotationList();
        }

        private void RefreshAnnotationList()
        {
            annotationsListView.BeginUpdate();
            annotationsListView.Items.Clear();
            foreach (var a in _annotations)
            {
                var item = annotationsListView.Items.Add(a.Word);
                item.SubItems.Add(a.Content);
                var fileName = Path.GetFileName(a.Document.Path);
                item.SubItems.Add(fileName);
                item.Tag = a;
                if (_wordsInDoc.Contains(a.Word.ToLower()))
                {
                    item.BackColor = Color.LightGreen;
                    item.SubItems.Add("Yes");
                }
                else
                {
                    item.SubItems.Add("No");
                }
            }
            annotationsListView.EndUpdate();
        }

        private void takeAnnotationButton_Click(object sender, EventArgs e)
        {
            if (_context == null) return;
            var focused = annotationsListView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is WordAnnotation annotation))
            {
                MessageBox.Show("Please select an annotation word first.", "No annotation selected", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            var wrd = _context.Words.FirstOrDefault(x => x.Text.ToLower() == annotation.Word.ToLower());
            if (wrd == null)
            {
                MessageBox.Show($"The word {annotation.Word} is not part of the current document.", "Word not part of document", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (_context.Annotations.TryGetValue(wrd, out var ann))
            {
                if (annotation.Content == ann.Content) return;
                if (MessageBox.Show($"The word {wrd.Text} is already annotated: \n\n{ann.Content}\n\nDo you want to overwrite the existing annotation?", "Overwrite existing annotation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No) return;
                ann.Content = annotation.Content;
            }
            else
            {
                ann = new Annotation.Annotation(wrd) { Content = annotation.Content };
                _context.Annotations.Add(wrd, ann);
            }
            _context.SaveToDb();
            DidChangesToAnnotationsInContext = true;
        }

        private void CopyAnnotationContentButton_Click(object sender, EventArgs e)
        {
            if (_context == null) return;
            var focused = annotationsListView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is WordAnnotation annotation))
            {
                MessageBox.Show("Please select an annotation word first.", "No annotation selected", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            System.Windows.Forms.Clipboard.SetText(annotation.Content);
        }
    }
}
