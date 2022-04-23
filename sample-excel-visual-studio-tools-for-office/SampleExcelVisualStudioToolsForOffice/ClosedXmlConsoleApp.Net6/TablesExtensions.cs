﻿namespace ClosedXmlConsoleApp.Net6
{
    public static class TablesExtensions
    {
        public static void ExportTo(this IEnumerable<Table> tables, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            using (var writer = File.CreateText(filePath))
            {
                foreach (Table table in tables)
                {
                    writer.WriteLine($"◆{table.Name}");

                    var csvHeader = string.Join(",", table.Rows.First().Columns.Keys);
                    writer.WriteLine(csvHeader);

                    foreach (Row row in table.Rows)
                    {
                        var csvRecord = string.Join(",", row.Columns.Values);
                        writer.WriteLine(csvRecord);
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
