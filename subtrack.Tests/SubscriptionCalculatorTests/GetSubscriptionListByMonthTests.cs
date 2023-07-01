using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Collections;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class GetSubscriptionListByMonthTests
    {
        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_Returns_UnpaidSubscriptions()
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            var result = SubscriptionsCalculator.GetSubscriptionListByMonth(subscriptions, DateTime.Today);

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
            var result = SubscriptionsCalculator.GetSubscriptionListByMonth(subscriptions, DateTime.Today);

            // Assert
            Assert.DoesNotContain(subscriptions[2], result);
            Assert.DoesNotContain(subscriptions[3], result);
        }

        public static IList<Subscription> CreateSubscriptions()
        {
            DateTime today = DateTime.Today;

            Subscription firstSub = new Subscription
            {
                Id = 1,
                Cost = 65,
                IsAutoPaid = true,
                LastPayment = today.AddMonths(-1),
                Name = "Netflix",
                Description = ""
            };

            Subscription secondSub = new Subscription
            {
                Id = 2,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today.AddMonths(-1),
                Name = "AmazonPrime",
                Description = ""
            };

            Subscription thirdSub = new Subscription
            {
                Id = 3,
                Cost = 220,
                IsAutoPaid = true,
                LastPayment = today,
                Name = "Disney+",
                Description = ""
            };

            Subscription fourthSub = new Subscription
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
