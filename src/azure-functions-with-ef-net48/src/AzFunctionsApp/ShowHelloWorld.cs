using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
// ReSharper disable ClassNeverInstantiated.Global

namespace AzFunctionsApp
{
    public class ShowHelloWorld
    {
        private const string TimerScheduleForProduction = "0 0 15 * * 1-5";  // ŒŽ—j“ú‚©‚ç‹à—j“ú‚Ì15Žž
        private const string TimerScheduleForDevelopment = "*/5 * * * * *";  // 5•b‚²‚Æ

        private readonly ILogger _logger;

        public ShowHelloWorld(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ShowHelloWorld>();
        }

        [Function("ShowHelloWorld")]
        public void Run([TimerTrigger(TimerScheduleForDevelopment)] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus != null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
