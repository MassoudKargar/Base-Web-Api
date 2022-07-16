namespace Base.Infrastructure.Databases.Connections;

public sealed class SqlConnectionString : ISingletonDependency, ISqlConnection
{
    public SqlConnectionString(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    private IConfiguration Configuration { get; }

    public IDbConnection GetDbConnectionAsync() =>
#if DEBUG
        new SqlConnection(Configuration.GetConnectionString("ServerDevelop"));
#else
        new SqlConnection(Configuration.GetConnectionString("ServerFinally"));
#endif

    public async Task<List<dynamic>> GetQueryMultipleAsync(dynamic dynamicParameters, string storedProcedure, CancellationToken cancellationToken)
    {
        using IDbConnection db = GetDbConnectionAsync();
        SqlMapper.GridReader? gridReader = null;
        List<dynamic> result = new();
        try
        {
            db.Open();
            DynamicParameters getSamplet = new(dynamicParameters);
            getSamplet.Add("@Result", null, DbType.Int64, ParameterDirection.ReturnValue);
            gridReader = await db.QueryMultipleAsync(
                storedProcedure,
                getSamplet,
                commandType: CommandType.StoredProcedure);
            while (true)
            {
                try
                {
                    result.Add(await gridReader.ReadAsync<dynamic>());
                }
                catch
                {
                    return result;
                }
            }
        }
        catch
        {
            return null;
        }
        finally
        {
            db.Close();
            db.Dispose();
        }
        return result;
    }
}