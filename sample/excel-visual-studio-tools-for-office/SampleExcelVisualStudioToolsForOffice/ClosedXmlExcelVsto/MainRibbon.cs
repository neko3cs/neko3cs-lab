using ClosedXML.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace ClosedXmlExcelVsto
{
    public partial class MainRibbon
    {
        private Application Excel => Globals.ThisAddIn.Application;

        private void MainRibbon_Load(object sender, RibbonUIEventArgs e) { }

        private void RunButton_Click(object sender, RibbonControlEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = @"Text files (*.txt)|*.txt",
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var workbook = new XLWorkbook(Excel.ActiveWorkbook.FullName);
            Excel.ActiveWorkbook.Close();
            var tables = workbook.Load();
            tables.ExportTo(saveFileDialog.FileName);

            stopwatch.Stop();
            File.AppendAllText(saveFileDialog.FileName, $@"★処理時間(ms)：{stopwatch.Elapsed.TotalMilliseconds:g}{Environment.NewLine}");

            MessageBox.Show(
                text: $@"出力が完了しました。{Environment.NewLine}処理時間：{stopwatch.Elapsed:g}",
                caption: @"テストデータ作成ツール",
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Information
            );

            workbook.Dispose();
            Excel.Workbooks.Open(Excel.ActiveWorkbook.FullName);
        }
    }
}
