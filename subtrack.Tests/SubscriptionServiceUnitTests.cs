namespace subtrack.Tests;
public class SubscriptionServiceUnitTests : IntegrationTestBase
{
    private readonly SubscriptionService _sut;
    private readonly IDateProvider _dateProvider = Substitute.For<IDateProvider>();

    public SubscriptionServiceUnitTests()
    {
        _sut = new SubscriptionService(_dbContext, new SubscriptionsCalculator(_dateProvider));
    }

    [Fact]
    public async Task AutoPay_ShouldMarkAllDuePaymentsAsPaid()
    {
        _dateProvider.Today.Returns(new DateTime(2023, 6, 2));
        var expected = new DateTime(2023, 6, 1);

        var dueSubscription = CreateSubscription(expected.AddMonths(-3), expected.Day, autoPaid: true);
        SaveEntities(dueSubscription);

        var result = await _sut.AutoPayAsync(dueSubscription.Id);

        Assert.Equal(expected, result.LastPayment);
    }
}
