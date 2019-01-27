using System.Collections.Generic;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Page : IPage
    {
        public Page(IReadOnlyList<Word> words, Analysis parent)
        {
            Words = words;
            Parent = parent;
        }

        public IReadOnlyList<Word> Words { get; }
        public Analysis Parent { get; }

        IReadOnlyList<IWord> IPage.Words => Words;

        IAnalysis IPage.Parent => Parent;
    }
}
