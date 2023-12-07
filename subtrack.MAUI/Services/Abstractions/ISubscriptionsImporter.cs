using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions;

public interface ISubscriptionsImporter
{
    Task<IReadOnlyCollection<Subscription>> ImportFromCsvAsync(Stream content);
}
