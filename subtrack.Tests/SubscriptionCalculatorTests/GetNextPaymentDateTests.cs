using NSubstitute;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using subtrack.MAUI.Services.Abstractions;
using System.Collections;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetNextPaymentDateTests
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateProvider _dateTimeProvider = Substitute.For<IDateProvider>();

    public GetNextPaymentDateTests()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
    }

    [Fact]
    public void GetNextPaymentDate_ReturnsCorrectNextPaymentDateWeekly()
    {
        // Arrange
        var subscription = new Subscription { LastPayment = new DateTime(2023, 6, 2) , FirstPaymentDay = new DateTime(2023, 6, 2).Day, BillingInterval = 1, BillingOccurrence=BillingOccurrence.Week};
        var expected = new DateTime(2023, 6, 9);

        // Act
        var result = _sut.GetNextPaymentDate(subscription);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetNextPaymentDate_ReturnsCorrectNextPaymentDateMonthly()
    {
        // Arrange
        var subscription = new Subscription { LastPayment = new DateTime(2023, 6, 2), FirstPaymentDay = new DateTime(2023, 6, 2).Day, BillingInterval = 1, BillingOccurrence = BillingOccurrence.Month };
        var expected = new DateTime(2023, 7, 2);

        // Act
        var result = _sut.GetNextPaymentDate(subscription);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetNextPaymentDate_ReturnsCorrectNextPaymentDateYearly()
    {
        // Arrange
        var subscription = new Subscription { LastPayment = new DateTime(2023, 6, 2), FirstPaymentDay = new DateTime(2023, 6, 2).Day, BillingInterval = 1, BillingOccurrence = BillingOccurrence.Year };
        var expected = new DateTime(2024, 6, 2);

        // Act
        var result = _sut.GetNextPaymentDate(subscription);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetNextPaymentDate_ReturnsCorrectNextPaymentDateWeeklyTwoInterval()
    {
        // Arrange
        var subscription = new Subscription { LastPayment = new DateTime(2023, 6, 2), FirstPaymentDay = new DateTime(2023, 6, 2).Day, BillingInterval = 2, BillingOccurrence = BillingOccurrence.Week };
        var expected = new DateTime(2023, 6, 16);

        // Act
        var result = _sut.GetNextPaymentDate(subscription);

        // Assert
        Assert.Equal(expected, result);
    }
    [Theory]
    [ClassData(typeof(GetNextPaymentDateTestData))]
    public void GetNextPaymentDate_WhenLastPaymentIsLastDayOfMonth_ReturnsPaymentDateWithLastDayOfNextMonth(DateTime lastPayment, DateTime expectedNextPaymentDate)
    {
        // Arrange
        var subscription = new Subscription { LastPayment = lastPayment, FirstPaymentDay = lastPayment.Day, BillingOccurrence = BillingOccurrence.Month};
   

        // Act
        var result = _sut.GetNextPaymentDate(subscription);

        // Assert
        Assert.Equal(expectedNextPaymentDate, result);
    }

    [Fact]
    public void GetNextPaymentDate_ReturnsSameNextPaymentDateDay_AsFirstPaymentDay()
    {
        // Arrange
        var subscription = new Subscription { FirstPaymentDay = 31, LastPayment = new DateTime(2023,11,30),BillingOccurrence = BillingOccurrence.Month};
   

        // Act
        var result = _sut.GetNextPaymentDate(subscription);

        // Assert
        Assert.Equal(new DateTime(2023, 12, 31), result);
    }
}
public class GetNextPaymentDateTestData : IEnumerable<object[]>
{

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new DateTime(2023, 01, 31), new DateTime(2023, 02, 28) };
        yield return new object[] { new DateTime(2024, 01, 31), new DateTime(2024, 02, 29) };
        yield return new object[] { new DateTime(2023, 2, 28), new DateTime(2023, 3, 28) };
        yield return new object[] { new DateTime(2023, 7, 31), new DateTime(2023, 8, 31) };
        yield return new object[] { new DateTime(2023, 8, 31), new DateTime(2023, 9, 30) };
        yield return new object[] { new DateTime(2023, 9, 30), new DateTime(2023, 10, 30) };
        yield return new object[] { new DateTime(2023, 10, 31), new DateTime(2023, 11, 30) };
        yield return new object[] { new DateTime(2023, 11, 30), new DateTime(2023, 12, 30) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
