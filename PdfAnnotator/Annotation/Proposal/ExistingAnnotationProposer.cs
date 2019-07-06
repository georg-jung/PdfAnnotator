using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfAnnotator.Words;

namespace PdfAnnotator.Annotation.Proposal
{
    internal class ExistingAnnotationProposer : IProposer
    {
        public Task<IReadOnlyList<IProposal>> ProposeAsync(IWord word)
        {
            var proposals = new List<IProposal>();
            var anns = Persistence.Annotations.GetAnnotationsByWord(word.Text);
            anns = anns.OrderByDescending(ann => ann.Document.LastSeen).ToList();
            foreach (var saved in anns)
            {
                var prop = new Proposal();
                var annotation = new Annotation(word);
                annotation.Content = saved.Content;
                prop.Annotation = annotation;
                prop.Description = System.IO.Path.GetFileName(saved.Document.Path);
                proposals.Add(prop);
            }

            return Task.FromResult<IReadOnlyList<IProposal>>(proposals);
        }
    }
}
