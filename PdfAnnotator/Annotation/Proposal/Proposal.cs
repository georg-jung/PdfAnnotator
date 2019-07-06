using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Annotation.Proposal
{
    internal class Proposal : IProposal
    {
        public IAnnotation Annotation { get; set; }
        public string Description { get; set; }
    }
}
