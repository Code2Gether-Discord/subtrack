namespace subtrack.Tests.DateAndTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;
    }
}
