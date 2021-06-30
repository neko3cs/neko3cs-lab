using System;
using SampleEntityFrameworkCore.Models.DbFirst;

namespace SampleEntityFrameworkCore
{
    public static class DbFirst
    {
        public static void Run()
        {
            using (var db = new DbFirstDbContext())
            {
                db.Blog.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blog)
                {
                    Console.WriteLine(" - {0}", blog.Url);
                }

                Console.WriteLine();
                Console.WriteLine("All posts in database:");
                foreach (var post in db.Post)
                {
                    Console.WriteLine(" - {0}", post.Title);
                }
            }
        }
    }
}