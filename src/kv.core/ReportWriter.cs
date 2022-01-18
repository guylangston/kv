using kv.core;


public abstract class ReportWriter
{
    public string FormatIdent { get; }
    public string Name { get; }

    protected ReportWriter(string formatIdent, string name)
    {
        FormatIdent = formatIdent;
        Name = name;
    }

    public abstract void WriteTo(TextWriter writer, IEnumerable<KeyValue> items);
}

public class ReportWriterCsv : ReportWriter
{
    public ReportWriterCsv() : base("csv", "Comma Seperated Values")
    {
    }

    public override void WriteTo(TextWriter writer, IEnumerable<KeyValue> items)
    {
        foreach (var pair in items)
        {
            writer.WriteLine($"{pair.Key},{DataFormatCsv.EncodeStringForCSV(pair.Value)}");
        }
    }
}

public class ReportWriterHtml : ReportWriter
{
    public ReportWriterHtml() : base("html", "Hyper Text Markup Language")
    {
    }

    public override void WriteTo(TextWriter writer, IEnumerable<KeyValue> items)
    {
        writer.WriteLine("<table>");
        foreach (var pair in items)
        {
            writer.WriteLine($"<tr> <th>{pair.Key}</th> <td>{pair.Value}</td> </tr>");
        }

        writer.WriteLine("</table>");
    }
}

public class ReportWriterXml : ReportWriter
{
    public ReportWriterXml() : base("xml", "eXtensibble Markup Language")
    {
    }

    public override void WriteTo(TextWriter writer, IEnumerable<KeyValue> items)
    {
        writer.WriteLine("<data>");
        foreach (var pair in items)
        {
            writer.WriteLine($" <value name='{pair.Key}'>{pair.Value}</value>");
        }

        writer.WriteLine("</data>");
    }
}