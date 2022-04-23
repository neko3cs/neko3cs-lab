using System.Collections.Generic;

namespace SampleExcelVisualStudioToolsForOffice.Models
{
    public class Row
    {
        private readonly Dictionary<string, string> _columns = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> Columns => _columns;

        public void AddColumn(string columnName, string value) => _columns.Add(columnName, value);
    }
}
