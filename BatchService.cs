using Cocona;
using System;

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
        public void Greet(int number, string name, int age)
        {
            var person = new Person
            {
                Number = number,
                Name = name,
                Age = age
            };

            Console.WriteLine(person.Greet());
        }
    }
}
