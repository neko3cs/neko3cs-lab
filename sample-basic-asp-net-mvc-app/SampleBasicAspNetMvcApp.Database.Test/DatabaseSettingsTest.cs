using FluentAssertions;
using Xunit;

namespace SampleBasicAspNetMvcApp.Database.Test;

public class DatabaseSettingsTest
{
    public class Ú‘±•¶š—ñ‚ğæ“¾‚·‚é
    {
        [Fact]
        public void Ú‘±•¶š—ñ‚ªæ“¾o—ˆ‚é‚±‚Æ()
        {
            const string connectionString =
                "Data Source=localhost:1434;Initial Catalog=SampleDatabase;User ID=sa;Password=P@ssword!;";
            var databaseSettings = new DatabaseSettings(connectionString);

            databaseSettings.ConnectionString.Should().Be(connectionString);
        }
    }
}