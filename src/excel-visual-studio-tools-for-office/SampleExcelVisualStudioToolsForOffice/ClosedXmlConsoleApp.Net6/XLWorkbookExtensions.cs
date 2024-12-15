using ClosedXML.Excel;

namespace ClosedXmlConsoleApp.Net6;

public static class XlWorkbookExtensions
{
    private const int HeaderRowIndex = 1;
    private const int RowIndexDataStartAt = 3;
    private const int ColumnIndexDataStartAt = 1;

    public static IEnumerable<Table> Load(this XLWorkbook workbook)
    {
        foreach (var worksheet in workbook.Worksheets)
        {
            var range = worksheet.RangeUsed();
            if (range is null) continue;

            var table = new Table(worksheet.Name);
            for (var rowIndex = RowIndexDataStartAt; rowIndex <= range.RowCount(); rowIndex++)
            {
                var row = new Row();
                for (var
                     columnIndex = ColumnIndexDataStartAt; columnIndex <= range.ColumnCount(); columnIndex++)
                {
                    row.AddColumn(
                        columnName: range.Cell(HeaderRowIndex, columnIndex).GetFormattedString(),
                        value: range.Cell(rowIndex, columnIndex).GetFormattedString()
                    );
                }

                table.AddRow(row);
            }
            yield return table;
        }
    }
}
