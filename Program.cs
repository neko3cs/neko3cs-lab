using System;
using System.Text.Json;

namespace PlaySystemTextJson
{
    class Program
    {
        static void Main(string[] args)
        {
            SerializeToJsonText();
            Console.WriteLine();

            DeserializeFromJsonText();
        }

        private static void SerializeToJsonText()
        {
            var person = new
            {
                Name = "John Maxwel",
                Age = 24,
                Job = "Engineer"
            };

            var json = JsonSerializer.Serialize(person);
            Console.WriteLine(json);
        }

        private static void DeserializeFromJsonText()
        {
            var json = "{ \"Name\": \"MacBook Pro\", \"Price\": 2879, \"Currency\": \"USD\" }";

            var product = JsonSerializer.Deserialize<Product>(json);

            Console.WriteLine(product.ToString());
        }
    }
}
