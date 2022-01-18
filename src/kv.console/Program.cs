using kv.core;

if (args.Length == 0)
{
    PrintHeader();
    PrintHelp();
    return 1;
}

var controller = BuildController(args);
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

            Console.WriteLine(await controller.Get(args[1], args[2]));
            return 0;

        case "SET":
            if (args.Length < 3)
            {
                PrintError("DataStore missing");
                return 1;
            }

            await controller.Set(args[1], args[2], args[3]);
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
    Console.Error.WriteLine(e);
#endif

    return 2;
}

IKeyValueApi BuildController(string[] args)
{
    return new KeyValueApi();
}

void PrintError(string err)
{
    Console.Error.WriteLine(err);
}


void PrintHeader()
{
    Console.WriteLine("KV - Key:Value");
    Console.WriteLine();
}

void PrintHelp()
{
    Console.WriteLine(
        @"kv init {datastore}
kv get -ds {datastore} -k {keyname}
kv set -ds {datastore} -k {keyname} -v {value}
kv set -ds {datastore} -k {keyname} <<< {value}
kv rm -ds {datastore} -k {keyname}
kv rm -ds {datastore}
");
}