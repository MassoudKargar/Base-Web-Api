namespace Base.Infrastructure.Utilities;



public static class ReflectionHelper
{
    public static bool HasAttribute<T>(this MemberInfo type, bool inherit = false) where T : Attribute
        => HasAttribute(type, typeof(T), inherit);

    public static bool HasAttribute(this MemberInfo type, Type attribute, bool inherit = false)
        => Attribute.IsDefined(type, attribute, inherit);
    //return type.IsDefined(attribute, inherit);
    //return type.GetCustomAttributes(attribute, inherit).Length > 0;

    public static bool IsInheritFrom<T>(this Type type)
        => IsInheritFrom(type, typeof(T));

    public static bool IsInheritFrom(this Type type, Type parentType)
        => parentType.IsAssignableFrom(type);
    //the 'is' keyword do this too for values (new ChildClass() is ParentClass)

    public static bool BaseTypeIsGeneric(this Type type, Type genericType)
        => type.BaseType?.IsGenericType == true && type.BaseType.GetGenericTypeDefinition() == genericType;

    public static IEnumerable<Type> GetTypesAssignableFrom<T>(params Assembly[] assemblies)
        => typeof(T).GetTypesAssignableFrom(assemblies);

    public static IEnumerable<Type> GetTypesAssignableFrom(this Type type, params Assembly[] assemblies)
        => assemblies.SelectMany(p => p.GetTypes()).Where(p => p.IsInheritFrom(type));

    public static IEnumerable<Type> GetTypesHasAttribute<T>(params Assembly[] assemblies) where T : Attribute
        => typeof(T).GetTypesHasAttribute(assemblies);

    public static IEnumerable<Type> GetTypesHasAttribute(this Type type, params Assembly[] assemblies)
    => assemblies.SelectMany(p => p.GetTypes()).Where(p => p.HasAttribute(type));

    public static bool IsEnumerable(this Type type)
        => type != typeof(string) && type.IsInheritFrom<IEnumerable>();

    public static bool IsEnumerable<T>(this Type type)
        => type != typeof(string) && type.IsInheritFrom<IEnumerable<T>>() && type.IsGenericType;

    public static IEnumerable<Type> GetBaseTypesAndInterfaces(this Type type)
    {
        if ((type == null) || (type.BaseType == null))
            yield break;

        foreach (var i in type.GetInterfaces())
            yield return i;

        var currentBaseType = type.BaseType;
        while (currentBaseType != null)
        {
            yield return currentBaseType;
            currentBaseType = currentBaseType.BaseType;
        }
    }

    public static bool IsCustomType(this Type type) =>
        //return type.Assembly.GetName().Name != "mscorlib";
        type.IsCustomValueType() || type.IsCustomReferenceType();

    public static bool IsCustomValueType(this Type type)
    => type.IsValueType && !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System", StringComparison.Ordinal);

    public static bool IsCustomReferenceType(this Type type)
    => !type.IsValueType && !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System", StringComparison.Ordinal);
}
