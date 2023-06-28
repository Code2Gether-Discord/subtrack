using subtrack.DAL.Entities;
using subtrack.MAUI.Services;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetTotalPrice
{
    [Fact]
    public void GetTotalPrice_EmptySubscriptions_ReturnsZero()
    {
        var subscriptions = new List<Subscription>();

        var result = SubscriptionsCalculator.GetTotalPrice(subscriptions);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalPrice_NullSubscriptions_ReturnsZero()
    {
        List<Subscription>? subscriptions = null;

        var result = SubscriptionsCalculator.GetTotalPrice(subscriptions);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalPrice_OneSubscription_ReturnsCorrectMonthlyCost()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Cost = 10.5m }
        };

        var result = SubscriptionsCalculator.GetTotalPrice(subscriptions);

        Assert.Equal(10.5m, result);
    }

    [Fact]
    public void GetTotalPrice_MultipleSubscriptions_ReturnsCorrectMonthlyCost()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Cost = 10.5m },
            new Subscription { Cost = 5.25m },
            new Subscription { Cost = 7.75m }
        };

        var result = SubscriptionsCalculator.GetTotalPrice(subscriptions);

        Assert.Equal(23.5m, result);
    }
}
