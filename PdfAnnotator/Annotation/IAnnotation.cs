using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfAnnotator.Words;

namespace PdfAnnotator.Annotation
{
    internal interface IAnnotation
    {
        IWord Subject { get; }
        string Content { get; }
        IReadOnlyList<Pdf.IWord> SelectedTargets { get; }
    }
}
