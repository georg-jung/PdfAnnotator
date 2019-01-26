using System.Threading.Tasks;

namespace PdfAnnotator.Pdf
{
    internal interface IAnalyzer
    {
        Task<IAnalysis> AnalyzeAsync(PdfFile document);
    }
}
