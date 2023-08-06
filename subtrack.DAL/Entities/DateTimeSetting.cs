using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtrack.DAL.Entities
{
    public class DateTimeSetting : SettingsBase
    {
        public const string Key = "LastAutoPaymentTimeStampKey";
        public DateTime? Value { get; set; }
    }
}
