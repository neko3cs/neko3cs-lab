using System.Text.Json;
using EfCoreCodeFirst;
using EfCoreCodeFirst.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var connectionString = new SqlConnectionStringBuilder
{
  DataSource = "localhost",
  InitialCatalog = "EfCoreCodeFirst",
  UserID = "sa",
  Password = "P@ssword!",
  TrustServerCertificate = true
}.ConnectionString;

using var db = new DatabaseContext(connectionString);
if (await db.Database.EnsureCreatedAsync())
{
  Console.WriteLine("DEBUG: Created database.");
  Console.ReadLine();
}
else
{
  // db.Database.EnsureCreated()とは同時に実行出来ないらしい
  // URL: https://learn.microsoft.com/ja-jp/ef/core/managing-schemas/ensure-created
  await db.Database.MigrateAsync();
  Console.WriteLine("DEBUG: Migrated database.");
}

db.Blogs.Add(new Blog { Url = "http://neko3cs.net" });
await db.SaveChangesAsync();

Console.WriteLine("DEBUG: Inserted [Blogs] item.");
Console.ReadLine();

var blog = db.Blogs
  .OrderBy(b => b.BlogId)
  .First();
Console.WriteLine($"DEBUG: Selected [Blogs]. item: {JsonSerializer.Serialize(blog)}");
Console.ReadLine();

blog.Url = "https://neko3cs.net/entry";
blog.Posts.Add(new Post
{
  Title = "Hello, World!",
  Content = "I wrote an app using EF Core!",
});
await db.SaveChangesAsync();

Console.WriteLine("DEBUG: Changed [Blogs] item and insert [Posts] item.");
Console.ReadLine();

db.Remove(blog);
await db.SaveChangesAsync();

Console.WriteLine("DEBUG: Deleted [Blogs] and [Posts] items.");
