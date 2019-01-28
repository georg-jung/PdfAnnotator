using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Pdf
{
    internal class AggregatedWord : IWord
    {
        public string Text { get; set; }
        public float XMin { get; set; }
        public float YMin { get; set; }
        public float XMax { get; set; }
        public float YMax { get; set; }
        public IPage Parent { get; set; }

        IPage IWord.Parent => Parent;
    }
}
