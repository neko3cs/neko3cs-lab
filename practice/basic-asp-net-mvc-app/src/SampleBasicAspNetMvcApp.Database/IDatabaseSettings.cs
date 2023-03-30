namespace SampleBasicAspNetMvcApp.Database;

public interface IDatabaseSettings
{
    string ConnectionString { get; }
}