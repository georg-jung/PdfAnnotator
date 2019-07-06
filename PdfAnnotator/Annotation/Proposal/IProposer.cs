using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfAnnotator.Words;

namespace PdfAnnotator.Annotation.Proposal
{
    internal interface IProposer
    {
        Task<IReadOnlyList<IAnnotation>> ProposeAsync(IWord word);
    }
}
