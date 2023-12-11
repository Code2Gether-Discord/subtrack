namespace subtrack.DAL.Entities
{
    public class DateTimeSetting : SettingsBase
    {
        public const string LastAutoPaymentTimeStampKey = "LastAutoPaymentTimeStamp";
        public const string LastSubscriptionExportTimeStampKey = "LastSubscriptionExportTimeStamp";
        public DateTime? Value { get; set; }
    }
}
