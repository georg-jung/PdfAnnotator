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
        private readonly EditContext _context;
        private List<WordAnnotation> _annotations;
        private readonly HashSet<string> _wordsInDoc = new HashSet<string>();

        internal AllAnnotationsForm(EditContext context)
        {
            InitializeComponent();
            _context = context;
            if (context != null && context.Words != null) context.Words.ForEach(w => _wordsInDoc.Add(w.Text.ToLower()));
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
    }
}
