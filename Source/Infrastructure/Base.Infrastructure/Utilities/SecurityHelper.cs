namespace Base.Infrastructure.Utilities;

/// <summary>
/// برای رمز نگاری متن ها کاربرد دارد
/// </summary>
public static class SecurityHelper
{
    /// <summary>
    /// به کار میرود Sha256 برای رمزنگاری داده ها به روش 
    /// </summary>
    /// <param name="input">پارامتری که باید رمز نگاری شود را دریافت میکند</param>
    /// <returns>پارامتر رمزنگاری شده را در قالب متن باز میگرداند</returns>
    public static string GetSha256Hash(string input) => Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));
    /// <summary>
    /// به کار میرود SHA512 برای رمزنگاری پسورد ها به روش 
    /// </summary>
    /// <param name="input">پارامتری که باید رمز نگاری شود را دریافت میکند</param>
    /// <returns>پارامتر رمزنگاری شده را در قالب متن باز میگرداند</returns>
    public static string HashFull(string input) => Convert.ToBase64String(SHA512.Create().ComputeHash(SHA384.Create().ComputeHash(SHA256.Create().ComputeHash(SHA1.Create().ComputeHash(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input)))))));
}