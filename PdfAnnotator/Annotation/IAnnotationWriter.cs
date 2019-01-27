using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Annotation
{
    internal interface IAnnotationWriter
    {
        Task WriteAnnotatedPdfAsync(PdfFile original, IEnumerable<IAnnotation> annotations, string filePath);
    }
}
