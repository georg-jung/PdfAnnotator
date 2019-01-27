using System.Collections.Generic;

namespace PdfAnnotator.Pdf
{
    internal interface IPage
    {
        IReadOnlyList<IWord> Words { get; }
        IAnalysis Parent { get; }
        int Index { get; }
    }
}
