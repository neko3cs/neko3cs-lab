using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace PracticeExcelVisualStudioToolsForOffice
{
    public partial class MainRibbon
    {
        private void MainRibbon_Load(object sender, RibbonUIEventArgs e) { }

        private void RunButton_Click(object sender, RibbonControlEventArgs e)
        {
            var sheets = Globals.ThisAddIn.Application.Sheets;

            var stream = GetStreamFromFileDialog();
            var dataSet = ConvertToDataSet(sheets);

            Save(stream, dataSet);
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

        private DataSet ConvertToDataSet(Sheets sheets)
        {
            var dataSet = new DataSet();

            foreach (Worksheet sheet in sheets)
            {
                var table = new DataTable();

                for (int rowIndex = 0; rowIndex < sheet.UsedRange.Rows.Count; rowIndex++)
                {
                    if (rowIndex is 0)
                    {
                        for (int columnIndex = 0; columnIndex < sheet.UsedRange.Columns.Count; columnIndex++)
                        {
                            table.Columns.Add(sheet.UsedRange[rowIndex, columnIndex].ToString(), typeof(string));  // FIXME: ここで落ちる。多分Excelにアクセス出来ていない
                        }

                        rowIndex++; // 2行目は型が入ってる。今回は無視
                    }
                    else
                    {
                        var row = table.NewRow();
                        for (int columnIndex = 0; columnIndex < sheet.UsedRange.Columns.Count; columnIndex++)
                        {
                            row[columnIndex] = sheet.UsedRange[rowIndex, columnIndex].ToString();
                        }
                    }
                }

                dataSet.Tables.Add(table);
            }

            return dataSet;
        }

        private void Save(FileStream stream, DataSet dataSet)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                var header = new StringBuilder();
                foreach (DataColumn column in table.Columns)
                {
                    header.Append($"{column.ColumnName}, ");
                }
                header.Remove(header.Length - 2, 2);
                WriteToStream(stream, header.ToString());

                var record = new StringBuilder();
                foreach (DataRow row in table.Rows)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        record.Append($"{row[i]}, ");
                    }
                }
                record.Remove(header.Length - 2, 2);
                WriteToStream(stream, record.ToString());
            }
        }

        private void WriteToStream(FileStream stream, string value)
        {
            var info = new UTF8Encoding(true).GetBytes(value);
            stream.Write(info, 0, info.Length);
        }
    }
}
