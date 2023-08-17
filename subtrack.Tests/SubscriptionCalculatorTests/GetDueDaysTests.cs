using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Collections;
using NSubstitute;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetDueDaysTests
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateProvider _dateTimeProvider = Substitute.For<IDateProvider>();

    public GetDueDaysTests()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
    }

    [Fact]
    public void GetDueDays_DueDateHasPassed_ReturnsNegativeDueDays()
    {
        //Arrange
        _dateTimeProvider.Today
                         .Returns(new DateTime(2023, 06, 05));

        var subscription = new Subscription { LastPayment = new DateTime(2023, 05, 02), 
            FirstPaymentDay = 2 , BillingOccurrence = BillingOccurrence.Month, BillingInterval = 1 };

        //Act
        var result = _sut.GetDueDays(subscription);

        //Assert
        Assert.Equal(-3, result);
    }

    [Theory]
    [ClassData(typeof(GetDueDaysTestData))]
    public void GetDueDays_ReturnsCorrectDueDate(DateTime date, int expectedResult)
    {
        // Arrange
        _dateTimeProvider.Today
                         .Returns(new DateTime(2023, 06, 05));
        var subscription = new Subscription {LastPayment = date, 
            FirstPaymentDay = date.Day, BillingOccurrence = BillingOccurrence.Month, BillingInterval = 1 };

        // Act
        var result = _sut.GetDueDays(subscription);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetDueDays_NullValue_ThrowsArgumentNullException()
    {
        //Arrange
        var subscription = new Subscription();
        //Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => _sut.GetDueDays(null));
    }
}

public class GetDueDaysTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new DateTime(2023, 06, 05), 30};
        yield return new object[] { new DateTime(2023, 05, 05), 0 };
        yield return new object[] { new DateTime(2023, 05, 04), -1};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
