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

        private static List<PdfFile> GetPdfsByPath(this LiteRepository repo, string path)
        {
            var lPath = path.ToLowerInvariant();
            // attention: this just works because there exists an LOWER(Path) index, as otherwise it would compare SoMeString against somestring
            // if one would add a lower function here to the left the index would not be used
            return repo.Fetch<PdfFile>(p => p.Path == lPath);
        }

        private static List<PdfFile> GetPdfs(this LiteRepository repo)
        {
            return repo.Query<PdfFile>().ToList();
        }

        public static List<WordAnnotation> GetAnnotations(bool includeDocuments, int? maxCount = null)
        {
            using (var repo = Database.GetRepository())
            {
                var q = repo.Query<WordAnnotation>();
                if (includeDocuments) q = q.Include(wa => wa.Document);
                if (maxCount.HasValue) q = q.Limit(maxCount.Value);
                return q.ToList();
            }
        }

        private static List<WordAnnotation> GetAnnotations(this LiteRepository repo, string contentMd5)
        {
            var pdf = repo.GetPdf(contentMd5);
            if (pdf == null) return null;
            return repo.Fetch<WordAnnotation>(wa => wa.Document.Id == pdf.Id);
        }

        public static List<WordAnnotation> GetAnnotations(string contentMd5)
        {
            using (var repo = Database.GetRepository())
            {
                return repo.GetAnnotations(contentMd5);
            }
        }

        public static (PdfFile, List<WordAnnotation>) GetAnnotationsByPath(string path)
        {
            using (var repo = Database.GetRepository())
            {
                var pdfs = repo.GetPdfsByPath(path);
                if (pdfs == null || !pdfs.Any()) return (null, null);
                var newest = pdfs.OrderByDescending(p => p.LastSeen).First();
                return (newest, repo.Fetch<WordAnnotation>(wa => wa.Document.Id == newest.Id));
            }
        }

        public static List<WordAnnotation> GetAnnotationsByWord(string word)
        {
            return GetAnnotationsByWords(word.Yield()).First().Item2;
        }

        public static IEnumerable<(string, List<WordAnnotation>)> GetAnnotationsByWords(IEnumerable<string> words)
        {
            using (var repo = Database.GetRepository())
            {
                foreach (var word in words)
                {
                    yield return (word, repo.Query<WordAnnotation>().Where(wa => wa.Word == word).Include(wa => wa.Document).ToList());
                }
            }
        }

        private static void EnsurePdfFile(this LiteRepository repo, PdfFile value, bool updateLastSeen = true)
        {
            var pdf = repo.GetPdf(value.Md5);
            if (pdf == null)
                value.Id = (Guid)repo.Insert<PdfFile>(value);
            else
                value.Id = pdf.Id;
            if (updateLastSeen)
                value.LastSeen = DateTime.Now;
            repo.Update(value);
        }

        public static void EnsurePdfFile(PdfFile value)
        {
            using (var repo = Database.GetRepository())
            {
                EnsurePdfFile(repo, value);
            }
        }

        private static void SaveAnnotations(this LiteRepository repo, PdfFile document, IEnumerable<WordAnnotation> annotations, bool updatePdfLastSeen = true)
        {
            repo.EnsurePdfFile(document, updatePdfLastSeen);
            repo.Delete<WordAnnotation>(wa => wa.Document.Id == document.Id);
            repo.Insert<WordAnnotation>(annotations.Select(a =>
            {
                a.Document = document;
                return a;
            }));
        }

        public static void SaveAnnotations(PdfFile document, IEnumerable<WordAnnotation> annotations)
        {
            using (var repo = Database.GetRepository())
            {
                repo.SaveAnnotations(document, annotations);
            }
        }

        public static List<PdfFile> GetLruPdfs(int? maxCount = 10)
        {
            using (var repo = Database.GetRepository())
            {
                var q = repo.Query<PdfFile>().Where(Query.All(nameof(PdfFile.LastSeen), Query.Descending));
                if (maxCount.HasValue) q = q.Limit(maxCount.Value);
                return q.ToList();
            }
        }

        public static (int pdfCount, int annotationCount, int mergeCandidatesCount) ImportUnseenPdfs(string sourcePath)
        {
            int pdfCnt = 0;
            int annotCnt = 0;
            int mergeCandidatePdfCnt = 0;
            using (var repo = Database.GetRepository())
            using (var toImport = Database.GetReadOnlyRepository(sourcePath))
            {
                foreach (var pdf in toImport.GetPdfs())
                {
                    if (repo.GetPdf(pdf.Md5) != null)
                    {
                        ++mergeCandidatePdfCnt;
                        continue;
                    }
                    var annots = toImport.GetAnnotations(pdf.Md5);
                    repo.SaveAnnotations(pdf, annots, false);
                    annotCnt += annots.Count;
                    ++pdfCnt;
                }
            }
            return (pdfCnt, annotCnt, mergeCandidatePdfCnt);
        }

        public static (int pdfCount, int annotationCount) MergeSeenPdfs(string sourcePath)
        {
            int pdfCnt = 0;
            int annotCnt = 0;
            using (var repo = Database.GetRepository())
            using (var toImport = Database.GetReadOnlyRepository(sourcePath))
            {
                foreach (var pdf in toImport.GetPdfs())
                {
                    if (repo.GetPdf(pdf.Md5) == null) continue;
                    var importAnnots = toImport.GetAnnotations(pdf.Md5);
                    var existingAnnots = repo.GetAnnotations(pdf.Md5);
                    // https://stackoverflow.com/questions/6331193/add-items-to-a-collection-if-the-collection-does-not-already-contain-it-by-compa
                    // ToList just for count below
                    var newOnes = importAnnots.Where(ia => !existingAnnots.Any(ea => ea.Word.Equals(ia.Word, StringComparison.InvariantCultureIgnoreCase))).ToList();
                    existingAnnots.AddRange(newOnes);
                    repo.SaveAnnotations(pdf, existingAnnots, false);
                    annotCnt += newOnes.Count;
                    ++pdfCnt;
                }
            }
            return (pdfCnt, annotCnt);
        }
    }
}
