using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Words
{
    internal interface IWordAggregator
    {
        IEnumerable<Pdf.IWord> Aggregate(IEnumerable<Pdf.IWord> words);
    }
}
