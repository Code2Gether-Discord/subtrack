namespace subtrack.MAUI.Services.Abstractions
{
    public interface IDateProvider
    {
        public DateTime Today { get; }
        public DateTime Now { get; }
    }
}
