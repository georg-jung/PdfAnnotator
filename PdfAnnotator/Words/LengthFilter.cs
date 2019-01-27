using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Words
{
    internal class LengthFilter : IWordStringFilter
    {
        public string Map(string word)
        {
            if (word?.Length <= 1) return null;
            return word;
        }
    }
}
