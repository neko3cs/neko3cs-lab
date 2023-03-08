using ClosedXML.Excel;
using ClosedXmlConsoleApp.Net6;
using System.Diagnostics;

ConsoleApp.Run(args, (string input, string output) =>
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var workbook = new XLWorkbook(input);
    var tables = workbook.Load();
    tables.ExportTo(output);

    stopwatch.Stop();
    File.AppendAllText(output, $@"★処理時間(ms)：{stopwatch.Elapsed.TotalMilliseconds:g}{Environment.NewLine}");
});
