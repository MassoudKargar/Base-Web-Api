namespace Base.Application.Generics;

public interface IGenericsRepository
{
    Task<IEnumerable<dynamic>> GetServiceDynamic(GenericsServiceDbDynamicEntry dbDynamic,CancellationToken cancellationToken);
}
