using System.Collections.Generic;

namespace PdfAnnotator.Pdf
{
    internal interface IAnalysis
    {
        PdfFile Document { get; }
        IReadOnlyList<IPage> Pages { get; }
    }
}
