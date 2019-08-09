using System;
using System.Threading.Tasks;
using MicroBatchFramework;
using Microsoft.Extensions.Logging;

namespace PlayMicroBatchFramework
{
    public class Daemon : BatchBase
    {
        public async Task Run()
        {
            try
            {
                while (!Context.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        Context.Logger.LogDebug("Wait One Minutes");
                    }
                    catch (Exception ex)
                    {
                        Context.Logger.LogError(ex, "Found error");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), Context.CancellationToken);
                }
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                Context.Logger.LogError(ex, "Exception throws!");
            }
        }
    }
}
