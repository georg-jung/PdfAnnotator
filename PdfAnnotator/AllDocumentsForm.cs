using MoreLinq;
using PdfAnnotator.Pdf;
using PdfAnnotator.Persistence;
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

namespace PdfAnnotator
{
    public partial class AllDocumentsForm : Form
    {
        public bool DidChangesToAnnotationsInContext { get; private set; }
        private readonly EditContext _context;
        private List<PdfFile> _documents;
        
        internal AllDocumentsForm(EditContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void AllDocumentsForm_Load(object sender, EventArgs e)
        {
            if (_documents == null)
            {
                _documents = Annotations.GetLruPdfs(null);
            }
            RefreshDocumentList();
            showAllAnnotationsForDocumentButton.Enabled = _documents.Count >= 0;
        }

        private void RefreshDocumentList()
        {
            documentsListView.BeginUpdate();
            documentsListView.Items.Clear();
            foreach (var d in _documents)
            {
                var fileName = Path.GetFileName(d.Path);
                var item = documentsListView.Items.Add(fileName);
                item.SubItems.Add(d.LastSeen.ToString());
                item.SubItems.Add(d.Md5);
                item.Tag = d;
            }
            documentsListView.EndUpdate();
        }

        private void ShowAllAnnotationsForDocumentButton_Click(object sender, EventArgs e)
        {
            var focused = documentsListView.FocusedItem;
            if (focused?.Selected != true || !(focused.Tag is PdfFile file))
            {
                MessageBox.Show("Please select a document first.", "No document selected", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            var annotations = Annotations.GetAnnotations(file.Md5);

            if (!(annotations?.Count >= 0))
            {
                MessageBox.Show($"There are no existing annotations saved for this document.", "No Saved Annotations for Document", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            using (var frm = new AllAnnotationsForm(_context, annotations))
            {
                frm.ShowDialog();
                if (frm.DidChangesToAnnotationsInContext) DidChangesToAnnotationsInContext = true;
            }
        }
    }
}
