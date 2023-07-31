using NSubstitute;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class GetNextPaymentDateTests
    {
        private readonly ISubscriptionsCalculator _sut;
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        public GetNextPaymentDateTests()
        {
            _sut = new SubscriptionsCalculator(_dateTimeProvider);
        }

        [Fact]
        public void GetNextPaymentDate_ReturnsCorrectNextPaymentDate()
        {
            // Arrange
            var subscription = new Subscription { LastPayment = new DateTime(2023, 06, 02) };
            var expected = new DateTime(2023, 07, 02);

            // Act
            var result = _sut.GetNextPaymentDate(subscription);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetNextPaymentDate_WhenLastPaymentIsLastDayOfMonth_ReturnsPaymentDateWithLastDayOfNextMonth()
        {
            // Arrange
            var subscription = new Subscription { LastPayment = new DateTime(2023, 05, 31) };
            var expected = new DateTime(2023, 06, 30);

            // Act
            var result = _sut.GetNextPaymentDate(subscription);

            // Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetNextPaymentDate_WhenLastPaymentIsLastDayOfFebruary_ReturnsPaymentDateWithLastDayOfNextMonth()
        {
            // Arrange
            var lastPayment = new DateTime(2023, 02, 28);
            var subscription = new Subscription { LastPayment = lastPayment, FirstPaymentDay = lastPayment.Day };
            var expected = new DateTime(2023, 03, 31);

            // Act
            var result = _sut.GetNextPaymentDate(subscription);

            // Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetNextPaymentDate_WhenLastPaymentIsLastDayOfJanuary_ReturnsPaymentDateWithLastDayOfNextMonth()
        {
            // Arrange
            var subscription = new Subscription { LastPayment = new DateTime(2023, 01, 31) };
            var expected = new DateTime(2023, 02, 28);

            // Act
            var result = _sut.GetNextPaymentDate(subscription);

            // Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetNextPaymentDate_WhenLastPaymentIsLastDayOfJanuary_LeapYear_ReturnsPaymentDateWithLastDayOfNextMonth()
        {
            // Arrange
            var subscription = new Subscription { LastPayment = new DateTime(2024, 01, 31) };
            var expected = new DateTime(2024, 02, 29);

            // Act
            var result = _sut.GetNextPaymentDate(subscription);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
