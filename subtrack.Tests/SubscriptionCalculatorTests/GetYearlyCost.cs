using NSubstitute;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using subtrack.MAUI.Services.Abstractions;
using System.Collections;

namespace subtrack.Tests.SubscriptionCalculatorTests;
public class GetYearlyCostTests
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateProvider _dateTimeProvider = Substitute.For<IDateProvider>();

    public GetYearlyCostTests()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
    }

    [Theory]
    [ClassData(typeof(GetYearlyCostsTestData))]
    public void GetYearlyCost_ReturnsCorrectCost(Subscription subscription, decimal expectedCost)
    {
        _dateTimeProvider.Today.Returns(new DateTime(2023, 1, 1));
        var result = _sut.GetYearlyCost(subscription);
        Assert.Equal(expectedCost, result);
    }

    [Fact]
    public void GetYearlyCost_NullSubscription_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _sut.GetYearlyCost(null));
    }

    [Theory]
    [ClassData(typeof(GetInvalidYearlyCostsTestData))]
    public void GetYearlyCost_InvalidBillingProps_Throws_NotImplementedException(Subscription subscription)
    {
        Assert.Throws<NotImplementedException>(
            () => _sut.GetYearlyCost(subscription));
    }
}

internal class GetYearlyCostsTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new Subscription { Cost = 2.5m, BillingOccurrence = BillingOccurrence.Month, BillingInterval = 1 }, 30 };
        yield return new object[] { new Subscription { Cost = 5m, BillingOccurrence = BillingOccurrence.Week, BillingInterval = 2 }, 130 };
        yield return new object[] { new Subscription { Cost = 0, BillingOccurrence = BillingOccurrence.Month, BillingInterval = 1 }, 0 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class GetInvalidYearlyCostsTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new Subscription { Cost = 2.5m, BillingOccurrence = BillingOccurrence.Year, BillingInterval = 1 } };
        yield return new object[] { new Subscription { Cost = 5m, BillingOccurrence = BillingOccurrence.Week, BillingInterval = 60 } };
        yield return new object[] { new Subscription { Cost = 0, BillingOccurrence = BillingOccurrence.Month, BillingInterval = 14 } };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
