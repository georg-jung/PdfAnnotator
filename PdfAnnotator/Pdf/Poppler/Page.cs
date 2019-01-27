using System.Collections.Generic;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Page : IPage
    {
        public Page(IReadOnlyList<Word> words, Analysis parent, int index)
        {
            Words = words;
            Parent = parent;
            Index = index;
        }

        public IReadOnlyList<Word> Words { get; }
        public Analysis Parent { get; }
        public int Index { get; }
        public float Width { get; set; }
        public float Height { get; set; }

        IReadOnlyList<IWord> IPage.Words => Words;

        IAnalysis IPage.Parent => Parent;
    }
}
