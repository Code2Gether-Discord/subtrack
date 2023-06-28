using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Collections;

namespace subtrack.Tests.SubscriptionCalculatorTests;

public class GetDueDaysTests
{

    [Fact]
    public void GetDueDays_DueDateHasPassed_ReturnsNegativeDueDays()
    {
        //Arrange
        var subscription = new Subscription { LastPayment = DateTime.Now.AddMonths(-1).AddDays(-3) };
        
        //Act
        var result = SubscriptionsCalculator.GetDueDays(subscription);
        
        //Assert
        Assert.Equal(-3, result);
    }

    [Theory(Skip = "Waiting for dates to be mocked")]
    [ClassData(typeof(GetDueDaysTestData))]
    public void GetDueDays_ReturnsCorrectDueDate(DateTime date, int expectedResult)
    {
        var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month);
        var last = firstDayOfMonth.AddDays(daysInMonth - 1);

        //Arrange
        var subscription = new Subscription { LastPayment = date };

        //Act
        var result = SubscriptionsCalculator.GetDueDays(subscription);
        
        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(last, last.AddMonths(1));
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
    public IEnumerator<object[]> GetEnumerator()
    {
        
        yield return new object[] { DateTime.Now, 30 };
        yield return new object[] { DateTime.Now.AddMonths(-1), 0 };
        yield return new object[] { DateTime.Now.AddMonths(-1).AddDays(-1), -1 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


