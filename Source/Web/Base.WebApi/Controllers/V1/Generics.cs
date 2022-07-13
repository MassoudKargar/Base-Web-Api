namespace Base.WebApi.Controllers.V1;

/// <summary>
/// نمونه ها
/// </summary>
[ApiVersion("1")]
public class Generics : BaseController<Generics, IGenericsRepository>
{
    public Generics(ILogger<Generics> logger, IGenericsRepository baseInterface) : base(logger, baseInterface){}

    /// <summary>
    /// اتوماتیک سرویس 
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken">در صورت لغو درخواست از طرف کاربر عملیات متوقف میشود </param>
    /// <returns><![CDATA[ApiResult]]> تایید عملیات</returns>
    /// <exception cref="SecurityTokenExpiredException">در صورت نداشتن دسترسی خطا برمیکرداند</exception>
    [HttpPost]
    [CustomAuthorize(PersonRole.Admin, PersonRole.Operator)]
    public virtual async Task<IEnumerable<dynamic>> ServiceDynamic(GenericsServiceDbDynamicEntry dto, CancellationToken cancellationToken) =>
        await BaseInterface.GetServiceDynamic(dto, cancellationToken);
}
