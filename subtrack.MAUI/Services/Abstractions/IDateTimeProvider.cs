namespace subtrack.MAUI.Services.Abstractions
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset Now { get; }
    }
}
