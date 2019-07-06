using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PdfAnnotator.Pdf;

namespace PdfAnnotator.Words
{
    internal sealed class ParagraphAggregator : IWordAggregator
    {
        private static readonly Lazy<ParagraphAggregator> SingletonInstance =
            new Lazy<ParagraphAggregator>(() => new ParagraphAggregator());

        public static ParagraphAggregator Instance => SingletonInstance.Value;

        private ParagraphAggregator()
        {
        }

        private static readonly Regex ValidChain = new Regex(@"^§?§(\s|Nummer|Nr\.|Absatz|Abs\.|bis|und|\p{N}|\.|\,)*$");
        public static readonly Regex ValidParagraphExpression = new Regex(@"^§?§(\s*(Nummer|Nr\.|Absatz|Abs\.|bis|und|\p{N}|\.|\,))*\s*(\p{N}|\.|\,)+$");

        public IEnumerable<Pdf.IWord> Aggregate(IEnumerable<Pdf.IWord> words)
        {
            Pdf.IWord oldCandidate = null;
            Pdf.IWord paragraphCandidate = null;
            foreach (var word in words)
            {
                if (word.Text.StartsWith("§"))
                {
                    paragraphCandidate = word;
                }
                else if (paragraphCandidate != null)
                {
                    var candidate = $"{paragraphCandidate.Text} {word.Text}";
                    if (EqualsVaguely(paragraphCandidate.YMin, word.YMin) && EqualsVaguely(paragraphCandidate.YMax, word.YMax) && ValidChain.IsMatch(candidate))
                    {
                        oldCandidate = paragraphCandidate;
                        paragraphCandidate = new AggregatedWord
                        {
                            Parent = word.Parent,
                            Text = candidate,
                            XMax = Math.Max(word.XMax, paragraphCandidate.XMax),
                            YMax = Math.Max(word.YMax, paragraphCandidate.YMax),
                            XMin = Math.Min(word.XMin, paragraphCandidate.XMin),
                            YMin = Math.Min(word.YMin, paragraphCandidate.YMin)
                        };
                    }
                    else
                    {
                        if (ValidParagraphExpression.IsMatch(paragraphCandidate.Text))
                            yield return paragraphCandidate;
                        else if (ValidParagraphExpression.IsMatch(oldCandidate?.Text ?? ""))
                            yield return oldCandidate;
                        paragraphCandidate = null;
                    }
                }
                yield return word;
            }
        }

        private static bool EqualsVaguely(float val1, float val2, float maxDelta = 1)
        {
            return Math.Abs(val1 - val2) <= maxDelta;
        }
    }
}

