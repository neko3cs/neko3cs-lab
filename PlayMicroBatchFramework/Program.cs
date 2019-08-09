using System.Threading.Tasks;
using MicroBatchFramework;

namespace PlayMicroBatchFramework
{
    class Program
    {
        static async Task Main(string[] args)
            => await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<MyFirstBatch>(args);
    }
}
