using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using NSubstitute;
using subtrack.MAUI.Services.Abstractions;
using subtrack.Tests.Utilities;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class GetSubscriptionListByMonthTests
    {
        private readonly ISubscriptionsCalculator _sut;
        private readonly IDateProvider _dateTimeProvider = Substitute.For<IDateProvider>();
        private IEqualityComparer<Subscription> _subscriptionComparer;
        private readonly DateTime today = new(2023, 4, 18);
        public GetSubscriptionListByMonthTests()
        {
            _sut = new SubscriptionsCalculator(_dateTimeProvider);
            _dateTimeProvider.Today.Returns(today);
            _subscriptionComparer = new SubscriptionEqualityComparer();
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_Returns_UnpaidSubscriptions()
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            var result = _sut.GetSubscriptionListByMonth(subscriptions, today).ToList();

            // Assert
            Assert.Contains(subscriptions[0], result, _subscriptionComparer);
            Assert.Contains(subscriptions[1], result, _subscriptionComparer);
        }

        [Fact]
        public void GetSubscriptionListByMonth_DoesNotReturn_SubscriptionsWithNonStartedPayments()
        {
            // Arrange
            IList<Subscription> subscriptions = new[] { new Subscription() { LastPayment = today.AddMonths(1) } };

            // Act
            var result = _sut.GetSubscriptionListByMonth(subscriptions, today).ToList();

            // Assert
            Assert.DoesNotContain(subscriptions[0], result, _subscriptionComparer);
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_ShouldNotReturn_PaidSubscriptions()
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            var result = _sut.GetSubscriptionListByMonth(subscriptions, today).ToList();

            // Assert
            Assert.DoesNotContain(subscriptions[2], result, _subscriptionComparer);
            Assert.DoesNotContain(subscriptions[3], result, _subscriptionComparer);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(4, 3)]
        public void GetSubscriptionListByMonth_MonthProvided_ShouldReturnWeeklySubscriptionsMultipleTimes(int subscriptionsIndex, int expectedNumberOfIterationsInMonth)
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            var result = _sut.GetSubscriptionListByMonth(subscriptions, today.AddMonths(2)).ToList();

            // Assert
            Assert.Equal(expectedNumberOfIterationsInMonth, result.Count(s => s.Name == subscriptions[subscriptionsIndex].Name));
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_SubscriptionsHavingLongerThanMonthlyCycle_ShouldReturnAsExpected()
        {
            // Arrange
            IList<Subscription> subscriptions = CreateSubscriptions();

            // Act
            List<Subscription> first = _sut.GetSubscriptionListByMonth(subscriptions, today.AddMonths(2)).ToList(),
                second = _sut.GetSubscriptionListByMonth(subscriptions, today.AddMonths(4)).ToList(),
                third = _sut.GetSubscriptionListByMonth(subscriptions, today.AddMonths(5)).ToList();

            // Assert
            Assert.Contains(subscriptions[6], first, _subscriptionComparer);
            Assert.DoesNotContain(subscriptions[6], second, _subscriptionComparer);
            Assert.Contains(third, s => s.Name == subscriptions[6].Name);
        }

        private IList<Subscription> CreateSubscriptions()
        {
            var firstSub = new Subscription
            {
                Id = 1,
                Cost = 65,
                IsAutoPaid = true,
                LastPayment = today.AddMonths(-1),
                Name = "Netflix",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            var secondSub = new Subscription
            {
                Id = 2,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today.AddMonths(-1),
                Name = "AmazonPrime",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            var thirdSub = new Subscription
            {
                Id = 3,
                Cost = 220,
                IsAutoPaid = true,
                LastPayment = today,
                Name = "Disney+",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            var fourthSub = new Subscription
            {
                Id = 4,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today,
                Name = "HboMax",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            var fifthSub = new Subscription
            {
                Id = 4,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today.AddMonths(1).AddDays(2),
                Name = "Curiosity Stream",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Week,
                BillingInterval = 2,
            };

            var sixthSub = new Subscription
            {
                Id = 4,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today.AddMonths(1).AddDays(2),
                Name = "Nebula",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Week,
                BillingInterval = 1,
            };

            var seventhSub = new Subscription
            {
                Id = 4,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = today.AddMonths(-1).AddDays(2),
                Name = "Youtube Premium",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 3,
            };

            return new List<Subscription> { firstSub, secondSub, thirdSub, fourthSub, fifthSub, sixthSub, seventhSub };
        }
    }
}
