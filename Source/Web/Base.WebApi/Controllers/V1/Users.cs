namespace Base.WebApi.Controllers.V1;

/// <summary>
/// تایید اطلاعات کاربر
/// </summary>
[ApiVersion("1")]
public class Users : BaseController<Users, IUserInterfaces>
{
    public Users(IJwtInterface jwtInterface, IMapper mapper, ILogger<Users> logger, IUserInterfaces baseInterface) : base(jwtInterface, mapper, logger, baseInterface)
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
        !userDto.UserName.Trim().HasValue() &&
        !userDto.Password.Trim().HasValue()
            ? throw new BadRequestException("اطلاعات وارد شده اشتباه است")
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            }
            : await BaseInterface.GetTokenAsync(
            userDto.ToEntity(Mapper).AutoCleanString(),
            UserType.Admin,
            $"{userDto.UserName}=>{nameof(Token)}",
            DateTime.Now.TimeOfDay,
            cancellationToken);
}
