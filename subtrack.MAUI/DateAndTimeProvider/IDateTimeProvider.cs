namespace subtrack.MAUI.DateAndTimeProvider
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset Now { get; }
    }
}
