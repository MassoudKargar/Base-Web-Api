namespace Base.Infrastructure.Utilities;

public static class ServiceSerialize
{
    public static void ForAll<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action(item);
        }
    }
    public static string JsonSerialize(object? obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        });
    }
}