using System.Collections.Generic;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Page : IPage
    {
        public Page(IReadOnlyList<Word> words)
        {
            Words = words;
        }

        public IReadOnlyList<Word> Words { get; }

        IReadOnlyList<IWord> IPage.Words => Words;
    }
}
