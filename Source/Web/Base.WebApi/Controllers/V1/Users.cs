namespace Base.WebApi.Controllers.V1;

/// <summary>
/// تایید اطلاعات کاربر
/// </summary>
[ApiVersion("1")]
public class Users : BaseController<Users, IUserInterfaces>
{
    public Users(ILogger<Users> logger, IUserInterfaces baseInterface) : base(logger, baseInterface)
    {
    }
    /// <summary>
    /// دریافت توکن برای کاربر
    /// </summary>
    /// <param name="userDto">اطلاعات کاربر به صورت مدل های قراردادی ار طریق این فیلد به سیستم وارد میشود</param>
    /// <param name="cancellationToken">در صورت لغو درخواست از طرف کاربر عملیات متوقف میشود </param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public virtual async Task<AccessToken> Token(UserDto userDto, CancellationToken cancellationToken) =>
        await BaseInterface.GetTokenAsync(userDto, cancellationToken);
}
