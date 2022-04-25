using ClosedXML.Excel;

namespace ClosedXmlConsoleApp.Net6;

public static class XlWorkbookExtensions
{
    private const int HeaderRowIndex = 1;
    private const int IndexHeaderRowStartAt = 1;
    private const int IndexHeaderColumnStartAt = 1;

    public static IEnumerable<Table> Load(this XLWorkbook workbook)
    {
        foreach (var worksheet in workbook.Worksheets)
        {
            var range = worksheet.RangeUsed();
            if (range is null) continue;

            var table = new Table(worksheet.Name);
            for (var rowIndex = IndexHeaderRowStartAt; rowIndex <= range.RowCount(); rowIndex++)
            {
                var row = new Row();
                for (var
                     columnIndex = IndexHeaderColumnStartAt; columnIndex <= range.ColumnCount(); columnIndex++)
                {
                    row.AddColumn(
                        columnName: range.Cell(HeaderRowIndex, columnIndex).GetFormattedString(),
                        value: range.Cell(rowIndex, columnIndex).GetFormattedString()
                    );
                }
            }
            yield return table;
        }
    }
}
