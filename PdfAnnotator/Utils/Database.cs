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
        private const int NewestDbUserVersion = 1;
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
            return OpenDatabase(DbPath);
        }

        private static LiteDatabase OpenDatabase(string dbPath)
        {
            var db = new LiteDatabase(dbPath, null, new Logger(Logger.FULL));
            var engine = db.Engine;
            if (engine.UserVersion == 0)
            {
                var pdf = db.GetCollection<PdfFile>();
                var annotation = db.GetCollection<WordAnnotation>();
                pdf.EnsureIndex(x => x.Md5, true);
                pdf.EnsureIndex(x => x.Path, "LOWER($.Path)");
                pdf.EnsureIndex(x => x.LastSeen);
                // todo: which index is right?
                //annotation.EnsureIndex(x => x.Document);
                annotation.EnsureIndex(x => x.Document.Id);
                annotation.EnsureIndex(x => x.Word);
                engine.UserVersion = 1;
            }
            if (engine.UserVersion != NewestDbUserVersion) throw new InvalidOperationException($"Version of {dbPath} is {engine.UserVersion} but should be {NewestDbUserVersion}. None of the available migrations was able to produce this version either.");
#if DEBUG
            db.Log.Logging += (s) => Debug.WriteLine(s);
#endif
            return db;
        }

        private static LiteDatabase OpenDatabaseReadonly(string dbPath)
        {
            var db = new LiteDatabase($"Filename={dbPath}; Mode=ReadOnly");
            var engine = db.Engine;
            if (engine.UserVersion != NewestDbUserVersion) throw new InvalidOperationException($"Version of {dbPath} is {engine.UserVersion} but should be {NewestDbUserVersion}. Other versions than {NewestDbUserVersion} can not be opened in readonly mode.");
            return db;
        }

        public static LiteRepository GetRepository()
        {
            return new LiteRepository(OpenDatabase(), true);
        }

        public static LiteRepository GetReadOnlyRepository(string databasePath)
        {
            return new LiteRepository(OpenDatabaseReadonly(databasePath), true);
        }

        public static bool Exists()
        {
            return File.Exists(DbPath);
        }

        public static void Export(string targetPath)
        {
            File.Copy(DbPath, targetPath);
        }

        public static void Restore(string sourcePath, bool keepBackupOfExisting = true)
        {
            if (keepBackupOfExisting && Exists()) File.Move(DbPath, $"{DbPath}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.bak");
            File.Copy(sourcePath, DbPath, true);
        }
    }
}
