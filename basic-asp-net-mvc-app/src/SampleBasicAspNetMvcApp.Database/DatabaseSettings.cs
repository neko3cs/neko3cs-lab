namespace SampleBasicAspNetMvcApp.Database
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public DatabaseSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}