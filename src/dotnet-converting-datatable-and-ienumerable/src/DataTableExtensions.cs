using System;
using System.Collections.Generic;
using System.Data;

namespace ConvertingDataTableAndIEnumerable;

public static class DataTableExtensions
{
  public static IEnumerable<T> ToEnumerable<T>(this DataTable table) where T : new()
  {
    var props = typeof(T).GetProperties();
    foreach (DataRow row in table.Rows)
    {
      T item = new T();
      foreach (var prop in props)
      {
        if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
        {
          prop.SetValue(item, Convert.ChangeType(row[prop.Name], prop.PropertyType));
        }
      }
      yield return item;
    }
  }
}