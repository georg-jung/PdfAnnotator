using System;
using LiteDB;
using PdfAnnotator.Utils;

namespace PdfAnnotator.Pdf
{
    internal class PdfFile : IPdfFile
    {
        [BsonId(true)]
        public Guid Id { get; set; }

        public string Path { get; set; }
        public string Md5 { get; set; }
        public DateTime LastSeen { get; set; }

        public void ComputeMd5()
        {
            Md5 = Hashes.GetFileContentSha1(Path);
        }
    }
}
