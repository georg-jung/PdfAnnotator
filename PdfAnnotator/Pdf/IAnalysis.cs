using System.Collections.Generic;

namespace PdfAnnotator.Pdf
{
    internal interface IAnalysis<out TPage, out TWord> where TPage : IPage<TWord> where TWord : IWord
    {
        PdfFile Document { get; }
        IReadOnlyList<TPage> Pages { get; }
    }
}
