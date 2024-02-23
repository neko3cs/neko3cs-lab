using System.Data;
using System.Collections.Generic;

namespace ConvertingDataTableAndIEnumerable;

public static class IEnumerableExtensions
{
  public static DataTable ToDataTable<T>(this IEnumerable<T> items)
  {
    var table = new DataTable();
    var props = typeof(T).GetProperties();

    foreach (var prop in props)
    {
      var isNullableType = prop.PropertyType.IsGenericType &&
                           prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
      table.Columns.Add(new DataColumn
      {
        ColumnName = prop.Name,
        DataType = isNullableType ? Nullable.GetUnderlyingType(prop.PropertyType)
                                  : prop.PropertyType
      });
    }
    foreach (var item in items)
    {
      var row = table.NewRow();
      foreach (var prop in props)
      {
        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
      }
      table.Rows.Add(row);
    }

    return table;
  }
}