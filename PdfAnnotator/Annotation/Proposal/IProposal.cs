using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Annotation.Proposal
{
    internal interface IProposal
    {
        IAnnotation Annotation { get; }
        string Description { get; }
    }
}
