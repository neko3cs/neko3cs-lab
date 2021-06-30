using System;
using System.Data.SqlClient;

namespace SampleQueryNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryNotify = new QueryNotify
            {
                ConnectionString = "Data Source=localhost;User ID=sa;Password=hogehoge",
                NotificationQuery = "select * from News"
            };
            queryNotify.MessageReceived += OnMessageReceived;
            queryNotify.ReceiveStart();
        }

        private static void OnMessageReceived(object sender, SqlNotificationEventArgs e)
            => Console.WriteLine("Message Received!");
    }
}
