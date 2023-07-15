using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Utilities;
public static class DateTimeExtensions
{
    public static TimeSpan TimeRemainingFromToday(this DateTime date, IDateTimeProvider dateTimeProvider)
    {
        return date.Subtract(dateTimeProvider.Now.Date);
    }

    public static bool IsPastDate(this DateTime date, IDateTimeProvider dateTimeProvider)
    {
        return date < dateTimeProvider.Now.Date;
    }
}
