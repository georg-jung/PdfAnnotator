using System;
using System.Threading;
using System.Threading.Tasks;

namespace PdfAnnotator.Pdf
{
    internal interface IAnalyzer
    {
        Task<IAnalysis> AnalyzeAsync(string pdfPath, IProgress<int> pageProgress = null, CancellationToken ct = default);
    }
}
