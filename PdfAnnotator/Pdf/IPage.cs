using System.Collections.Generic;

namespace PdfAnnotator.Pdf
{
    internal interface IPage
    {
        IReadOnlyList<IWord> Words { get; }
    }
}
