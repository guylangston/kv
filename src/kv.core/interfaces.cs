namespace kv.core;

public interface IKeyValueProvider 
{
   IReadOnlyCollection<KeyValue> GetAll();
   bool TryGet(string key, out KeyValue res);
   void Store(KeyValue up);
   void Delete(string key);
}

public class ProviderConfig
{
   public string Provider { get; set; }
   public IReadOnlyDictionary<string, string> Args { get; set; }
}
