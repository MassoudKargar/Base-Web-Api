namespace Base.Domain.Generics;
using System;
using System.Collections.Generic;

public class GenericsServiceDbDynamicEntry : BaseDto<GenericsServiceDbDynamicEntry, GenericsServiceDbDynamic>
{
    public Guid ControllerId { get; set; }
    public Dictionary<string, dynamic> DynamicProperty { get; set; }
}
