namespace Base.Infrastructure.Utilities;
/// <summary>
/// برای چک کردن خالی بودن یا نبودن هر چیزی در سطح پروژه به کار میرود
/// </summary>
public static class Assert
{
    /// <summary>
    /// برای چک کردن خالی بودن کلاس ها در سطح پروژه به کار میرود
    /// </summary>
    /// <typeparam name="T">نوع پارامتر ورودی دا دریافت میکند</typeparam>
    /// <param name="obj">نوع پارامتر ورودی دا دریافت میکند</param>
    /// <param name="name"> نام پارامتر ورودی دا دریافت میکند</param>
    /// <param name="message"> متن بازگشتی در صورت خالی بودن پارامتر ورودی بازگردانده میشود</param>
    public static void NotNull<T>(T obj, string name, string message = null) where T : class
    {
        if (obj is null) throw new ArgumentNullException($"{name} : {typeof(T)}", message);
    }

    /// <summary>
    ///ها در سطح پروژه به کار میرود struct برای چک کردن خالی بودن
    /// </summary>
    /// <typeparam name="T">نوع پارامتر ورودی دا دریافت میکند</typeparam>
    /// <param name="obj">نوع پارامتر ورودی دا دریافت میکند</param>
    /// <param name="name"> نام پارامتر ورودی دا دریافت میکند</param>
    /// <param name="message"> متن بازگشتی در صورت خالی بودن پارامتر ورودی بازگردانده میشود</param>
    public static void NotNull<T>(T? obj, string name, string message = null) where T : struct
    {
        if (!obj.HasValue) throw new ArgumentNullException($"{name} : {typeof(T)}", message);
    }

    /// <summary>
    /// برای چک کردن خالی بودن لیست ها در سطح پروژه به کار میرود 
    /// </summary>
    /// <typeparam name="T">نوع پارامتر ورودی دا دریافت میکند</typeparam>
    /// <param name="obj">نوع پارامتر ورودی دا دریافت میکند</param>
    /// <param name="name"> نام پارامتر ورودی دا دریافت میکند</param>
    /// <param name="message"> متن بازگشتی در صورت خالی بودن پارامتر ورودی بازگردانده میشود</param>
    /// <param name="defaultValue">  مقداری که انتظار میرود در پارامتر وردوی باشد را دریافت کرده و با پارامتر ورودی چک میکند</param>
    public static void NotEmpty<T>(T obj, string name, string message = null, T defaultValue = null)
        where T : class
    {
        if (obj == defaultValue
            || (obj is string str && string.IsNullOrWhiteSpace(str))
            || (obj is IEnumerable list && !list.Cast<object>().Any()))
        {
            throw new ArgumentException("Argument is empty : " + message, $"{name} : {typeof(T)}");
        }
    }
}
