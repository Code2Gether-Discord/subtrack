using CsvHelper.Configuration;
using subtrack.DAL.Entities;
using System.Globalization;

namespace subtrack.MAUI.Utilities;

public class SubscriptionCsvMapping : ClassMap<Subscription>
{
    public SubscriptionCsvMapping()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.Id).Ignore();
    }
}
