namespace kv.core;

public class KeyValueApi : IKeyValueApi
{
    private readonly Feedback feedback;

    public KeyValueApi(Feedback feedback)
    {
        this.feedback = feedback;
    }

    public async Task InitDataStore(string datastore)
    {
        var pathDataStore = await GetDataStorePath(datastore, true, false);
        if (File.Exists(pathDataStore))
        {
            throw new Exception("Already Exists");
        }

        var cfg = new ProviderConfig(nameof(StdJsonKeyValueProvider), new Dictionary<string, string>()
        {
            {"PathData", pathDataStore}
        });
        var provider = new StdJsonKeyValueProvider(feedback);
        provider.Init(cfg);
        provider.CreateBlankStore();
    
        feedback.PrintMessage($"Creating File: {pathDataStore}");
        provider.Commit();
    }

    public async Task<string> GetDataStorePath(string datastore, bool createDir, bool mustExist)
    {
        datastore = datastore.ToLowerInvariant().Trim();
        if (!KeyHelper.IsValid(datastore)) throw new ArgumentException(nameof(datastore));
        
        var homedir = Environment.GetEnvironmentVariable("HOME") ?? throw new Exception("ENV:HOME not found");

        var cfgDir = Path.Combine(homedir, ".config/kv/");
        if (!Directory.Exists(cfgDir))
        {
            if (createDir)
            {
                feedback.PrintMessage($"Creating Directory: {cfgDir}");
                Directory.CreateDirectory(cfgDir);
            }
            else
            {
                throw new Exception($"Directory missing: {cfgDir}");
            }
        }
        
        var pathDataStore = Path.Combine(cfgDir, $"{datastore}.json");

        if (mustExist)
        {
            if (!File.Exists(pathDataStore))
            {
                throw new Exception($"DataStore missing: {pathDataStore}");
            }
            
        }
        
        return pathDataStore;
    }
    

    public async Task<string> Get(string datastore, string key)
    {
        var pathDataStore = await GetDataStorePath(datastore, false, true);
        
        var cfg = new ProviderConfig(nameof(StdJsonKeyValueProvider), new Dictionary<string, string>()
        {
            {"PathData", pathDataStore}
        });
        var provider = new StdJsonKeyValueProvider(feedback);
        provider.Init(cfg);
        provider.Load();
        
        if (provider.TryGet(key, out var res))
        {
            // TODO: Update stats and write?
            return res.Value;
        }
        else
        {
            throw new KeyNotFoundException($"Not Found: {key}");
        }
    }

    public async Task Set(string datastore, string key, string value)
    {
        var user = Environment.GetEnvironmentVariable("USER") ?? throw new Exception("User Not Set");
        var pathDataStore = await GetDataStorePath(datastore, false, true);

        var cfg = new ProviderConfig(nameof(StdJsonKeyValueProvider), new Dictionary<string, string>()
        {
            {"PathData", pathDataStore}
        });
        var provider = new StdJsonKeyValueProvider(feedback);
        provider.Init(cfg);
       
        provider.Load();

        if (provider.TryGet(key, out var exists))
        {
            exists = exists with
            {
               Value = value,
               TypeHint = KeyHelper.GuessType(value)?.Name,
               Modified = DateTime.Now,
               ModifiedUser = user
            };
            provider.Store(exists);
        }
        else
        {
            provider.Store(new KeyValue(key, value, KeyHelper.GuessType(value)?.Name, 
                DateTime.Now, user,
                DateTime.Now, user,
                DateTime.Now, user));
        }
        
        provider.Commit();

    }

    public Task Remove(string datastore, string key)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<KeyValue>> GetAll(string datastore)
    {
        var user = Environment.GetEnvironmentVariable("USER") ?? throw new Exception("User Not Set");
        var pathDataStore = await GetDataStorePath(datastore, false, true);

        var cfg = new ProviderConfig(nameof(StdJsonKeyValueProvider), new Dictionary<string, string>()
        {
            {"PathData", pathDataStore}
        });
        var provider = new StdJsonKeyValueProvider(feedback);
        provider.Init(cfg);
       
        provider.Load();

        return provider.GetAll();
    }
}