﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfAnnotator.Pdf;

namespace PdfAnnotator.Words
{
    internal class WordExtractor : IWordExtractor
    {
        private readonly JustLettersFilter _jlf = new JustLettersFilter();
        private readonly CommonEnglishWordsFilter _cewf = new CommonEnglishWordsFilter();
        private readonly CommonGermanWordsFilter _cgwf = new CommonGermanWordsFilter();
        private readonly LengthFilter _lf = new LengthFilter();

        public Task<IReadOnlyList<IWord>> ExtractAsync(IAnalysis analysis)
        {
            var words = new Dictionary<string, Word>();
            foreach (var pg in analysis.Pages)
            {
                foreach (var w in pg.Words)
                {
                    var wordString = w.Text.Normalize();
                    wordString = FilterWordString(wordString);
                    if (string.IsNullOrWhiteSpace(wordString)) continue;
                    var word = words.GetOrDefault(wordString) ?? words.AddAndReturn(wordString, new Word(wordString));
                    word.Appearances.Add(w);
                }
            }

            IReadOnlyList<IWord> ret = words.Values.ToList();
            return Task.FromResult(ret);
        }

        private string FilterWordString(string word)
        {
            var w = _jlf.Map(word);
            if (w == null) return null;
            w = _cewf.Map(w);
            if (w == null) return null;
            w = _cgwf.Map(w);
            if (w == null) return null;
            w = _lf.Map(w);
            return w;
        }
    }
}