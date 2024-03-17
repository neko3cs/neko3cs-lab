using Microsoft.Data.SqlClient;
using System.Data;

namespace SampleBasicAspNetMvcApp.Database;

public class DatabaseContext
{
    private readonly IDatabaseSettings _databaseSettings;

    public DatabaseContext(IDatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }

    public void ExecuteTransaction(Action<IDbTransaction> action)
    {
        using var connection = new SqlConnection(_databaseSettings.ConnectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        action(transaction);
    }
}