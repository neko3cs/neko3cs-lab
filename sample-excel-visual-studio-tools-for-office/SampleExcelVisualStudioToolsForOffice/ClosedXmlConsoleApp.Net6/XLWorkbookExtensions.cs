using ClosedXML.Excel;

namespace ClosedXmlConsoleApp.Net6
{
    public static class XLWorkbookExtensions
    {
        public static IEnumerable<Table> Load(this XLWorkbook workbook)
        {
            foreach (var worksheet in workbook.Worksheets)
            {
                var range = worksheet.RangeUsed();
                if (range is null) continue;

                var table = new Table(worksheet.Name);
                for (int row_i = 1; row_i <= range.RowCount(); row_i++)
                {
                    var row = new Row();
                    for (int col_i = 1; col_i <= range.ColumnCount(); col_i++)
                    {
                        row.AddColumn(
                            columnName: range.Cell(1, col_i).GetFormattedString(),
                            value: range.Cell(row_i, col_i).GetFormattedString()
                        );
                    }
                }
                yield return table;
            }
        }
    }
}
