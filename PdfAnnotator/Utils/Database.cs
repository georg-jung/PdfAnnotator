using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LiteDB;
using PdfAnnotator.Pdf;
using PdfAnnotator.Pdf.Poppler;
using PdfAnnotator.Persistence.Model;

namespace PdfAnnotator.Utils
{
    internal static class Database
    {
        // don't use expression body; DbPath should not change during one AppDomain lifetime
        private static string DbPath { get; } = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DBPath);
        
        static Database()
        {
            EnsureParentDirectoryExists();
        }

        private static void EnsureParentDirectoryExists()
        {
            var folder = Path.GetDirectoryName(DbPath);
            if (string.IsNullOrWhiteSpace(folder)) return;
            Directory.CreateDirectory(folder);
        }

        
        private static LiteDatabase OpenDatabase()
        {
            var db = new LiteDatabase(DbPath, null, new Logger(Logger.FULL));
            var engine = db.Engine;
            if (engine.UserVersion == 0)
            {
                var pdf = db.GetCollection<PdfFile>();
                var annotation = db.GetCollection<WordAnnotation>();
                pdf.EnsureIndex(x => x.Md5, true);
                pdf.EnsureIndex(x => x.Path, "LOWER($.Path)");
                // todo: which index is right?
                //annotation.EnsureIndex(x => x.Document);
                annotation.EnsureIndex(x => x.Document.Id);
                annotation.EnsureIndex(x => x.Word);
                engine.UserVersion = 1;
            }
            db.Log.Logging += (s) => Debug.WriteLine(s);
            return db;
        }

        public static LiteRepository GetRepository()
        {
            return new LiteRepository(OpenDatabase(), true);
        }
    }
}
