using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    public class DateProvider : IDateProvider
    {
        public DateTime Today => DateTime.Today;
    }
}
