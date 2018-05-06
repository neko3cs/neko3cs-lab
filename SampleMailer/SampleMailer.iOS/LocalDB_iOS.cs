using SampleMailer.iOS;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalDB_iOS))]
namespace SampleMailer.iOS
{
    public class LocalDB_iOS : ILocalDB
    {
        public SQLiteConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentPath, "..", "Library");
            var dbPath = Path.Combine(libraryPath, "SampleMailer.db3");

            return new SQLiteConnection(new SQLitePlatformIOS(), dbPath);
        }
    }
}