using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Collections;

namespace subtrack.Tests.SubscriptionCalculatorTests;
public class GetYearlyCostTests
{
    [Theory]
    [ClassData(typeof(GetYearlyCostsTestData))]
    public void GetYearlyCost_ReturnsCorrectCost(Subscription subscription, decimal expectedCost)
    {
        var result = SubscriptionsCalculator.GetYearlyCost(subscription);
        Assert.Equal(expectedCost, result);
    }

    [Fact]
    public void GetYearlyCost_NullSubscription_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => SubscriptionsCalculator.GetYearlyCost(null));
    }
}

internal class GetYearlyCostsTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new Subscription { Cost = 2.5m }, 30 };
        yield return new object[] { new Subscription { Cost = 1 }, 12 };
        yield return new object[] { new Subscription { Cost = 0 }, 0 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
