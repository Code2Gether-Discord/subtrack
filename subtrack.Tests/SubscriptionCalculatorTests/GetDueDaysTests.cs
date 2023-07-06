using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Collections;
using NSubstitute;
using NSubstitute.Core;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetDueDaysTests
{
    private readonly ISubscriptionsCalculator _sut;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public GetDueDaysTests()
    {
        _sut = new SubscriptionsCalculator(_dateTimeProvider);
    }

    [Fact]
    public void GetDueDays_DueDateHasPassed_ReturnsNegativeDueDays()
    {
        //Arrange
        _dateTimeProvider.Now
                         .Returns(new DateTimeOffset(2023, 06, 05, 8, 0, 0, TimeSpan.Zero));

        var subscription = new Subscription { LastPayment = new DateTime(2023, 05, 02, 8, 0, 0) };

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
        _dateTimeProvider.Now
                         .Returns(new DateTimeOffset(2023, 06, 05, 8, 0, 0, TimeSpan.Zero));
        var subscription = new Subscription { LastPayment = date };
        
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
