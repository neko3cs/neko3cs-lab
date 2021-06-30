using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PlaySystemTextJson
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new JsonSerializerOptions
            {
                // 日本語が含まれる場合は "JavaScriptEncoder.UnsafeRelaxedJsonEscaping" を指定する
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                // インデント・改行が入る
                WriteIndented = true
            };

            var json1 = "{ \"Name\": \"あんぱん\", \"Price\": 999999999, \"Currency\": \"JPY\" }";
            var json2 = "{ \"Name\": \"MacBook Pro\", \"Price\": 2879, \"Currency\": \"USD\" }";

            var product1 = JsonSerializer.Deserialize<Product>(json1);
            var product2 = JsonSerializer.Deserialize<Product>(json2);

            Console.WriteLine(product1.ToString());
            Console.WriteLine(product2.ToString());

            Console.WriteLine(JsonSerializer.Serialize(product1, options));
            Console.WriteLine(JsonSerializer.Serialize(product2, options));
        }
    }
}
