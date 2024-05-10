using CsvHelper.TypeConversion;
using System.Text;

namespace subtrack.Tests.Integration;

public class SubscriptionImporterTests : IntegrationTestBase
{
    private ISubscriptionsImporter _sut;

    public SubscriptionImporterTests()
    {
        _sut = _serviceProvider.GetRequiredService<ISubscriptionsImporter>();
    }

    [Fact]
    public async Task ImportCsv_InvalidCsv_ShouldThrow()
    {
        var csvText = @"Name,Description,IsAutoPaid,Cost,FirstPaymentDay,LastPayment,BillingOccurrence,BillingInterval,PrimaryColor,Icon,SecondaryColor,NotificationDays
Subscription1,Description1,True,29.99,1,2023-12-01,Monthly,1,,,,,,,,
";
        var csv = new MemoryStream(Encoding.UTF8.GetBytes(csvText));

        await Assert.ThrowsAsync<TypeConverterException>(async () => await _sut.ImportFromCsvAsync(csv));
    }

    [Fact]
    public async Task ImportCsv_EmptyValuesForOptionalProperties_ShouldReturnImportedItems()
    {
        var csvText = @"Name,Description,IsAutoPaid,Cost,FirstPaymentDay,LastPayment,BillingOccurrence,BillingInterval,PrimaryColor,Icon,SecondaryColor,NotificationDays
Subscription1,Description1,True,29.99,1,2023-12-01,Month,1,,,,,,,,
Subscription2,,True,29.99,1,2023-12-01,Month,1,,,,,,,,
";
        var csv = new MemoryStream(Encoding.UTF8.GetBytes(csvText));

        var importedSubs = await _sut.ImportFromCsvAsync(csv);

        Assert.Equal(2, importedSubs.Count);
    }

    [Fact]
    public async Task ImportCsv_WithSameDetailsAsExisting_ShouldReturnImportedSubs()
    {
        var existingSubscription = CreateSubscription(DateTime.Today, description: null);
        var csvText = $@"Name,Description,IsAutoPaid,Cost,FirstPaymentDay,LastPayment,BillingOccurrence,BillingInterval,PrimaryColor,Icon,SecondaryColor,NotificationDays
{existingSubscription.Name},,{existingSubscription.IsAutoPaid},{existingSubscription.Cost},{existingSubscription.FirstPaymentDay},{existingSubscription.LastPayment},{existingSubscription.BillingOccurrence},{existingSubscription.BillingInterval},,,,,,
Subscription1,,True,29.99,1,2023-12-01,Month,1,,,,,,,,
Subscription2,,False,29.99,1,2022-12-01,Week,1,,,,,,,,
";
        var csv = new MemoryStream(Encoding.UTF8.GetBytes(csvText));

        var importedSubs = await _sut.ImportFromCsvAsync(csv);

        Assert.Equal(3, importedSubs.Count);
    }
}
