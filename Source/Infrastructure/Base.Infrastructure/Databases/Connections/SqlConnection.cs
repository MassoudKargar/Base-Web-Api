namespace Base.Infrastructure.Databases.Connections;

public sealed class SqlConnectionString : ISingletonDependency, ISqlConnection
{
    public SqlConnectionString(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    private IConfiguration Configuration { get; }

    private IDbConnection GetDbConnectionAsync() =>
        new SqlConnection(Configuration.GetConnectionString("CommandServerFinally"));

    public async Task<SqlMapper.GridReader?> GetQueryMultipleAsync(dynamic dynamicParameters, string storedProcedure, string? databaseName, CancellationToken cancellationToken, bool setSubNameDatabase)
    {
        using IDbConnection db = GetDbConnectionAsync();
        SqlMapper.GridReader? gridReader;
        try
        {
            db.Open();
            DynamicParameters getSamplet = new(dynamicParameters);
            getSamplet.Add("@Result", null, DbType.Int64, ParameterDirection.ReturnValue);
            gridReader = await db.QueryMultipleAsync(
                storedProcedure,
                getSamplet,
                commandType: CommandType.StoredProcedure);
        }
        catch 
        { 
            gridReader = null;
        }
        finally
        {
            db.Close();
            db.Dispose();
        }

        return gridReader;
    }
}