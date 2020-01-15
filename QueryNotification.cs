using System;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Security.Permissions;

namespace SampleQueryNotification
{
    public delegate void MessageReceivedEventHandler(object sender, SqlNotificationEventArgs e);

    public class QueryNotify
    {
        /// <summary> クエリ通知が開始されているかどうかを判断します。 </summary>
        private static bool SqlDependency_Started { get; set; } = false;
        /// <summary> クエリ通知を要求するデータベースの接続文字列。 </summary>
        public string ConnectionString { get; set; }
        /// <summary> クエリ通知に用いるSQL。 </summary>
        public string NotificationQuery { get; set; }
        /// <summary> クエリ通知を受信した際に実行するイベント。 </summary>
        public event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// データベースに対してクエリ通知のアクセス許可を申請します。
        /// </summary>
        private bool CanRequestNotifications()
        {
            var permission = new SqlClientPermission(PermissionState.Unrestricted);
            try
            {
                permission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// クエリ通知の受信を開始します。
        /// </summary>
        public void ReceiveStart()
        {
            if (string.IsNullOrEmpty(this.ConnectionString) || string.IsNullOrEmpty(this.NotificationQuery))
            {
                throw new InvalidOperationException("クエリ通知を登録する前に接続文字列と通知用クエリを設定してください。");
            }
            if (this.CanRequestNotifications())
            {
                throw new SecurityException("指定のデータベースへのアクセス許可を得られませんでした。");
            }

            if (!SqlDependency_Started)
            {
                SqlDependency_Started = SqlDependency.Start(this.ConnectionString);
            }

            this.SubscribeNotification();
        }

        /// <summary>
        /// クエリ通知をデータベースへ登録します。
        /// </summary>
        private void SubscribeNotification()
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var command = conn.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = this.NotificationQuery;
                command.Notification = null;

                var dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// クエリが変更された際に次戦に登録した<code>MessageReceived</code>イベントを実行し、クエリ通知を再度登録します。
        /// </summary>
        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.MessageReceived?.Invoke(sender, e);

            var dependency = sender as SqlDependency;
            dependency.OnChange -= new OnChangeEventHandler(this.OnDependencyChange);

            this.SubscribeNotification();
        }
    }
}
