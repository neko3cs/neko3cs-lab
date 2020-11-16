namespace TryDotNetFive
{
    public record Person
    {
        public int Number { get; init; }
        public string Name { get; init; }
        public int Age { get; init; }

        public string Greet()
        {
            return $"Hi! No. {Number}. My name is {Name}. I'm {Age} year-old! Thanks!";
        }
    }
}
