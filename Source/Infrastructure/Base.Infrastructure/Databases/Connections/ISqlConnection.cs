using Base.Domain.Configuration;

namespace Base.Infrastructure.Databases.Connections;
public interface ISqlConnection
{
    IDbConnection GetDbConnectionAsync();
    /// <summary>
    /// اتصال با دیتابیس
    /// </summary>
    /// <param name="dynamicParameters"></param>
    /// <param name="storedProcedure"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<dynamic>> GetQueryMultipleAsync(dynamic dynamicParameters, string storedProcedure,CancellationToken cancellationToken);
}