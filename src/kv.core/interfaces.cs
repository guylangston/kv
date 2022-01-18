namespace kv.core;

public interface IKeyValueProvider
{
    IReadOnlyCollection<KeyValue> GetAll();
    bool TryGet(string key, out KeyValue res);
    void Store(KeyValue up);
    void Delete(string key);
}

public interface IKeyValueProviderWithConfigInit : IKeyValueProvider
{
    void Init(ProviderConfig cfg);
    void GuardInit();
}

public class ProviderConfig
{
    public ProviderConfig(string provider, IReadOnlyDictionary<string, string> args)
    {
        Provider = provider;
        Args = args;
    }

    public string Provider { get; }
    public IReadOnlyDictionary<string, string> Args { get; }

    public string Get(string key) => Args[key];
}

public interface IKeyValueApi
{
    Task InitDataStore(string datastore);
    Task<string> Get(string datastore, string key); // return value
    Task Set(string datastore, string key, string value);
    Task Remove(string datastore, string key);
}

// Single JSON file. Entire contents read into memory