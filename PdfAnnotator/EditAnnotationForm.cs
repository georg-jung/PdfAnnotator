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
using PdfAnnotator.Annotation.Proposal;

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

        private async void EditAnnotationForm_Load(object sender, EventArgs e)
        {
            await SearchTemplates();
        }

        private async Task SearchTemplates()
        {
            templatesGroupBox.Enabled = false;
            templatesGroupBox.Text = "Templates - Searching...";
            templatesListView.Items.Clear();

            var yourContentTemplate = new Proposal();
            yourContentTemplate.Annotation = Value;
            yourContentTemplate.Description = "Your latest input (savepoint)";

            var subj = _value.Subject;

            var wikiProper = new MultiWikipediaProposer(new[] { "de", "en" });
            var existingProper = new ExistingAnnotationProposer();
            var exTask = existingProper.ProposeAsync(subj);
            var wikiTask = wikiProper.ProposeAsync(subj);

            var results = (await Task.WhenAll(exTask, wikiTask).ConfigureAwait(true)).SelectMany(lst => lst).ToList();

            if (results.Count == 0)
            {
                templatesGroupBox.Text = $"Templates - none found";
                return;
            }

            results.Add(yourContentTemplate);
            templatesListView.BeginUpdate();
            foreach (var prop in results)
            {
                var lvi = templatesListView.Items.Add(prop.Description);
                lvi.Tag = prop;
            }

            templatesListView.EndUpdate();
            templatesGroupBox.Text = $"Templates - {templatesListView.Items.Count - 1} results";
            templatesGroupBox.Enabled = true;
        }

        private void TemplatesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitInfo = templatesListView.HitTest(e.X, e.Y);
            var lvi = hitInfo.Item;

            if (lvi != null)
            {
                var prop = lvi.Tag as Proposal;
                contentTextBox.Text = prop.Annotation.Content;
            }
        }
    }
}
