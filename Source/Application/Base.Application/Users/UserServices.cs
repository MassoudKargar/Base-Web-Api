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
    /// <param name="userDatabase">اطلاعات کاربر</param>
    /// <param name="logMessage"></param>
    /// <param name="delayTimeSpan"></param>
    /// <param name="cancellationToken">در صورت لغو درخواست از طرف کاربر عملیات را متوقف میکند </param>
    /// <returns></returns>
    async Task<AccessToken> IUserInterfaces.GetTokenAsync(
        UserDatabase userDatabase,
        UserType layerType,
        string logMessage,
        TimeSpan delayTimeSpan,
        CancellationToken cancellationToken)
    {
        string message = logMessage + $"|/|{nameof(IUserInterfaces.GetTokenAsync)}";
        if (userDatabase is null)
        {
            var ex = new BadRequestException(ApiResultStatusCode.BadRequest);
            await Task.FromException(ex);
            message = logMessage + $"|PNULL|";
            throw ex;
        }
        using IDbConnection db = SqlConnection.GetDbConnectionAsync(null);
        try
        {
            db.Open();
            message += $"|DBN=>CommandCenter|SP=>{UserStoredProcedure.UserLogin}";
            var getSamplet = new DynamicParameters(userDatabase);
            getSamplet.Add("@Result", null, DbType.Int64, ParameterDirection.ReturnValue);
            SqlMapper.GridReader v = await db.QueryMultipleAsync(
                UserStoredProcedure.UserLogin,
                userDatabase,
                commandType: CommandType.StoredProcedure);

            var user = await v.ReadFirstAsync<User>();
            var role = await v.ReadFirstAsync<Role>();

            if (user is null)
            {
                var ex = new UnauthorizedException("اطلاعات وارد شده اشتباه است")
                {
                    HttpStatusCode = HttpStatusCode.Unauthorized
                };
                message += $"|DER-1|";
                await Task.FromException(ex);
                throw ex;
            }
            if (role is null)
            {
                var ex = new UnauthorizedException("اطلاعات وارد شده اشتباه است")
                {
                    HttpStatusCode = HttpStatusCode.Unauthorized
                };
                message += $"|DER-1|";
                await Task.FromException(ex);
                throw ex;
            }

            switch (layerType)
            {
                case UserType.Admin:
                    {
                        if (role.RoleId == PersonRole.Operator) // برای سرور ادمین اجازه ورود کاربران را میبندیم
                        {
                            var ex = new BadRequestException("اطلاعات وارد شده اشتباه است")
                            {
                                HttpStatusCode = HttpStatusCode.Unauthorized
                            };
                            message += $"|DER-1|";
                            await Task.FromException(ex);
                            throw ex;
                        }
                        break;
                    }
                case UserType.Operator:
                    {
                        if (role.RoleId != PersonRole.Operator) // برای سرور ادمین اجازه ورود کاربران را میبندیم
                        {
                            var ex = new BadRequestException("اطلاعات وارد شده اشتباه است")
                            {
                                HttpStatusCode = HttpStatusCode.Unauthorized
                            };
                            message += $"|DER-1|";
                            await Task.FromException(ex);
                            throw ex;
                        }
                        break;
                    }
            }

            AccessToken jwt = JwtInterface.Generate(new()
            {
                { ClaimTypes.NameIdentifier, user.FullName },
                { ClaimTypes.Role, role.RoleId.ToString() },
                { UserClaimName.Ip, userDatabase.Ip ?? "" },
                { UserClaimName.UserName, userDatabase.UserName },
                { UserClaimName.ComputerName, userDatabase.ComputerName ?? "" },
                { UserClaimName.UserFullName, user.FullName },
                { UserClaimName.PersonId, user.PersonId.ToString() },
                { UserClaimName.UserId, user.UserId.ToString() },
                { UserClaimName.RoleName, role.RoleName },
                { UserClaimName.RoleCaption, role.RoleCaption },
                { UserClaimName.RoleId, role.RoleId.ToString() },
                { UserClaimName.PersonRoleId, role.PersonRoleId.ToString() }
            });

            jwt.RoleId = role.RoleId;
            jwt.UserId = role.PersonRoleId;
            message += $"|DC|";
            await Task.CompletedTask;
            jwt.FullName = user.FullName;
            return jwt;
        }
        catch (Exception e)//درصورت بروز خطا سیستم به سمت ارور هندلر هدایت میشود
        {
            Logger.LogCritical(e.Message);// متن خطا به عنوان لاگ در برنامه ثبت میشود
            var ex = new UnauthorizedException("اطلاعات وارد شده اشتباه است")
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
            await Task.FromException(ex);
            message += $"|DEM|";
            throw ex;
        }
        finally
        {
            var dT = DateTime.Now.TimeOfDay;
            message += $"|DT=>{(dT - delayTimeSpan)}";
            Logger.LogInformation(message);
            db.Close();
            db.Dispose();
        }

    }
}
