using FluentAssertions;
using Xunit;

namespace SampleBasicAspNetMvcApp.Database.Test;

public class DatabaseSettingsTest
{
    public class 接続文字列を取得する
    {
        [Fact]
        public void 接続文字列が取得出来ること()
        {
            const string connectionString =
                "Data Source=localhost:1434;Initial Catalog=SampleDatabase;User ID=sa;Password=P@ssword!;";
            var databaseSettings = new DatabaseSettings(connectionString);

            databaseSettings.ConnectionString.Should().Be(connectionString);
        }
    }
}