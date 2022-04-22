using Microsoft.Office.Tools.Ribbon;
using PracticeExcelVisualStudioToolsForOffice.Extensions;
using System.IO;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace PracticeExcelVisualStudioToolsForOffice
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
                var tables = Excel.Sheets.ConvertToTables();
                stream.SaveAsCsv(tables);

                MessageBox.Show(
                    text: @"テストデータ作成ツール",
                    caption: @"出力が完了しました。",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Information
                );
            }
        }
    }
}
