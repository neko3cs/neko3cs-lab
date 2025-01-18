using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHostConsoleApp;

public class Worker : BackgroundService
{
  private readonly ILogger<Worker> _logger;
  private readonly IConfiguration _configuration;
  private readonly IHostApplicationLifetime _applicationLifetime;

  public Worker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    IHostApplicationLifetime applicationLifetime
  )
  {
    _logger = logger;
    _configuration = configuration;
    _applicationLifetime = applicationLifetime;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);

    var appName = _configuration["Settings:ApplicationName"];
    var version = _configuration["Settings:Version"];
    var awaitAt_ms = int.Parse(_configuration["Settings:AwaitAt_ms"]);

    _logger.LogInformation("Application Name: {AppName}, Version: {Version}", appName, version);
    await Task.Delay(awaitAt_ms, cancellationToken);

    _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);

    _applicationLifetime.StopApplication();
  }
}
