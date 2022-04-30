using Microsoft.Office.Tools.Ribbon;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace SampleExcelVisualStudioToolsForOffice
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

            using (var stream = saveFileDialog.OpenFile() as FileStream)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var tables = Excel.Sheets.ConvertToTables();
                stream.SaveAsCsv(tables);

                stopwatch.Stop();
                stream.WriteTextLine($@"★処理時間(ms)：{stopwatch.Elapsed.TotalMilliseconds:g}");

                MessageBox.Show(
                    text: $@"出力が完了しました。{Environment.NewLine}処理時間：{stopwatch.Elapsed:g}",
                    caption: @"テストデータ作成ツール",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Information
                );
            }
        }
    }
}
