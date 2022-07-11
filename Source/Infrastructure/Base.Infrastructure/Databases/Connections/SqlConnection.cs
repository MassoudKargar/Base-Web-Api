namespace Base.Infrastructure.Databases.Connections;

sealed class SqlConnectionString : ISingletonDependency, ISqlConnection
{
    public SqlConnectionString(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    //برای جا ب جا کردن کانکشنن های سرور دولپ و سرور اصلی

    private readonly string Server = "Finally";//سرور نهایی
                                               //private readonly string Server = "Development";  //سرور دولپ

    private IConfiguration Configuration { get; }
    IDbConnection ISqlConnection.GetDbConnectionAsync(string? databaseName, bool setSubNameDatabase)
    {
        string? dbname = setSubNameDatabase ? "prj" + databaseName : databaseName;
        return Server switch
        {
            "Finally" => new SqlConnection(databaseName switch
            {
                null => Configuration.GetConnectionString("CommandServerFinally"),
                _ => Configuration.GetConnectionString("OtherServerFinally").Replace("DBNAME", dbname),
            }),
            _ => new SqlConnection(databaseName switch
            {
                null => Configuration.GetConnectionString("CommandServer"),
                _ => Configuration.GetConnectionString("OtherServer").Replace("DBNAME", dbname),
            })
        };
    }
    SqlConnection ISqlConnection.GetSqlConnection(string? databaseName, bool setSubNameDatabase)
    {
        string? dbName = setSubNameDatabase ? "prj" + databaseName : databaseName;
        return Server switch
        {
            "Finally" => new SqlConnection(databaseName switch
            {
                null => Configuration.GetConnectionString("CommandServerFinally"),
                _ => Configuration.GetConnectionString("OtherServerFinally").Replace("DBNAME", dbName),
            }),
            _ => new SqlConnection(databaseName switch
            {
                null => Configuration.GetConnectionString("CommandServer"),
                _ => Configuration.GetConnectionString("OtherServer").Replace("DBNAME", dbName),
            })
        };
    }
    NpgsqlConnection ISqlConnection.WebRtcServer() => new(Configuration.GetConnectionString("WebRtcServerFinally"));

    NpgsqlConnection ISqlConnection.FreeSwitchPlineServer(string ip)
    {
        return new(Configuration.GetConnectionString("FreeSwitchServerFinally").Replace("IP", ip));
    }
}