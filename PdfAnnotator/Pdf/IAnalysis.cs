using System.Collections.Generic;

namespace PdfAnnotator.Pdf
{
    internal interface IAnalysis
    {
        IReadOnlyList<IPage> Pages { get; }
    }
}
