using System.Collections.Immutable;


public class ReportProvider
{
    public IReadOnlyDictionary<string, ReportWriter> Reports { get; }

    public ReportProvider()
    {
        Reports = ReflectionHelper.AllTypesImplementing<ReportWriter>()
            .Select(x => (ReportWriter)Activator.CreateInstance(x))
            .ToImmutableDictionary(x => x.FormatIdent);
    }
}