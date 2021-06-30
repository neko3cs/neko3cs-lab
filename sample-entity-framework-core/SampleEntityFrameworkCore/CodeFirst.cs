using System;
using SampleEntityFrameworkCore.Models.CodeFirst;

namespace SampleEntityFrameworkCore
{
    public static class CodeFirst
    {
        public static void Run()
        {
            using (var db = new CodeFirstDbContext())
            {
                db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blogs)
                {
                    Console.WriteLine(" - {0}", blog.Url);
                }

                Console.WriteLine();
                Console.WriteLine("All posts in database:");
                foreach (var post in db.Posts)
                {
                    Console.WriteLine(" - {0}", post.Title);
                }
            }
        }
    }
}