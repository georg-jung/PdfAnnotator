using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator.Utils
{
    internal static class Hashes
    {
        public static string GetFileContentSha1(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var sha = new SHA256Managed();
                var hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            }
        }
    }
}
