using System;
using System.IO;
using System.Text;

namespace PracticeExcelVisualStudioToolsForOffice.Extensions
{
    public static class FileStreamExtensions
    {
        public static void WriteText(this FileStream stream, string value)
        {
            var info = new UTF8Encoding(true).GetBytes(value);
            stream.Write(info, 0, info.Length);
        }

        public static void WriteTextLine(this FileStream stream, string value = "") =>
            WriteText(stream, $"{value}{Environment.NewLine}");
    }
}
