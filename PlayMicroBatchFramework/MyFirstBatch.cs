using System;
using System.Reflection;
using System.Threading.Tasks;
using MicroBatchFramework;
using Microsoft.Extensions.Logging;

namespace PlayMicroBatchFramework
{
    public class MyFirstBatch : BatchBase
    {
        public void Hello(
            [Option("n", "name of send user.")]string name,
            [Option("r", "repeat count.")]int repeat = 3
        )
        {
            for (int i = 0; i < repeat; i++)
            {
                this.Context.Logger.LogInformation($"Hello My Batch from {name}");
            }
        }

        [Command("version")]
        public void ShowVersion()
        {
            var version = Assembly.GetExecutingAssembly()
                            .GetCustomAttribute<AssemblyFileVersionAttribute>()
                            .Version;
            Console.WriteLine(version);
        }

        [Command("escape")]
        public void UrlEscape([Option(0)] string input)
        {
            Console.WriteLine(Uri.EscapeDataString(input));
        }

        [Command("timer")]
        public async Task Timer([Option(0)] uint waitSeconds)
        {
            Console.WriteLine(waitSeconds + " sec.");
            while (waitSeconds != 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), Context.CancellationToken);
                waitSeconds--;
                Console.WriteLine(waitSeconds + " sec.");
            }
        }
    }
}
