using subtrack.MAUI.Responses;
using subtrack.MAUI.Utilities;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class GetSubscriptionListByMonthTests
    {
        private readonly IMonthlyPageCalculator _sut;
        private readonly int _numberOfMonths = 3;
        private readonly DateTime _fromIncludedDate = new(2023, 4, 1),
                                  _toIncludedDate;

        public GetSubscriptionListByMonthTests()
        {
            _toIncludedDate = _fromIncludedDate.AddMonths(_numberOfMonths - 1);
            var dateProvider = Substitute.For<IDateProvider>();
            var subscriptionsCalculator = new SubscriptionsCalculator(dateProvider);
            _sut = new MonthlyPageCalculator(subscriptionsCalculator);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetSubscriptionListByMonth_MonthProvided_Returns_UnpaidSubscriptions(int subscriptionIndex)
        {
            // Arrange
            var subscriptions = CreateSubscriptions();
            var subscription = subscriptions.ElementAt(subscriptionIndex);

            // Act
            var result = _sut.GetMonthlySubscriptionLists(subscriptions, _fromIncludedDate, _toIncludedDate);
            var collectedSubscriptions = CollectSubscriptions(result);

            // Assert
            Assert.Equal(_numberOfMonths, result.First().Value.Count);
            Assert.Equal(_numberOfMonths, collectedSubscriptions.Count(sub => sub.Id.Equals(subscription.Id)));
        }

        [Fact]
        public void GetSubscriptionListByMonth_DoesNotReturn_SubscriptionsWithNonStartedPayments()
        {
            // Arrange
            var subscriptions = new[] { new Subscription() { Name = "Subscription", LastPayment = _fromIncludedDate.AddMonths(1), BillingInterval = 1, BillingOccurrence = BillingOccurrence.Month, FirstPaymentDay = 1 } };

            // Act
            var result = _sut.GetMonthlySubscriptionLists(subscriptions, _fromIncludedDate, _fromIncludedDate.LastDayOfMonthDate());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_ShouldNotReturn_PaidSubscriptions()
        {
            // Arrange
            var subscriptions = CreateSubscriptions();

            // Act
            var result = _sut.GetMonthlySubscriptionLists(subscriptions, _fromIncludedDate, _toIncludedDate);

            // Assert
            Assert.Equal(_numberOfMonths, result.First().Value.Count);
        }

        [Theory]
        [InlineData(5, 4)]
        [InlineData(4, 2)]
        public void GetSubscriptionListByMonth_MonthProvided_ShouldReturnWeeklySubscriptionsMultipleTimes(int subscriptionsIndex, int expectedNumberOfIterationsInMonth)
        {
            // Arrange
            var subscriptions = CreateSubscriptions();
            var subscription = subscriptions.ElementAt(subscriptionsIndex);

            // Act
            var result = _sut.GetMonthlySubscriptionLists(subscriptions, _fromIncludedDate, _toIncludedDate);
            var collectedSubscriptions = CollectSubscriptions(result);

            // Assert
            Assert.Equal(_numberOfMonths, result.First().Value.Count);
            Assert.Equal(expectedNumberOfIterationsInMonth, collectedSubscriptions.Count(sub => sub.Id.Equals(subscription.Id)));
        }

        [Fact]
        public void GetSubscriptionListByMonth_WeeklySub_ShouldNotContainLastPayment()
        {
            // Arrange
            var sub = new Subscription { Name = "Subscriptions", FirstPaymentDay = 1, BillingInterval = 1, BillingOccurrence = BillingOccurrence.Week, LastPayment = new DateTime(2023, 4, 19) };

            // Act
            var result = _sut.GetMonthlySubscriptionLists(new[] { sub }, _fromIncludedDate, _toIncludedDate);

            // Assert
            Assert.Equal(2, result.First().Value.Count);
        }

        [Fact]
        public void GetSubscriptionListByMonth_MonthProvided_SubscriptionsHavingLongerThanMonthlyCycle_ShouldReturnAsExpected()
        {
            // Arrange
            var subscriptions = CreateSubscriptions();
            var subscription = subscriptions.ElementAt(6);

            // Act
            var result = _sut.GetMonthlySubscriptionLists(subscriptions, _fromIncludedDate, _toIncludedDate);

            // Assert
            Assert.Equal(_numberOfMonths, result.First().Value.Count);
            Assert.Contains(result.Last().Value.Last().Subscriptions, (sub) => sub.Id.Equals(subscription.Id));
        }

        private IEnumerable<Subscription> CreateSubscriptions()
        {
            yield return new Subscription
            {
                Id = 1,
                Cost = 65,
                IsAutoPaid = true,
                LastPayment = _fromIncludedDate.AddMonths(-1),
                Name = "Netflix",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            yield return new Subscription
            {
                Id = 2,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = _fromIncludedDate.AddMonths(-1),
                Name = "AmazonPrime",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            yield return new Subscription
            {
                Id = 3,
                Cost = 220,
                IsAutoPaid = true,
                LastPayment = _fromIncludedDate,
                Name = "Disney+",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            yield return new Subscription
            {
                Id = 4,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = _fromIncludedDate,
                Name = "HboMax",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 1,
            };

            yield return new Subscription
            {
                Id = 5,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = _fromIncludedDate.AddMonths(1).AddDays(2),
                Name = "Curiosity Stream",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Week,
                BillingInterval = 2,
            };

            yield return new Subscription
            {
                Id = 6,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = _fromIncludedDate.AddMonths(1).AddDays(2),
                Name = "Nebula",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Week,
                BillingInterval = 1,
            };

            yield return new Subscription
            {
                Id = 7,
                Cost = 19,
                IsAutoPaid = false,
                LastPayment = _fromIncludedDate.AddMonths(-1).AddDays(2),
                Name = "Youtube Premium",
                Description = "",
                FirstPaymentDay = 1,
                BillingOccurrence = BillingOccurrence.Month,
                BillingInterval = 3,
            };
        }

        private static IEnumerable<Subscription> CollectSubscriptions(IDictionary<int, List<SubscriptionsMonthResponse>> subscriptionsMonthResponses)
        {
            return subscriptionsMonthResponses.SelectMany(response => response.Value.SelectMany(r => r.Subscriptions));
        }
    }
}
