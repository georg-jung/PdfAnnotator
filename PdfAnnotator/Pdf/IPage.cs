using System.Collections.Generic;

namespace PdfAnnotator.Pdf
{
    internal interface IPage<out TWord> where TWord : IWord
    {
        IReadOnlyList<TWord> Words { get; }
    }
}
