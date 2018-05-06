using SampleMailer.Droid;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalDB_Droid))]
namespace SampleMailer.Droid
{
    public class LocalDB_Droid : ILocalDB
    {
        public SQLiteConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documentPath, "SampleMailer.db3");

            return new SQLiteConnection(new SQLitePlatformAndroid(), dbPath);
        }
    }
}