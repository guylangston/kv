using System.Text.Json;

namespace kv.core;

public class StdJsonKeyValueProvider : IKeyValueProviderWithConfigInit
{
    private readonly Feedback feedback;
    private JsonDataFormat? data;
    ProviderConfig? config;
    string? normFilePath;
    string? sysLock;

    public StdJsonKeyValueProvider(Feedback feedback)
    {
        this.feedback = feedback;
    }

    public void Init(ProviderConfig cfg)
    {
        if (cfg is null) throw new ArgumentNullException(nameof(cfg));
        this.config = cfg;

        // Global Config file path => $HOME/.config/global.json 

        var fi = new FileInfo(cfg.Get("PathData"));
        normFilePath = fi.FullName; //bin/ Normalise the path -- always the same, so useable for a global lock
    }

    public void Load()
    {
        if (config == null || normFilePath == null) throw new Exception("Not Init()");

        using (var file = File.OpenRead(normFilePath))
        {
            data = JsonSerializer.Deserialize<JsonDataFormat>(file);
        }

        data.Globals["PathData"] = normFilePath;
        if (data.Data == null)
        {
            data.Data = new Dictionary<string, KeyValue>();
        }

    }

    public void Commit()
    {
        GuardInit();
        
        // TODO: Confirm file access lock

        using (var file = File.OpenWrite(normFilePath))
        {
            JsonSerializer.Serialize(file, data, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }
    }

    class JsonDataFormat
    {
        public Dictionary<string, string> Globals { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, KeyValue> Data { get; set; } 
    }
    

    public void GuardInit()
    {
        if (config == null || normFilePath == null) throw new Exception("No Initialised");
        if (data == null) throw new Exception();
    }

    public void Delete(string key)
    {
        GuardInit();
        data.Data.Remove(key);
    }

    public IReadOnlyCollection<KeyValue> GetAll()
    {
        GuardInit();
        return data.Data.Values.ToArray();
    }

    public void Store(KeyValue up)
    {
        GuardInit();
        data.Data[up.Key] = up;
    }

    public bool TryGet(string key, out KeyValue res)
    {
        GuardInit();
        return data.Data.TryGetValue(key, out res);
    }
}