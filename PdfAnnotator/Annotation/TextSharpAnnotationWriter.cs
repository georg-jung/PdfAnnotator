using System.Collections.Generic;
using System.Threading.Tasks;

namespace PdfAnnotator.Annotation
{
    internal class TextSharpAnnotationWriter : IAnnotationWriter
    {
        public Task WriteAnnotatedPdfAsync(PdfFile original, IEnumerable<IAnnotation> annotations, string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}
