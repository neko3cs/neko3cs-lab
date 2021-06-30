using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AnnotationInConsoleAppFramework
{
    class Program : ConsoleAppBase
    {
        static async Task Main(string[] args) =>
            await Host
            .CreateDefaultBuilder()
            .RunConsoleAppFrameworkAsync<Program>(args);

        public void Run(
            // ConsoleAppFramework ではアノテーションは使えないので、
            // アノテーション使いたかったら Cocona 使った方がいいかも
            [PathExists]
            [Option("path")] string filePath
        )
        {
            Console.WriteLine($"Path here!: {Path.GetFullPath(filePath)}");
        }
    }
}
