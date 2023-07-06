using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
