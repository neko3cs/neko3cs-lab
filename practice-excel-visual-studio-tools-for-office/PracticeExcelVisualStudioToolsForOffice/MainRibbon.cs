using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PracticeExcelVisualStudioToolsForOffice
{
    public partial class MainRibbon
    {
        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void RunButton_Click(object sender, RibbonControlEventArgs e)
        {
            var sheets = Globals.ThisAddIn.Application.Sheets;

            var tables = new List<Dictionary<string, string>>();
            foreach (Worksheet sheet in sheets)
            {
                for (var rowIndex = 0; rowIndex < sheet.UsedRange.Rows.Count; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < sheet.UsedRange.Columns.Count; colIndex++)
                    {
                        var value = sheet.Range[rowIndex, colIndex].Cells.Value.ToString();
                    }
                }
            }

            //SaveAsFile();
        }

        private static void SaveAsFile(List<Dictionary<string, string>> tables)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = saveFileDialog.OpenFile())
                {
                    if (stream is null) throw new FileNotFoundException("ファイルが見つからないか、開けませんでした。");
                }
            }
        }
    }
}
