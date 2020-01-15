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
                ConnectionString = "conn;hogehoge;fugafuga;piyopiyo;",
                NotificationQuery = "select * from t_buy_hist"
            };
            queryNotify.MessageReceived += OnMessageReceived;
            queryNotify.ReceiveStart();
        }

        private static void OnMessageReceived(object sender, SqlNotificationEventArgs e)
            => Console.WriteLine("Message Received!");
    }
}
