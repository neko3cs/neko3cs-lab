using System;
namespace SampleBasicAspNetMvcApp.Domain;

public interface ISweets
{
    int Id { get; }
    string Name { get; }
    int Calories { get; }
}