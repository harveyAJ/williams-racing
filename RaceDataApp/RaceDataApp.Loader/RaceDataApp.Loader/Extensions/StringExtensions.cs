using System.Globalization;

namespace RaceDataApp.Loader.Extensions;

public static class StringExtensions
{
    public static DateTime? ToDateTime(this string value)
    {
        return string.IsNullOrWhiteSpace(value) || value == "\\N"
            ? null
            : DateTime.ParseExact(value.Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
    
    public static TimeSpan? ToTimeSpan(this string value)
    {
        return string.IsNullOrWhiteSpace(value) || value == "\\N"
            ? null
            : TimeSpan.Parse(value.Trim('"'), CultureInfo.InvariantCulture);
    }
}