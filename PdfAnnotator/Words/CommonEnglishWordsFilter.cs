using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Words
{
    internal class CommonEnglishWordsFilter : IWordStringFilter
    {
        private static readonly HashSet<string> CommonWords;

        static CommonEnglishWordsFilter()
        {
            CommonWords = new HashSet<string>();
            using (var tr = new StringReader(Properties.Resources.CommonEnglishWords))
            {
                string word;
                while ((word = tr.ReadLine()) != null)
                {
                    CommonWords.Add(word);
                }
            }
        }

        public string Map(string word)
        {
            if (CommonWords.Contains(word)) return null;
            return word;
        }
    }
}
