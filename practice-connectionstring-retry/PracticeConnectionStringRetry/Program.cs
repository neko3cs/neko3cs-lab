using Dapper;
using System;
using System.Data.SqlClient;
using System.Threading;
// ReSharper disable FunctionNeverReturns

namespace PracticeConnectionStringRetry;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = new SqlConnectionStringBuilder()
        {
            DataSource = "localhost",
            InitialCatalog = "AdventureWorks2017",
            UserID = "AdventureWorks",
            Password = "P@ssw0rd!",
            ConnectRetryCount = 10,
            ConnectRetryInterval = 10
        };
        Console.WriteLine(builder.ConnectionString);

        using var connection = new SqlConnection(builder.ConnectionString);
        connection.Open();

        // ★トランザクションを張ってるとConnectRetryCountとConnectRetryIntervalが効かなくなる
        //using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        // ループ中にSQL Serverのサービスを再起動したりして障害を再現するとConnectRetryCountとConnectRetryIntervalが効いてるか確認出来る
        while (true)
        {
            var now = connection
                .QuerySingle<DateTime>(
                    sql: "select GETDATE()",
                    //transaction: transaction,
                    commandTimeout: 100
                );

            Console.WriteLine(now);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}