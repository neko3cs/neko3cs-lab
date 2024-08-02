using Microsoft.Azure.Functions.Worker;
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
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                })
                .Build();

            host.Run();
        }
    }
}
