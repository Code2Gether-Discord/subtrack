namespace subtrack.Tests.DateAndTimeProvider
{
    public class FixedDateTimeProvider : IDateTimeProvider
    {
        private DateTime _fixedDateTime;

        public FixedDateTimeProvider(DateTime fixedDateTime) 
            => _fixedDateTime = fixedDateTime;

        public DateTime GetNow() => _fixedDateTime;

        public int DaysInMonth()
        {
            var firstDayOfMonth = _fixedDateTime;
            var daysInMonth = DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month);

            return daysInMonth;
        }
    }
}
