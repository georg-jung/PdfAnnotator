using System.Collections.Generic;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Page : IPage<Word>
    {
        public Page(IReadOnlyList<Word> words)
        {
            Words = words;
        }

        public IReadOnlyList<Word> Words { get; }
    }
}
