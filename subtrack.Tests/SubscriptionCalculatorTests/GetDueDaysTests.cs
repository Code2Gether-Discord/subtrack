using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using subtrack.Tests.Extensions;
using subtrack.Tests.DateAndTimeProvider;
using System.Collections.Generic;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetDueDaysTests
{

    [Fact]
    public void GetDueDays_DueDateHasPassed_ReturnsNegativeDueDays()
    {
        //Arrange
        var date = new FixedDateTimeProvider(DateTime.Now
                                                     .AddMonths(-1)
                                                     .AddDays(-3));

        var subscription = new Subscription { LastPayment = date.GetNow() };

        //Act
        var result = SubscriptionsCalculator.GetDueDays(subscription);

        //Assert
        Assert.Equal(-3, result);
    }

    [Theory]
    [ClassData(typeof(GetDueDaysTestData))]
    public void GetDueDays_ReturnsCorrectDueDate(DateTime date, int expectedResult)
    {
        // Arrange
        var subscription = new Subscription { LastPayment = date };

        // Act
        var result = SubscriptionsCalculator.GetDueDays(subscription);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetDueDays_NullValue_ThrowsArgumentNullException()
    {
        //Arrange
        var subscription = new Subscription();
        //Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => SubscriptionsCalculator.GetDueDays(null));
    }
}

public class GetDueDaysTestData : IEnumerable<object[]>
{
    private readonly FixedDateTimeProvider _fixedDateTimeProvider;

    public GetDueDaysTestData()
    {
        _fixedDateTimeProvider = new FixedDateTimeProvider(DateTime.Now);
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { _fixedDateTimeProvider.GetNow(), _fixedDateTimeProvider.DaysInMonth() };
        yield return new object[] { _fixedDateTimeProvider.GetNow().AddMonths(-1), 0 };
        yield return new object[] { _fixedDateTimeProvider.GetNow().AddMonths(-1).AddDays(-1), -1 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
