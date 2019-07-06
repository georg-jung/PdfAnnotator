using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PdfAnnotator.Words;

namespace PdfAnnotator.Annotation.Proposal
{
    internal class WikipediaProposer : IProposer
    {
        public string LanguageCode { get; }
        private readonly string _apiUrlBase;

        public WikipediaProposer(string languageCode)
        {
            LanguageCode = languageCode;
            _apiUrlBase =
                $@"https://{languageCode}.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles=";
        }

        private async Task<string> GetJsonAsync(string pageTitle)
        {
            // dont just return task as WebClient would be disposed to early
            // https://stackoverflow.com/a/19103343/1200847
            using (var wc = new System.Net.WebClient())
                return await wc.DownloadStringTaskAsync(_apiUrlBase + WebUtility.UrlEncode(pageTitle)).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<IAnnotation>> ProposeAsync(IWord word)
        {
            var query = word.Text;
            var json = await GetJsonAsync(query).ConfigureAwait(false);
            var res = JsonConvert.DeserializeObject<WikiApiResult>(json);
            var proposals = new List<IAnnotation>();
            
            foreach (var pgEntry in res.query.pages)
            {
                if (!(Int32.TryParse(pgEntry.Key, out int pgid) && pgid > 0)) continue;
                var pg = pgEntry.Value;
                if (!string.IsNullOrWhiteSpace(pg.extract))
                {
                    var ann = new Annotation(word);
                    ann.Content = pg.extract;
                    proposals.Add(ann);
                }
            }

            return proposals;
        }

        private class WikiApiResult
        {
            public string batchcomplete { get; set; }
            public WikiApiQuery query { get; set; }

            public class WikiApiQuery
            {
                public Dictionary<string, WikiPage> pages { get; set; }
            }

            public class WikiPage
            {
                public int pageid { get; set; }
                public string title { get; set; }
                public string extract { get; set; }
            }
        }
    }
}
