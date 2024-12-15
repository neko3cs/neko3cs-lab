namespace EfCoreCodeFirst;

public class Post
{
  public int PostId { get; set; }
  public required string Title { get; set; }
  public required string Content { get; set; }

  public int BlogId { get; set; }
  public Blog? Blog { get; set; }
}
