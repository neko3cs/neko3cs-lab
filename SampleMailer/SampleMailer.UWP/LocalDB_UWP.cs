using SampleMailer.UWP;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalDB_UWP))]
namespace SampleMailer.UWP
{
    public class LocalDB_UWP : ILocalDB
    {
        public SQLiteConnection GetConnection()
        {
            var dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "SampleMailer.db3");

            return new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
        }
    }
}
