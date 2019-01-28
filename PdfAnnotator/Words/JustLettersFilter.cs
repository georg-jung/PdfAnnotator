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

        public List<Regex> Whitelist { get; set; }

        public string Map(string word)
        {
            if (Whitelist != null)
            {
                foreach (var wlRegex in Whitelist)
                {
                    if (wlRegex.IsMatch(word)) return word.ToLowerInvariant();
                }
            }

            var mapped = NonLetterRegex.Replace(word, "").ToLowerInvariant();
            return string.IsNullOrWhiteSpace(mapped) ? null : mapped;
        }
    }
}
