using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DotNetOrmBench;

[Config(typeof(BenchmarkConfig))]
public class OrmBenchmarks
{
    private const string ConnectionString = "Data Source=localhost,1433;Initial Catalog=AdventureWorksDW2019;Trusted_Connection=True;TrustServerCertificate=True";
    private SampleDbContext? _dbContext;

    [GlobalSetup]
    public void Setup()
    {
        _dbContext = new SampleDbContext(ConnectionString);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _dbContext!.Dispose();
    }

    [Benchmark(Description = "SqlCommand - SELECT")]
    public void SqlClientBenchmark()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT * FROM DimCustomer", connection);
        var reader = command.ExecuteReader();
        var customers = new List<DimCustomer>();
        while (reader.Read())
        {
            customers.Add(new DimCustomer
            {
                CustomerKey = reader.GetInt32(0),
                GeographyKey = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                CustomerAlternateKey = reader.GetString(2),
                Title = reader.IsDBNull(3) ? null : reader.GetString(3),
                FirstName = reader.IsDBNull(4) ? null : reader.GetString(4),
                MiddleName = reader.IsDBNull(5) ? null : reader.GetString(5),
                LastName = reader.IsDBNull(6) ? null : reader.GetString(6),
                NameStyle = reader.IsDBNull(7) ? null : reader.GetBoolean(7),
                BirthDate = reader.IsDBNull(9) ? null : reader.GetDateTime(8),
                MaritalStatus = reader.IsDBNull(9) ? null : reader.GetString(9),
                Suffix = reader.IsDBNull(10) ? null : reader.GetString(10),
                Gender = reader.IsDBNull(11) ? null : reader.GetString(11),
                EmailAddress = reader.IsDBNull(12) ? null : reader.GetString(12),
                YearlyIncome = reader.IsDBNull(13) ? null : reader.GetDecimal(13),
                TotalChildren = reader.IsDBNull(14) ? null : reader.GetByte(14),
                NumberChildrenAtHome = reader.IsDBNull(15) ? null : reader.GetByte(15),
                EnglishEducation = reader.IsDBNull(16) ? null : reader.GetString(16),
                SpanishEducation = reader.IsDBNull(17) ? null : reader.GetString(17),
                FrenchEducation = reader.IsDBNull(18) ? null : reader.GetString(18),
                EnglishOccupation = reader.IsDBNull(19) ? null : reader.GetString(19),
                SpanishOccupation = reader.IsDBNull(20) ? null : reader.GetString(20),
                FrenchOccupation = reader.IsDBNull(21) ? null : reader.GetString(21),
                HouseOwnerFlag = reader.IsDBNull(22) ? null : reader.GetString(22),
                NumberCarsOwned = reader.IsDBNull(23) ? null : reader.GetByte(23),
                AddressLine1 = reader.IsDBNull(24) ? null : reader.GetString(24),
                AddressLine2 = reader.IsDBNull(25) ? null : reader.GetString(25),
                Phone = reader.IsDBNull(26) ? null : reader.GetString(26),
                DateFirstPurchase = reader.IsDBNull(27) ? null : reader.GetDateTime(27),
                CommuteDistance = reader.IsDBNull(28) ? null : reader.GetString(28)
            });
        }
    }

    [Benchmark(Description = "EntityFramework Core (from Model) - SELECT")]
    public void EfCoreModelBenchmark()
    {
        var customers = _dbContext!.DimCustomer.ToList();
    }

    [Benchmark(Description = "EntityFramework Core (from SQL) - SELECT")]
    public void EfCoreRawSqlBenchmark()
    {
        var customers = _dbContext!.DimCustomer.FromSqlRaw("SELECT * FROM DimCustomer").ToList();
    }

    [Benchmark(Description = "Dapper - SELECT")]
    public void DapperBenchmark()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        var customers = connection.Query<DimCustomer>("SELECT * FROM DimCustomer").ToList();
    }
}