using System.Globalization;

namespace kv.core;

public static class KeyHelper
{
    public static bool IsValid(char c) => char.IsLetter(c) || c == '_' || c == '.';

    public static bool IsValid(string key) => key.All(IsValid);

    public static Type GuessType(string value)
    {
        if (DateTime.TryParse(value, out _)) return typeof(DateTime);
        if (double.TryParse(value, out _)) return typeof(double);
        if (Boolean.TryParse(value, out _)) return typeof(bool);

        return typeof(string);
    }
}