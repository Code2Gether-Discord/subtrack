namespace subtrack.MAUI.Services.Abstractions
{
    public interface IDateTimeProvider
    {
        public DateTime Now { get; }
        public DateTime NowUtc { get; }
    }
}
