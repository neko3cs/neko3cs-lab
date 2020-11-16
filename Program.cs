using Cocona;
using System;
using TryDotNetFive;

CoconaLiteApp.Run<BatchService>(args);

namespace TryDotNetFive
{
    public class BatchService
    {
        [PrimaryCommand]
        public void Run(int? value)
        {
            if (value is not null)
            {
                if (value.Value is > 0 and < 10)
                {
                    Console.WriteLine("Value is 1..9!");
                }
            }
            else
            {
                Console.WriteLine("Value is not null!");
            }
        }

        [Command]
        public void Greet()
        {
            var person1 = new Person(1, "John", 25);
            var person2 = new Person(1, "John", 25);

            Console.WriteLine(person1.Greet());
            Console.WriteLine(person2.Greet());

            Console.WriteLine(person1.Equals(person2) ? "We are same person" : "We are not same person.");
        }
    }

    public record Person(
        int Number,
        string Name,
        int Age
    )
    {
        public string Greet()
        {
            return $"Hi! No. {Number}. My name is {Name}. I'm {Age} year-old! Thanks!";
        }
    }
}
