using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Utilities;

public static class DateTimeExtensions
{
    public static TimeSpan TimeRemainingFromToday(this DateTime date, IDateProvider dateTimeProvider) => date.Subtract(dateTimeProvider.Today);

    public static bool IsPastDate(this DateTime date, IDateProvider dateTimeProvider) => date < dateTimeProvider.Today;

    public static DateTime AddWeeks(this DateTime date, int weeks) => date.AddDays(weeks * 7);

    public static string MonthName(this DateTime date) => date.ToString("MMMM", Constants.UsCulture);

    public static DateTime LastDayOfMonthDate(this DateTime date) => new(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

    public static DateTime FirstDayOfMonthDate(this DateTime date) => new(date.Year, date.Month, 1);
}