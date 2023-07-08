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
            //Arrange
            _dateTimeProvider.Now
                             .Returns(new DateTimeOffset(2023, 06, 05, 8, 0, 0, TimeSpan.Zero));

            var subscription = new Subscription { LastPayment = new DateTime(2023, 05, 02, 8, 0, 0) };

            //Act
            var result = _sut.GetNextPaymentDate(subscription);

            //Assert
            Assert.Equal(new DateTime(2023, 04, 29, 8, 0, 0), result);
        }
    }
}
