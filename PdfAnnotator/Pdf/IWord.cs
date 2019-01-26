using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Pdf
{
    internal interface IWord
    {
        string Text { get; }
        float XMin { get; }
        float YMin { get; }
        float XMax { get; }
        float YMax { get; }
    }
}
