using AzFunctionsApp.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// ReSharper disable ClassNeverInstantiated.Global

namespace AzFunctionsApp
{
    internal class Program
    {
        private static void Main()
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();

                    var connectionString = context.Configuration.GetConnectionString("AzFunctionsAppDatabaseContext");
                    services.AddSingleton<AzFunctionsAppDatabaseContext>(new AzFunctionsAppDatabaseContext(connectionString));
                })
                .Build();

            host.Run();
        }
    }
}
