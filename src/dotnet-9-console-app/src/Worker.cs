using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostConsoleApp;

public class Worker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    IHostApplicationLifetime applicationLifetime
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);

        var appName = configuration["Settings:ApplicationName"];
        var awaitAtMilliSeconds = int.Parse(configuration["Settings:AwaitAtMilliSeconds"] ?? "0");

        logger.LogInformation($"Application Name: {appName}");
        await Task.Delay(awaitAtMilliSeconds, cancellationToken);

        logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);

        applicationLifetime.StopApplication();
    }
}