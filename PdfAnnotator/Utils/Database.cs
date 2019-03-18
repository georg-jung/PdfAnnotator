using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LiteDB;
using PdfAnnotator.Pdf;
using PdfAnnotator.Pdf.Poppler;

namespace PdfAnnotator.Utils
{
    internal static class Database
    {
        // don't use expression body; DbPath should not change during one AppDomain lifetime
        private static string DbPath { get; } = Properties.Settings.Default.DBPath;
        
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
            var db = new LiteDatabase(DbPath);
            var engine = db.Engine;
            if (engine.UserVersion == 0)
            {
                var pdfs = db.GetCollection<PdfFile>();
                var analyses = db.GetCollection<Analysis>();
                pdfs.EnsureIndex(x => x.Md5, true);
                analyses.EnsureIndex(x => x.Document.Md5, true);
                engine.UserVersion = 1;
            }
            return db;
        }

        public static LiteRepository GetRepository()
        {
            return new LiteRepository(OpenDatabase(), true);
        }
    }
}
