using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
