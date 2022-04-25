namespace ClosedXmlConsoleApp.Net6;

public class Row
{
    private readonly Dictionary<string, string> _columns = new();

    public IReadOnlyDictionary<string, string> Columns => _columns;

    public void AddColumn(string columnName, string value) => _columns.Add(columnName, value);
}
