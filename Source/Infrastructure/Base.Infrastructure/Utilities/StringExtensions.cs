namespace Base.Infrastructure.Utilities;

/// <summary>
/// تبدیل متن به مقدار های مورد نظر
/// </summary>
public static class StringExtensions
{
    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
    => ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    public static bool HasValue(this StringValues value, bool ignoreWhiteSpace = true)
    => ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    public static int ToInt(this string value)
    => Convert.ToInt32(value);
    public static decimal ToDecimal(this string value)
    => Convert.ToDecimal(value);
    public static string ToNumeric(this int value)
    => value.ToString("N0");
    public static string ToNumeric(this decimal value)
    => value.ToString("N0");
    public static string ToCurrency(this int value)
    => value.ToString("C0");
    public static string ToCurrency(this decimal value)
    => value.ToString("C0");
    public static string En2Fa(this string str)
    => str.Replace("0", "۰")
        .Replace("1", "۱")
        .Replace("2", "۲")
        .Replace("3", "۳")
        .Replace("4", "۴")
        .Replace("5", "۵")
        .Replace("6", "۶")
        .Replace("7", "۷")
        .Replace("8", "۸")
        .Replace("9", "۹");
    public static string Fa2En(this string str)
        => str.HasValue()
            ? str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9")
            : null;
    public static string SecondsToMinutes(string seconds)
        => $"{(int)Math.Truncate(TimeSpan.FromSeconds(double.Parse(seconds)).TotalHours):D2}" +
           $":{TimeSpan.FromSeconds(double.Parse(seconds)).Minutes:D2}" +
           $":{TimeSpan.FromSeconds(double.Parse(seconds)).Seconds:D2}";
    /// <summary>
    /// 1 => 01
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToFullNumber(this string str)

    => str.Replace("0", "00")
        .Replace("1", "01")
        .Replace("2", "02")
        .Replace("3", "03")
        .Replace("4", "04")
        .Replace("5", "05")
        .Replace("6", "06")
        .Replace("7", "07")
        .Replace("8", "08")
        .Replace("9", "09");
    public static string FixPersianChars(this string str)
    => str.Replace("ﮎ", "ک")
        .Replace("ﮏ", "ک")
        .Replace("ﮐ", "ک")
        .Replace("ﮑ", "ک")
        .Replace("ك", "ک")
        .Replace("ي", "ی")
        .Replace(" ", " ")
        .Replace("‌", " ")
        .Replace("ھ", "ه");
    public static string FixSqlInjectionChars(this string str)
    => str.Replace("'", " ")
        .Replace("%", " ")
        .Replace("=", " ")
        .Replace("-", " ")
        .Replace("or", " ")
        .Replace("OR", " ")
        .Replace("Or", " ")
        .Replace("oR", " ")
        .Replace(";", " ")
        .Replace("|", " ")
        .Replace("*", " ")
        .Replace("#", " ");
    public static string NullIfEmpty(this string str) => str?.Length == 0 ? null : str;
    public static string CleanString(this string str) =>
        str
            .Trim()
            .FixPersianChars()
            .Fa2En()
            /*.FixSqlInjectionChars()*/
            .NullIfEmpty();
    public static T AutoCleanString<T>(this T entity) where T : class
    {
        var entityType = typeof(T);

        foreach (var property in entityType.GetProperties())
        {
            var p = entityType.GetProperty(property.Name);
            var type = property.PropertyType;
            var value = p.GetValue(entity, null);
            if (value != null && type == typeof(string))
            {
                var newValue = ((string)value).CleanString();
                property.SetValue(entity, newValue);
            }
        }
        return entity;
    }
    /// <summary>
    /// Removes first occurrence of the given postfixes from end of the given string.
    /// Ordering is important. If one of the postFixes is matched, others will not be tested.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="postFixes">one or more postfix.</param>
    /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
    public static string RemovePostFix(this string str, params string[] postFixes)
    {
        if (str == null)
            return (string)null;
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        if (((ICollection<string>)postFixes).IsNullOrEmpty<string>())
            return str;
        foreach (string postFix in postFixes)
        {
            if (str.EndsWith(postFix))
                return str.Left(str.Length - postFix.Length);
        }
        return str;
    }
    /// <summary>
    /// Gets a substring of a string from beginning of the string.
    /// </summary>
    /// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
    /// <exception cref="T:System.ArgumentException">Thrown if <paramref name="len" /> is bigger that string's length</exception>
    public static string Left(this string str, int len)
    {
        if (str == null)
            throw new ArgumentNullException(nameof(str));
        if (str.Length < len)
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        return str[..len];
    }

