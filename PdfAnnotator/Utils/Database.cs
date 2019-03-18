using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LiteDB;
using PdfAnnotator.Pdf.Poppler;

namespace PdfAnnotator.Utils
{
    internal static class Database
    {
        // don't use expression body; DbPath should not change during one AppDomain lifetime
        private static string DbPath { get; } = Properties.Settings.Default.DBPath;
        private static BsonMapper Mapper { get; } = BsonMapper.Global;

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

        private static string ResolveCollectionName<T>()
        {
            return Mapper.ResolveCollectionName(typeof(T));
        }

        private static void EnsureCollection<T>(this LiteDatabase db)
        {
            db.GetCollection<T>(ResolveCollectionName<T>());
        }

        private static LiteDatabase OpenDatabase()
        {
            var db = new LiteDatabase(DbPath);
            var engine = db.Engine;
            if (engine.UserVersion == 0)
            {
                db.EnsureCollection<PdfFile>();
                db.EnsureCollection<Analysis>();
                
                // engine.UserVersion = 1;
            }
            return db;
        }

        public static LiteRepository GetRepository()
        {
            return new LiteRepository(OpenDatabase(), true);
        }
    }
}
