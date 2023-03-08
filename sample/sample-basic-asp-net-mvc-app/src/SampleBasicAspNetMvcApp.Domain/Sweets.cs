namespace SampleBasicAspNetMvcApp.Domain;

public class Sweets : ISweets
{

    public Sweets(
        int id,
        string name,
        int calories
    )
    {
        Id = id;
        Name = name;
        Calories = calories;
    }

    public int Id { get; }
    public string Name { get; }
    public int Calories { get; }
}