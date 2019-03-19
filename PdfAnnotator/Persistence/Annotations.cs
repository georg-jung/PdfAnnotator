using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using PdfAnnotator.Pdf;
using PdfAnnotator.Persistence.Model;
using PdfAnnotator.Utils;

namespace PdfAnnotator.Persistence
{
    internal static class Annotations
    {
        private static PdfFile GetPdf(this LiteRepository repo, string contentMd5)
        {
            var md5 = contentMd5.ToLowerInvariant();
            return repo.FirstOrDefault<PdfFile>(p => p.Md5 == md5);
        }

        public static List<WordAnnotation> GetAnnotations(PdfFile document)
        {
            using (var repo = Database.GetRepository())
            {
                var pdf = repo.GetPdf(document.Md5);
                if (pdf == null) return new List<WordAnnotation>();
                return repo.Fetch<WordAnnotation>(wa => wa.Document.Id == pdf.Id);
            }
        }

        public static void EnsurePdfFile(PdfFile value)
        {
            using (var repo = Database.GetRepository())
            {
                var pdf = repo.GetPdf(value.Md5);
                if (pdf == null)
                    value.Id = (Guid)repo.Insert<PdfFile>(value);
                else
                    value.Id = pdf.Id;
            }
        }

        public static void SaveAnnotations(PdfFile document, IEnumerable<WordAnnotation> annotations)
        {
            using (var repo = Database.GetRepository())
            {
                EnsurePdfFile(document);
                repo.Delete<WordAnnotation>(wa => wa.Document.Id == document.Id);
                repo.Insert<WordAnnotation>(annotations.Select(a =>
                {
                    a.Document = document;
                    return a;
                }));
            }
        }
    }
}
