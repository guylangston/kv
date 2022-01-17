namespace kv.core;

public record KeyValue
{
    public KeyValue(string key, string value, string? typeHint, DateTime lastAccess, string lastAccessUser, DateTime created, string createdUser, DateTime modified, string modifiedUser)
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
    }

    public string Key { get; init; }
    public string Value { get; init; }
    public string? TypeHint { get; init; }

    public DateTime LastAccess { get; init; }
    public string LastAccessUser { get; init; }

    public DateTime Created { get; init; }
    public string CreatedUser { get; init; }

    public DateTime Modified { get; init; }
    public string ModifiedUser { get; init; }
}

