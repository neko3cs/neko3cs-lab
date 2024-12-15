namespace EfCoreCodeFirst;

public class Blog
{
  public int BlogId { get; set; }
  public required string Url { get; set; }
  public List<Post> Posts { get; set; } = [];
}
