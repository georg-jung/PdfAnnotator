using System.Threading.Tasks;

namespace PdfAnnotator.Pdf
{
    internal interface IAnalyzer<TAnalysis, TPage, TWord> where TAnalysis : IAnalysis<TPage, TWord> where TPage : IPage<TWord> where TWord : IWord
    {
        Task<IAnalysis<TPage, TWord>> AnalyzeAsync(PdfFile document);
    }
}
