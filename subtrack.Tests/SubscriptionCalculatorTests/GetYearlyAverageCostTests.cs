namespace subtrack.Tests.SubscriptionCalculatorTests;
public class GetYearlyAverageCostTests
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateProvider _dateTimeProvider = Substitute.For<IDateProvider>();

    public GetYearlyAverageCostTests()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
    }

    [Theory]
    [ClassData(typeof(GetYearlyCostsTestData))]
    public void GetYearlyCost_ReturnsCorrectCost(Subscription subscription, decimal expectedCost)
    {
        _dateTimeProvider.Today.Returns(new DateTime(2023, 1, 1));
        var result = _sut.GetYearlyAverageCost(subscription);
        Assert.Equal(expectedCost, result, 0);
    }

    [Fact]
    public void GetYearlyCost_NullSubscription_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _sut.GetYearlyAverageCost(null));
    }
}

internal class GetYearlyCostsTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new Subscription { Cost = 2.5m, BillingOccurrence = BillingOccurrence.Month, BillingInterval = 1 }, 30 };
        yield return new object[] { new Subscription { Cost = 5m, BillingOccurrence = BillingOccurrence.Week, BillingInterval = 2 }, 130 };
        yield return new object[] { new Subscription { Cost = 0, BillingOccurrence = BillingOccurrence.Month, BillingInterval = 1 }, 0 };
        yield return new object[] { new Subscription { Cost = 2, BillingOccurrence = BillingOccurrence.Year, BillingInterval = 1 }, 2 };
        yield return new object[] { new Subscription { Cost = 2, BillingOccurrence = BillingOccurrence.Year, BillingInterval = 2 }, 1 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
