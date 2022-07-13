namespace Base.Domain.Generics;

public class GenericsServiceDbDynamic
{
    public Guid ControllerId { get; set; }
    public Dictionary<string, dynamic> DynamicProperty { get; set; }
}
