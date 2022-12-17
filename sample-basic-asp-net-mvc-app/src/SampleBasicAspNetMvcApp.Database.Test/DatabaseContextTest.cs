using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Xunit;

namespace SampleBasicAspNetMvcApp.Database.Test;

public class DatabaseContextTest
{
    private const string CorrectConnectionString =
        "Data Source=localhost,1434;Initial Catalog=SampleDatabase;User ID=sa;Password=P@ssword!;TrustServerCertificate=true;";
    private const string IncorrectConnectionString =
        "Data Source=localhost,9999;Initial Catalog=SampleDatabase;User ID=sa;Password=P@ssword!;TrustServerCertificate=true;";

    public class トランザクションを開始する
    {
        [Fact]
        public void トランザクションを開始出来ること()
        {
            var databaseContext = new DatabaseContext(new DatabaseSettings(CorrectConnectionString));
            var result = 0;

            databaseContext.ExecuteTransaction(transaction =>
            {
                var connection = (SqlConnection)transaction.Connection!;

                result = connection.QuerySingle<int>("select 1", transaction: transaction);

                transaction.Commit();
            });

            result.Should().Be(1);
        }

        [Fact]
        public void トランザクション開始に失敗した場合SqlExceptionがスローされること()
        {
            var databaseContext = new DatabaseContext(new DatabaseSettings(IncorrectConnectionString));

            databaseContext.Invoking(x => x.ExecuteTransaction(transaction =>
            {
                var connection = (SqlConnection)transaction.Connection!;

                connection.QuerySingle<int>("select 1", transaction: transaction);

                transaction.Commit();
            })).Should().Throw<SqlException>();
        }
    }
}