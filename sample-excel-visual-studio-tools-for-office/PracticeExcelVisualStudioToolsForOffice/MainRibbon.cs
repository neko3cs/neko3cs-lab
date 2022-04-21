using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using PracticeExcelVisualStudioToolsForOffice.Extensions;
using PracticeExcelVisualStudioToolsForOffice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PracticeExcelVisualStudioToolsForOffice
{
    public partial class MainRibbon
    {
        private void MainRibbon_Load(object sender, RibbonUIEventArgs e) { }

        private void RunButton_Click(object sender, RibbonControlEventArgs e)
        {
            using (var stream = GetStreamFromFileDialog())
            {
                var tables = ConvertToTables(Globals.ThisAddIn.Application.Sheets);
                Save(stream, tables);

                MessageBox.Show(@"テストデータ作成ツール", @"出力が完了しました。", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private FileStream GetStreamFromFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return null;

            var stream = saveFileDialog.OpenFile();
            if (stream is null) throw new FileNotFoundException();

            return stream as FileStream;
        }

        private IEnumerable<Table> ConvertToTables(Sheets sheets)
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

        private void Save(FileStream stream, IEnumerable<Table> tables)
        {
            foreach (Table table in tables)
            {
                stream.WriteTextLine($"◆{table.Name}");

                var csvHeader = string.Join(",", table.Rows.First().Columns.Keys);
                stream.WriteTextLine(csvHeader);

                foreach (Row row in table.Rows)
                {
                    var csvRecord = string.Join(",", row.Columns.Values);
                    stream.WriteTextLine(csvRecord);
                }

                stream.WriteTextLine();
            }
        }
    }
}