    //
    // Summary:
    //     Converts English digits of a given number to their equivalent Persian digits.
    public static string ToPersianNumbers(this int number, string format = "")
    {
        return ((!string.IsNullOrEmpty(format)) ? number.ToString(format, CultureInfo.InvariantCulture) : number.ToString(CultureInfo.InvariantCulture)).ToPersianNumbers();
    }

    //
    // Summary:
    //     Converts English digits of a given number to their equivalent Persian digits.
    public static string ToPersianNumbers(this long number, string format = "")
    {
        return ((!string.IsNullOrEmpty(format)) ? number.ToString(format, CultureInfo.InvariantCulture) : number.ToString(CultureInfo.InvariantCulture)).ToPersianNumbers();
    }

    //
    // Summary:
    //     Converts English digits of a given number to their equivalent Persian digits.
    public static string ToPersianNumbers(this int? number, string format = "")
    {
        if (!number.HasValue)
        {
            number = 0;
        }

        return ((!string.IsNullOrEmpty(format)) ? number.Value.ToString(format, CultureInfo.InvariantCulture) : number.Value.ToString(CultureInfo.InvariantCulture)).ToPersianNumbers();
    }

    //
    // Summary:
    //     Converts English digits of a given number to their equivalent Persian digits.
    public static string ToPersianNumbers(this long? number, string format = "")
    {
        if (!number.HasValue)
        {
            number = 0L;
        }

        return ((!string.IsNullOrEmpty(format)) ? number.Value.ToString(format, CultureInfo.InvariantCulture) : number.Value.ToString(CultureInfo.InvariantCulture)).ToPersianNumbers();
    }

    //
    // Summary:
    //     Converts English digits of a given string to their equivalent Persian digits.
    //
    // Parameters:
    //   data:
    //     English number
    public static string ToPersianNumbers(this string? data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        char[] array = data!.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            switch (array[i])
            {
                case '0':
                case '٠':
                    array[i] = '۰';
                    break;
                case '1':
                case '١':
                    array[i] = '۱';
                    break;
                case '2':
                case '٢':
                    array[i] = '۲';
                    break;
                case '3':
                case '٣':
                    array[i] = '۳';
                    break;
                case '4':
                case '٤':
                    array[i] = '۴';
                    break;
                case '5':
                case '٥':
                    array[i] = '۵';
                    break;
                case '6':
                case '٦':
                    array[i] = '۶';
                    break;
                case '7':
                case '٧':
                    array[i] = '۷';
                    break;
                case '8':
                case '٨':
                    array[i] = '۸';
                    break;
                case '9':
                case '٩':
                    array[i] = '۹';
                    break;
            }
        }

        return new string(array);
    }

    //
    // Summary:
    //     Converts Persian and Arabic digits of a given string to their equivalent English
    //     digits.
    //
    // Parameters:
    //   data:
    //     Persian number
    public static string ToEnglishNumbers(this string? data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        char[] array = data!.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            switch (array[i])
            {
                case '٠':
                case '۰':
                    array[i] = '0';
                    break;
                case '١':
                case '۱':
                    array[i] = '1';
                    break;
                case '٢':
                case '۲':
                    array[i] = '2';
                    break;
                case '٣':
                case '۳':
                    array[i] = '3';
                    break;
                case '٤':
                case '۴':
                    array[i] = '4';
                    break;
                case '٥':
                case '۵':
                    array[i] = '5';
                    break;
                case '٦':
                case '۶':
                    array[i] = '6';
                    break;
                case '٧':
                case '۷':
                    array[i] = '7';
                    break;
                case '٨':
                case '۸':
                    array[i] = '8';
                    break;
                case '٩':
                case '۹':
                    array[i] = '9';
                    break;
            }
        }

        return new string(array);
    }
}
