using System.Text;
using System.Text.Json;
using System.Xml.XPath;
using kv.core;

public class AppRunner
{
    private readonly TextWriter stdout;
    private readonly TextReader stdin;
    private readonly TextWriter err;
    private readonly string[] args;

    public AppRunner(TextWriter stdout, TextReader stdin, TextWriter err, string[] args)
    {
        this.stdout = stdout ?? throw new ArgumentNullException(nameof(stdout));
        this.stdin = stdin ?? throw new ArgumentNullException(nameof(stdin));
        this.err = err ?? throw new ArgumentNullException(nameof(err));
        this.args = args ?? throw new ArgumentNullException(nameof(args));
    }

    string ArgValueOrStdIn(int x)
    {
        if (args.Length < x) throw new Exception($"Argument No {x} missing");
        var s = args[x];
        if (s == "--stdin") return stdin.ReadToEnd();
        return s;
    }

    IKeyValueApi BuildController() => new KeyValueApi(new Feedback(stdout));

    void PrintError(string errTxt) => err.WriteLine(errTxt);

    void PrintHeader()
    {
        stdout.WriteLine("KV > trivial Key:Value CLI");
        stdout.WriteLine();
    }

    void PrintHelp()
    {
        stdout.WriteLine(
            @"kv init     {datastore}
kv ls       {datastore}
kv ls-keys  {datastore}
kv get      {datastore} <{keyname}|--stdin>
kv set      {datastore} {keyname} <{value}|--stdin>
kv rm       {datastore} {keyname}
kv rm-store {datastore}
kv export   {datastore} {format}
    where: f
        format = {json,header,csv,dict}
");
    }

    async Task ReportLS(IKeyValueApi keyValueApi, string[] strings)
    {
        var items = await keyValueApi.GetAll(strings[1]);
        foreach (var item in items)
        {
            stdout.Write($"| {item.TypeHint,-10}| {item.Key,15} | ");

            if (string.IsNullOrEmpty(item.Value))
            {
                stdout.WriteLine();
            }
            else
            {
                var cc = 0;
                using (var sr = new StringReader(item.Value))
                {
                    string? ln = null;

                    while ((ln = sr.ReadLine()) != null)
                    {
                        if (cc > 0) stdout.Write("\t > ");
                        stdout.WriteLine(ln);
                        cc++;
                    }
                }
            }
        }
    }

    async Task ReportExtract(IKeyValueApi api, string[] args1)
    {
        var items = await api.GetAll(args1[1]);

        if (args.Length >= 4)
        {
            var mob = args[2];
            var fmt = args[3];
            if (mob == "-f" && fmt == "header")
            {
                foreach (var pair in items)
                {
                    stdout.WriteLine($"{pair.Key}:{pair.Value}");
                }

                return;
            }
            else if (mob == "-f" && fmt == "csv")
            {
                foreach (var pair in items)
                {
                    stdout.WriteLine($"{pair.Key},{EncodeStringForCSV(pair.Value)}");
                }

                return;
            }
            else if (mob == "-f" && fmt == "dict")
            {
                foreach (var pair in items)
                {
                    stdout.WriteLine($"dict[\"{pair.Key}\"] = \"{pair.Value.Replace("\"", "\\\"")}\";");
                }

                return;
            }
            else if (mob == "-f" && fmt == "setter")
            {
                foreach (var pair in items)
                {
                    stdout.WriteLine($"obj.{pair.Key} = \"{pair.Value}\";");
                }

                return;
            }
            else if (mob == "-f" && fmt == "html")
            {
                stdout.WriteLine("<table>");
                foreach (var pair in items)
                {
                    stdout.WriteLine($"<tr> <th>{pair.Key}</th> <td>{pair.Value}</td> </tr>");
                }

                stdout.WriteLine("</table>");
                return;
            }
            else if (mob == "-f" && fmt == "xml")
            {
                stdout.WriteLine("<data>");
                foreach (var pair in items)
                {
                    stdout.WriteLine($" <value name='{pair.Key}'>{pair.Value}</value>");
                }

                stdout.WriteLine("</data>");
                return;
            }
        }

        var simple = items.ToDictionary(x => x.Key, x => x.Value);

        stdout.WriteLine(JsonSerializer.Serialize(simple));
    }

    /// <summary>
    /// Turn a string into a CSV cell output
    /// http://stackoverflow.com/questions/6377454/escaping-tricky-string-to-csv-format
    /// </summary>
    /// <param name="str">String to output</param>
    /// <returns>The CSV cell formatted string</returns>
    static string EncodeStringForCSV(string str)
    {
        if (str == null) return null;
        str = str.Replace("\n", "<br/>");
        bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
        if (mustQuote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char nextChar in str)
            {
                sb.Append(nextChar);
                if (nextChar == '"')
                    sb.Append("\"");
            }

            sb.Append("\"");
            return sb.ToString();
        }

        return str;
    }

    static string EncodeObjectForCSV(object value)
    {
        if (value == null) return null;
        if (value is DateTime || value is DateTime?)
        {
            return EncodeStringForCSV(((DateTime) value).ToString("O"));
        }

        return EncodeStringForCSV(value.ToString());
    }

    public async Task<int> Run()
    {
        var controller = BuildController();
        var cmd = args.First().ToUpperInvariant().Trim();

        try
        {
            switch (cmd)
            {
                case "INIT":
                    if (args.Length < 1)
                    {
                        PrintError("DataStore missing");
                        return 1;
                    }

                    await controller.InitDataStore(args[1]);
                    return 0;

                case "GET":
                    if (args.Length < 2)
                    {
                        PrintError("DataStore missing");
                        return 1;
                    }

                    stdout.WriteLine(await controller.Get(args[1], ArgValueOrStdIn(2)));
                    return 0;

                case "SET":
                    if (args.Length < 3)
                    {
                        PrintError("DataStore missing");
                        return 1;
                    }

                    await controller.Set(args[1], args[2], ArgValueOrStdIn(3));
                    return 0;

                case "LS":
                    if (args.Length < 1)
                    {
                        PrintError("DataStore missing");
                        return 1;
                    }

                    await ReportLS(controller, args);
                    return 0;


                case "EXPORT":
                    if (args.Length < 1)
                    {
                        PrintError("DataStore missing");
                        return 1;
                    }

                    await ReportExtract(controller, args);
                    return 0;

                default:
                    PrintHeader();
                    PrintHelp();
                    PrintError("Unknown Command");
                    return 1;
            }
        }
        catch (Exception e)
        {
            PrintError(e.Message);

#if DEBUG
            err.WriteLine(e);
#endif

            return 2;
        }
    }
}