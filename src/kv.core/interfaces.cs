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
    void Init(ProviderConfig config);
    void ConfirmInit();
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
}

// Single JSON file. Entire contents read into memory
public class StdJsonKeyValueProvider : IKeyValueProviderWithConfigInit
{
    IDictionary<string, KeyValue>? data;
    ProviderConfig? config;
    string? normFilePath;
    string? sysLock;

    public StdJsonKeyValueProvider()
    {
    }

    public void Init(ProviderConfig config)
    {
        if (config is null) throw new ArgumentNullException(nameof(config));

        // Global Config file path => $HOME/.config/global.json 

        var fi = new FileInfo(config.Get("XXX"));
        normFilePath = fi.FullName; //bin/ Normalise the path -- always the same, so useable for a global lock
    }

    public void Load()
    {
        ConfirmInit();
    }

    public void Commit()
    {
        ConfirmInit();
        using (var mutex = new System.Threading.Mutex(normFilePath))
        {
        }
    }

    private void ConfirmInit()
    {
        throw new NotImplementedException();
    }

    public void Delete(string key)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<KeyValue> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Store(KeyValue up)
    {
        throw new NotImplementedException();
    }

    public bool TryGet(string key, out KeyValue res)
    {
        throw new NotImplementedException();
    }
}
