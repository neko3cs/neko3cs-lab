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
        private static bool SqlDependency_Started { get; set; } = false;
        public string ConnectionString { get; set; }
        public string NotificationQuery { get; set; }
        public event MessageReceivedEventHandler MessageReceived;

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

        public void ReceiveStart()
        {
            if (string.IsNullOrEmpty(this.ConnectionString) || string.IsNullOrEmpty(this.NotificationQuery))
            {
                throw new InvalidOperationException("Need to set connection string and query for notification before register query notification.");
            }
            if (this.CanRequestNotifications())
            {
                throw new SecurityException("Access denied by database.");
            }

            if (!SqlDependency_Started)
            {
                SqlDependency_Started = SqlDependency.Start(this.ConnectionString);
            }

            this.SubscribeNotification();
        }

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

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.MessageReceived?.Invoke(sender, e);

            var dependency = sender as SqlDependency;
            dependency.OnChange -= new OnChangeEventHandler(this.OnDependencyChange);

            this.SubscribeNotification();
        }
    }
}
