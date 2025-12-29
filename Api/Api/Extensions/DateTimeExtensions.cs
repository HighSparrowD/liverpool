namespace Api.Extensions;

public static class DateTimeExtensions
{
    public static DateTime NormalizeDateTime(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime();
    }

    public static DateTime DropSecondsAndNormalize(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0).NormalizeDateTime();
    }
}