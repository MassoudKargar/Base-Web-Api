namespace Base.Infrastructure.Databases.Connections;
public interface ISqlConnection
{
    /// <summary>
    ///  کانکشن دیتابیس
    /// </summary>
    /// <param name="databaseName">در صورت ارسال نام دیتابیس آن دا در کانکشن تعویض میکند</param>
    /// <param name="setSubNameDatabase"></param>
    /// <returns>new <![CDATA[IDbConnection]]></returns>
    IDbConnection GetDbConnectionAsync(string? databaseName, bool setSubNameDatabase = true);

    /// <summary>
    ///  کانکشن دیتابیس
    /// </summary>
    /// <param name="databaseName"></param>
    /// <param name="setSubNameDatabase"></param>
    /// <returns></returns>
    SqlConnection GetSqlConnection(string? databaseName, bool setSubNameDatabase = true);

    /// <summary>
    /// کانکشن سرور تماس
    /// </summary>
    /// <returns></returns>
    NpgsqlConnection WebRtcServer();

    /// <summary>
    /// freeswitch کانکشن سرور  
    /// </summary>
    /// <returns></returns>
    NpgsqlConnection FreeSwitchPlineServer(string ip);

}