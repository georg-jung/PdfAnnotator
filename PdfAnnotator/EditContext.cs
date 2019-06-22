using PdfAnnotator.Pdf;
using PdfAnnotator.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWord = PdfAnnotator.Words.IWord;

namespace PdfAnnotator
{
    internal class EditContext
    {
        public EditContext(PdfFile openFile)
        {
            OpenFile = openFile ?? throw new ArgumentNullException(nameof(openFile));
        }

        public IReadOnlyList<IWord> Words { get; set; }
        public Dictionary<IWord, Annotation.Annotation> Annotations { get; set; }
        public PdfFile OpenFile { get; }
    }
}
