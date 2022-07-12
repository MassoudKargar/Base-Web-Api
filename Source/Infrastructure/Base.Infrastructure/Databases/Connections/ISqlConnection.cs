namespace Base.Infrastructure.Databases.Connections;
public interface ISqlConnection
{
    /// <summary>
    /// اتصال با دیتابیس
    /// </summary>
    /// <param name="dynamicParameters"></param>
    /// <param name="storedProcedure"></param>
    /// <param name="databaseName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="setSubNameDatabase"></param>
    /// <returns></returns>
    Task<SqlMapper.GridReader?> GetQueryMultipleAsync(dynamic dynamicParameters, string storedProcedure, string? databaseName, CancellationToken cancellationToken, bool setSubNameDatabase = true);
}