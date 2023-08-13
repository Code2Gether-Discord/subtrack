namespace subtrack.DAL.Entities
{
    public class DateTimeSetting : SettingsBase
    {
        public const string LastAutoPaymentTimeStampKey = "LastAutoPaymentTimeStamp";
        public DateTime? Value { get; set; }
    }
}
