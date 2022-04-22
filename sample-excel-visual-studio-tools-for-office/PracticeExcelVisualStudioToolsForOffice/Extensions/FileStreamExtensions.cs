using PracticeExcelVisualStudioToolsForOffice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PracticeExcelVisualStudioToolsForOffice.Extensions
{
    public static class FileStreamExtensions
    {
        private static void WriteText(this FileStream stream, string value)
        {
            var info = new UTF8Encoding(true).GetBytes(value);
            stream.Write(info, 0, info.Length);
        }

        public static void WriteTextLine(this FileStream stream, string value = "") =>
            stream.WriteText($"{value}{Environment.NewLine}");


        public static void SaveAsCsv(this FileStream stream, IEnumerable<Table> tables)
        {
            foreach (Table table in tables)
            {
                stream.WriteTextLine($"◆{table.Name}");

                var csvHeader = string.Join(",", table.Rows.First().Columns.Keys);
                stream.WriteTextLine(csvHeader);

                foreach (Row row in table.Rows)
                {
                    var csvRecord = string.Join(",", row.Columns.Values);
                    stream.WriteTextLine(csvRecord);
                }

                stream.WriteTextLine();
            }
        }
    }
}
