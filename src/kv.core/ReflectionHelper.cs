
public static class ReflectionHelper
{
    public static IEnumerable<Type> AllTypesImplementing<T>()
    {
        return typeof(T)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);
    }
}