namespace Base.Application.Users;

public interface IUserInterfaces
{
    /// <summary>
    /// دریافت تایید اطلاعات کاربر از دیتابیس با استفاده از نام و رمز عبور
    /// </summary>
    /// <param name="userDatabase">اطلاعات کاربر</param>
    /// <param name="logMessage"></param>
    /// <param name="delayTimeSpan"></param>
    /// <param name="cancellationToken">در صورت لغو درخواست از طرف کاربر عملیات را متوقف میکند </param>
    /// <returns></returns>
    Task<AccessToken> GetTokenAsync(
        UserDatabase userDatabase,
        UserType layerType,
        string logMessage,
        TimeSpan delayTimeSpan,
        CancellationToken cancellationToken);
}
