using System.Collections.Generic;
using PdfAnnotator.Words;

namespace PdfAnnotator.Annotation
{
    internal class Annotation : IAnnotation
    {
        public Annotation(IWord subject)
        {
            Subject = subject;
        }

        public IWord Subject { get; }
        public string Content { get; set; }
        public List<Pdf.IWord> SelectedTargets { get; set; }

        IReadOnlyList<Pdf.IWord> IAnnotation.SelectedTargets => SelectedTargets;
    }
}