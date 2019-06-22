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

namespace PdfAnnotator
{
    public partial class AllAnnotationsForm : Form
    {
        private readonly EditContext _context;
        private List<WordAnnotation> _annotations;

        internal AllAnnotationsForm(EditContext context)
        {
            InitializeComponent();
            _context = context;
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
            }
            annotationsListView.EndUpdate();
        }
    }
}
