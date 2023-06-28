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

    
    [Theory]
    [ClassData(typeof(GetDueDaysTestData))]
    public void GetDueDays_ReturnsCorrectDueDate(DateTime date, int expectedResult)
    {
        //Arrange
        var subscription = new Subscription { LastPayment = date };

        //Act
        var result = SubscriptionsCalculator.GetDueDays(subscription);
        
        //Assert
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
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { DateTime.Now, 30 };
        yield return new object[] { DateTime.Now.AddMonths(-1), 0 };
        yield return new object[] { DateTime.Now.AddMonths(-1).AddDays(-1), -1 };
        //THIS is an edge case that needs to be edited
        yield return new object[] { DateTime.Now.AddMonths(1), 61 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


