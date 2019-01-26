﻿using System.Collections.Generic;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Analysis : IAnalysis<Page, Word>
    {
        public Analysis(PdfFile document, IReadOnlyList<Page> pages)
        {
            Document = document;
            Pages = pages;
        }

        public PdfFile Document { get; }
        public IReadOnlyList<Page> Pages { get; }
    }
}