using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using PdfAnnotator.Words;

namespace PdfAnnotator.Annotation.Proposal
{
    internal class MultiWikipediaProposer : IProposer
    {
        private readonly List<WikipediaProposer> _proposers;
        private readonly bool _ignoreExceptions;

        public MultiWikipediaProposer(IEnumerable<string> languageCodes, bool ignoreExceptions = true)
        {
            _proposers = new List<WikipediaProposer>();
            foreach (var l in languageCodes)
            {
                _proposers.Add(new WikipediaProposer(l));
            }
            if (_proposers.Count == 0) throw new ArgumentException("At least one language code must be given");
            _ignoreExceptions = ignoreExceptions;
        }

        public async Task<IReadOnlyList<IProposal>> ProposeAsync(IWord word)
        {
            var tasks = _proposers.Select(p => p.ProposeAsync(word));
            var proposals = new List<IProposal>();
            try
            {
                var results = await Task.WhenAll(tasks);
                results.ForEach(r => proposals.AddRange(r));
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                if (!_ignoreExceptions) throw;
            }
            return proposals;
        }
    }
}
