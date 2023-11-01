namespace subtrack.Tests.Integration;
public class SubscriptionServiceTests : IntegrationTestBase
{
    private readonly SubscriptionService _sut;
    private readonly IDateProvider _dateProvider = Substitute.For<IDateProvider>();

    public SubscriptionServiceTests()
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
