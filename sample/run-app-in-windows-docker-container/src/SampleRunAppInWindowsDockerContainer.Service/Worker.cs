using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SampleRunAppInWindowsDockerContainer.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer _timer;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer()
            {
                Interval = TimeSpan.FromSeconds(1).TotalMilliseconds,
                Enabled = true,
                AutoReset = true
            };
            _timer.Elapsed += new ElapsedEventHandler(OnElapsed);

            _timer.Start();
            _logger.LogInformation("Worker greeting: Start Worker");

            return Task.CompletedTask;
        }
        private async void OnElapsed(object sender, ElapsedEventArgs e)
        {
            _logger.LogInformation("Worker greeting: Hello World");

            await Task.Delay(TimeSpan.FromSeconds(3));

            _logger.LogInformation("Worker greeting: it waited for 3 seconds.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            _logger.LogInformation("Worker greeting: Stop Worker");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _timer.Close();
            base.Dispose();
        }
    }
}
