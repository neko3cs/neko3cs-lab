using System;

namespace SampleEntityFrameworkCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("code first --------------");
            CodeFirst.Run();
            Console.WriteLine("database first ----------");
            DbFirst.Run();
        }
    }
}
