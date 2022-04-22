using Microsoft.Office.Interop.Excel;
using SampleExcelVisualStudioToolsForOffice.Models;
using System.Collections.Generic;

namespace SampleExcelVisualStudioToolsForOffice.Extensions
{
    public static class SheetsExtensions
    {
        public static IEnumerable<Table> ConvertToTables(this Sheets sheets)
        {
            foreach (Worksheet sheet in sheets)
            {
                var table = new Table(sheet.Name);
                for (int rowIndex = 3; rowIndex < sheet.UsedRange.Rows.Count; rowIndex++) // データは3行目から入ってる
                {
                    var row = new Row();
                    for (int columnIndex = 1; columnIndex <= sheet.UsedRange.Columns.Count; columnIndex++)
                    {
                        row.AddColumn(
                            columnName: sheet.Cells[1, columnIndex].Value.ToString(),
                            value: sheet.Cells[rowIndex, columnIndex].Value.ToString()
                        );
                    }
                    table.AddRow(row);
                }
                yield return table;
            }
        }
    }
}
