using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace ConvertingDataTableAndIEnumerable;

internal class Program
{
  private static void Main(string[] args)
  {
    var persons = GetPersons();

    Console.WriteLine("#元データ");
    persons
      .ToList()
      .ForEach(Console.WriteLine);
    Console.WriteLine();

    Console.WriteLine("#DataTable");
    var table = persons.ToDataTable<Person>();
    Console.WriteLine(
      string.Join(",", table.Columns.Cast<DataColumn>().Select(col => $"{col.ColumnName}<{col.DataType.Name}>"))
    );
    table
      .AsEnumerable()
      .ToList()
      .ForEach(row => Console.WriteLine(string.Join(",", row.ItemArray.Select(item => item?.ToString()))));
    Console.WriteLine();

    Console.WriteLine("#IEnumerable");
    table
      .ToEnumerable<Person>()
      .ToList()
      .ForEach(Console.WriteLine);
    Console.WriteLine();
  }

  private static IEnumerable<Person> GetPersons() =>
    new List<Person>{
      new Person(1, "Taro", 30),
      new Person(2, "Jiro", 25),
      new Person(3, "Saburo", 20),
    };
}