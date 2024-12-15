namespace ConvertingDataTableAndIEnumerable;

public record class Person
{
  public int Id { get; init; } = 0;
  public string Name { get; init; } = string.Empty;
  public int Age { get; init; } = 0;

  // Genericで扱うためには引数なしパラメーターが必要っぽい。
  // DataTableExtensionsでReflection使って引数渡しでインスタンス生成しようとしたけど、CS0310のエラーが消えなかった。
  public Person() { }

  public Person(int id, string name, int age)
  {
    Id = id;
    Name = name;
    Age = age;
  }
}