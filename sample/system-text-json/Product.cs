namespace PlaySystemTextJson
{
    public record Product(
        string Name,
        int Price,
        string Currency
    )
    {
        public override string ToString() => $"{Name}: {Currency} {Price:#,0}";
    }
}
