using FluentAssertions;
using Xunit;

namespace SampleBasicAspNetMvcApp.Database.Test;

public class DatabaseSettingsTest
{
    public class �ڑ���������擾����
    {
        [Fact]
        public void �ڑ������񂪎擾�o���邱��()
        {
            const string connectionString =
                "Data Source=localhost:1434;Initial Catalog=SampleDatabase;User ID=sa;Password=P@ssword!;";
            var databaseSettings = new DatabaseSettings(connectionString);

            databaseSettings.ConnectionString.Should().Be(connectionString);
        }
    }
}