namespace Base.Application.Jwt;
public interface IJwtInterface
{
    /// <summary>
    /// این متود برای ساخت توکن و بازگرداندن توکن به کاربر میباشد
    /// </summary>
    /// <param name="otherCash">اطلاعات کاربر را گرفته و برای ساخت توکن استفاده میکند</param>
    /// <returns> باز میگرداند <![CDATA[AccessToken]]> توکن در قالب </returns>
    AccessToken Generate(Dictionary<string, string> otherCash);

    /// <summary>
    /// آپدیت کردن توکن
    /// </summary>
    /// <param name="claims">اطلاعاتی که میخواهیم درتوکن ذخیره شود</param>
    /// <returns><![CDATA[string]]>توکن رمزنگاری شده</returns>
    string UpdateToken(IEnumerable<Claim> claims);

    /// <summary>
    /// بازگشایی توکن با استفاده از کلید های مخصوص
    /// </summary>
    /// <param name="token">توکن کاربر</param>
    /// <returns><![CDATA[ClaimsPrincipal]]>اطلاعات کاربر</returns>
    /// <returns><![CDATA[JwtSecurityToken]]>اطلاعات توکن</returns>
    (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);

    /// <summary>
    /// بازگردن توکن
    /// </summary>
    /// <param name="token">توکن کاربر</param>
    /// <param name="claimTypes">اطلاعات درخواستی</param>
    /// <returns><![CDATA[List<Claim>]]> اطلاعات درخواستی را به صورت لیست باز میکرداند</returns>
    List<Claim> ReadToken(string token, params string[] claimTypes);
}
