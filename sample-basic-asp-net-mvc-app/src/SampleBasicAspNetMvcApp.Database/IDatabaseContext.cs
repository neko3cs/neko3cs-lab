using System.Data;

namespace SampleBasicAspNetMvcApp.Database;

public interface IDatabaseContext
{
    void ExecuteTransaction(Action<IDbTransaction> action);
}