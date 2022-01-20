namespace kv.core;

public enum ValueType
{
    Raw,
    AllowVars,      // "Normal Text ${somekey}, also £{somestore:somekey} with escaping \$ 100.00"
    Alias           // points to another key or store:key
}

public record KeyValue
{
    public KeyValue(string key, string value, string? typeHint, DateTime lastAccess, string lastAccessUser, DateTime created, string createdUser, DateTime modified, string modifiedUser, ValueType type)
    {
        Key = key;
        Value = value;
        TypeHint = typeHint;
        LastAccess = lastAccess;
        LastAccessUser = lastAccessUser;
        Created = created;
        CreatedUser = createdUser;
        Modified = modified;
        ModifiedUser = modifiedUser;
        Type = type;
    }

    // Core
    public string Key { get; init; }
    public string Value { get; init; }
    
    // Features
    public ValueType Type { get; init; }
    
    // Optional
    public string? TypeHint { get; init; }
    
    // Access Controls
    public DateTime LastAccess { get; init; }
    public string LastAccessUser { get; init; }

    public DateTime Created { get; init; }
    public string CreatedUser { get; init; }

    public DateTime Modified { get; init; }
    public string ModifiedUser { get; init; }
}

