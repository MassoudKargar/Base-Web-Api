namespace Base.Application.Users;

public interface IUserInterfaces
{
    /// <summary>
    /// دریافت تایید اطلاعات کاربر از دیتابیس با استفاده از نام و رمز عبور
    /// </summary>
    /// <param name="cancellationToken">در صورت لغو درخواست از طرف کاربر عملیات را متوقف میکند </param>
    /// <returns></returns>
    Task<AccessToken> GetTokenAsync(
        UserDto userDto,
        CancellationToken cancellationToken);
}
