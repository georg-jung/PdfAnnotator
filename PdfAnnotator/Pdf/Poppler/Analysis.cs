using System.Collections.Generic;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Analysis : IAnalysis
    {
        public Analysis(IReadOnlyList<Page> pages)
        {
            Pages = pages;
        }

        public PdfFile Document { get; set; }
        public IReadOnlyList<Page> Pages { get; }

        IReadOnlyList<IPage> IAnalysis.Pages => Pages;
    }
}
