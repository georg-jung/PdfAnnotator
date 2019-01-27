using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfAnnotator.Words
{
    internal class JustLettersFilter : IWordStringFilter
    {
        private static readonly Regex NonLetterRegex = new Regex("\\P{L}");

        public string Map(string word)
        {
            var mapped = NonLetterRegex.Replace(word, "").ToLowerInvariant();
            return string.IsNullOrWhiteSpace(mapped) ? null : mapped;
        }
    }
}
