using System.Diagnostics;
using kv.console;

var runner = new AppRunner(Console.Out, Console.In, Console.Error, args);
try
{
    return await runner.Run();
}
catch (Exception ex)
{
    Debug.WriteLine(ex.ToString());
    return 100;
}