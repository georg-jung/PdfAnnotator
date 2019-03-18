using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfAnnotator.Utils;

namespace PdfAnnotator
{
    internal class PdfFile : IPdfFile
    {
        public string Path { get; set; }
        public string Md5 { get; set; }

        public void ComputeMd5()
        {
            Md5 = Hashes.GetFileContentSha1(Path);
        }
    }
}
