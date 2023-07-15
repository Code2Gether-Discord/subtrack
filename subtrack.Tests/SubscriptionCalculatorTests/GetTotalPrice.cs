using NSubstitute;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetTotalPrice
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public GetTotalPrice()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
    }

    [Fact]
    public void GetTotalCost_EmptySubscriptions_ReturnsZero()
    {
        var subscriptions = new List<Subscription>();

        var result = _sut.GetTotalCost(subscriptions);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalCost_NullSubscriptions_ReturnsZero()
    {
        List<Subscription>? subscriptions = null;

        var result = _sut.GetTotalCost(subscriptions!);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalCost_OneSubscription_ReturnsCorrectMonthlyCost()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Cost = 10.5m }
        };

        var result = _sut.GetTotalCost(subscriptions);

        Assert.Equal(10.5m, result);
    }

    [Fact]
    public void GetTotalCost_MultipleSubscriptions_ReturnsCorrectMonthlyCost()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Cost = 10.5m },
            new Subscription { Cost = 5.25m },
            new Subscription { Cost = 7.75m }
        };

        var result = _sut.GetTotalCost(subscriptions);

        Assert.Equal(23.5m, result);
    }
}
