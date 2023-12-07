using CsvHelper;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;
using System.Globalization;

namespace subtrack.MAUI.Services;
internal class SubscriptionsImporter : ISubscriptionsImporter
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsImporter(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IReadOnlyCollection<Subscription>> ImportFromCsvAsync(Stream content)
    {
        using var csv = new StreamReader(content);
        using var csvReader = new CsvReader(csv, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<SubscriptionCsvMapping>();

        var subscriptionsToImport = new List<Subscription>();
        await foreach (var subscription in csvReader.GetRecordsAsync<Subscription>())
        {
            subscriptionsToImport.Add(subscription);
        }

        return await _subscriptionService.CreateSubscriptionsAsync(subscriptionsToImport);
    }
}
