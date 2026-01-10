using GenericHostConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration((hostContext, config) =>
  {
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
  })
  .ConfigureServices((hostContext, services) =>
  {
    services.AddHostedService<Worker>();
  })
  .ConfigureLogging((hostContext, logging) =>
  {
    logging.AddConsole();
  })
  .Build();

await host.RunAsync();
