using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfAnnotator.Pdf;

namespace PdfAnnotator.Words
{
    internal class Word : IWord
    {
        public Word(string text, List<Pdf.IWord> appearances)
        {
            Text = text;
            Appearances = appearances;
        }

        public Word(string text)
        {
            Text = text;
            Appearances = new List<Pdf.IWord>();
        }

        public string Text { get; }
        public List<Pdf.IWord> Appearances { get; }

        IReadOnlyList<Pdf.IWord> IWord.Appearances => Appearances;
    }
}
