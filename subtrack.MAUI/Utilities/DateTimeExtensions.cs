using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Utilities;
public static class DateTimeExtensions
{
    public static TimeSpan TimeRemainingFromToday(this DateTime date, IDateProvider dateTimeProvider)
    {
        return date.Subtract(dateTimeProvider.Today);
    }

    public static bool IsPastDate(this DateTime date, IDateProvider dateTimeProvider)
    {
        return date < dateTimeProvider.Today;
    }
}
