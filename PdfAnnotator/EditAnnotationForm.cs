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

namespace PdfAnnotator
{
    internal partial class EditAnnotationForm : Form
    {
        private Annotation.Annotation _value;

        public Annotation.Annotation Value
        {
            get => _value;
            set
            {
                _value = value;
                subjectWordTextBox.Text = _value.Subject.Text;
                contentTextBox.Text = _value.Content;
            }
        }

        public EditAnnotationForm()
        {
            InitializeComponent();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            _value.Content = contentTextBox.Text;
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
