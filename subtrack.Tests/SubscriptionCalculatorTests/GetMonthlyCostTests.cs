using subtrack.DAL.Entities;
using subtrack.MAUI.Services;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetMonthlyCostTests
{
    [Fact]
    public void GetMonthlyCost_EmptySubscriptions_ReturnsZero()
    {
        var subscriptions = new List<Subscription>();

        var result = subscriptions.GetMonthlyCost();

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetMonthlyCost_NullSubscriptions_ReturnsZero()
    {
        List<Subscription>? subscriptions = null;

        var result = subscriptions.GetMonthlyCost();

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetMonthlyCost_OneSubscription_ReturnsCorrectMonthlyCost()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Cost = 10.5m }
        };

        var result = subscriptions.GetMonthlyCost();

        Assert.Equal(10.5m, result);
    }

    [Fact]
    public void GetMonthlyCost_MultipleSubscriptions_ReturnsCorrectMonthlyCost()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Cost = 10.5m },
            new Subscription { Cost = 5.25m },
            new Subscription { Cost = 7.75m }
        };

        var result = subscriptions.GetMonthlyCost();

        Assert.Equal(23.5m, result);
    }
}
