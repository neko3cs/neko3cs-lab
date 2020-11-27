using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PlaySystemTextJson
{
    class Program
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        static void Main(string[] args)
        {
            SerializeToJsonText();
            Console.WriteLine();

            DeserializeFromJsonText();
        }

        private static void SerializeToJsonText()
        {
            var person1 = new
            {
                Name = "おぎゃりすと次郎",
                Age = 34,
                Job = "あんぱん職人"
            };
            var person2 = new
            {
                Name = "John Maxwel",
                Age = 24,
                Job = "Engineer"
            };

            Console.WriteLine(JsonSerializer.Serialize(person1, options));
            Console.WriteLine(JsonSerializer.Serialize(person2, options));
        }

        private static void DeserializeFromJsonText()
        {
            var json1 = "{ \"Name\": \"あんぱん\", \"Price\": 999999999, \"Currency\": \"JPY\" }";
            var json2 = "{ \"Name\": \"MacBook Pro\", \"Price\": 2879, \"Currency\": \"USD\" }";

            Console.WriteLine(JsonSerializer.Deserialize<Product>(json1).ToString());
            Console.WriteLine(JsonSerializer.Deserialize<Product>(json2).ToString());
        }
    }
}
