namespace ClosedXmlConsoleApp.Net6;

public class Table
{
    private readonly List<Row> _rows = new();

    public string Name { get; }
    public IReadOnlyList<Row> Rows => _rows;

    public Table(string name) => Name = name;

    public void AddRow(Row row) => _rows.Add(row);
}
