namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetAverageMonthlyCostTests
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateProvider _dateTimeProvider = Substitute.For<IDateProvider>();
    private readonly DateTime _date = new(2023, 4, 1);

    public GetAverageMonthlyCostTests()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
        _dateTimeProvider.Today.Returns(_date);
    }

    [Theory]
    [ClassData(typeof(GetAverageMonthlyCostsTestData))]
    public void GivenSubscriptions_WhenCalculatingAverageMonthlyCosts_ShouldReturnExpectedResults(IEnumerable<Subscription> subscriptions, decimal expectedCost)
    {
        // Act
        var averageMonthlyCost = _sut.GetAverageMonthlyCost(subscriptions);

        // Assert
        Assert.Equal(expectedCost, averageMonthlyCost, 2);
    }

    [Fact]
    public void GivenNullSubscriptions_WhenCalculatingAverageMonthlyCosts_ShouldThrowExpectedException()
    {
        // Arrange
        IEnumerable<Subscription> subscriptions = new List<Subscription> { null };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _sut.GetAverageMonthlyCost(subscriptions));
    }
}

public class GetAverageMonthlyCostsTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 
            new[] { new Subscription { Name = "Subscription 1", BillingInterval = 1, Cost = 24, BillingOccurrence = BillingOccurrence.Year } }, 
            2 
        };
        yield return new object[] {
            new[] { new Subscription { Name = "Subscription 2", BillingInterval = 1, Cost = 5, BillingOccurrence = BillingOccurrence.Week } },
            21.67
        };
        yield return new object[] {
            new[] { new Subscription { Name = "Subscription 3", BillingInterval = 2, Cost = 5, BillingOccurrence = BillingOccurrence.Week } },
            10.83
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
