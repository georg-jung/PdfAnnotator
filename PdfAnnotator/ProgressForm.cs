using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfAnnotator
{
    public partial class ProgressForm : Form
    {
        private Func<Task> _showWhile = null;

        public ProgressForm()
        {
            InitializeComponent();
        }

        public void Report(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Report(status)));
                return;
            }
            statusTextBox.AppendText(status + Environment.NewLine);
            statusTextBox.Select(statusTextBox.TextLength, 0);
            statusTextBox.ScrollToCaret();
        }

        public void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Clear));
                return;
            }
            statusTextBox.Clear();
            statusTextBox.ClearUndo();
        }

        public DialogResult ShowWhile(Func<Task> task, IWin32Window owner)
        {
            _showWhile = task;
            return ShowDialog(owner);
        }

        public DialogResult ShowWhile(string message, Func<Task> task, IWin32Window owner)
        {
            _showWhile = task;
            Report(message);
            return ShowDialog(owner);
        }

        private async void ProgressForm_Shown(object sender, EventArgs e)
        {
            if (_showWhile == null) return;
            await _showWhile().ConfigureAwait(true);
            _showWhile = null;
            Close();
        }
    }
}
