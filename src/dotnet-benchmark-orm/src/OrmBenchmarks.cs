using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetOrmBench;

[MemoryDiagnoser]
public class OrmBenchmarks
{
    private const string ConnectionString = "Data Source=localhost,1433;Initial Catalog=neko3cs;Trusted_Connection=True;TrustServerCertificate=True";
    private SampleDbContext? _dbContext;

    [GlobalSetup]
    public void Setup()
    {
        _dbContext = new SampleDbContext(ConnectionString);

        // サンプルデータの準備
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        connection.Execute("IF OBJECT_ID('People', 'U') IS NULL CREATE TABLE [People] (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50))");
        connection.Execute("TRUNCATE TABLE [People]");
        for (var i = 0; i < 1000; i++)
        {
            connection.Execute("INSERT INTO People (Name) VALUES (@Name)", new { Name = $"Person {i}" });
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _dbContext!.Dispose();
    }

    [Benchmark]
    public int SqlClientBenchmark()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT COUNT(*) FROM People", connection);
        return (int)command.ExecuteScalar();
    }

    [Benchmark]
    public int EfCoreBenchmark()
    {
        return _dbContext!.People.Count();
    }

    [Benchmark]
    public int DapperBenchmark()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        return connection.Query<int>("SELECT COUNT(*) FROM People").Single();
    }
}