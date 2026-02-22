using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DotNetOrmBench;

[Config(typeof(BenchmarkConfig))]
public class OrmBenchmarks
{
    private const string ConnectionString = "Data Source=localhost,1433;Initial Catalog=AdventureWorks2025;User Id=sa;Password=P@ssword!;TrustServerCertificate=True";

    [Benchmark(Description = "SqlCommand - SELECT")]
    public static void SqlClientBenchmark()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        const string sql = "SELECT * FROM Person.Person";
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        int idIdx = reader.GetOrdinal("BusinessEntityID");
        int typeIdx = reader.GetOrdinal("PersonType");
        int styleIdx = reader.GetOrdinal("NameStyle");
        int titleIdx = reader.GetOrdinal("Title");
        int firstIdx = reader.GetOrdinal("FirstName");
        int middleIdx = reader.GetOrdinal("MiddleName");
        int lastIdx = reader.GetOrdinal("LastName");
        int suffixIdx = reader.GetOrdinal("Suffix");
        int promoIdx = reader.GetOrdinal("EmailPromotion");
        int infoIdx = reader.GetOrdinal("AdditionalContactInfo");
        int demoIdx = reader.GetOrdinal("Demographics");
        int guidIdx = reader.GetOrdinal("rowguid");
        int dateIdx = reader.GetOrdinal("ModifiedDate");

        var people = new List<Person>();
        while (reader.Read())
        {
            people.Add(new Person
            {
                BusinessEntityID = reader.GetInt32(idIdx),
                PersonType = reader.GetString(typeIdx),
                NameStyle = reader.GetBoolean(styleIdx),
                Title = reader.IsDBNull(titleIdx) ? null : reader.GetString(titleIdx),
                FirstName = reader.GetString(firstIdx),
                MiddleName = reader.IsDBNull(middleIdx) ? null : reader.GetString(middleIdx),
                LastName = reader.GetString(lastIdx),
                Suffix = reader.IsDBNull(suffixIdx) ? null : reader.GetString(suffixIdx),
                EmailPromotion = reader.GetInt32(promoIdx),
                AdditionalContactInfo = reader.IsDBNull(infoIdx) ? null : reader.GetString(infoIdx),
                Demographics = reader.IsDBNull(demoIdx) ? null : reader.GetString(demoIdx),
                rowguid = reader.GetGuid(guidIdx),
                ModifiedDate = reader.GetDateTime(dateIdx)
            });
        }
    }

    [Benchmark(Description = "EntityFramework Core (from Model) - SELECT")]
    public static void EfCoreModelBenchmark()
    {
        using var dbContext = new SampleDbContext(ConnectionString);
        var _ = dbContext!.Person.ToList();
    }

    [Benchmark(Description = "EntityFramework Core (from Model and AsNoTracking) - SELECT")]
    public static void EfCoreModelAsNoTrackingBenchmark()
    {
        using var dbContext = new SampleDbContext(ConnectionString);
        var _ = dbContext!.Person.AsNoTracking().ToList();
    }

    [Benchmark(Description = "EntityFramework Core (from SQL) - SELECT")]
    public static void EfCoreRawSqlBenchmark()
    {
        using var dbContext = new SampleDbContext(ConnectionString);
        var _ = dbContext!.Person.FromSqlRaw("SELECT * FROM Person.Person").ToList();
    }

    [Benchmark(Description = "EntityFramework Core (from SQL and AsNoTracking) - SELECT")]
    public static void EfCoreRawSqlAsNoTrackingBenchmark()
    {
        using var dbContext = new SampleDbContext(ConnectionString);
        var _ = dbContext!.Person.FromSqlRaw("SELECT * FROM Person.Person").AsNoTracking().ToList();
    }


    [Benchmark(Description = "Dapper - SELECT")]
    public static void DapperBenchmark()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var _ = connection.Query<Person>("SELECT * FROM Person.Person").ToList();
    }
}