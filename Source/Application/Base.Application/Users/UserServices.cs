using Base.Infrastructure.Utilities;

namespace Base.Application.Users;
public class UserServices : BaseService<UserServices>, IUserInterfaces
{
    public UserServices(IMapper mapper, ISqlConnection sqlConnection, ILogger<UserServices> logger, IJwtInterface jwtInterface) : base(mapper, sqlConnection, logger)
    {
        JwtInterface = jwtInterface;
    }
    private IJwtInterface JwtInterface { get; }

    /// <summary>
    /// دریافت تایید اطلاعات کاربر از دیتابیس با استفاده از نام و رمز عبور
    /// </summary>
    /// <param name="cancellationToken">در صورت لغو درخواست از طرف کاربر عملیات را متوقف میکند </param>
    /// <returns></returns>
    async Task<AccessToken> IUserInterfaces.GetTokenAsync(
        UserDto userDto,
        CancellationToken cancellationToken)
    {
        if (userDto.UserName.Trim().HasValue() && !userDto.Password.Trim().HasValue())
        {
            throw new BadRequestException("اطلاعات وارد شده اشتباه است");
        }
        var userDatabase = userDto.ToEntity(Mapper).AutoCleanString();
        if (userDatabase is null)
        {
            throw new BadRequestException(ApiResultStatusCode.BadRequest);
        }

        var gridReader = await SqlConnection.GetQueryMultipleAsync(userDatabase, UserStoredProcedure.UserLogin, null, cancellationToken);

        if (gridReader is null)
        {
            var ex = new UnauthorizedException("اطلاعات وارد شده اشتباه است")
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
            await Task.FromException(ex);
            throw ex;
        }
        User? user = await gridReader.ReadFirstOrDefaultAsync<User>();
        if (user is null)
        {
            throw new UnauthorizedException("اطلاعات وارد شده اشتباه است")
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
        }
        Role? role = await gridReader.ReadFirstOrDefaultAsync<Role>();
        if (role is null)
        {
            throw new UnauthorizedException("اطلاعات وارد شده اشتباه است")
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
        }

        AccessToken jwt = JwtInterface.Generate(new()
        {
            { ClaimTypes.NameIdentifier, user.FullName },
            { UserClaimName.UserFullName, user.FullName },
            { UserClaimName.PersonId, user.PersonId.ToString() },
            { UserClaimName.UserId, user.UserId.ToString() },

            { ClaimTypes.Role, role.RoleId.ToString() },
            { UserClaimName.RoleName, role.RoleName },
            { UserClaimName.RoleCaption, role.RoleCaption },
            { UserClaimName.RoleId, role.RoleId.ToString() },
            { UserClaimName.PersonRoleId, role.PersonRoleId.ToString() },
        });
        jwt.RoleId = role.RoleId;
        jwt.FullName = user.FullName;
        return jwt;
    }
}
