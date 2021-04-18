using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Linq;

namespace SampleRunAppInWindowsDockerContainer.Service
{
    public static class Program
    {
        public static void Main(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            })
            .Build(isService: !(Debugger.IsAttached || args.Contains("--console")))
            .Run();

        public static IHost Build(this IHostBuilder hostBuilder, bool isService) => isService
            ? hostBuilder.UseWindowsService().Build()
            : hostBuilder.UseConsoleLifetime().Build();
    }
}
