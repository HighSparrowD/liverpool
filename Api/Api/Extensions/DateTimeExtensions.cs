namespace Api.Extensions;

public static class DateTimeExtensions
{
    public static DateTime NormalizeDateTime(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime();
    }
}