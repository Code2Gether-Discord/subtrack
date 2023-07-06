using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using NSubstitute;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class GetSubscriptionListByMonthTests
    {
        private readonly ISubscriptionsCalculator _sut;
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        public GetSubscriptionListByMonthTests()
        {
            _sut = new SubscriptionsCalculator(_dateTimeProvider);
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_Returns_UnpaidSubscriptions()
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            var result = _sut.GetSubscriptionListByMonth(subscriptions, DateTime.Now.Month);

            // Assert
            Assert.Contains(subscriptions[0], result);
            Assert.Contains(subscriptions[1], result);
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_ShouldNotReturn_PaidSubscriptions()
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            var result = _sut.GetSubscriptionListByMonth(subscriptions, DateTime.Now.Month);

            // Assert
            Assert.DoesNotContain(subscriptions[2], result);
            Assert.DoesNotContain(subscriptions[3], result);
        }

        public static IList<Subscription> CreateSubscriptions()
        {
            DateTime today = DateTime.Today;

            var firstSub = new Subscription
            {
                Id = 1,
                Cost = 65,
                IsAutoPaid = true,
                LastPayment = today.AddMonths(-1),
                Name = "Netflix",
                Description = ""
            };

            var secondSub = new Subscription
            {
                Id = 2,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today.AddMonths(-1),
                Name = "AmazonPrime",
                Description = ""
            };

            var thirdSub = new Subscription
            {
                Id = 3,
                Cost = 220,
                IsAutoPaid = true,
                LastPayment = today,
                Name = "Disney+",
                Description = ""
            };

            var fourthSub = new Subscription
            {
                Id = 4,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today,
                Name = "HboMax",
                Description = ""
            };

            return new List<Subscription> { firstSub, secondSub, thirdSub, fourthSub};
        }
    }
}
