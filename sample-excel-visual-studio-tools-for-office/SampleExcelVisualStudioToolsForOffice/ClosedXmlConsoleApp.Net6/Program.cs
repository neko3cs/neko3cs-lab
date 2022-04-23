using ClosedXML.Excel;
using ClosedXmlConsoleApp.Net6;

ConsoleApp.Run(args, (string input, string output) =>
{
    var workbook = new XLWorkbook(input);
    var tables = workbook.Load();
    tables.ExportTo(output);
});
